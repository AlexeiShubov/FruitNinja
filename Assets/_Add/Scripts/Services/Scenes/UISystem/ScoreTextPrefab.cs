using TMPro;
using UnityEngine;

public class ScoreTextPrefab : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    
    public TextMeshProUGUI CountFruitText;
    public TextMeshProUGUI X;

    private void OnEnable()
    {
        Destroy(gameObject, _animator.GetCurrentAnimatorClipInfo(0).Length);
    }
}
