public class SpecialBlockCutting : ICutting
{
    private readonly SpecialBlock _specialBlock;

    public SpecialBlockCutting(SpecialBlock specialBlock)
    {
        _specialBlock = specialBlock;
    }

    public virtual void Cut()
    {
        _specialBlock.TransformMainSprite.gameObject.SetActive(false);

        _specialBlock.TransformSplat.gameObject.SetActive(true);

        if (_specialBlock.IsCutting) return;

        _specialBlock.IsCutting = true;

        GameEvents.OnSpecialBlockIsCutting.Invoke(_specialBlock);
    }
}

public class BrickCutting : ICutting
{
    private readonly SpecialBlock _specialBlock;

    public BrickCutting(SpecialBlock specialBlock)
    {
        _specialBlock = specialBlock;
    }
    
    public void Cut()
    {
        GameEvents.OnSpecialBlockIsCutting.Invoke(_specialBlock);
    }
}
