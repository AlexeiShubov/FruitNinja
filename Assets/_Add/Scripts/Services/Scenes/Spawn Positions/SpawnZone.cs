using System;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class SpawnZone
{
    [Header("Zone Priority")]
    [SerializeField] [Range(0, 100)] private int _zonePriority;
    [Space] 
    [SerializeField] [Range(-3, 3)] private float _minZonePositionX;
    [SerializeField] [Range(-3, 3)] private float _minZonePositionY;
    [SerializeField] [Range(-3, 3)] private float _maxZonePositionX;
    [SerializeField] [Range(-3, 3)] private float _maxZonePositionY;

    [Space] [Header("Start Settings Move")] 
    [SerializeField] private Vector2 _startMoveSpeedBlock;
    [SerializeField] private float _rotationSpeed;

    private Vector2 _screenSize;
    private Vector2 _minPositionPoint;
    private Vector2 _maxPositionPoint;
    private Vector2 _startPosition;

    public int ZonePriority => _zonePriority;
    public float RotationSpeed => _rotationSpeed;
    public Vector2 StartMoveSpeedBlock => _startMoveSpeedBlock;

    #region For DrowLine

#if UNITY_EDITOR
    public Vector2 MinPositionSpawnPoint()
    {
        return GetPosition(_screenSize, _minZonePositionX, _minZonePositionY);
    }

    public Vector2 MaxPositionSpawnPoint()
    {
        return GetPosition(_screenSize, _maxZonePositionX, _maxZonePositionY);
    }
#endif

    #endregion

    public void AcceptStartSettings(Vector2 screenSize)
    {
        _screenSize = screenSize;
        _minPositionPoint = GetPosition(_screenSize, _minZonePositionX, _minZonePositionY);
        _maxPositionPoint = GetPosition(_screenSize, _maxZonePositionX, _maxZonePositionY);

        _minPositionPoint.x = _minPositionPoint.x < _screenSize.x * 0.15f + _screenSize.x ? _screenSize.x * 0.15f + _screenSize.x : _minPositionPoint.x;
        _minPositionPoint.x = _minPositionPoint.x > -_screenSize.x * 0.15f - _screenSize.x ? -_screenSize.x * 0.15f - _screenSize.x : _minPositionPoint.x;
        _maxPositionPoint.x = _maxPositionPoint.x < _screenSize.x * 0.15f + _screenSize.x ? _screenSize.x * 0.15f + _screenSize.x : _maxPositionPoint.x;
        _maxPositionPoint.x = _maxPositionPoint.x > -_screenSize.x * 0.15f - _screenSize.x ? -_screenSize.x * 0.15f - _screenSize.x : _maxPositionPoint.x;

        _minPositionPoint.y = _minPositionPoint.y < _screenSize.y * 0.2f + _screenSize.y ? _screenSize.y * 0.2f + _screenSize.y : _minPositionPoint.y;
        _minPositionPoint.y = _minPositionPoint.y > -_screenSize.y * 0.2f - _screenSize.y ? -_screenSize.y * 0.2f - _screenSize.y : _minPositionPoint.y;
        _maxPositionPoint.y = _maxPositionPoint.y < _screenSize.y * 0.2f + _screenSize.y ? _screenSize.y * 0.2f + _screenSize.y : _maxPositionPoint.y;
        _maxPositionPoint.y = _maxPositionPoint.y > -_screenSize.y * 0.2f - _screenSize.y ? -_screenSize.y * 0.2f - _screenSize.y : _maxPositionPoint.y;
    }

    public Vector2 GetRandomSpawnPosition()
    {
        _startPosition = new Vector2(Random.Range(_minPositionPoint.x, _maxPositionPoint.x),
            Random.Range(_minPositionPoint.y, _maxPositionPoint.y));

        return _startPosition;
    }
    
    public Vector2 GetDirectionMove()
    {
        var directionMove = _startMoveSpeedBlock * -_startPosition;
        
        return directionMove;
    }

    private Vector2 GetPosition(Vector2 screenSize, float zonePositionX, float zonePositionY)
    {
        return new Vector2(screenSize.x + zonePositionX * screenSize.x,
            screenSize.y + zonePositionY * screenSize.y);
    }
}
