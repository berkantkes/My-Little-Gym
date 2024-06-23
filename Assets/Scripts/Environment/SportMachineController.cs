using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SportMachineController : MonoBehaviour
{
    [SerializeField] private Image _passingTimeImage;
    [SerializeField] private float _waitingTime = 5;

    private CustomerController _customer;
    private float _timer;
    private bool _isAvailable = true;
    private bool _isTiming;

    public bool IsActive => gameObject.activeSelf;
    //public bool IsAvailable => gameObject.activeSelf;

    public bool IsAvailable => _isAvailable;

    public void SetCustomerToMachine(CustomerController customer)
    {
        _customer = customer;
        _isAvailable = false;
    }

    public void PassingTime()
    {
        _isTiming = true;
        StartCoroutine(TimerCoroutine());
    }

    private IEnumerator TimerCoroutine()
    {
        while (_isTiming)
        {
            _timer += Time.deltaTime;

            if (_timer >= _waitingTime)
            {
                _timer = 0f;
                StopTiming();
                SetExitMachine();
            }

            _passingTimeImage.fillAmount = _timer / _waitingTime;
            yield return null;
        }
    }

    public void StopTiming()
    {
        _isTiming = false;
        _isAvailable = true;
        _customer.SetExitSportMachine(true);
    }

    private void SetExitMachine()
    {
        _isAvailable = true;
        _customer.SetExitSportMachine(true);
    }

}
