using System.Collections;
using UnityEngine;

public class IceEffectMode
{
    private readonly float _defaultColdTimeEffect;
    private readonly IHostCoroutine _hostCoroutine;
    private readonly BlocksController _blocksController;
    private readonly Transform _iceBG;

    private bool _iceEffectIsActive;
    private float _currentColdTimeEffect;

    public IceEffectMode(IHostCoroutine hostCoroutine, BlocksController blocksController, Transform iceBG)
    {
        _iceBG = iceBG;
        _defaultColdTimeEffect = 4f;
        _hostCoroutine = hostCoroutine;
        _blocksController = blocksController;
    }

    public void DoIceEffect(Ice iceBlock)
    {
        if (_iceEffectIsActive)
        {
            _currentColdTimeEffect = _defaultColdTimeEffect;
        }
        else
        {
            _hostCoroutine.StartCoroutine(IceEffect(iceBlock));
        }
    }

    private IEnumerator IceEffect(Ice iceBlock)
    {
        _iceEffectIsActive = true;
        _iceBG.gameObject.SetActive(true);
        _currentColdTimeEffect = _defaultColdTimeEffect;

        while (_iceEffectIsActive)
        {
            iceBlock.DoIceEffect(_blocksController);

            _currentColdTimeEffect -= Time.deltaTime;

            if (_currentColdTimeEffect <= 0f)
            {
                _iceEffectIsActive = false;
                
                iceBlock.CancelIceEffect(_blocksController);
            }
            
            yield return null;
        }
        
        _iceBG.gameObject.SetActive(false);
    }
}
