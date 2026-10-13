using UnityEngine;

public class FlechaAbejitas : MonoBehaviour
{
    [SerializeField] private float limiteXIzquierda = -6f;

    void Update()
    {
        // Si la flecha pasa el límite izquierdo, se considera esquivada con éxito
        if (transform.position.x < limiteXIzquierda)
        {
            if (GameControllerAbejitas.gameControllerAbejitas != null)
            {
                GameControllerAbejitas.gameControllerAbejitas.SumarFlechaEsquivada();
            }
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Si choca con el jugador (asegúrate de que tu jugador tenga la etiqueta "Player")
        if (collision.CompareTag("Player"))
        {
            if (GameControllerAbejitas.gameControllerAbejitas != null)
            {
                GameControllerAbejitas.gameControllerAbejitas.PerderVida();
            }
            Destroy(gameObject);
        }
    }
}