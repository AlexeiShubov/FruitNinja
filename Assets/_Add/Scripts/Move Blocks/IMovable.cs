using UnityEngine;

public interface IMovable
{
    public abstract Vector2 MoveDirection { get; set; }
    public abstract float CurrentGravity { get; set; }

    public abstract void Move();
}
