using UnityEngine;
using UnityEngine.InputSystem;

public class RotacionTriangouloPulsacionRaton : MonoBehaviour
{
    [SerializeField] private Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.qKey.wasPressedThisFrame) {
            Debug.Log("rotando");
            animator.SetTrigger("Rotate");
        }
    }
    public void ejecutarSonido()
    {
        Debug.Log("Fernando tienes un parte");
    }
}
