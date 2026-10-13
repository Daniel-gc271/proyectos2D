using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para detectar el cambio de escena
using TMPro;

public class GameControllerAbejitas : MonoBehaviour
{
    public static GameControllerAbejitas gameControllerAbejitas;

    [Header("Estado del Juego")]
    [SerializeField] private int vidas = 3;
    [SerializeField] private int flechasEsquivadas = 0;

    [Header("Configuración de Tiempos y Spawn")]
    [SerializeField] private GameObject[] objectToSpawn;
    [SerializeField] private float esperaInicialSpawneo = 1f;
    [SerializeField] private float intervaloSpawneo = 3f;
    [SerializeField] private float xSpawn = 6f;
    [SerializeField] private float fuerzaHorizontal = 5f;

    [Header("UI TextMeshPro")]
    private TextMeshProUGUI textoVidas;
    private TextMeshProUGUI textoFlechasEsquivadas;

    [Header("Cambio de Escena")]
    [SerializeField] private string nombreEscenaSiguiente;

    private void Awake()
    {
        if (gameControllerAbejitas == null)
        {
            gameControllerAbejitas = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        // Nos suscribimos al evento de carga de escena
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // Nos desuscribimos para evitar fugas de memoria
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Se ejecuta automáticamente cada vez que se carga una escena
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        VincularUI();
    }

    void Start()
    {
        VincularUI();
        InvokeRepeating("crearBola", esperaInicialSpawneo, intervaloSpawneo);
    }

    void Update()
    {
        ActualizarUI();

        if (Input.GetKeyDown(KeyCode.Q))
        {
            CambiarEscena();
        }
    }

    // Método para encontrar los textos en la nueva escena por su nombre en la jerarquía
    private void VincularUI()
    {
        // Buscamos el objeto de vidas en la escena actual
        GameObject objVidas = GameObject.Find("Cont vidas");
        if (objVidas != null)
        {
            textoVidas = objVidas.GetComponent<TextMeshProUGUI>();
        }

        // Buscamos el objeto de flechas esquivadas en la escena actual
        GameObject objEsquivadas = GameObject.Find("Cont esquivadas");
        if (objEsquivadas != null)
        {
            textoFlechasEsquivadas = objEsquivadas.GetComponent<TextMeshProUGUI>();
        }
    }

    private void CambiarEscena()
    {
        if (!string.IsNullOrEmpty(nombreEscenaSiguiente))
        {
            SceneManager.LoadScene(nombreEscenaSiguiente);
        }
        else
        {
            Debug.LogWarning("No has escrito el nombre de la escena en el Inspector.");
        }
    }

    private void crearBola()
    {
        if (objectToSpawn == null || objectToSpawn.Length == 0) return;

        Vector2 posAleatoria = new Vector2(xSpawn, Random.Range(-4f, 4f));

        GameObject prefabElegido = objectToSpawn[Random.Range(0, objectToSpawn.Length)];
        if (prefabElegido == null) return;

        GameObject nuevaBola = Instantiate(prefabElegido, posAleatoria, Quaternion.identity);

        Rigidbody2D rb = nuevaBola.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = new Vector2(-fuerzaHorizontal, 0f);
        }
    }

    public void PerderVida()
    {
        vidas--;
        if (vidas <= 0)
        {
            vidas = 0;
            Debug.Log("¡Game Over!");
        }
    }

    public void SumarFlechaEsquivada()
    {
        flechasEsquivadas++;
    }

    private void ActualizarUI()
    {
        if (textoVidas != null)
        {
            textoVidas.text = "Vidas: " + vidas;
        }

        if (textoFlechasEsquivadas != null)
        {
            textoFlechasEsquivadas.text = "Esquivadas: " + flechasEsquivadas;
        }
    }
}