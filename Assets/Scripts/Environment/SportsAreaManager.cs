using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SportsAreaManager : MonoBehaviour
{
    [SerializeField] private List<SportMachineController> _sportMachineControllers;

    public SportMachineController GetAvailableMachine(CustomerController customer)
    {
        // IsActive ve IsAvailable olan makineleri LINQ ile filtrele
        var availableMachines = _sportMachineControllers
            .Where(machine => machine.IsActive && machine.IsAvailable)
            .ToList();

        // Eðer hiç uygun makine yoksa iþlemi durdur
        if (availableMachines.Count == 0)
        {
            Debug.LogWarning("No available machines!");
            return null;
        }

        // Rastgele bir makine seç
        int randomIndex = Random.Range(0, availableMachines.Count);
        SportMachineController selectedMachine = availableMachines[randomIndex];

        // Seçilen makineye müþteri atama iþlemi yapabilirsiniz
        selectedMachine.SetCustomerToMachine(customer);
        return selectedMachine;
    }

    public bool IsAvailableSportMachine()
    {
        var availableMachines = _sportMachineControllers
            .Where(machine => machine.IsActive && machine.IsAvailable)
            .ToList();

        return availableMachines.Count != 0;
    }

}
