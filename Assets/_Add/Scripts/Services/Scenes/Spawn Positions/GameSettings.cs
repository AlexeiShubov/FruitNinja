using UnityEngine;

#if UNITY_EDITOR
    [ExecuteAlways]
#endif

public sealed class GameSettings : MonoBehaviour
{
    [Space]
    [Header("Min and Max Count Blocks For Spawn In One Time")]
    [SerializeField][Min(2)] private int _minCount;
    [SerializeField][Min(2)] private int _maxCount;
    [Space]
    [SerializeField] private float _timeBetweenBlocksLaunch;
    [Space]
    [SerializeField] private PoolingBlockSettings[] _blockVariants;
    [Space] 
    [SerializeField] private float _gravityScale;
    [Space]
    [Header("Spawn Zones Settings")]
    [SerializeField] private SpawnZone[] _spawnZones;

    public int MinCountSpawnBlocksInOneTime => _minCount;
    public int MaxCountSpawnBlocksInOneTime => _maxCount;
    public float TimeBetweenBlocksLaunch => _timeBetweenBlocksLaunch;
    public float GravityScale => _gravityScale;
    
    public PoolingBlockSettings[] BlockVariants => _blockVariants;
    public SpawnZone[] SpawnZones => _spawnZones;

    public void Init(Vector2 screenSize)
    {
        InitSpawnZones(screenSize);
    }

    private void InitSpawnZones(Vector2 screenSize)
    {
        foreach (var zone in _spawnZones)
        {
            zone.AcceptStartSettings(screenSize);
        }
    }

    #region Drow Line
#if UNITY_EDITOR
    private void Update()
    {
        Init(Camera.main.ScreenToWorldPoint(Vector2.zero));
        
        foreach (var zone in _spawnZones)
        {
            Debug.DrawLine(zone.MinPositionSpawnPoint(), zone.MaxPositionSpawnPoint());
        }
    }
#endif
    #endregion
}
