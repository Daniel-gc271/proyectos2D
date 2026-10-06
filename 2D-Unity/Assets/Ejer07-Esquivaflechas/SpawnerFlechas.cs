using UnityEngine;
using TMPro;

public class SpawnerFlechas : MonoBehaviour
{
    [SerializeField] private GameObject bola;
    [SerializeField] private TextMeshProUGUI contadorEsquivadasUI; // Aquí arrastrarás "cont Bolas"

    [Header("Configuración de Tiempos")]
    [SerializeField] private float esperaInicialSpawneo = 1f;
    [SerializeField] private float intervaloSpawneo = 3f;

    [Header("Configuración de Movimiento")]
    [SerializeField] private float xSpawn = 6f;
    [SerializeField] private float fuerzaHorizontal = 5f;
    [SerializeField] private float limiteXIzquierda = -6f;

    private int flechasBorradas = 0;
    private RectTransform rectTextoEsquivadas;

    void Start()
    {
        if (contadorEsquivadasUI != null)
        {
            rectTextoEsquivadas = contadorEsquivadasUI.GetComponent<RectTransform>();
            ConfigurarTextoInicial();
            ActualizarTextoUI();
        }

        InvokeRepeating("crearBola", esperaInicialSpawneo, intervaloSpawneo);
    }

    private void ConfigurarTextoInicial()
    {
        if (rectTextoEsquivadas == null) return;

        // Lo anclamos en la misma zona que el de vidas pero una línea más abajo
        rectTextoEsquivadas.anchorMin = new Vector2(0.5f, 0.5f);
        rectTextoEsquivadas.anchorMax = new Vector2(0.5f, 0.5f);
        rectTextoEsquivadas.pivot = new Vector2(0.5f, 0.5f);
        rectTextoEsquivadas.anchoredPosition = new Vector2(-270f, -90f); // -90 para que quede justo debajo de Vidas
        rectTextoEsquivadas.sizeDelta = new Vector2(250f, 100f);

        contadorEsquivadasUI.alignment = TextAlignmentOptions.TopLeft;
    }

    private void crearBola()
    {
        if (bola == null) return;

        Vector2 posAleatoria = new Vector2(xSpawn, Random.Range(-4f, 4f));

        GameObject nuevaBola = Instantiate(bola);
        nuevaBola.transform.position = posAleatoria;

        Rigidbody2D rb = nuevaBola.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = new Vector2(-fuerzaHorizontal, 0f);
        }

        DestructorFlecha destructor = nuevaBola.AddComponent<DestructorFlecha>();
        destructor.ConfigurarLimite(limiteXIzquierda, this); // Le pasamos este spawner para que avise al borrar
    }

    public void RegistrarFlechaBorrada()
    {
        flechasBorradas++;
        ActualizarTextoUI();
    }

    private void ActualizarTextoUI()
    {
        if (contadorEsquivadasUI != null)
        {
            contadorEsquivadasUI.text = "Esquivadas: " + flechasBorradas;
        }
    }
}

public class DestructorFlecha : MonoBehaviour
{
    private float xLimite;
    private SpawnerFlechas spawner;

    public void ConfigurarLimite(float limite, SpawnerFlechas scriptSpawner)
    {
        xLimite = limite;
        spawner = scriptSpawner;
    }

    void Update()
    {
        if (transform.position.x <= xLimite)
        {
            if (spawner != null)
            {
                spawner.RegistrarFlechaBorrada();
            }

            Destroy(gameObject);
        }
    }
}
