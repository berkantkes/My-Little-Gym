using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SportMachineController : EnvironmentAbstract
{
    [SerializeField] private Image _passingTimeImage;
    [SerializeField] private float _waitingTime = 5;
    [SerializeField] private Transform _target;

    private CustomerController _customer;
    private float _useMachinetimer;
    private float _cleanMachinetimer;
    private bool _isAvailable = true;
    private bool _isUseMachineTiming;
    private bool _isCleanMachineTiming;
    private bool _isClean = true;

    public Transform Target => _target;

    public bool IsActive => gameObject.activeInHierarchy;

    public bool IsAvailable => _isAvailable;
    public bool IsClean => _isClean;

    public void SetCustomerToMachine(CustomerController customer)
    {
        _customer = customer;
        _isAvailable = false;
    }

    public void PassingUseMachineTime()
    {
        _customer.SetNavMesh(false);
        _customer.transform.position = transform.position + new Vector3(-.5f, .3f, -1); // target position + y
        _customer.transform.rotation = Quaternion.identity;
        _isUseMachineTiming = true;
        StartCoroutine(UseMachineTimerCoroutine());
    }
    public void PassingCleanMachineTime()
    {
        _isCleanMachineTiming = true;
        StartCoroutine(CleanMachineTimerCoroutine());
    }

    private IEnumerator UseMachineTimerCoroutine()
    {
        while (_isUseMachineTiming)
        {
            _useMachinetimer += Time.deltaTime;

            if (_useMachinetimer >= _waitingTime)
            {
                _useMachinetimer = 0f;
                StopUseMachineTiming();
                SetExitMachine();
            }
            yield return null;
        }
    }

    private IEnumerator CleanMachineTimerCoroutine()
    {
        while (_isCleanMachineTiming && !_isClean)
        {
            _cleanMachinetimer += Time.deltaTime;

            if (_cleanMachinetimer >= _waitingTime)
            {
                _cleanMachinetimer = 0f;
                StopCleanMachineTiming();
                _customer.SetExitSportMachine(true);
                SetCleanMachine();
            }

            _passingTimeImage.fillAmount = _cleanMachinetimer / _waitingTime;
            yield return null;
        }
    }

    public void StopUseMachineTiming()
    {
        _isUseMachineTiming = false;
        _customer.SetExitSportMachine(true);
    }
    public void StopCleanMachineTiming()
    {
        _isCleanMachineTiming = false;
    }

    private void SetExitMachine()
    {
        _isClean = false;
        _customer.SetExitSportMachine(true);
        _customer.transform.position = transform.position + new Vector3(2, 0, 0); // target position + y
        _customer.SetNavMesh(true);
    }
    private void SetCleanMachine()
    {
        _isClean = true;
        _isAvailable = true;
    }

}
