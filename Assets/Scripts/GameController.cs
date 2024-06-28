using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private UIManager _uiManager;
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private AllEnvironmentManager _allEnvironmentManager;
    [SerializeField] private MoneyStackManager _moneyStackManager;
    [SerializeField] private MoneyStackManager _moneyStackManager2;
    [SerializeField] private CustomersManager _customersManager;
    [SerializeField] private WCManager _wcManager;

    private void Start()
    {
        QualitySettings.vSyncCount = 0;

        Application.targetFrameRate = 60;

        PlayerPrefsHelper.SetInt(PlayerPrefsHelper.MoneyKey, 5000);

        _uiManager.Initialize();
        _playerMovement.Initialize(_uiManager, _customersManager);
        _allEnvironmentManager.Initialize();
        _moneyStackManager.Initialize(_uiManager);
        _moneyStackManager2.Initialize(_uiManager);
        _customersManager.Initialize();
        _wcManager.Initialize();

    }
}
