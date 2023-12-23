using UnityEngine;

public class StandardRotateBlock : IRotator
{
    private readonly float _rotateSpeed;
    private readonly Transform[] _transform;

    public float RotateSpeed => _rotateSpeed * Time.deltaTime;

    public StandardRotateBlock(float rotateSpeed, params Transform[] transform)
    {
        _transform = transform;
        _rotateSpeed = rotateSpeed;
    }

    public void Rotate()
    {
        foreach (var transform in _transform)
        {
            transform.Rotate(0, 0, _rotateSpeed * Time.deltaTime);
        }
    }
}
