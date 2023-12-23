using UnityEngine;

public interface IScaler
{
    public Vector2 Increase(Vector2 currentScale);
    public Vector2 Decrease(Vector2 currentScale);
}
