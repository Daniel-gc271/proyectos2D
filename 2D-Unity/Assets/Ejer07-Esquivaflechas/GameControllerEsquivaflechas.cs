using UnityEngine;
using TMPro;

public class GameControllerEsquivaflechas : MonoBehaviour
{
    public static GameControllerEsquivaflechas Instancia { get; private set; }

    [Header("Configuración de Interfaz")]
    [SerializeField] private TextMeshProUGUI contadorVidasUI; // Aquí arrastrarás "cont Vidas"

    [Header("Configuración de Partida")]
    [SerializeField] private int vidasIniciales = 3;

    private int vidasActuales;
    private RectTransform rectTextoVidas;
    private bool juegoTerminado = false;

    private void Awake()
    {
        if (Instancia == null) { Instancia = this; }
        else { Destroy(gameObject); }
    }

    void Start()
    {
        vidasActuales = vidasIniciales;

        if (contadorVidasUI != null)
        {
            rectTextoVidas = contadorVidasUI.GetComponent<RectTransform>();
            ConfigurarTextoInicial();
            ActualizarTextoUI();
        }
    }

    private void ConfigurarTextoInicial()
    {
        if (rectTextoVidas == null) return;

        // Mantener la posición exacta que tienes en tu Inspector de la captura
        rectTextoVidas.anchorMin = new Vector2(0.5f, 0.5f);
        rectTextoVidas.anchorMax = new Vector2(0.5f, 0.5f);
        rectTextoVidas.pivot = new Vector2(0.5f, 0.5f);
        rectTextoVidas.anchoredPosition = new Vector2(-270f, -50f);
        rectTextoVidas.sizeDelta = new Vector2(250f, 100f);

        contadorVidasUI.alignment = TextAlignmentOptions.TopLeft;
    }

    private void ActualizarTextoUI()
    {
        if (contadorVidasUI != null && !juegoTerminado)
        {
            // Solo muestra las vidas
            contadorVidasUI.text = "Vidas: " + vidasActuales;
        }
    }

    public void RestarVida()
    {
        if (juegoTerminado) return;

        vidasActuales--;
        ActualizarTextoUI();

        if (vidasActuales <= 0)
        {
            ActivarGameOver();
        }
    }

    private void ActivarGameOver()
    {
        juegoTerminado = true;

        if (contadorVidasUI != null && rectTextoVidas != null)
        {
            rectTextoVidas.anchoredPosition = Vector2.zero;
            contadorVidasUI.alignment = TextAlignmentOptions.Center;
            contadorVidasUI.fontSize = 40;
            contadorVidasUI.color = Color.red;
            contadorVidasUI.text = "GAME OVER";
        }

        Time.timeScale = 0f;
    }
}
