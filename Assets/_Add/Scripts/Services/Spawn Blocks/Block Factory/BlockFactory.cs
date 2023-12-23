using UnityEngine;

public class BlockFactory
{
    public AbstractBlock SpawnBlock(AbstractBlock block, Transform parent)
    {
        var newBlock = Object.Instantiate(block, Vector2.zero, Quaternion.identity, parent);
        newBlock.GetRandomSprites();

        return newBlock;
    }
}
