using UnityEngine;

public class FruitCutting : ICutting
{
    private readonly float _gravityScale;
    private readonly AbstractBlock _block;
    private readonly StandardScalerBlock _standardScalerBlock;

    public FruitCutting(AbstractBlock block, float gravityScale)
    //public FruitCutting(AbstractBlock block, GameSettings gameSettings)
    {
        _gravityScale = gravityScale;
        //_gravityScale = gameSettings.GravityScale;
        _block = block;
        _standardScalerBlock = new StandardScalerBlock(block.ScriptableObjectBlock.ScaleBlockStep,
            block.ScriptableObjectBlock.DefaultBlockScale,
            block.ScriptableObjectBlock.MaxBlockScale);
    }

    public void Cut()
    {
        _block.IsCutting = true;

        SetParentsObjects();
        SetActiveSelf();
        
        _block.LeftHalf.StartMove(_standardScalerBlock, _gravityScale, 
            _block.CurrentDirectionMove, _block.CurrentRotator.RotateSpeed);
        _block.RightHalf.StartMove(_standardScalerBlock, _gravityScale, 
            _block.CurrentDirectionMove, _block.CurrentRotator.RotateSpeed);
        
        if (_block.gameObject.TryGetComponent(out Box box))
        {
            GameEvents.OnSpecialBlockIsCutting.Invoke(box);

            return;
        }

        if (_block.Particle.Length == 0) return;
        
        if (Random.value < 0.5)
        {
            foreach (var particle in _block.Particle)
            {
                particle.Play();
            }
        }

        if (!(_block is Fruit)) return;
        
        var randomScore = Random.Range(10, 15);
        
        _block.Particle[Random.Range(0, _block.Particle.Length)].Play();
        _block.SetScore(randomScore);

        GameEvents.OnCuttingFruitEvent?.Invoke(randomScore, _block.transform);
    }

    private void SetParentsObjects()
    {
        _block.TransformSplat.SetParent(_block.transform.parent);
        _block.TransformSplat.position = _block.transform.position;
        
        _block.LeftHalf.transform.SetParent(_block.transform.parent);
        _block.LeftHalf.transform.position = _block.transform.position;
        
        _block.RightHalf.transform.SetParent(_block.transform.parent);
        _block.RightHalf.transform.position = _block.transform.position;
    }

    private void SetActiveSelf()
    {
        _block.TransformMainSprite.gameObject.SetActive(false);
        _block.TransformSplat.gameObject.SetActive(true);
        _block.LeftHalf.gameObject.SetActive(true);
        _block.RightHalf.gameObject.SetActive(true);
    }
}
