using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CleanerController : MonoBehaviour
{
    [SerializeField] private SportsAreaManager _sportsAreaManager;
    [SerializeField] private NavMeshAgent _navMeshAgent;
    [SerializeField] private Transform _startPosition;

    private SportMachineController _currentTargetMachine;
    private bool _isCleaning;

    private void Start()
    {
        FindAndSetTarget();
    }

    private void Update()
    {
        if (_currentTargetMachine == null || _currentTargetMachine.IsClean)
        {
            if (!_isCleaning)
            {
                FindAndSetTarget();
            }
        }
    }

    private void FindAndSetTarget()
    {
        var uncleanMachines = _sportsAreaManager.GetUncleanMachines();

        if (uncleanMachines.Count > 0)
        {
            _currentTargetMachine = uncleanMachines[Random.Range(0, uncleanMachines.Count)];
            SetTarget(_currentTargetMachine.transform.position);
        }
        else
        {
            _currentTargetMachine = null;
            SetTarget(_startPosition.position + new Vector3(0,0.02f,0));
        }
    }

    public void SetTarget(Vector3 target)
    {
        _navMeshAgent.SetDestination(target);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_currentTargetMachine != null && other.gameObject == _currentTargetMachine.gameObject)
        {
            CleanMachine(_currentTargetMachine).Forget(); 
        }
    }

    private async UniTask CleanMachine(SportMachineController machine)
    {
        _isCleaning = true;
        machine.PassingCleanMachineTime();

        await UniTask.WaitUntil(() => machine.IsClean);

        machine.StopCleanMachineTiming();
        _currentTargetMachine = null; 
        _isCleaning = false;
        FindAndSetTarget();
    }
}
