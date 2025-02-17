using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CashierMoneyArea : MoneyPayAreaController
{
    [SerializeField] private CustomerWaitArea _customerWaitArea;

    public override void PaidArea()
    {
        _singleEnvironmentController.PaidArea();
        PlayerPrefsHelper.GetBool(PlayerPrefsHelper.Cashier, true);
        _customerWaitArea.SetCashierAsync();
    }
}
