using UnityEngine;

public class CursorController : MonoBehaviour
{
    [SerializeField] private Camera _main;
    [SerializeField] private Transform _cursor;
    [SerializeField] private ParticleSystem _tap;

    public void InputControllerUpdate()
    {
        if (!Input.GetMouseButtonDown(0)) return;
        
        _cursor.position = (Vector2)_main.ScreenToWorldPoint(Input.mousePosition);
        _tap.Play();
    }
}
