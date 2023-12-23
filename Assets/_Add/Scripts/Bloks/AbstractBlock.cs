using JetBrains.Annotations;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public abstract class AbstractBlock : MonoBehaviour
{
    [SerializeField] protected SpriteRenderer _mainSprite;
    [SerializeField] [CanBeNull] protected Half _leftHalf;
    [SerializeField] [CanBeNull] protected Half _rightHalf;
    [SerializeField] protected SpriteRenderer _leftSprite;
    [SerializeField] protected SpriteRenderer _leftShadow;
    [SerializeField] protected SpriteRenderer _rightSprite;
    [SerializeField] protected SpriteRenderer _rightShadow;
    [SerializeField] protected SpriteRenderer _splat;
    [SerializeField] protected SpriteRenderer _shadow;
    [SerializeField] protected ParticleSystem[] _particle;
    [SerializeField] protected ScriptableObjectBlock _scriptableObjectBlocks;
    [SerializeField] private TextMeshProUGUI _score;
    [SerializeField] private Animator _frozenAnimator;

    public abstract IRotator CurrentRotator { get; set; }
    public abstract IMovable CurrentMovable { get; set; }
    public abstract ICutting CurrentCutting { get; set; }
    public bool IsCutting { get; set; }
    public bool IsFrozen { get; set; }

    public ScriptableObjectBlock ScriptableObjectBlock => _scriptableObjectBlocks;
    public ParticleSystem[] Particle => _particle;
    public Vector2 CurrentDirectionMove => CurrentMovable.MoveDirection;
    public virtual Half LeftHalf => _leftHalf;
    public virtual Half RightHalf => _rightHalf;
    public Transform TransformMainSprite => _mainSprite.transform;
    public Transform TransformSplat => _splat.transform;
    public Animator FrozenAnimator => _frozenAnimator;

    public virtual void SetDefaultsSettingsBlock()
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

    public virtual void SetControllersMono(IMovable newMovable, IRotator newRotator, ICutting newCutting)
    {
        CurrentMovable = newMovable;
        CurrentRotator = newRotator;
        CurrentCutting = newCutting;
    }

    public virtual void GetRandomSprites()
    {
        var value = Random.Range(0, _scriptableObjectBlocks.MainSpriteBlock.Length);
        
        _mainSprite.sprite = _scriptableObjectBlocks.MainSpriteBlock[value];
        _leftSprite.sprite = _scriptableObjectBlocks.LeftBlock[value];
        _leftShadow.sprite = _leftSprite.sprite;
        _rightSprite.sprite = _scriptableObjectBlocks.RightBlock[value];
        _rightShadow.sprite = _rightSprite.sprite;
        _splat.sprite = _scriptableObjectBlocks.Splat[value];
        _shadow.sprite = _scriptableObjectBlocks.Shadow[value];
    }
    
    public void SetScore(int score)
    {
        _score.gameObject.SetActive(true);
        _score.text = $"{score}";
    }

    protected void SetDefaultParents()
    {
        _leftHalf.transform.SetParent(transform);
        _rightHalf.transform.SetParent(transform);
        _splat.transform.SetParent(transform);
    }

    protected void SetDefaultActiveSelf()
    {
        _mainSprite.gameObject.SetActive(true);
        _leftHalf.gameObject.SetActive(false);
        _rightHalf.gameObject.SetActive(false);
        _splat.gameObject.SetActive(false);
    }

    protected void SetDefaultPosition()
    {
        if (_score != null)
        {
            _score.gameObject.SetActive(false);
        }
        
        _leftHalf.transform.position = Vector2.zero;
        _rightHalf.transform.position = Vector2.zero;
        _splat.transform.position = Vector2.zero;
        _splat.transform.localScale = _scriptableObjectBlocks.DefaultBlockScale;
    }
}
