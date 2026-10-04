using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class MovimientoPlataformas : MonoBehaviour
{
    [Header("Escala del Jugador")]
    [SerializeField] private float playerScale = 1.96f; // Escala base requerida

    [Header("Movimiento")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 8f;
    [SerializeField] private float jumpForce = 10f;

    [Header("Escalera")]
    [SerializeField] private float climbSpeed = 5f;
    [SerializeField] private bool isClimbing;
    [SerializeField] private bool canClimb;

    [Header("Componentes")]
    [SerializeField] private Animator animator;
    private Rigidbody2D rb;
    private BoxCollider2D playerCollider;

    [Header("Estado actual")]
    [SerializeField] private bool isGrounded;
    [SerializeField] private bool isRunning;
    [Header("Ataque con Espada")]
    [SerializeField] private Transform attackPoint; // Punto desde donde ataca la espada
    [SerializeField] private float attackRange = 0.5f; // Radio del ataque
    [SerializeField] private LayerMask enemyLayers; // Layer de los enemigos
    private bool canEnterDoor;
    private float movH;
    private float movV;

    private float originalGravity;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<BoxCollider2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        originalGravity = rb.gravityScale;

        // Asignar escala base al iniciar
        transform.localScale = new Vector3(playerScale, playerScale, 1f);

        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }
    }

    private void Update()
    {
        // 1. Lectura de entradas
        movH = Input.GetAxisRaw("Horizontal");
        movV = Input.GetAxisRaw("Vertical");

        // 2. Lógica para empezar/dejar de escalar (W o Flecha Arriba)
        if (canClimb && (movV > 0.1f || (isClimbing && Mathf.Abs(movV) > 0.1f)))
        {
            isClimbing = true;
        }

        if (!canClimb)
        {
            isClimbing = false;
        }

        // 3. Aplicar físicas según si está escalando o moviéndose normalmente
        if (isClimbing)
        {
            rb.gravityScale = 0f;
            rb.linearVelocity = new Vector2(movH * walkSpeed, movV * climbSpeed);
            playerCollider.isTrigger = true; // Permite atravesar la plataforma superior al subir
        }
        else
        {
            rb.gravityScale = originalGravity;
            playerCollider.isTrigger = false;

            if (Input.GetKeyDown(KeyCode.LeftControl) || (Keyboard.current != null && Keyboard.current.leftCtrlKey.wasPressedThisFrame))
            {
                isRunning = !isRunning;
            }

            float currentSpeed = isRunning ? runSpeed : walkSpeed;
            rb.linearVelocity = new Vector2(movH * currentSpeed, rb.linearVelocity.y);

            // Salto
            if ((Input.GetKeyDown(KeyCode.Space) || (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)) && isGrounded)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                if (animator != null) animator.SetTrigger("Jump");
                isGrounded = false;
            }
        }

        // 4. Voltear el Sprite (Flip) manteniendo la escala 1.96
        if (movH > 0)
        {
            transform.localScale = new Vector3(playerScale, playerScale, 1f); // Derecha
        }
        else if (movH < 0)
        {
            transform.localScale = new Vector3(-playerScale, playerScale, 1f); // Izquierda
        }

        // 5. Parámetros del Animator e Interacciones
        if (animator != null)
        {
            float speedForAnimator = Mathf.Abs(movH) * (isRunning ? 2f : 1f);
            animator.SetFloat("Speed", speedForAnimator);
            animator.SetBool("IsGrounded", isGrounded);

            // Interacción con Puerta o Daño al presionar Q
            if (Input.GetKeyDown(KeyCode.Q) || (Keyboard.current != null && Keyboard.current.qKey.wasPressedThisFrame))
            {
                if (canEnterDoor)
                {
                    EntrarPuerta();
                }
                else
                {
                    animator.SetTrigger("Hurt");
                }
            }
        }
        // Ataque con Espada (Tecla E o Clic Izquierdo)
        if (Input.GetKeyDown(KeyCode.E) || (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame))
        {
            AtacarEspada();
        }
    }
    private void AtacarEspada()
    {
        // Disparar animación de ataque si existe
        if (animator != null)
        {
            animator.SetTrigger("Attack"); // Asegúrate de tener este Trigger en tu Animator si usas animación
        }

        // Detectar enemigos en el rango de ataque
        Vector3 puntoAtaque = attackPoint != null ? attackPoint.position : transform.position;
        Collider2D[] enemigosGolpeados = Physics2D.OverlapCircleAll(puntoAtaque, attackRange, enemyLayers);

        // Infligir daño / Matar enemigos detectados
        foreach (Collider2D enemigo in enemigosGolpeados)
        {
            Enemigo scriptEnemigo = enemigo.GetComponent<Enemigo>();
            if (scriptEnemigo != null)
            {
                scriptEnemigo.Morir();
            }
        }
    }

    // Dibuja el rango del ataque en el editor de Unity para facilitar el ajuste
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
    private void EntrarPuerta()
    {
        Debug.Log("¡Entrando por la puerta!");
        // Ejemplo opcional para teletransportar al jugador al 2º piso:
        // transform.position = new Vector3(transform.position.x, transform.position.y + 4f, transform.position.z);
    }

    // --- DETECCIÓN DE TRIGGERS ---
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("ladder"))
        {
            canClimb = true;
        }

        if (other.CompareTag("door"))
        {
            canEnterDoor = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("ladder"))
        {
            canClimb = false;
            isClimbing = false;
            playerCollider.isTrigger = false;
        }

        if (other.CompareTag("door"))
        {
            canEnterDoor = false;
        }
    }

    // --- DETECCIÓN DE SUELO ---
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("floor"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("floor"))
        {
            isGrounded = false;
        }
    }
}