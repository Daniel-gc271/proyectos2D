using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerejer09 : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI contadorGolpes;
    [SerializeField] private int textSpeed;
    private int agujerosGolpeados = 0;
    private bool isGameOver = false;
    public void agujeroGolpeado()
    {
        agujerosGolpeados++;
        contadorGolpes.text = "Agujeros caidos: " + agujerosGolpeados + "/4\n";
        Debug.Log("Agujero golpeasdo");
        if (agujerosGolpeados > 4) {
            gameOver();
        }
    }

    private void gameOver()
    {
        contadorGolpes.rectTransform.position = new Vector2(0, -200);
        contadorGolpes.text = "Has caido en: " + agujerosGolpeados + " agujeros.\nHas perdido!"; 
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        contadorGolpes.text = "Agujeros caidos: " + agujerosGolpeados + "/4\n";
        DontDestroyOnLoad(this);
        contadorGolpes.rectTransform.position = new Vector2(0, 128);
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameOver&& contadorGolpes.rectTransform.position.y<140) { contadorGolpes.transform.Translate(Vector2.up * Time.deltaTime * textSpeed); }
    }
}
