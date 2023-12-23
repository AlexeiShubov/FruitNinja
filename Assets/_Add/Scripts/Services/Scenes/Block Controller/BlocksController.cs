using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BlocksController : IRestarteble
{
    private const float TIME_TO_CLEAR_OUTSIDE_BLOCKS = 1.5f;
    private const float RADIUS_BLOCKS = 1.7f;
    
    private readonly Vector2 _screenSize;
    private readonly GameSettings _gameSettings;
    private readonly ObjectPool _objectPool;
    private readonly PlayerController _player;
    private readonly IceEffectMode _iceEffectMode;
    private readonly MagneticFieldMode _magneticFieldMode;
    private readonly IHostCoroutine _hostCoroutine;
    private readonly HashSet<AbstractBlock> _activeBlocks;

    private bool _gameIsLoss;
    private IEnumerator _updateBlockTransform;

    public HashSet<AbstractBlock> ActiveBlocks => _activeBlocks;
    public ObjectPool ObjectPool => _objectPool;
    public GameSettings GameSettings => _gameSettings;

    public BlocksController (PlayerController player, ObjectPool objectPool, GameSettings gameSettings, Vector2 screenSize, Transform iceBG, IHostCoroutine hostCoroutine)
    {
        _activeBlocks = new HashSet<AbstractBlock>();
        _player = player;
        _gameSettings = gameSettings;
        _objectPool = objectPool;
        _screenSize = screenSize;
        _hostCoroutine = hostCoroutine;
        _iceEffectMode = new IceEffectMode(_hostCoroutine, this, iceBG);
        _magneticFieldMode = new MagneticFieldMode(this);
        
        _updateBlockTransform = MoveBlockUpdate();
        _hostCoroutine.StartCoroutine(_updateBlockTransform);
        _hostCoroutine.StartCoroutine(CheckBlockInCameraArea(_activeBlocks));

        GameEvents.OnSpecialBlockIsCutting += SpecialBlockIsCutting;
        GameEvents.OnMenuSceneLoad += MenuSceneLoad;
        GameEvents.OnLossGameEvent += ClearScene;
    }
    
    public void RestartGame()
    {
        _gameIsLoss = false;
    }
    
    public void ProcessingNewBlocks(Stack<AbstractBlock> blocksPack, Stack<SpawnZone> spawnZones, Stack<float> rotateSpeed, Stack<Vector2> moveSpeed)
    {
        _hostCoroutine.StartCoroutine(AddActiveBlocks(blocksPack, spawnZones, rotateSpeed, moveSpeed));
    }
    
    private IEnumerator AddActiveBlocks(Stack<AbstractBlock> blocksPack, Stack<SpawnZone> spawnZones, Stack<float> rotateSpeed, Stack<Vector2> moveSpeed)
    {
        while (blocksPack.Count > 0 && !_gameIsLoss)
        {
            SetBlockSettings(blocksPack.Peek(), spawnZones.Pop(), rotateSpeed.Pop(), moveSpeed.Pop());

            if (blocksPack.Peek() is Brick )
            {
                if (!_activeBlocks.Any(t => t is Brick))
                {
                    if (blocksPack.Peek().FrozenAnimator != null)
                    {
                        blocksPack.Peek().FrozenAnimator.CrossFade("FrozenIdle", 0);
                    }
                    
                    _activeBlocks.Add(blocksPack.Pop());
                }
                else
                {
                    DeactivationBlock(blocksPack.Pop());
                }
                
                continue;
            }

            if (blocksPack.Peek().FrozenAnimator != null)
            {
                blocksPack.Peek().FrozenAnimator.CrossFade("FrozenIdle", 0);
            }
            
            _activeBlocks.Add(blocksPack.Pop());
            
            yield return new WaitForSeconds(_gameSettings.TimeBetweenBlocksLaunch);
        }
    }
    
    private void SetBlockSettings(AbstractBlock block, SpawnZone spawnZone, float rotateSpeed, Vector2 moveSpeed)
    {
        block.SetDefaultsSettingsBlock();
        block.gameObject.SetActive(true);
        block.transform.position = spawnZone.GetRandomSpawnPosition();
        block.SetControllersMono(
            new ParabalMoveblock(_gameSettings.GravityScale, moveSpeed, 
                new StandardScalerBlock(block.ScriptableObjectBlock.ScaleBlockStep, 
                block.ScriptableObjectBlock.DefaultBlockScale, 
                block.ScriptableObjectBlock.MaxBlockScale),
                block.transform),
            new StandardRotateBlock(rotateSpeed, block.TransformMainSprite),
            new FruitCutting(block, _gameSettings.GravityScale));
    }

    private IEnumerator CheckBlockInCameraArea(HashSet<AbstractBlock> activeBlocks)
    {
        while (true)
        {
            if (_activeBlocks.Count == 0) yield return new WaitForSeconds(TIME_TO_CLEAR_OUTSIDE_BLOCKS);

            foreach (var block in activeBlocks)
            {
                if (block.transform.position.y > _screenSize.y || block.CurrentDirectionMove.y > 0)
                {
                    continue;
                }
                
                if (_gameIsLoss && block.transform.position.y > -_screenSize.y)
                {
                    DeactivationBlock(block);

                    _updateBlockTransform = CheckBlockInCameraArea(activeBlocks);
                    _hostCoroutine.StartCoroutine(_updateBlockTransform);

                    yield break;
                }

                if (!block.IsCutting && !_gameIsLoss && block is Fruit)
                {
                    GameEvents.OnDamageEvent.Invoke();
                }

                DeactivationBlock(block);

                _updateBlockTransform = CheckBlockInCameraArea(activeBlocks);
                _hostCoroutine.StartCoroutine(_updateBlockTransform);

                yield break;
            }

            yield return new WaitForSeconds(TIME_TO_CLEAR_OUTSIDE_BLOCKS);
        }
    }

    private void ClearScene(bool loss)
    {
        _hostCoroutine.StartCoroutine(GameIsLoss());
    }
    
    private IEnumerator GameIsLoss()
    {
        _gameIsLoss = true;
        
        yield return new WaitUntil(() => _activeBlocks.Count == 0);

        GameEvents.OnSceneClear.Invoke();

        _gameIsLoss = false;
    }
    
    private IEnumerator MoveBlockUpdate()
    {
        var blocksForMove = new HashSet<AbstractBlock>();
        
        while (true)
        {
            blocksForMove.Clear();
            
            foreach (var item in _activeBlocks)
            {
                blocksForMove.Add(item);
            }
            
            foreach (var block in blocksForMove)
            {
                block.CurrentMovable?.Move();
                block.CurrentRotator?.Rotate();

                if (CheckSliceSituation(block, _player))
                {
                    block.CurrentCutting.Cut();
                }
            }

            yield return null;
        }
    }

    private void DeactivationBlock(AbstractBlock block)
    {
        block.SetDefaultsSettingsBlock();
        block.gameObject.SetActive(false);
        _objectPool.ReturnBlockToPool(block);
        _activeBlocks.Remove(block);
        block.IsFrozen = false;
    }

    private bool CheckSliceSituation(AbstractBlock block, PlayerController playerController)
    {
        if (!playerController.CanBeCutting || block.IsCutting || playerController.Trail.gameObject.activeSelf == false) return false;

        return Vector2.Distance(block.transform.position, playerController.Trail.position) <= RADIUS_BLOCKS;
    }

    private void SpecialBlockIsCutting(SpecialBlock specialBlock)
    {
        if (specialBlock.TryGetComponent(out Ice ice))
        {
            _iceEffectMode.DoIceEffect(ice);
            
            return;
        }
        if (specialBlock.TryGetComponent(out Magnet magnet))
        {
            _magneticFieldMode.DoMagneticFieldEffect(magnet);
            
            return;
        }
        
        specialBlock.DoEffect(specialBlock, this);
    }

    private void MenuSceneLoad()
    {
        GameEvents.OnLossGameEvent -= ClearScene;
        GameEvents.OnSpecialBlockIsCutting -= SpecialBlockIsCutting;
        GameEvents.OnMenuSceneLoad -= MenuSceneLoad;
    }
}