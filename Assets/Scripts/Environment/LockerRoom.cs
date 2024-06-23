using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LockerRoom : MonoBehaviour
{
    [SerializeField] private List<ChangingCubicle> _cubicles;

    public ChangingCubicle GoLockerRoom() //CustomerController customer
    {
        var targetCubicle = _cubicles
            .Where(cubicle => cubicle.IsActive)
            .OrderBy(cubicle => cubicle.CustomerCount)
            .FirstOrDefault();

        if (targetCubicle != null)
        {
            //targetCubicle.AddCustomerQueue(customer);
            return targetCubicle;
            Debug.Log("Customer added to cubicle with least queue.");
        }
        else
        {
            return null;
            Debug.Log("No active cubicle available.");
        }

    }
}
