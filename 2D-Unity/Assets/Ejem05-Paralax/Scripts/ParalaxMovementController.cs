using Unity.VisualScripting;
using UnityEngine;

public class ParalaxMovementController : MonoBehaviour
{
    [SerializeField] private float movSpeed = 5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float movBolaH = Input.GetAxisRaw("Horizontal");
        this.transform.Translate(Vector2.right * movSpeed * Time.deltaTime * movBolaH);
    }
}
