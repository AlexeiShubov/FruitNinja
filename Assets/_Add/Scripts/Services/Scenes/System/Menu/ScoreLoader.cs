using UnityEngine;
using TMPro;

public class ScoreLoader : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _score;

    private void Awake()
    {
        LoadScore();
    }

    private void LoadScore()
    {
        _score.text = PlayerPrefs.GetInt("BestScore").ToString();
    }
}
