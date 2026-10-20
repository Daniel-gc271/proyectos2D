using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GameManagerLaberinto : MonoBehaviour
{
    [SerializeField] private int vidasIniciales = 3;
    private int golpes = 0;
    public static GameManagerLaberinto gameManagerLaberinto;
    private string playerName;
    private Vector2 posicionInicial = new Vector2(-11.53f,-3.49f);
    private TextMeshProUGUI texto_golpes;
    [SerializeField] private TMP_InputField campoNombreUsuario;
    [SerializeField] private GameObject[] delteteAfterMainScreen;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (gameManagerLaberinto == null)
        {
            gameManagerLaberinto = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void incrementarGolpe() { 
        this.golpes++;
        Debug.Log("Has golpeado a la pared #" +golpes + " veces");
    }
    public void startSetup() {
        string playerName = campoNombreUsuario.text.Trim();
        Debug.Log(playerName);
        
        foreach (GameObject objt in delteteAfterMainScreen)
        {
            Destroy(objt,0.2f);
        }

    }
}
