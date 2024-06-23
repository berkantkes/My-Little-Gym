using System;
using System.Collections.Generic;
using UnityEngine;

public class SingleEnvironmentController : MonoBehaviour
{
    [SerializeField] private MoneyPayAreaController _moneyArea;
    [SerializeField] private GameObject _environmentObject;
    [SerializeField] private List<SingleEnvironmentController> _willBeOpenEnvironmentObjects;
    [SerializeField] private EnvironmentType _environmentType;

    private AllEnvironmentManager _allEnvironmentManager;
    private EnvironmentData _environmentData;

    public EnvironmentType EnvironmentType => _environmentType;
    public int CurrentPrice => _moneyArea.CurrentPrice;

    public void Initialize(AllEnvironmentManager allEnvironmentManager)
    {
        _allEnvironmentManager = allEnvironmentManager;
        _environmentData.isOpen = true;
        ProcessEnvironmentData();
        _allEnvironmentManager.UpdateEnvironmentDataList();
    }

    public void SetEnvironmentData(EnvironmentData environmentData, AllEnvironmentManager allEnvironmentManager)
    {
        _environmentData = environmentData;
        _allEnvironmentManager = allEnvironmentManager;
        ProcessEnvironmentData();
    }

    private void ProcessEnvironmentData()
    {
        if(_environmentData.isOpen)
        {
            gameObject.SetActive(true);
            if (!_environmentData.isPaid)
            {
                _moneyArea.Initialize(this);
            }
            _moneyArea.gameObject.SetActive(!_environmentData.isPaid);
            _environmentObject.gameObject.SetActive(_environmentData.isPaid);
            _moneyArea.SetCurrentPrice(_environmentData.currentPrice);
        }
    }

    public void ChangeCurrentPrice()
    {
        _environmentData.currentPrice = _moneyArea.CurrentPrice;
        _allEnvironmentManager.UpdateEnvironmentDataList();
    }

    public void PaidArea()
    {
        _moneyArea.gameObject.SetActive(false);
        _environmentObject.gameObject.SetActive(true);
        _environmentData.isPaid = true;

        foreach (var env in _willBeOpenEnvironmentObjects)
        {
            env.Initialize(_allEnvironmentManager);
        }
        _allEnvironmentManager.UpdateEnvironmentDataList();
    }

    public EnvironmentData GetEnvironmentData()
    {
        if (_environmentData == null)
        {
            _environmentData = new EnvironmentData(_environmentType, false, false, 0);
        }
        return _environmentData;
    }
}
