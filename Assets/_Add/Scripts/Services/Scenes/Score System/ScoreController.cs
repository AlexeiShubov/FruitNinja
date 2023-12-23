using System.Collections;
using UnityEngine;
using TMPro;

public class ScoreController : IRestarteble
{
    private int START_COMBO_VALUE = 3;
    
    private readonly TextMeshProUGUI _currentScoreText;
    private readonly TextMeshProUGUI _currentBestScoreText;
    
    private readonly int _defaultBonusValue = 0;
    private readonly int _maxBonusValue = 5;
    private readonly int _defaultCuttingBlocksText = 1;
    private readonly float _defaultTimeBonus = 1f;
    
    private readonly ScoreTextPrefab _scoreTextPrefab;
    private readonly IHostCoroutine _hostCoroutine;
    
    private  int _defaultScore;
    private int _currentScore;
    private int _currentBestScore;
    private int _currentBonusValue;
    private int _currentCountCuttingBlocksText;
    private float _currentTimeBonus;
    private bool _bonusIsActive;

    public int CurrentScore => _currentScore;

    public ScoreController(ScoreTextPrefab scoreTextPrefab, TextMeshProUGUI currentScoreText, TextMeshProUGUI currentBestScoreText, IHostCoroutine hostCoroutine)
    {
        _scoreTextPrefab = scoreTextPrefab;
        _currentScoreText = currentScoreText;
        _currentBestScoreText = currentBestScoreText;
        _hostCoroutine = hostCoroutine;

        _currentTimeBonus = _defaultTimeBonus;

        _currentBestScore = PlayerPrefs.GetInt("BestScore");
        _currentBestScoreText.text += $" {_currentBestScore}";
        
        GameEvents.OnCuttingFruitEvent += FruitCutting;
        GameEvents.OnMenuSceneLoad += MenuSceneLoad;
        _hostCoroutine.StartCoroutine(UpdateTextScore());
    }

    private void FruitCutting(int scoreForCutting, Transform fruit)
    {
        CheckBonusScore(fruit);
        
        _currentScore += scoreForCutting * _currentBonusValue;
    }

    private IEnumerator UpdateTextScore()
    {
        while (true)
        {
            if (_defaultScore <= _currentScore)
            {
                _currentScoreText.text = $"{_defaultScore++}";
            }
            
            yield return new WaitForSeconds(0.01f);
            
            if (_currentScore > _currentBestScore)
            {
                _currentBestScoreText.text = $"Лучший: {_currentScore}";
                
                PlayerPrefs.SetInt("BestScore", _currentScore);
            }
        }
    }
    
    private IEnumerator StartBonusTime()
    {
        while (_bonusIsActive)
        {
            _currentTimeBonus -= Time.deltaTime;
        
            yield return null;

            if (_currentTimeBonus > 0) continue;
            
            StopBonus();
        }
    }

    private void CheckBonusScore(Transform fruit)
    {
        _bonusIsActive = true;
        _currentBonusValue = _currentBonusValue + 1 <= _maxBonusValue ? _currentBonusValue + 1 : _currentBonusValue;

        _hostCoroutine.StartCoroutine(StartBonusTime());

        if (_currentBonusValue < START_COMBO_VALUE) return;

        _currentCountCuttingBlocksText++;

        SpawnSeriesPrefab(fruit);

        _currentTimeBonus = _defaultTimeBonus;
    }

    private void StopBonus()
    {
        _bonusIsActive = false;
        _currentTimeBonus = _defaultTimeBonus;
        _currentBonusValue = _defaultBonusValue;
        _currentCountCuttingBlocksText = _defaultCuttingBlocksText;
    }

    private void SpawnSeriesPrefab(Transform fruit)
    {
        _scoreTextPrefab.CountFruitText.text = _currentCountCuttingBlocksText > 4 
            ? $"{_currentCountCuttingBlocksText} фруктов" 
            : $"{_currentCountCuttingBlocksText} фрукта";
        _scoreTextPrefab.X.text = $"x{_currentBonusValue}";
        
        GameObject.Instantiate(_scoreTextPrefab, Random.insideUnitCircle * 1.5f, Quaternion.identity);
    }

    public void RestartGame()
    {
        _currentScore = 0;
        _defaultScore = 0;
        _currentScoreText.text = $"{_currentScore}";
    }

    public void MenuSceneLoad()
    {
        GameEvents.OnCuttingFruitEvent -= FruitCutting;
        GameEvents.OnMenuSceneLoad -= MenuSceneLoad;
    }
}
