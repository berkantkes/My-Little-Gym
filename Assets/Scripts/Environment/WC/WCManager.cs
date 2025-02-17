using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WCManager : MonoBehaviour
{
    [SerializeField] private List<ThreeWCController> _threeWcControllers;

    public void Initialize()
    {
        foreach (ThreeWCController controller in _threeWcControllers)
        {
            controller.Initialize();
        }
    }

    public ThreeWCController GetAvailableWCWC() //CustomerController customer
    {
        var targetWC = _threeWcControllers
            .Where(wc => wc.IsActive)
            .OrderBy(wc => wc.CustomerCount)
            .FirstOrDefault();

        if (targetWC != null)
        {
            //targetWC.SetCustomer(customer);
            return targetWC;
        }
        else
        {
            return null;
        }

    }
}
