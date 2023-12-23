using System.Collections;
using UnityEngine;

public class Half : MonoBehaviour
{
    [SerializeField] private Vector2 _moveForce;
    [SerializeField] private Transform _sprite;
    
    private IMovable _currentMovable;
    private IRotator _currentRotator;

    public void StartMove(StandardScalerBlock standardScalerBlock, float gravityScale, Vector2 directionMove, float rotateSpeed)
    {
        _currentMovable = new ParabalMoveblock(gravityScale, directionMove * _moveForce, standardScalerBlock, transform);
        _currentRotator = new StandardRotateBlock(rotateSpeed / Time.deltaTime, _sprite);

        StartCoroutine(Move());
    }

    public void StopMove()
    {
        SetDefaultSettings();
        StopCoroutine(Move());
    }

    private IEnumerator Move()
    {
        while (true)
        {
            _currentMovable?.Move();
            _currentRotator?.Rotate();

            yield return null;
        }
    }

    private void SetDefaultSettings()
    {
        _sprite.transform.position = Vector2.zero;
    }
}
