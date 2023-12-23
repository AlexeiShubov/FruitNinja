using UnityEngine;

public interface IRotator
{
    public abstract float RotateSpeed { get; }
    public abstract void Rotate();
}
