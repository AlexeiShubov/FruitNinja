using UnityEngine;

public class PlayerController : IRestarteble
{
    private const float MIN_CUTTING_VELOCITY = 0.2f;
    
    private readonly Transform _player;
    private readonly Transform _trail;
    private readonly Camera _mainCamera;
    
    private bool _gameOnPaused;
    private Vector2 _previousPosition;
    
    public bool CanBeCutting { get; private set; }
    public Transform Trail => _trail;

    public PlayerController(Camera mainCamera, Transform player, Transform trail)
    {
        _mainCamera = mainCamera;
        _player = player;
        _trail = trail;

        GameEvents.OnLossGameEvent += (value) => _gameOnPaused = value;
    }

    public void InputUpdate()
    {
        if (_gameOnPaused)
        {
            _trail.gameObject.SetActive(false);
            
            return;
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            _previousPosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            _trail.gameObject.SetActive(true);
        }

        if (Input.GetMouseButton(0))
        {
            Cutting();
        }

        if (Input.GetMouseButtonUp(0))
        {
            _trail.gameObject.SetActive(false);
        }
    }
    
    public void RestartGame()
    {
        _gameOnPaused = false;
    }

    private void Cutting()
    {
        Vector2 currentCameraPos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);

        _player.position = currentCameraPos;

        Vector2 _currentPosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);

        var speedCutting = (_currentPosition - _previousPosition).magnitude / Time.deltaTime;

        CanBeCutting = speedCutting > MIN_CUTTING_VELOCITY / Time.deltaTime;
        
        _previousPosition = _currentPosition;
    }

    public void BlockSwipe()
    {
        CanBeCutting = false;
        _trail.gameObject.SetActive(false);
    }
}