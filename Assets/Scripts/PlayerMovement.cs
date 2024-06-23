using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private DynamicJoystick _dynamicJoystick;
    [SerializeField] private float _movementSpeed;

    private UIManager _uiManager;
    private Rigidbody _playerRigidbody;
    private CapsuleCollider _playerCollider;
    private CustomersManager _customersManager;

    private float _horizontal;
    private float _vertical;
    private int _slowDownFactor = 50;
    private bool isInMoneyPayTriggerArea = false;
    private bool _spendMoney = false;

    public void Initialize(UIManager uiManager, CustomersManager customersManager)
    {
        _uiManager = uiManager;
        _customersManager = customersManager;
        _playerRigidbody = GetComponent<Rigidbody>();
        _playerCollider = GetComponent<CapsuleCollider>();
    }

    private void Update()
    {
        GetMovementInputs();
    }

    private void FixedUpdate()
    {
        SetMovement();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<MoneyPayAreaController>() != null)
        {
            isInMoneyPayTriggerArea = true;
        }
        if (other.GetComponent<MoneyStackManager>() != null)
        {
            other.GetComponent<MoneyStackManager>().CollectMoney();
        }
        if (other.GetComponent<CustomerWaitArea>() != null)
        {
            other.GetComponent<CustomerWaitArea>().PassingTime();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<MoneyPayAreaController>() != null)
        {
            isInMoneyPayTriggerArea = false;
            _spendMoney = false;
        }
        if (other.GetComponent<CustomerWaitArea>() != null)
        {
            other.GetComponent<CustomerWaitArea>().StopTiming();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (isInMoneyPayTriggerArea)
        {
            if (_playerRigidbody.velocity == Vector3.zero && _spendMoney == false)
            {
                _spendMoney = true;
            }

            if (isInMoneyPayTriggerArea && _spendMoney)
            {
                MoneyPayAreaController moneyAreaManager = other.GetComponent<MoneyPayAreaController>();
                int decreaseAmount = 0;

                if (moneyAreaManager != null)
                {
                    if (moneyAreaManager.GetCurrentPrice() > 0)
                    {
                        int money = PlayerPrefsHelper.GetInt(PlayerPrefsHelper.MoneyKey);
                        float result = (float)moneyAreaManager.GetPrice() / _slowDownFactor;
                        int pricePerTick = (int)Math.Ceiling(result);

                        if (money >= pricePerTick)
                        {
                            if (pricePerTick > moneyAreaManager.GetCurrentPrice())
                            {
                                decreaseAmount = moneyAreaManager.GetCurrentPrice();
                            }
                            else
                            {
                                decreaseAmount = pricePerTick;
                            }
                        }
                        else
                        {
                            decreaseAmount = money;
                        }
                    }
                    moneyAreaManager.SetandUpdateCurrentPrice(moneyAreaManager.GetCurrentPrice() - decreaseAmount);
                }

                PlayerPrefsHelper.SetInt(PlayerPrefsHelper.MoneyKey, PlayerPrefsHelper.GetInt(PlayerPrefsHelper.MoneyKey) - decreaseAmount);

                _uiManager.SetMoneyAmountText();
            }
        }
    }

    private void SetMovement()
    {
        if (_playerRigidbody != null)
        {
            _playerRigidbody.velocity = new Vector3(_horizontal, _playerRigidbody.velocity.y, _vertical) * _movementSpeed * Time.fixedDeltaTime;
        }
    }

    private void GetMovementInputs()
    {
        _horizontal = _dynamicJoystick.Horizontal;
        _vertical = _dynamicJoystick.Vertical;
    }
}
