using System.Collections;
using UnityEngine;

public interface IHostCoroutine
{
    public abstract Coroutine StartCoroutine(IEnumerator routine);

    public abstract void StopCoroutine(IEnumerator routine);
}
