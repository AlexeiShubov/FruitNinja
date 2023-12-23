using UnityEngine;

public class StandardScalerBlock : IScaler
{
    private readonly float _scaleBlockStep;
    
    private readonly Vector2 _minScaleBlock;
    private readonly Vector2 _maxScaleBlock;
    
    public StandardScalerBlock(float scaleBlockStep, Vector2 minScaleBlock, Vector2 maxScaleBlock)
    {
        _scaleBlockStep = scaleBlockStep;
        _minScaleBlock = minScaleBlock;
        _maxScaleBlock = maxScaleBlock;
    }

    public Vector2 Increase(Vector2 currentScale)
    {
        if (currentScale.x <= _maxScaleBlock.x)
        {
            return currentScale + new Vector2(_scaleBlockStep, _scaleBlockStep) * Time.deltaTime;
        }

        return currentScale;
    }
    
    public Vector2 Decrease(Vector2 currentScale)
    {
        if(currentScale.x >= _minScaleBlock.x)
        {
            return currentScale - new Vector2(_scaleBlockStep, _scaleBlockStep) * Time.deltaTime;
        }

        return currentScale;
    }
}
