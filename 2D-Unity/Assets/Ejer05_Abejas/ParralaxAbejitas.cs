using UnityEngine;

public class ParralaxAbejitas : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private float wrapSpeed = 0.1f;
   

   

    void Update()
    {
        // 1. Obtenemos el offset actual en una variable Vector2
        Vector2 currentOffset = meshRenderer.material.mainTextureOffset;

        // 2. Eje Horizontal: Sigue moviéndose solo como un rollo de papel continuo
        currentOffset.x -= wrapSpeed * Time.deltaTime;

        // 3. Eje Vertical: Cambia en función de la tecla arriba/abajo (Vertical)
        float movV = Input.GetAxis("Vertical");
        // Multiplicamos por una velocidad vertical para que sea fluido
        float velocidadVerticalMultiplier = 1.8f;
        currentOffset.y -= movV * velocidadVerticalMultiplier * Time.deltaTime;

        // 4. Devolvemos el Vector2 modificado al material
        meshRenderer.material.mainTextureOffset = currentOffset;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    
}
