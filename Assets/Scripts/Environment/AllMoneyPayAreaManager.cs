using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllMoneyPayAreaManager : MonoBehaviour
{
    //[SerializeField] private List<MoneyPayAreaController> _singleMoneyPayAreaControllers;

    //public void Initialize()
    //{
    //    List<int> openMoneyAreas = GetOpenPayAreaIndicesStart();

    //    if (openMoneyAreas.Count == 0)
    //    {
    //        openMoneyAreas.Add(0);
    //    }

    //    foreach (var index in openMoneyAreas)
    //    {
    //        _singleMoneyPayAreaControllers[index].Initialize(this);
    //    }
    //}

    //public void OpenSingleMoneyArea()
    //{
    //    List<int> openMoneyAreas = GetOpenPayAreaIndices();
    //    PlayerPrefsHelper.SetIntList(PlayerPrefsHelper.IntListKey, openMoneyAreas);
    //}

    //public void CloseSingleMoneyArea()
    //{
    //    List<int> openMoneyAreas = GetOpenPayAreaIndices();
    //    PlayerPrefsHelper.SetIntList(PlayerPrefsHelper.IntListKey, openMoneyAreas);
    //}

    //public List<int> GetOpenPayAreaIndices()
    //{
    //    List<int> openIndices = new List<int>();

    //    for (int i = 0; i < _singleMoneyPayAreaControllers.Count; i++)
    //    {
    //        if (_singleMoneyPayAreaControllers[i].IsOpen)
    //        {
    //            openIndices.Add(i);
    //        }
    //    }

    //    return openIndices;
    //}
    //public List<int> GetOpenPayAreaIndicesStart()
    //{
    //    List<int> openIndices = PlayerPrefsHelper.GetIntList(PlayerPrefsHelper.IntListKey);

    //    return openIndices;
    //}

}
