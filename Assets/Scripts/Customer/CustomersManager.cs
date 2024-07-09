using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CustomersManager : MonoBehaviour
{
    [SerializeField] private Transform _waitingArea;
    [SerializeField] private int _initialCustomerCount = 50;
    [SerializeField] private GameObject _customerPrefab;
    [SerializeField] private int _maxQueueCount = 7;
    [SerializeField] private float _spawnInterval = 3f;
    [SerializeField] private LockerRoom _lockerRoom;
    [SerializeField] private SportsAreaManager _sportsAreaManager;
    [SerializeField] private WCManager _wcManager;

    private Queue<CustomerController> _customerPool = new Queue<CustomerController>();
    private Queue<CustomerController> _customerQueue = new Queue<CustomerController>();

    public CustomerController NextCustomer => _customerQueue.Peek();

    public void Initialize()
    {
        InitializeCustomerPool();
        StartCoroutine(SpawnCustomersRoutine());
    }


    private void InitializeCustomerPool()
    {
        for (int i = 0; i < _initialCustomerCount; i++)
        {
            GameObject customerObj = Instantiate(_customerPrefab, new Vector3(0, 0, -45), Quaternion.identity, transform);
            CustomerController customer = customerObj.GetComponent<CustomerController>();
            customer.Initialize(this);
            customerObj.SetActive(false);
            _customerPool.Enqueue(customer);
        }
    }

    private IEnumerator SpawnCustomersRoutine()
    {
        while (true)
        {
            if (_customerQueue.Count < _maxQueueCount)
            {
                SpawnCustomer();
            }
            yield return new WaitForSeconds(_spawnInterval);
        }
    }

    private void SpawnCustomer()
    {
        if (_customerPool.Count > 0)
        {
            CustomerController customer = _customerPool.Dequeue();
            _customerQueue.Enqueue(customer);
            customer.CustomerActivate();
            PositionCustomerInQueue();
        }
    }

    private void PositionCustomerInQueue()
    {
        int index = 0;

        foreach (var customer in _customerQueue)
        {
            customer.SetTarget(_waitingArea.position + new Vector3(0, 0, (index * -4f) - 5f));
            index++;
        }
    }

    public void CheckOut()
    {
        if (_customerQueue.Count > 0)
        {
            CustomerController customer = _customerQueue.Dequeue();
            ChangingCubicle changingCubicle = _lockerRoom.GoLockerRoom();
            //changingCubicle.AddCustomerQueue(customer);
            customer.MoveToTargets(_sportsAreaManager.GetAvailableMachine(customer), changingCubicle, _wcManager);
            PositionCustomerInQueue();
            //Customer money
        }
    }

    public void ExitCustomer(CustomerController customer)
    {
        _customerPool.Enqueue(customer);
        customer.gameObject.SetActive(false);
        customer.transform.position = new Vector3(0, 0, -20);
    }
}
