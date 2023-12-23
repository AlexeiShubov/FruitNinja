using UnityEngine;

public class ParabalMoveblock : IMovable
{
    private readonly Transform[] _transform;

    private float _gravityScale;
    private Vector2 _moveDirection;
    private readonly IScaler _currentScaler;

    public float CurrentGravity
    {
        get => _gravityScale;
        set => _gravityScale = value;
    }

    public Vector2 MoveDirection
    {
        get => _moveDirection;
        set => _moveDirection = value;
    }

    public ParabalMoveblock(float gravityScale, Vector2 moveDirection, IScaler scaler, params Transform[] transforms)
    {
        _currentScaler = scaler;
        _gravityScale = gravityScale + moveDirection.y * 0.1f;
        _moveDirection = moveDirection;
        _transform = transforms;
    }

    public void Move()
    {
        foreach (var item in _transform)
        {
            _moveDirection.y -= _gravityScale * Time.deltaTime;
            item.Translate(_moveDirection * Time.deltaTime);

            item.localScale = _moveDirection.y > 0
                ? _currentScaler.Increase(item.localScale)
                : _currentScaler.Decrease(item.localScale);
        }
    }
}
