using UnityEngine;
using UnityEngine.InputSystem;

public class MovimientoBolaEjer09 : MonoBehaviour
{
    [SerializeField] private float movSpeed = 5;
    [SerializeField] private GameManagerejer09 gameManager;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
      animator = this.GetComponent<Animator>();
      spriteRenderer = this.GetComponent<SpriteRenderer>();
      
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {

            animator.SetInteger("movimiento", 6);
            this.transform.Translate(Vector2.up * Time.deltaTime * movSpeed, Space.World);
           

        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            animator.SetInteger("movimiento", 6);
            this.transform.Translate(Vector2.down * Time.deltaTime * movSpeed,Space.World);
            
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            animator.SetInteger("movimiento", 6);
            this.transform.Translate(Vector2.right * Time.deltaTime * movSpeed, Space.World);
            spriteRenderer.flipX = false;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            animator.SetInteger("movimiento", 6);
            this.transform.Translate(Vector2.left * Time.deltaTime * movSpeed, Space.World);
            spriteRenderer.flipX = true;
        }
        else { animator.SetInteger("movimiento", 0); }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("hole")) {
            this.transform.position = new Vector2(0, 0);
            gameManager.agujeroGolpeado();
        }
    }
}
