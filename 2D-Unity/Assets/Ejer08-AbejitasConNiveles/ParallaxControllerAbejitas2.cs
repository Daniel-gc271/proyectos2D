using UnityEngine;

public class ParallaxControllerAbejitas2 : MonoBehaviour
{
    private Material mat;
    public float parallaxEffect = 0.5f;

    void Start()
    {
        // Obtenemos el material del componente Renderer (funciona tanto para Cubos/Quads como para Sprites)
        mat = GetComponent<Renderer>().material;
        if (mat == null) Debug.LogWarning("No se ha encontrado el material");
    }

    void Update()
    {
        float verticalInput = Input.GetAxis("Vertical");

        if (mat != null)
        {
            // Modificamos el offset de forma directa y limpia usando mainTextureOffset
            Vector2 offset = mat.mainTextureOffset;
            offset.y += parallaxEffect * verticalInput * Time.deltaTime;
            mat.mainTextureOffset = offset;
        }
    }
}