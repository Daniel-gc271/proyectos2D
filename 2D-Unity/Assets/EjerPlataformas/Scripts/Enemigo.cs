using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemigo : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Verifica si colisionó con el jugador por su Tag
        if (collision.gameObject.CompareTag("Player"))
        {
            MatarJugador(collision.gameObject);
        }
    }

    private void MatarJugador(GameObject jugador)
    {
        Debug.Log("¡El jugador ha muerto!");

        // Opción 1: Destruir el objeto del jugador
        Destroy(jugador);

        // Opción 2: Reiniciar la escena actual (descomenta si prefieres que reinicie)
        // SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Morir()
    {
        Debug.Log("¡El enemigo ha sido derrotado!");
        Destroy(gameObject);
    }
}