using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyStackManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> _moneyList;

    private UIManager _uiManager;
    private int _moneyValue = 0;

    private Vector3 _startLocalPosition = new Vector3(-0.5f, 0, -.5f);

    private float xSpacing = 1.0f;
    private float ySpacing = 0.25f;
    private float zSpacing = -0.5f;

    private int _index = 0;
    private int _openMoneyIndex = 0;

    public void Initialize(UIManager uIManager)
    {
        _uiManager = uIManager;

        AddMoney(700);
        
        for (int y = 0; y < 2; y++)
        {
            for (int x = 0; x < 2; x++)
            {
                for (int z = 0; z < 4; z++)
                {
                    if (_index >= _moneyList.Count) return;

                    Vector3 newPosition = _startLocalPosition + new Vector3(x * xSpacing, y * ySpacing, z * zSpacing);
                    _moneyList[_index].transform.position = newPosition;
                    _index++;
                }
            }
        }
    }
    private void AddMoney(int amount)
    {
        _moneyValue += amount;

        for (int i = 0; i < amount; i++)
        {
            if (_openMoneyIndex >= _moneyList.Count)
                return;

            _moneyList[_openMoneyIndex].SetActive(true);
            _openMoneyIndex++;
        }
    }

    public void CollectMoney()
    {
        foreach (GameObject money in _moneyList)
        {
            money.SetActive(false);
        }

        PlayerPrefsHelper.SetInt(PlayerPrefsHelper.MoneyKey, PlayerPrefsHelper.GetInt(PlayerPrefsHelper.MoneyKey) + _moneyValue);

        _uiManager.SetMoneyAmountText();

        _moneyValue = 0;
        _openMoneyIndex = 0;
    }

}
