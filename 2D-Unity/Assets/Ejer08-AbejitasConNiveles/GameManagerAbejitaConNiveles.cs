using UnityEngine;
using TMPro; // Asegúrate de incluir esto si usas TextMeshPro para los textos en pantalla

public class GameManagerAbejitaConNiveles : MonoBehaviour
{
    // Instancia estática para el patrón Singleton (para que otros scripts accedan con GameManagerAbejitaConNiveles.Instance)
    public static GameManagerAbejitaConNiveles Instance { get; private set; }

    [Header("Configuración de Persistencia")]
    [SerializeField] private GameObject[] dontDestroyObjects; // Objetos que no se destruyen al cambiar de escena

    [Header("Estadísticas del Jugador")]
    [SerializeField] private int vidas = 3;
    [SerializeField] private int esquivadas = 0;
    private int nivel = 1;

    [Header("Referencias UI (Opcional)")]
    [SerializeField] private TextMeshProUGUI textoVidas, textoEsquivadas, textoNiveles;
    

    void Awake()
    {
        // Patrón Singleton: Asegurar que solo exista un GameManager
        if (Instance == null)
        {
            Instance = this;

            // Si quieres que este GameObject persista entre escenas:
            DontDestroyOnLoad(gameObject);

            // Si tienes un array de objetos adicionales que deben persistir:
            foreach (GameObject obj in dontDestroyObjects)
            {
                if (obj != null) DontDestroyOnLoad(obj);
            }
        }
        else
        {
            // Si ya existe otro GameManager, destruimos este duplicado para evitar conflictos
            Destroy(gameObject);
        }
    }

    void Start()
    {
        ActualizarUI();
    }

    // Métodos públicos para que otros scripts (abeja, flechas, etc.) puedan modificar vidas o esquives

    public void PerderVida(int cantidad = 1)
    {
        vidas -= cantidad;
        ActualizarUI();
        Debug.Log("Vida perdida. Vidas restantes: " + vidas);

        if (vidas <= 0)
        {
            GameOver();
        }
    }

    public void SumarEsquivada(int cantidad = 1)
    {
        esquivadas += cantidad;
        ActualizarUI();
        Debug.Log("Flecha esquivada. Total esquivadas: " + esquivadas);
    }

    private void ActualizarUI()
    {
        if (textoVidas != null) textoVidas.text = "Vidas: " + vidas;
        if (textoEsquivadas != null) textoEsquivadas.text = "Esquivadas: " + esquivadas;
        if (textoNiveles != null) textoNiveles.text = "Nivel: " + nivel;
    }

    private void GameOver()
    {
        Debug.Log("¡Game Over!");
        // Aquí puedes cargar la escena de Game Over o mostrar un panel
    }
}