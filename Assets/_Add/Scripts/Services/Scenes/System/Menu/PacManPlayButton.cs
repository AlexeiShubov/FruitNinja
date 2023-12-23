using System;
using UnityEngine.UI;
using UnityEngine;

public class PacManPlayButton : MonoBehaviour
{
    [SerializeField] private Transform _finish;
    [SerializeField] private Image _button;
    [Space]
    [SerializeField] private float _speedMove;

    private float _fullDistance;
    
    public event Action PlayButtonIsDead;

    private void Start()
    {
        _fullDistance = _finish.position.x - transform.position.x;
    }

    private void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, _finish.position, _speedMove * Time.deltaTime);

        if (_button)
        {
            _button.fillAmount -= _speedMove * Time.deltaTime / _fullDistance;
        }
        
        if (_button.fillAmount <= 0)
        {
            ButtonIsDead();
        }
    }

    protected virtual void ButtonIsDead()
    {
        PlayButtonIsDead?.Invoke();
        
        _button.gameObject.SetActive(false);
    }
}
