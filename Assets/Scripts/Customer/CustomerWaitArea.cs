using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomerWaitArea : MonoBehaviour
{
    [SerializeField] private Image _passingTimeImage;
    [SerializeField] private float _waitingTime = 2;
    [SerializeField] private CustomersManager _customersManager;
    [SerializeField] private SportsAreaManager sportsAreaManager;

    private float _timer;
    private bool _isTiming;

    public void PassingTime()
    {
        _isTiming = true;
        StartCoroutine(TimerCoroutine());
    }

    private IEnumerator TimerCoroutine()
    {
        while (_isTiming && _customersManager.NextCustomer != null && sportsAreaManager.IsAvailableSportMachine()) // await sportarea available
        {
            if (!_customersManager.NextCustomer.IsMoving)
            {
                _timer += Time.deltaTime;

                if (_timer >= _waitingTime)
                {
                    _timer = 0f;
                    _customersManager.CheckOut();
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

}
