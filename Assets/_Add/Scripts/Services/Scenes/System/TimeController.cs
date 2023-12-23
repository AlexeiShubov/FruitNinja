using System.Collections;
using UnityEngine;

public sealed class TimeController : MonoBehaviour, IRestarteble
{
    [SerializeField] private float _timeToSpawnBlocks;
    [SerializeField] private float _stepTimeToSpawn;
    [SerializeField] private float _minTimeToSpawnBlocks;

    [SerializeField] float _currentTimeToSpawn;
    [SerializeField] bool _gameOnPaused;

    private float _defaultTimeToSpawnBlocks;

    private Randomizer _randomizer;
    private IHostCoroutine _hostCoroutine;

    public void Init(Randomizer randomizer, IHostCoroutine hostCoroutine)
    {
        _randomizer = randomizer;
        _hostCoroutine = hostCoroutine;
        _currentTimeToSpawn = _timeToSpawnBlocks;
        _defaultTimeToSpawnBlocks = _timeToSpawnBlocks;

        _hostCoroutine.StartCoroutine(CheckPause());
        _hostCoroutine.StartCoroutine(TimerForSpawn());

        GameEvents.OnLossGameEvent += (value) => _gameOnPaused = value;
    }
    
    private IEnumerator CheckPause()
    {
        while (true)
        {
            yield return new WaitWhile(() => !_gameOnPaused);

            _hostCoroutine.StopCoroutine(TimerForSpawn());

            yield return new WaitWhile(() => _gameOnPaused);

            _hostCoroutine.StartCoroutine(TimerForSpawn());
        }
    }
    
    private IEnumerator TimerForSpawn()
    {
        while (!_gameOnPaused)
        {
            _currentTimeToSpawn -= Time.deltaTime;
            
            if (_currentTimeToSpawn <= 0)
            {
                _randomizer.GenerateNewRandomSituation();
                
                if (_timeToSpawnBlocks > _minTimeToSpawnBlocks)
                {
                    _timeToSpawnBlocks -= _stepTimeToSpawn;
                }
                
                _currentTimeToSpawn = _timeToSpawnBlocks;
            }

            yield return null;
        }
    }

    public void RestartGame()
    {
        _timeToSpawnBlocks = _defaultTimeToSpawnBlocks;
        _currentTimeToSpawn = _timeToSpawnBlocks;
        _gameOnPaused = false;
    }
}
