using UnityEngine;

public class MovimientoAbejas : MonoBehaviour
{
    [SerializeField] private float velocidadEggman = 1.8f;
    private Rigidbody2D rb; // Cambia a Rigidbody2D si tu juego es 2D
    private float movV;

    void Start()
    {
        // Obtenemos el componente Rigidbody del objeto
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Leemos la entrada del usuario en el Update
        movV = Input.GetAxis("Vertical");
    }

    void FixedUpdate()
    {
        // Aplicamos la velocidad respetando las físicas de Unity 6
        Vector3 velocidadActual = rb.linearVelocity; 
        velocidadActual.y = movV * velocidadEggman;
        rb.linearVelocity = velocidadActual;
    }
}
