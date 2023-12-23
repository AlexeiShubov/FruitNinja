using System.Collections.Generic;
using UnityEngine;

public class Box : SpecialBlock
{
    [SerializeField] private PoolingBlockSettings _poolingBlockSettings;
    
    public override void GetRandomSprites()
    {
        
    }

    public override void SetControllersMono(IMovable newMovable, IRotator newRotator, ICutting newCutting)
    {
        CurrentMovable = newMovable;
        CurrentRotator = newRotator;
        CurrentCutting = new FruitCutting(this, CurrentMovable.CurrentGravity);
    }

    public override void SetDefaultsSettingsBlock()
    {
        IsCutting = false;
        IsFrozen = false;
        
        transform.localScale = _scriptableObjectBlocks.DefaultBlockScale;
        _leftHalf?.StopMove();
        _rightHalf?.StopMove();
        
        SetDefaultParents();
        SetDefaultActiveSelf();
        SetDefaultPosition();
    }

    protected override void GetEffect(BlocksController blocksController)
    {
        var packBlocks = new Stack<AbstractBlock>();

        for (var i = 0; i < _poolingBlockSettings.PoolSize; i++)
        {
            packBlocks.Push(_poolingBlockSettings.Block);
        }

        packBlocks = blocksController.ObjectPool.GetBlocksPack(packBlocks);

        foreach (var block in packBlocks)
        {
            SetBlockSettings(blocksController, block);
            blocksController.ActiveBlocks.Add(block);
        }
    }

    private void SetBlockSettings(BlocksController blocksController, AbstractBlock block)
    {
        block.SetDefaultsSettingsBlock();
        block.gameObject.SetActive(true);
        block.transform.position = new Vector2(transform.position.x, transform.position.y + 2.5f);
        block.SetControllersMono(
            new ParabalMoveblock(8f, GetRandomDirectionMove(CurrentMovable.MoveDirection), 
                new StandardScalerBlock(block.ScriptableObjectBlock.ScaleBlockStep, 
                    block.ScriptableObjectBlock.DefaultBlockScale, 
                    block.ScriptableObjectBlock.MaxBlockScale),
                block.transform),
            new StandardRotateBlock(Random.Range(-300f, 300f), block.TransformMainSprite),
            new FruitCutting(block, 8f));
    }

    private Vector2 GetRandomDirectionMove(Vector2 currentDirection)
    {
        var resultDirection = new Vector2
        {
            x = Random.Range(-3f, 3f),
            y = Random.Range(3f, 6f)
        };

        return resultDirection;
    }
}
