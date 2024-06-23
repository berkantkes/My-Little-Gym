using System.Collections.Generic;
using UnityEngine;

public class AllEnvironmentManager : MonoBehaviour
{
    [SerializeField] private List<SingleEnvironmentController> _singleEnvironmentControllers;

    private Dictionary<EnvironmentType, SingleEnvironmentController> _environmentDictionary;

    public void Initialize()
    {
        InitializeEnvironmentDictionary();
        InitializeEnvironmentDataList();
        LoadEnvironmentDataList();
    }

    private void InitializeEnvironmentDictionary()
    {
        _environmentDictionary = new Dictionary<EnvironmentType, SingleEnvironmentController>();

        foreach (SingleEnvironmentController controller in _singleEnvironmentControllers)
        {
            if (!_environmentDictionary.ContainsKey(controller.EnvironmentType))
            {
                _environmentDictionary.Add(controller.EnvironmentType, controller);
            }
        }
    }

    private void InitializeEnvironmentDataList()
    {
        if (PlayerPrefs.HasKey(PlayerPrefsHelper.EnvironmentDataKey)) return;

        List<EnvironmentData> initialDataList = new List<EnvironmentData>();

        foreach (SingleEnvironmentController controller in _singleEnvironmentControllers)
        {
            if (controller.EnvironmentType == EnvironmentType.SalesTable)
            {
                EnvironmentData salesTableData = new EnvironmentData(controller.EnvironmentType, true, false, controller.CurrentPrice);
                initialDataList.Add(salesTableData);
                continue;
            }

            EnvironmentData data = new EnvironmentData(controller.EnvironmentType, false, false, controller.CurrentPrice);
            initialDataList.Add(data);
        }

        PlayerPrefsHelper.SaveEnvironmentDataList(initialDataList);
    }

    private void LoadEnvironmentDataList()
    {
        List<EnvironmentData> dataList = PlayerPrefsHelper.LoadEnvironmentDataList();

        foreach (SingleEnvironmentController controller in _singleEnvironmentControllers)
        {
            int index = dataList.FindIndex(data => data.environmentType == controller.EnvironmentType);
            if (index >= 0)
            {
                controller.SetEnvironmentData(dataList[index], this);
            }
        }
    }

    public void UpdateEnvironmentDataList()
    {
        List<EnvironmentData> dataList = PlayerPrefsHelper.LoadEnvironmentDataList();

        foreach (SingleEnvironmentController controller in _singleEnvironmentControllers)
        {
            EnvironmentData updatedData = controller.GetEnvironmentData();

            int index = dataList.FindIndex(data => data.environmentType == updatedData.environmentType);
            if (index >= 0)
            {
                dataList[index] = updatedData;
            }
            else
            {
                dataList.Add(updatedData);
            }
        }

        PlayerPrefsHelper.SaveEnvironmentDataList(dataList);
    }
}
