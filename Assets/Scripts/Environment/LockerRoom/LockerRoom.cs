using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LockerRoom : MonoBehaviour
{
    [SerializeField] private List<ChangingCubicle> _cubicles;

    public ChangingCubicle GoLockerRoom() //CustomerController customer
    {
        var targetCubicle = _cubicles
            .Where(cubicle => cubicle.IsActive && cubicle.IsAvailable())
            .FirstOrDefault();

        if (targetCubicle != null)
        {
            return targetCubicle;
        }
        else
        {
            return null;
        }

    }
}
