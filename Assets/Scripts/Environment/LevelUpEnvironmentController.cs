using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpEnvironmentController : SingleEnvironmentController
{
    [SerializeField] private List<SingleEnvironmentController> _levelUpEnvironments;

    public override void SetEnvironmentData(EnvironmentData environmentData, AllEnvironmentManager allEnvironmentManager)
    {
        _environmentData = environmentData;
        _allEnvironmentManager = allEnvironmentManager;
        ProcessEnvironmentData();
        Debug.Log("SetEnvironmentData");
        if (environmentData.isPaid)
        {
            Debug.Log("environmentData.isPaid");
            gameObject.SetActive(false);
        }
    }

    protected override void ProcessEnvironmentData()
    {
        Debug.Log("LevelUpEnvironmentController");
        if (_environmentData.isOpen)
        {
            gameObject.SetActive(true);
            if (!_environmentData.isPaid)
            {
                _moneyArea.Initialize(this);
            }
            //_moneyArea.gameObject.SetActive(!_environmentData.isPaid);
            //_environmentObject.gameObject.SetActive(_environmentData.isPaid);
            _moneyArea.SetCurrentPrice(_environmentData.currentPrice);
        }
    }

    public override void PaidArea()
    {
        gameObject.SetActive(false);
        _environmentData.isPaid = true;

        foreach (var env in _willBeOpenEnvironmentObjects)
        {
            env.Initialize(_allEnvironmentManager);
        }
        foreach (var env in _levelUpEnvironments)
        {
            env.UpLevel();
        }
        _allEnvironmentManager.UpdateEnvironmentDataList();
    }
}
