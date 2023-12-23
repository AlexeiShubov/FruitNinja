using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    private BlockFactory _blockFactory;
    private Dictionary<string, Stack<AbstractBlock>> _mapBlocks;
    private Transform _transformParentBlocks;

    public ObjectPool(IEnumerable<PoolingBlockSettings> blockVariants, Transform transformParentBlocks)
    {
        _transformParentBlocks = transformParentBlocks;
        
        _blockFactory = new BlockFactory();
        _mapBlocks = new Dictionary<string, Stack<AbstractBlock>>();
        
        GenerateStartBlocks(blockVariants);
    }

    public Stack<AbstractBlock> GetBlocksPack(IEnumerable<AbstractBlock> neededBlocks)
    {
        var packBlocks = new Stack<AbstractBlock>();

        foreach (var abstractBlock in neededBlocks)
        {
            var key = $"{abstractBlock.name}(Clone)";
            
            if (_mapBlocks[key].Count == 0)
            { 
                packBlocks.Push(_blockFactory.SpawnBlock(abstractBlock, _transformParentBlocks));
                packBlocks.Peek().gameObject.SetActive(false);
                
                continue;
            }
            
            packBlocks.Push(_mapBlocks[key].Pop());
        }

        return packBlocks;
    }

    public void ReturnBlockToPool(AbstractBlock block)
    {
        _mapBlocks[block.name].Push(block);
    }

    private void GenerateStartBlocks(IEnumerable<PoolingBlockSettings> blockVariants)
    {
        foreach (var variant in blockVariants)
        {
            var key = $"{variant.Block.name}(Clone)";
            
            if (!_mapBlocks.ContainsKey(key))
            {
                _mapBlocks.Add(key, new Stack<AbstractBlock>());
            }

            for (var i = 0; i < variant.PoolSize; i++)
            {
                var newBlock = _blockFactory.SpawnBlock(variant.Block, _transformParentBlocks);
            
                newBlock.gameObject.SetActive(false);
                _mapBlocks[key].Push(newBlock);
            }
        }
    }
}