using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangingCubicle : MonoBehaviour
{
    [SerializeField] private Image _passingTimeImage;
    [SerializeField] private float _waitingTime = 5;
    [SerializeField] private MoneyStackManager _moneyStackManager;

    private CustomerController _currentCustomer;
    private float _timer;
    private bool _isTiming;
    public bool IsActive => gameObject.activeInHierarchy;

    public void SetCustomer(CustomerController customer)
    {
        _currentCustomer = customer;
        CallCustomer();
    }

    private void CallCustomer()
    {
        _currentCustomer.SetTarget(transform.position);
        _currentCustomer.SetEnterLockerRoom(true);
    }

    public void PassingTime()
    {
        _isTiming = true;
        StartCoroutine(TimerCoroutine());
    }

    private IEnumerator TimerCoroutine()
    {
        while (_isTiming && _currentCustomer != null)
        {
            if (!_currentCustomer.IsMoving)
            {
                _timer += Time.deltaTime;
                
                if (_timer >= _waitingTime)
                {
                    _timer = 0f;
                    ExitCustomer();
                    StopTiming();
                }

                _passingTimeImage.fillAmount = _timer / _waitingTime;
                yield return null;
            }
            yield return null;
        }
    }

    public void StopTiming()
    {
        _isTiming = false;
    }

    private void ExitCustomer()
    {
        _currentCustomer.SetExitLockerRoom(true);
        _currentCustomer = null;
        _moneyStackManager.AddMoney(20);
    }

    public bool IsAvailable()
    {
        return _currentCustomer != null;
    }

}
