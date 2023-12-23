using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Randomizer : IRestarteble
{
    private const float SESSION_STEP_DEFAULT = 0.1f;
    private const int MAX_CONT_BOMBS_IN_PACK = 2;
    private readonly int _maxPriorityBlock;
    private readonly int _maxPriorityZone;

    private readonly ObjectPool _objectPool;
    private readonly BlocksController _blocksController;
    private readonly GameSettings _gameSettings;
    
    private readonly Dictionary<int, List<AbstractBlock>> _priorityBlocksMap;
    private readonly Dictionary<int, List<SpawnZone>> _priorityZonesMap;
    
    private float _currentSessionStep;

    private GameDifficultyController _gameDifficultyController;
    private Stack<AbstractBlock> _currentBlockPack;

    public Randomizer(ObjectPool objectPool, BlocksController blocksController, GameSettings gameSettings)
    {
        _objectPool = objectPool;
        _blocksController = blocksController;
        _gameSettings = gameSettings;
        _currentSessionStep = SESSION_STEP_DEFAULT;

        _priorityBlocksMap = GeneratePriorityBlocksMap(_gameSettings.BlockVariants);
        _priorityZonesMap = GeneratePrioritySpawnZonesMap(_gameSettings.SpawnZones);

        _maxPriorityBlock = _priorityBlocksMap.Keys.Max();
        _maxPriorityZone = _priorityZonesMap.Keys.Max();
    }

    public void GenerateNewRandomSituation()
    {
        _currentSessionStep += Time.deltaTime;
        _currentBlockPack = _objectPool.GetBlocksPack(GetBlocksPack());
        _gameDifficultyController = new GameDifficultyController(GetZones(_priorityZonesMap, _currentBlockPack.Count), _currentSessionStep);

        _blocksController.ProcessingNewBlocks(_currentBlockPack,
            _gameDifficultyController.SpawnZones,
            _gameDifficultyController.RotateSpeeds,
            _gameDifficultyController.MoveSpeedPack);
    }
    
    private Stack<AbstractBlock> GetBlocksPack()
    {
        var blocksPack = new Stack<AbstractBlock>();

        var neededCountBlocks = Random.Range(_gameSettings.MinCountSpawnBlocksInOneTime, _gameSettings.MaxCountSpawnBlocksInOneTime);
        var candidateBlocks = GetCandidateBlocks(_priorityBlocksMap, neededCountBlocks);

        while (blocksPack.Count < neededCountBlocks)
        {
            blocksPack.Push(candidateBlocks[Random.Range(0, candidateBlocks.Count)]);
        }

        return blocksPack;
    }
    
    private Stack<SpawnZone> GetZones(Dictionary<int, List<SpawnZone>> priorityZonesMap, int countBlocks)
    {
        var zones = new Stack<SpawnZone>();

        while (zones.Count < countBlocks)
        {
            var priority = Random.Range(0, _maxPriorityZone);

            foreach (var item in priorityZonesMap)
            {
                if (!(item.Key >= priority)) continue;

                zones.Push(item.Value[Random.Range(0, item.Value.Count)]);

                if (zones.Count == countBlocks)
                {
                    return zones;
                }
            }
        }

        return zones;
    }

    private Dictionary<int, List<AbstractBlock>> GeneratePriorityBlocksMap(PoolingBlockSettings[] blocks)
    {
        var priorityBlocksMap = new Dictionary<int, List<AbstractBlock>>();

        foreach (var item in blocks)
        {
            if (!priorityBlocksMap.ContainsKey(item.Priority))
            {
                priorityBlocksMap.Add(item.Priority, new List<AbstractBlock>());
            }

            priorityBlocksMap[item.Priority].Add(item.Block);
        }

        return priorityBlocksMap;
    }
    
    private Dictionary<int, List<SpawnZone>> GeneratePrioritySpawnZonesMap(IEnumerable<SpawnZone> spawnZones)
    {
        var priorityZonesMap = new Dictionary<int, List<SpawnZone>>();

        foreach (var item in spawnZones)
        {
            if (!priorityZonesMap.ContainsKey(item.ZonePriority))
            {
                priorityZonesMap.Add(item.ZonePriority, new List<SpawnZone>());
            }

            priorityZonesMap[item.ZonePriority].Add(item);
        }

        return priorityZonesMap;
    }

    private List<AbstractBlock> GetCandidateBlocks(Dictionary<int, List<AbstractBlock>> priorityBlocksMap, int countCandidates)
    {
        var candidateBlocks = new List<AbstractBlock>(){priorityBlocksMap[_maxPriorityBlock][Random.Range(0, priorityBlocksMap[_maxPriorityBlock].Count)]};
        var countBombs = 0;

        while (candidateBlocks.Count < countCandidates)
        {
            var priority = Random.Range(0, _maxPriorityBlock);
            
            foreach (var item in priorityBlocksMap)
            {
                if (item.Key < priority) continue;

                var block = item.Value[Random.Range(0, item.Value.Count)];

                if (block is Bomb && countBombs < MAX_CONT_BOMBS_IN_PACK)
                {
                    candidateBlocks.Add(block);

                    countBombs++;
                    
                    if (candidateBlocks.Count == countCandidates)
                    {
                        return candidateBlocks;
                    }
                    
                    continue;
                }

                candidateBlocks.Add(item.Value[Random.Range(0, item.Value.Count)]);

                if (candidateBlocks.Count == countCandidates)
                {
                    return candidateBlocks;
                }
            }
        }

        return candidateBlocks;
    }

    public void RestartGame()
    {
        _currentSessionStep = SESSION_STEP_DEFAULT;
    }
}
