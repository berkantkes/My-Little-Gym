using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _moneyAmount;

    public void Initialize()
    {
        SetMoneyAmountText();
    }

    public void SetMoneyAmountText()
    {
        _moneyAmount.SetText(PlayerPrefsHelper.GetInt(PlayerPrefsHelper.MoneyKey).ToString());
    }

}
