using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class CustomerWaitArea : MonoBehaviour
{
    [SerializeField] private Image _passingTimeImage;
    [SerializeField] private float _waitingTime = 2;
    [SerializeField] private CustomersManager _customersManager;
    [SerializeField] private SportsAreaManager _sportsAreaManager;
    [SerializeField] private MoneyStackManager _moneyStackManager;

    private float _timer;
    private bool _isTiming;
    private bool _cashier;

    private void OnEnable()
    {
        SetCashierAsync();
    }

    public void PassingTime()
    {
        _isTiming = true;
        StartCoroutine(TimerCoroutine());
    }

    private IEnumerator TimerCoroutine()
    {
        while (_isTiming && !PlayerPrefsHelper.GetBool(PlayerPrefsHelper.Cashier) && _customersManager.NextCustomer != null) // await sportarea available
        {
            if (!_customersManager.NextCustomer.IsMoving)
            {
                _timer += Time.deltaTime;

                if (_timer >= _waitingTime)
                {
                    _timer = 0f;
                    _customersManager.CheckOut();
                    _moneyStackManager.AddMoney(20);
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

    public async Task SetCashierAsync()
    {
        while (PlayerPrefsHelper.GetBool(PlayerPrefsHelper.Cashier)) // await sportarea available
        {
            await UniTask.WaitUntil(() => _sportsAreaManager.IsAvailableSportMachine());
            await UniTask.WaitUntil(() => _customersManager.NextCustomer != null);

            if (!_customersManager.NextCustomer.IsMoving)
            {
                _timer += Time.deltaTime;

                if (_timer >= _waitingTime)
                {
                    _timer = 0f;
                    _customersManager.CheckOut();
                    _moneyStackManager.AddMoney(20);
                }

                _passingTimeImage.fillAmount = _timer / _waitingTime;
            }
        }
    }

}
