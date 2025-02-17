using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SingleWCController : MonoBehaviour
{
    private ThreeWCController _threeWCController;
    private CustomerController _customer;

    private float _waitingTime = 5;
    private float _timer;
    private bool _isTiming;
    private bool _isAvailable = true;
    private bool _isTherePaper = true;

    public bool IsAvailable => _isAvailable && _isTherePaper;

    public void Initialize(ThreeWCController threeWCController)
    {
        _threeWCController = threeWCController;
    }

    public void SetCustomerToWC(CustomerController customer)
    {
        _customer = customer;
        _customer.SetTarget(new Vector3(transform.position.x, .5f, transform.position.z));
        _isAvailable = false;
        PassingTime();
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
            if (!_customer.IsMoving)
            {
                _timer += Time.deltaTime;

                if (_timer >= _waitingTime)
                {
                    _timer = 0f;
                    StopTiming();
                    _threeWCController.ExitCustomer(_customer);
                    _isAvailable = true;
                }

                yield return null;
            }
            yield return null;
        }
    }

    public void StopTiming()
    {
        _isTiming = false;
    }

    public CustomerController GetCustomer()
    {
        return _customer;
    }

    //private void ExitCustomer()
    //{
    //    CustomerController customer = _customerQueue.Dequeue();
    //    customer.SetExitLockerRoom(true);
    //    CallCustomer();
    //    _moneyStackManager.AddMoney(20);
    //}
}
