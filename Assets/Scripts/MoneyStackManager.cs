using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyStackManager : MonoBehaviour
{
    [SerializeField] private GameObject _moneyPrefab;

    private List<GameObject> _moneyList = new List<GameObject>();
    private UIManager _uiManager;
    private int _moneyValue = 0;

    private Vector3 _startLocalPosition = new Vector3(-0.5f, 0, 2.5f);

    private float xSpacing = 1.0f;
    private float ySpacing = 0.25f;
    private float zSpacing = -0.5f;

    private int _yCount = 5;
    private int _xCount = 2;
    private int _zCount = 4;

    private int _moneyListIndex = 0;
    private int _openMoneyIndex = 0;

    public void Initialize(UIManager uIManager)
    {
        _uiManager = uIManager;

        AddMoney(200);
        
        for (int y = 0; y < _yCount; y++)
        {
            for (int x = 0; x < _xCount; x++)
            {
                for (int z = 0; z < _zCount; z++)
                {
                    Vector3 newPosition = _startLocalPosition + new Vector3(x * xSpacing, y * ySpacing, z * zSpacing);
                    GameObject money = Instantiate(_moneyPrefab, transform);
                    money.SetActive(false);
                    _moneyList.Add(money);
                    _moneyList[_moneyListIndex].transform.localPosition = newPosition;
                    _moneyListIndex++;
                }
            }
        }
    }
    public void AddMoney(int amount)
    {
        _moneyValue += amount;

        for (int i = 0; i < amount / 5; i++)
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

        if (_moneyValue == 0)
            return;

        PlayerPrefsHelper.SetInt(PlayerPrefsHelper.MoneyKey, PlayerPrefsHelper.GetInt(PlayerPrefsHelper.MoneyKey) + _moneyValue);

        _uiManager.SetMoneyAmountText();

        _moneyValue = 0;
        _openMoneyIndex = 0;
    }

}
