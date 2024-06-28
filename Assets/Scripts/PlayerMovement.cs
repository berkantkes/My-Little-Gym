using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private DynamicJoystick _dynamicJoystick;
    [SerializeField] private float _movementSpeed;
    [SerializeField] private Animator _animator;

    private UIManager _uiManager;
    private Rigidbody _playerRigidbody;
    private CapsuleCollider _playerCollider;
    private CustomersManager _customersManager;

    private float _horizontal;
    private float _vertical;
    private int _slowDownFactor = 50;
    private bool isInMoneyPayTriggerArea = false;
    private bool _spendMoney = false;
    private bool _wasMoving;
    private Collider _currentTrigger;

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
        UpdateAnimation(); 
        if (_currentTrigger != null && !_currentTrigger.gameObject.activeSelf)
        {
            // OnTriggerExit manuel olarak çaðrýlýr
            OnTriggerExit(_currentTrigger);
        }
    }

    private void FixedUpdate()
    {
        SetMovement();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<MoneyPayAreaController>() != null)
        {
            _currentTrigger = other;
            Debug.Log("MoneyPayAreaControllerSTART");
            isInMoneyPayTriggerArea = true;
        }
        if (other.GetComponent<MoneyStackManager>() != null)
        {
            _currentTrigger = other;
            other.GetComponent<MoneyStackManager>().CollectMoney();
        }
        if (other.GetComponent<CustomerWaitArea>() != null)
        {
            _currentTrigger = other;
            other.GetComponent<CustomerWaitArea>().PassingTime();
        }
        if (other.GetComponent<SportMachineController>() != null)
        {
            _currentTrigger = other;
            other.GetComponent<SportMachineController>().PassingCleanMachineTime();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<MoneyPayAreaController>() != null)
        {
            _currentTrigger = null;
            Debug.Log("MoneyPayAreaControllerEXIT");
            isInMoneyPayTriggerArea = false;
            _spendMoney = false;
        }
        if (other.GetComponent<CustomerWaitArea>() != null)
        {
            _currentTrigger = null;
            other.GetComponent<CustomerWaitArea>().StopTiming();
        }
        if (other.GetComponent<SportMachineController>() != null)
        {
            _currentTrigger = null;
            other.GetComponent<SportMachineController>().StopCleanMachineTiming();
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

            Vector3 movement = new Vector3(_horizontal, 0, _vertical) * _movementSpeed * Time.fixedDeltaTime;

            if (movement != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(-movement), 0.15f);
            }
        }
    }

    private void GetMovementInputs()
    {
        _horizontal = _dynamicJoystick.Horizontal;
        _vertical = _dynamicJoystick.Vertical;
    }

    private void UpdateAnimation()
    {
        bool isMoving = _horizontal != 0 || _vertical != 0;

        if (_animator != null)
        {
            if (isMoving && !_wasMoving)
            {
                _animator.SetTrigger("Walk");
            }
            else if (!isMoving && _wasMoving)
            {
                _animator.SetTrigger("Idle");
            }
        }

        _wasMoving = isMoving;
    }
}
