using Unity.VisualScripting;
using UnityEngine;

public class MovFondo : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private float wrapSpeed = 0.1f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float movFondoH = Input.GetAxisRaw("Horizontal");
        meshRenderer.material.mainTextureOffset += new Vector2(wrapSpeed * movFondoH * Time.deltaTime, 0);
    }
}
