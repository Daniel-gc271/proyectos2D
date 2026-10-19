using UnityEngine;

public class MovFondo2 : MonoBehaviour
{
    private MeshRenderer meshRenderer;
    [SerializeField] private float wrapSpeed = 0.1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Get the MeshRenderer component attached to this GameObject
        meshRenderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        float movFondoH = Input.GetAxisRaw("Horizontal");

        // Fixed the typo: multiplied movFondoH by Time.deltaTime
        meshRenderer.material.mainTextureOffset += new Vector2(wrapSpeed * movFondoH * Time.deltaTime, 0);
    }
}