using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangingCubicle : MonoBehaviour
{
    [SerializeField] private Image _passingTimeImage;
    [SerializeField] private float _waitingTime = 5;

    private float _timer;
    private bool _isTiming;

    private Queue<CustomerController> _customerQueue = new Queue<CustomerController>();
    public bool IsActive => gameObject.activeInHierarchy;
    public int CustomerCount => _customerQueue.Count;


    public void AddCustomerQueue(CustomerController customer)
    {
        _customerQueue.Enqueue(customer);
        PositionCustomerInQueue();
    }

    private void PositionCustomerInQueue()
    {
        int index = 0;

        foreach (var customer in _customerQueue)
        {
            customer.SetTarget(transform.position + new Vector3(0, 0, index * -2f));
            index++;
        }
    }

    public void PassingTime()
    {
        _isTiming = true;
        StartCoroutine(TimerCoroutine());
    }

    private IEnumerator TimerCoroutine()
    {
        while (_isTiming && _customerQueue.Count > 0)
        {
            if (!_customerQueue.Peek().IsMoving)
            {
                _timer += Time.deltaTime;
                
                if (_timer >= _waitingTime)
                {
                    _timer = 0f;
                    ExitCustomer();
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
        CustomerController customer = _customerQueue.Dequeue();
        customer.SetExitLockerRoom(true);
        PositionCustomerInQueue();
    }
}
