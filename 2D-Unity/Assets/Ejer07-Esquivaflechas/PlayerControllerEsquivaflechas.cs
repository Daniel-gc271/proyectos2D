using UnityEngine;

public class PlayerControllerEsquivaflechas : MonoBehaviour
{
    [SerializeField] private Animator mainPlayerAnimator;

    [Header("Configuración de Movimiento")]
    [SerializeField] private float velocidadMovimiento = 5f;
    [SerializeField] private float fuerzaSalto = 7f;

    [Header("Detección de Suelo")]
    [SerializeField] private LayerMask capaSuelo; // Selecciona aquí la capa de tu suelo

    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    private float movimientoHorizontal;
    private bool mirandoDerecha = true;
    private bool enElSuelo;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
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

        // 2. Capturar el movimiento horizontal
        movimientoHorizontal = Input.GetAxisRaw("Horizontal");

        if (mainPlayerAnimator != null)
        {
            mainPlayerAnimator.SetFloat("Speed", Mathf.Abs(movimientoHorizontal));
        }

        // 3. Detectar el salto (solo si está tocando el suelo)
        if (Input.GetButtonDown("Jump") && enElSuelo)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, fuerzaSalto);

            if (mainPlayerAnimator != null)
            {
                mainPlayerAnimator.SetTrigger("Jump");
            }
        }

        // 4. Girar el renderizado del personaje
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
}
