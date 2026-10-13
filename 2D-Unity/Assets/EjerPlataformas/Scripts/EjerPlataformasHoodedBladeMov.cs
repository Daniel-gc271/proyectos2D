using UnityEngine;

public class EjerPlataformasHoodedBladeMov : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigidbody;
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float fuerzaSalto = 10f;
    [SerializeField] private LayerMask capaSuelo;
    [SerializeField] private float velocidadMovimiento = 5f;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip sonidoAterrizaje;
    [SerializeField] private AudioClip sonidoSalto;

    private bool isgrounded = true;

    void Update()
    {
        float mov = Input.GetAxis("Horizontal");
        rigidbody.linearVelocity = new Vector2(mov * velocidadMovimiento, rigidbody.linearVelocity.y);
        if (mov < 0) {
            Debug.Log("move left");
            spriteRenderer.flipX = true;


        }
        if (mov > 0)
        {
            Debug.Log("move right");
            spriteRenderer.flipX = false;
        }
        this.animator.SetBool("isRunning", mov != 0);
        // Al usar ForceMode2D.Impulse, el momento horizontal NO se altera en absoluto
        if (Input.GetKeyDown(KeyCode.Space) && isgrounded)
        {
            rigidbody.AddForce(Vector2.up * fuerzaSalto, ForceMode2D.Impulse);
            isgrounded = false;
        }

        // Pruebas manuales con Q y E (Opcional, las mantengo por tu script)
        if (Input.GetKeyDown(KeyCode.Q) && audioSource != null && sonidoAterrizaje != null)
        {
            audioSource.PlayOneShot(sonidoAterrizaje);
        }
        if (Input.GetKeyDown(KeyCode.E) && audioSource != null && sonidoSalto != null)
        {
            audioSource.PlayOneShot(sonidoSalto);
        }
    }

    // CORRECCIÓN: Cambiado a Collision2D porque estás usando Rigidbody2D
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & capaSuelo) != 0)
        {
            isgrounded = true;
            this.animator.SetBool("isGrounded",true);
            if (audioSource != null && sonidoAterrizaje != null)
            {
                audioSource.PlayOneShot(sonidoAterrizaje,0.2f);
            }
        }
    }

    // CORRECCIÓN: Cambiado a Collision2D y añadida la comprobación de velocidad de salto
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & capaSuelo) != 0)
        {
            isgrounded = false;
            this.animator.SetBool("isGrounded", false);
            // Solo suena el audio de salto si el jugador realmente va hacia arriba
            if (rigidbody.linearVelocity.y > 0.1f)
            {
                
                if (audioSource != null && sonidoSalto != null)
                {
                    audioSource.PlayOneShot(sonidoSalto);
                }
            }
        }
    }
}
