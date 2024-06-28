using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ThreeWCController : MonoBehaviour
{
    [SerializeField] private List<SingleWCController> _wcControllers;
    [SerializeField] private float _waitingTime = 5;
    [SerializeField] private MoneyStackManager _moneyStackManager;
    [SerializeField] private Transform _orderStartPosition;

    private List<CustomerController> _customerQueue = new List<CustomerController>();
    public bool IsActive => gameObject.activeInHierarchy;
    public int CustomerCount => _customerQueue.Count;

    public void Initialize()
    {
        foreach (var wc in _wcControllers)
        {
            wc.Initialize(this);
        }
    }

    public void AddCustomerQueue(CustomerController customer)
    {
        _customerQueue.Add(customer);
        PositionCustomerInQueue();
    }

    private void PositionCustomerInQueue()
    {
        int index = 0;

        foreach (var customer in _customerQueue)
        {
            bool skipCustomer = false;

            foreach (var item in _wcControllers)
            {
                if (customer == item.GetCustomer())
                {
                    skipCustomer = true;
                    break;
                }
            }

            if (skipCustomer)
            {
                continue;
            }

            var availableWC = _wcControllers.Where(wc => wc.IsAvailable).ToList();


            if (availableWC.Count > 0)
            {
                var randomIndex = Random.Range(0, availableWC.Count);
                var targetWC = availableWC[randomIndex];

                targetWC.SetCustomerToWC(customer);
                continue;
            }

            customer.SetTarget(_orderStartPosition.position + new Vector3(index * -2f, 0, 0));
            index++;
        }
    }
    public void ExitCustomer(CustomerController customer)
    {
        _customerQueue.Remove(customer);
        customer.SetExitWC(true);
        PositionCustomerInQueue();
        _moneyStackManager.AddMoney(20);
    }
}
