using UnityEngine;
using UnityEngine.InputSystem;

public class MovimientoPersonajeLaberinto : MonoBehaviour
{
    [SerializeField] private float movSpeed = 1f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float movV= Input.GetAxis("Vertical");
        float movH = Input.GetAxis("Horizontal");
        this.transform.Translate(new Vector2(movH, movV).normalized * movSpeed * Time.deltaTime);
    }
}
