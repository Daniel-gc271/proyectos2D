using UnityEngine;

public class ParallaxAbejaConNivelesRandom : MonoBehaviour
{
    private MeshRenderer meshRenderer;
    [SerializeField] private float minSpeed = 0.05f;
    [SerializeField] private float maxSpeed = 0.3f;
    [SerializeField] private float cambioIntervalo = 3f; // Tiempo en segundos para cambiar la velocidad

    private float currentSpeed;
    private float targetSpeed;
    private float timer;
    private bool enabled;
    void Start()
    {
        enable();
        meshRenderer = GetComponent<MeshRenderer>();
        // Asignamos una velocidad aleatoria inicial
        targetSpeed = Random.Range(minSpeed, maxSpeed);
        currentSpeed = targetSpeed;
    }

    void Update()
    {
        if (enabled) {

            // Temporizador para cambiar la velocidad aleatoriamente cada cierto tiempo
            timer += Time.deltaTime;
            if (timer >= cambioIntervalo)
            {
                // Elegimos una nueva velocidad aleatoria (positiva o negativa para cambiar de dirección)
                targetSpeed = Random.Range(minSpeed, maxSpeed) * (Random.value > 0.5f ? 1f : -1f);
                timer = 0f;
            }

            // Suavizamos la transición de la velocidad actual a la objetivo
            currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, Time.deltaTime);

            // Movemos la textura del material
            meshRenderer.material.mainTextureOffset += new Vector2(currentSpeed * Time.deltaTime, 0);
        }
        
    }
    public void enable() { this.enabled = true; }
    public void disable() { this.enabled = false; }
}