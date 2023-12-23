using System;
using UnityEngine;
using TMPro;

public sealed class GameInitialize : MonoBehaviour, IHostCoroutine
{
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private GameSettings _gameSettings;
    [SerializeField] private TimeController _timeController;
    [Space]
    [SerializeField] private Transform _playerPrefab;
    [SerializeField] private Transform _trail;

    [Space] 
    [Header("UI System")] 
    [SerializeField] private ScoreTextPrefab _scoreTextPrefab;
    [SerializeField] private TextMeshProUGUI _currentScoreText;
    [SerializeField] private TextMeshProUGUI _currentBestScore;
    [SerializeField] private HPController _hpController;
    [SerializeField] private RestartGame _restartGame;
    [SerializeField] private Transform _iceBG;

    private Vector2 _screenSize;
    private Randomizer _randomizer;
    private BlocksController _blocksController;
    private PlayerController _playerController;
    private ObjectPool _objectPool;
    private ScoreController _scoreController;

    private void Awake()
    {
        _screenSize = _mainCamera.ScreenToWorldPoint(Vector2.zero);

        _hpController.Init();
        _gameSettings.Init(_screenSize);
        _objectPool = new ObjectPool(_gameSettings.BlockVariants, transform);
        
        _playerController = new PlayerController(_mainCamera, _playerPrefab, _trail);
        _scoreController = new ScoreController(_scoreTextPrefab, _currentScoreText, _currentBestScore,this);
        _blocksController = new BlocksController(_playerController, _objectPool, _gameSettings,_screenSize, _iceBG, this);
        _randomizer = new Randomizer(_objectPool, _blocksController, _gameSettings);
        _timeController.Init(_randomizer, this);
        
        _restartGame.Init(_scoreController, _randomizer, _timeController, _playerController, _scoreController, _hpController, _blocksController);
        GameEvents.OnBlockSwipe += _playerController.BlockSwipe;
    }

    private void Update()
    {
        _playerController.InputUpdate();
    }

    private void OnDisable()
    {
        GameEvents.OnBlockSwipe -= _playerController.BlockSwipe;
    }
}
