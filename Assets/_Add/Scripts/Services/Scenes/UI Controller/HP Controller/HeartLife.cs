using UnityEngine;

public class HeartLife : MonoBehaviour
{
    [SerializeField] private Transform _pacMan;

    public void PlayAnim()
    {
        _pacMan.gameObject.SetActive(true);
    }
}
