using Unity.VisualScripting;
using UnityEngine;

public class JugadorPulsacionRaton : MonoBehaviour
{
    [SerializeField] private CircleCollider2D circleCollider;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip audioClip;
    [SerializeField] private GameControllerPulsacionRaton controlador;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (controlador == null) {
       controlador= GameObject.Find("GameController").GetComponent<GameControllerPulsacionRaton>();

        }
        audioSource = GetComponent<AudioSource>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {

            Vector2 mousePosition = Input.mousePosition;
            Vector2 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

            if (circleCollider ==Physics2D.OverlapPoint(worldPosition))
            {

                Debug.Log("No me golpeaste las bolas"); 
            }
            else
            {
                
                Debug.Log("me golpeaste las bolas");
                Destroy(this.gameObject);
                controlador.incrementarPuntuacion();
            }
            audioSource.PlayOneShot(audioClip, 0.1f);

        }
    }
}
