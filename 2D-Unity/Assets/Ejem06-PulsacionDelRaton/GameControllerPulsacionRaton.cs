using NUnit.Framework;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GameControllerPulsacionRaton : MonoBehaviour
{
    [SerializeField] private GameObject bola;
    [SerializeField] private TextMeshProUGUI contadorEsfericas;

    [SerializeField] private float esperaInicialSpawneo =1, intervaloSpawneo=3;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InvokeRepeating("crearBola", esperaInicialSpawneo, intervaloSpawneo);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            GameObject[] bolas = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject bola in bolas) {
                Destroy(bola);
                incrementarPuntuacion();
            };
        
        }
    }
    private void crearBola()
    {
        Vector2 posAleatorioa = new Vector2(Random.Range(-8f, 8f), Random.Range(-4f, 4f));
        GameObject nuevaBola = Instantiate(bola);
        nuevaBola.transform.position = posAleatorioa;
    }
    public void incrementarPuntuacion() {
        int puntuacionActual = int.Parse(contadorEsfericas.text);
        puntuacionActual++;
        contadorEsfericas.text= puntuacionActual.ToString();
    
    }
}
