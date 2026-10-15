using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameControllerAbejitas : MonoBehaviour
{
    public static GameControllerAbejitas gameControllerAbejitas;
    private bool isGameOver = false;

    private GameObject objEsquivadas, objVidas;

    [Header("Estado del Juego")]
    [SerializeField] private int vidas = 3;
    [SerializeField] private int flechasEsquivadas = 0;
    [SerializeField] private float escaladoVelocidad = 0.4f;

    [Header("Configuración de Tiempos y Spawn")]
    [SerializeField] private GameObject[] objectToSpawn;
    [SerializeField] private float esperaInicialSpawneo = 1f;
    [SerializeField] private float intervaloSpawneo = 3f;
    [SerializeField] private float xSpawn = 6f;
    [SerializeField] private float fuerzaHorizontal = 5f;

    [Header("UI TextMeshPro")]
    private TextMeshProUGUI textoVidas;
    private TextMeshProUGUI textoFlechasEsquivadas;
    private TextMeshProUGUI textoGameOver;

    [Header("Cambio de Escena")]
    [SerializeField] private string nombreEscenaSiguiente;

    [SerializeField] private AudioClip sonidoGameOver;
    [SerializeField] private AudioClip sonidoFondo;
    [SerializeField] private AudioClip sonidoImpacto;
    [SerializeField] private GameObject objetoJugador;

    private void Awake()
    {
        if (gameControllerAbejitas == null)
        {
            gameControllerAbejitas = this;
            DontDestroyOnLoad(this.gameObject);

            AudioSource audioSource = GetComponent<AudioSource>();
            if (audioSource != null && sonidoFondo != null)
            {
                audioSource.clip = sonidoFondo;
                audioSource.loop = true;
                audioSource.volume = 0.2f;
                audioSource.Play();
            }
        }
        else
        {
            Destroy(this.gameObject);
        }
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

    // El método ahora es público para poder llamarlo si es necesario, pero es simple y directo
    public void VincularUI()
    {
        objVidas = GameObject.Find("Cont vidas");
        if (objVidas != null)
        {
            textoVidas = objVidas.GetComponent<TextMeshProUGUI>();
        }

        objEsquivadas = GameObject.Find("Cont esquivadas");
        if (objEsquivadas != null)
        {
            textoFlechasEsquivadas = objEsquivadas.GetComponent<TextMeshProUGUI>();
        }

        GameObject objGameOver = GameObject.Find("Text game over");
        if (objGameOver != null)
        {
            textoGameOver = objGameOver.GetComponent<TextMeshProUGUI>();
            textoGameOver.text = "";
        }
    }

    private void CambiarEscena()
    {
        if (!string.IsNullOrEmpty(nombreEscenaSiguiente))
        {
            fuerzaHorizontal += escaladoVelocidad; // Aumenta la velocidad de las flechas al cambiar de escena
            // Cambia de escena de manera síncrona tradicional
            SceneManager.LoadScene(nombreEscenaSiguiente);
        }
        else
        {
            Debug.LogWarning("No has escrito el nombre de la escena en el Inspector.");
        }
    }

    private void crearBola()
    {
        if (objectToSpawn == null || objectToSpawn.Length == 0 || isGameOver) return;

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
        if (isGameOver) return;

        vidas--;
        AudioSource audioSource = GetComponent<AudioSource>();

        if (audioSource != null && sonidoImpacto != null)
        {
            audioSource.clip = sonidoImpacto;
            audioSource.loop = false;
            audioSource.volume = 0.2f;
            audioSource.Play();
        }

        if (vidas <= 0)
        {
            vidas = 0;
            isGameOver = true;
            Debug.Log("¡Game Over!");

            if (audioSource != null && sonidoGameOver != null)
            {
                audioSource.clip = sonidoGameOver;
                audioSource.loop = false;
                audioSource.volume = 0.2f;
                audioSource.Play();
            }

            if (objetoJugador != null)
            {
                Destroy(objetoJugador);
            }

            if (textoVidas != null) textoVidas.text = "";
            if (textoFlechasEsquivadas != null) textoFlechasEsquivadas.text = "";

            if (textoGameOver != null)
            {
                textoGameOver.text = "¡Game Over!";
            }
        }
    }

    public void SumarFlechaEsquivada()
    {
        if (!isGameOver)
        {
            flechasEsquivadas++;

            // Si llega a 10 flechas, cambia automáticamente de nivel manteniendo el contador intacto
            if (flechasEsquivadas >= 10)
            {
                CambiarEscena();
            }
        }
    }

    private void ActualizarUI()
    {
        if (isGameOver) return;

        // Comprobación dinámica: Si los textos no existen (porque cambiamos de escena), los busca automáticamente
        if (textoVidas == null || textoFlechasEsquivadas == null)
        {
            VincularUI();
        }

        // Asignación segura de textos en pantalla
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
