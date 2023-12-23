using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
    [ExecuteAlways]
#endif

public class HPController : MonoBehaviour, IRestarteble
{
    [Header("Number of starting lives")] 
    [SerializeField] private int _maxHP;
    [SerializeField][Min(3)] private int _countStartingLives;
    [Space]
    [SerializeField] private Transform _heartUIPrefab;
    [SerializeField] private Transform _heartEffectPrefab;
    [SerializeField] private Transform _parrentForUILives;
    [Space] 
    [Header("Settings HP Value")] 
    [SerializeField][Min(1)] private int _plusHPValue;
    [SerializeField][Min(0)] private int _minusHPValue;
    [SerializeField][Min(7)] private float _speedHPEffectMove;
    
    private int _countStartingLivesDefault;
    private bool _gameIsLoss;
    
    private HP _hp;
    private Stack<Transform> _heartUIPrefabs;

    public void Init()
    {
        _heartUIPrefabs = new Stack<Transform>();
        
        StartCoroutine(SpawnHeartUI(_countStartingLives));

        _hp = new HP(_countStartingLives);
        _countStartingLivesDefault = _countStartingLives;

        GameEvents.OnBonusLifeEvent += BonusHp;
        GameEvents.OnDamageEvent += SetDamage;
        GameEvents.OnLossGameEvent += (value) => _gameIsLoss = value;
    }

    private IEnumerator SpawnHeartUI(int countPrefabs = 1)
    {
        for (var i = 0; i < countPrefabs; i++)
        {
            var newLife = Instantiate(_heartUIPrefab, _parrentForUILives);
            
            _heartUIPrefabs.Push(newLife);
            
            yield return new WaitForSeconds(0.2f);
        }
    }

    private void BonusHp(Transform spawnPosition)
    {
        if (_countStartingLives + _plusHPValue > _maxHP) return;
        
        _countStartingLives++;
        
        StartCoroutine(SpawnHeartEffect(spawnPosition));

        _hp.CorrectCountHP(_plusHPValue);
    }

    private void SetDamage()
    {
        _hp.CorrectCountHP(-_minusHPValue);
        _countStartingLives--;

        if (_heartUIPrefabs.Count <= 0) return;
        
        _heartUIPrefabs.Peek().GetComponent<HeartLife>().PlayAnim();
        
        Destroy(_heartUIPrefabs.Pop().gameObject, 1f);
    }

    public void RestartGame()
    {
        _gameIsLoss = false;

        _heartUIPrefabs.Clear();

        _countStartingLives = _countStartingLivesDefault;

        _hp.CurrentHP = _countStartingLivesDefault;

        StartCoroutine(SpawnHeartUI(_countStartingLivesDefault));
    }

    private void OnDisable()
    {
        GameEvents.OnBonusLifeEvent -= BonusHp;
        GameEvents.OnDamageEvent -= SetDamage;
        GameEvents.OnLossGameEvent -= (value) => _gameIsLoss = value;
    }

    private IEnumerator SpawnHeartEffect(Transform spawnPosition)
    {
        if(_heartUIPrefabs.Count > 0 && !_gameIsLoss)
        {
            var newHeartEffectPrefab = Instantiate(_heartEffectPrefab, spawnPosition.position, Quaternion.identity, transform);
            Vector2 goalPosition = new Vector2(_heartUIPrefabs.Peek().position.x - 1f, _heartUIPrefabs.Peek().position.y);

            while (Vector2.Distance(newHeartEffectPrefab.position, goalPosition) > 1f)
            {
                if (_gameIsLoss)
                {
                    Destroy(newHeartEffectPrefab.gameObject);

                    yield break;
                }

                newHeartEffectPrefab.transform.position =
                    Vector2.MoveTowards(newHeartEffectPrefab.transform.position, goalPosition, _speedHPEffectMove * Time.deltaTime);

                yield return null;
            }

            StartCoroutine(SpawnHeartUI());
            Destroy(newHeartEffectPrefab.gameObject);
        }
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (_countStartingLives > _maxHP)
        {
            _countStartingLives = _maxHP;
        }
    }
#endif
}
