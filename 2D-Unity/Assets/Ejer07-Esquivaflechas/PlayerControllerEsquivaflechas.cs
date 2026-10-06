using UnityEngine;
using System.Collections; // Necesario para la corrutina del parpadeo

public class PlayerControllerEsquivaflechas : MonoBehaviour
{
    [SerializeField] private Animator mainPlayerAnimator;

    [Header("Configuración de Movimiento")]
    [SerializeField] private float velocidadMovimiento = 5f;
    [SerializeField] private float fuerzaSalto = 7f;

    [Header("Detección de Suelo")]
    [SerializeField] private LayerMask capaSuelo; // Selecciona aquí la capa de tu suelo

    [Header("Efecto de Daño (Shader GPU)")]
    [SerializeField] private Color colorDaño = Color.red;
    [SerializeField] private float duracionParpadeo = 0.15f;
    [SerializeField] private int cantidadParpadeos = 3;

    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    private SpriteRenderer spriteRenderer; // Componente que envía el color al shader de la GPU
    private float movimientoHorizontal;
    private bool mirandoDerecha = true;
    private bool enElSuelo;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // Inicializamos el SpriteRenderer
    }

    void Update()
    {
        // 1. Comprobar si estamos tocando el suelo de forma precisa
        enElSuelo = ChequearSuelo();

        // Enviar el estado del suelo al Animator para controlar las transiciones de salto
        if (mainPlayerAnimator != null)
        {
            mainPlayerAnimator.SetBool("OnGround", enElSuelo);
        }

        // 2. Capturar el movimiento horizontal con A y D (A = -1, D = 1)
        movimientoHorizontal = 0f;
        if (Input.GetKey(KeyCode.A))
        {
            movimientoHorizontal = -1f;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            movimientoHorizontal = 1f;
        }

        if (mainPlayerAnimator != null)
        {
            mainPlayerAnimator.SetFloat("Speed", Mathf.Abs(movimientoHorizontal));
        }

        // 3. Detectar el salto con la tecla Espacio (solo si está tocando el suelo)
        if (Input.GetKeyDown(KeyCode.Space) && enElSuelo)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, fuerzaSalto);

            if (mainPlayerAnimator != null)
            {
                mainPlayerAnimator.SetTrigger("Jump");
            }
        }

        // 4. Detectar el ataque con el Clic Izquierdo del ratón
        if (Input.GetMouseButtonDown(0))
        {
            Atacar();
        }

        // 5. Girar el renderizado del personaje
        GirarPersonaje(movimientoHorizontal);
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(movimientoHorizontal * velocidadMovimiento, rb.linearVelocity.y);
    }

    // Función que lanza un pequeño cuadro hacia abajo para ver si colisiona con el suelo
    private bool ChequearSuelo()
    {
        float distanciaDeteccion = 0.1f;
        RaycastHit2D hit = Physics2D.BoxCast(
            boxCollider.bounds.center,
            boxCollider.bounds.size,
            0f,
            Vector2.down,
            distanciaDeteccion,
            capaSuelo
        );

        return hit.collider != null;
    }

    private void GirarPersonaje(float movimiento)
    {
        if ((movimiento > 0 && !mirandoDerecha) || (movimiento < 0 && mirandoDerecha))
        {
            mirandoDerecha = !mirandoDerecha;
            Vector3 escala = transform.localScale;
            escala.x *= -1;
            transform.localScale = escala;
        }
    }

    private void Atacar()
    {
        if (mainPlayerAnimator != null)
        {
            mainPlayerAnimator.SetTrigger("Attack");
        }
    }

    // DETECCIÓN DE IMPACTO BASADA EN COMPONENTE
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Si el objeto con el que chocamos tiene el componente de la flecha
        if (collision.GetComponent<DestructorFlecha>() != null)
        {
            // Le quitamos una vida al jugador
            if (GameControllerEsquivaflechas.Instancia != null)
            {
                GameControllerEsquivaflechas.Instancia.RestarVida();
            }

            // Iniciamos el parpadeo de color directo en la gráfica
            StartCoroutine(EfectoParpadeoShader());

            // Borramos la flecha de inmediato para que no vuelva a golpearnos en el mismo fotograma
            Destroy(collision.gameObject);
        }
    }

    // CORRUTINA: Cambia los parámetros del material en la GPU
    private IEnumerator EfectoParpadeoShader()
    {
        for (int i = 0; i < cantidadParpadeos; i++)
        {
            spriteRenderer.color = colorDaño; // Aplica el tinte rojo en el shader
            yield return new WaitForSeconds(duracionParpadeo);

            spriteRenderer.color = Color.white; // Restaura el estado original sin tinte
            yield return new WaitForSeconds(duracionParpadeo);
        }
    }
}
