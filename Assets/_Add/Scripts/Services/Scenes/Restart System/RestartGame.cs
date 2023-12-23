using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RestartGame : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _currentScoreText;
    [SerializeField] private TextMeshProUGUI _bestScoreText;
    [SerializeField] private PacManPlayButton _restartPacMan;
    [SerializeField] private Image _restartButtonIMG;
    [SerializeField] private Image _blackFon;
    [SerializeField] private Button _menuButton;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Transform _lossPanel;
    [SerializeField] private Transform _startPositionPacMan;
    
    private ScoreController _scoreController;
    private IRestarteble[] _restartebles;

    public void Init(ScoreController scoreController, params IRestarteble[] restartebles)
    {
        _restartebles = restartebles;
        _scoreController = scoreController;

        _restartPacMan.PlayButtonIsDead += DoRestartGame;
        GameEvents.OnMenuSceneLoad += MenuSceneLoad;
        GameEvents.OnSceneClear += GameIsLoss;
    }

    public void ResetPaсMan()
    {
        _restartPacMan.transform.position = _startPositionPacMan.position;
    }

    private void DoRestartGame()
    {
        foreach (var item in _restartebles)
        {
            item.RestartGame();
        }

        StartCoroutine(RestartPanelOff());
    }

    private void GameIsLoss()
    {
        ResetSettingsButton();
        
        _currentScoreText.text = $"{_scoreController.CurrentScore}";
        _bestScoreText.text = $"Лучший: {PlayerPrefs.GetInt("BestScore")}";
        gameObject.SetActive(true);
        _blackFon.gameObject.SetActive(true);
    }

    private void ResetSettingsButton()
    {
        _menuButton.enabled = true;
        _restartButton.enabled = true;
        _restartButton.gameObject.SetActive(true);
        _restartPacMan.gameObject.SetActive(false);
        //_restartPacMan.transform.position = _startPositionPacMan.position;
        _restartButtonIMG.fillAmount = 1f;
    }

    private void MenuSceneLoad()
    {
        _restartPacMan.PlayButtonIsDead -= DoRestartGame;
        GameEvents.OnSceneClear -= GameIsLoss;
        GameEvents.OnMenuSceneLoad -= MenuSceneLoad;
    }

    private IEnumerator RestartPanelOff()
    {
        if(_blackFon.TryGetComponent(out Animator animatorBlackFon))
        {
            animatorBlackFon.CrossFade("BlackFonOff", 0);
        }

        if (_lossPanel.TryGetComponent(out Animator animatorLosePanel))
        {
            animatorLosePanel.CrossFade("RestartPanelOff", 0);
        }

        yield return new WaitForSeconds(animatorBlackFon.GetCurrentAnimatorClipInfo(0).Length);
        
        _blackFon.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
}
