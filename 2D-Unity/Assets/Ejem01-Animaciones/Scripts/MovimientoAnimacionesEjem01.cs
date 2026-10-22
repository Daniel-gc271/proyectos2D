using UnityEngine;
using UnityEngine.InputSystem; // Por si usas el Input System

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class MovimientoAnimacionesEjem01 : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 8f;
    [SerializeField] private float jumpForce = 10f;

    [Header("Componentes")]
    [SerializeField] private Animator animator;
    [SerializeField]  private Rigidbody2D rb;

    [Header("Estado actual")]
    [SerializeField] private bool IsGrounded;
    [SerializeField] private bool isRunning;
    private float movH;

    private void Start()
    {
        if (rb == null) { Debug.LogWarning("No hay rigidBody"); }
        if (animator == null) { Debug.LogWarning("No hay Animator"); }
        
    }

    private void Update()
    {
        // 1. Entrada horizontal (-1, 0, 1)
        movH = Input.GetAxisRaw("Horizontal");

        // 2. Alternar carrera con Control Izquierdo
        if (Input.GetKeyDown(KeyCode.LeftControl) || (Keyboard.current != null && Keyboard.current.leftCtrlKey.wasPressedThisFrame))
        {
            isRunning = !isRunning;
        }

        // 3. Aplicar velocidad horizontal a la física
        float currentSpeed = isRunning ? runSpeed : walkSpeed;
        rb.linearVelocity = new Vector2(movH * currentSpeed, rb.linearVelocity.y);

        // 4. Salto (Sacado fuera del if de animator para que funcione siempre)
        if ((Input.GetKeyDown(KeyCode.Space) || (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)) && IsGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            if (animator != null) animator.SetTrigger("Jump");
            IsGrounded = false;
        }

        // 5. Daño (Sacado fuera del if de animator)
        if (Input.GetKeyDown(KeyCode.Q) || (Keyboard.current != null && Keyboard.current.qKey.wasPressedThisFrame))
        {
            if (animator != null) animator.SetTrigger("Hurt");
        }

        // 6. Actualizar variables del Animator (Protegido correctamente)
        if (animator != null)
        {
            animator.SetBool("IsGrounded", IsGrounded);
            animator.SetBool("IsRunning", isRunning);

            if (movH > 0)
            {
                // Va a la derecha
                animator.SetBool("DirectionRight", true);
                animator.SetBool("IsIdle", false);
            }
            else if (movH < 0)
            {
                // Va a la izquierda
                animator.SetBool("DirectionRight", false);
                animator.SetBool("IsIdle", false);
            }
            else
            {
                // Quieto
                animator.SetBool("IsIdle", true);
            }
        }
    } // <- Aquí cierra correctamente el Update

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("floor"))
        {
            IsGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("floor"))
        {
            IsGrounded = false;
        }
    }
}
