using UnityEngine;

public abstract class SpecialBlock : AbstractBlock
{
    public override IRotator CurrentRotator { get; set; }
    public override IMovable CurrentMovable { get; set; }
    public override ICutting CurrentCutting { get; set; }

    public override void GetRandomSprites()
    {
        _mainSprite.sprite = _scriptableObjectBlocks.MainSpriteBlock[0];
        
        if (_scriptableObjectBlocks.Splat.Length > 0)
        {
            _splat.sprite = _scriptableObjectBlocks.Splat[0];
        }
        
        _shadow.sprite = _scriptableObjectBlocks.Shadow[0];
    }

    public override void SetDefaultsSettingsBlock()
    {
        IsCutting = false;
        transform.localScale = _scriptableObjectBlocks.DefaultBlockScale;
        
        _mainSprite.gameObject.SetActive(true);
        
        if (_splat == null) return;
        
        _splat.gameObject.SetActive(false);
        _splat.transform.parent = transform;
        _splat.transform.localPosition = Vector2.zero;
        _splat.transform.localScale = _scriptableObjectBlocks.DefaultBlockScale;
    }

    public void DoEffect(SpecialBlock specialBlock, BlocksController blocksController)
    {
        specialBlock.GetEffect(blocksController);
    }
    
    public override void SetControllersMono(IMovable newMovable, IRotator newRotator, ICutting newCutting)
    {
        CurrentMovable = newMovable;
        CurrentRotator = newRotator;
        CurrentCutting = new SpecialBlockCutting(this);
    }

    protected virtual void GetEffect(BlocksController blocksController)
    {
        
    }
}
