using System;
using UnityEngine;

[Serializable]
public class PoolingBlockSettings
{
    [Header("Spawn Block Priority")] 
    [SerializeField] [Range(0, 100)] private int _priority;
    [Space] 
    [SerializeField] private AbstractBlock _block;
    [SerializeField] private int _poolSize;

    public int Priority => _priority;
    public int PoolSize => _poolSize;
    public AbstractBlock Block => _block;
}
