using UnityEngine;
using TMPro;

public class GameManagerAbejitaConNiveles : MonoBehaviour
{
    public static GameManagerAbejitaConNiveles Instance { get; private set; }

    [Header("Configuración de Jugador")]
    [SerializeField] private TMP_InputField inputNombreJugador; 
    private string nombreJugador = "Abejita"; // Nombre por defecto si no escribe nada

    [Header("Estadísticas del Jugador")]
    [SerializeField] private int vidas = 3;
    [SerializeField] private int esquivadas = 0;
    private int nivel = 1;

    [Header("Referencias UI")]
    [SerializeField] private TextMeshProUGUI textoVidas, textoEsquivadas, textoNiveles;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        ActualizarUI();
    }

    // Método para capturar el texto del InputField antes de guardar o cambiar de escena
    public void GuardarNombreDesdeInput()
    {
        if (inputNombreJugador != null && !string.IsNullOrEmpty(inputNombreJugador.text))
        {
            nombreJugador = inputNombreJugador.text;
        }
    }

    public void PerderVida(int cantidad = 1)
    {
        vidas -= cantidad;
        ActualizarUI();

        if (vidas <= 0)
        {
            GameOver();
        }
    }

    public void SumarEsquivada(int cantidad = 1)
    {
        esquivadas += cantidad;
        ActualizarUI();
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

        // 1. Asegurarnos de tomar el texto actual del InputField
        GuardarNombreDesdeInput();

        // 2. Guardar en el ranking usando el nombre introducido, nivel y esquivadas
        int posicionLograda = Ranking.GuardarPuntuacion(nombreJugador, nivel, esquivadas);

        if (posicionLograda != -1)
        {
            Debug.Log("¡Nuevo récord para " + nombreJugador + " en el puesto " + (posicionLograda + 1) + "!");
        }
        else
        {
            Debug.Log(nombreJugador + " no ha entrado en el Top 5.");
        }

        // Aquí puedes cargar la escena de fin de juego o mostrar el panel de puntuaciones
    }
}