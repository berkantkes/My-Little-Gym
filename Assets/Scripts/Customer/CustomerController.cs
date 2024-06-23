using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class CustomerController : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _navMeshAgent;

    private bool _isExitLockerRoom = false;
    private bool _isExitSportMachine = false;
    private bool _isMoving = false;
    public bool IsMoving => _isMoving;

    void Update()
    {
        if (CheckIsMoving())
        {
            _isMoving = true;
        }
        else
        {
            _isMoving = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<ChangingCubicle>() != null)
        {
            other.GetComponent<ChangingCubicle>().PassingTime();
        }
        if (other.GetComponent<SportMachineController>() != null)
        {
            other.GetComponent<SportMachineController>().PassingTime();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<ChangingCubicle>() != null)
        {
            other.GetComponent<ChangingCubicle>().StopTiming();
        }
        if (other.GetComponent<SportMachineController>() != null)
        {
            other.GetComponent<SportMachineController>().StopTiming();
        }
    }


    public void SetTarget(Vector3 target)
    {
        _navMeshAgent.SetDestination(target);
    }

    public async void MoveToTargets(Transform sportMachine)
    {
        await UniTask.WaitUntil(() => _isExitLockerRoom);
        SetTarget(sportMachine.position);
        await UniTask.WaitUntil(() => _isExitSportMachine);

        SetTarget(new Vector3(-20, .1f, -20));
        // exit area
    }


    private bool CheckIsMoving()
    {
        return _navMeshAgent.hasPath && _navMeshAgent.velocity.sqrMagnitude > 0f;
    }

    public void SetExitLockerRoom(bool status)
    {
        _isExitLockerRoom = status;
    }
    public void SetExitSportMachine(bool status)
    {
        _isExitSportMachine = status;
    }

    public void ExitCustomer()
    {
        _isExitLockerRoom = false;
        _isExitSportMachine = false;
    }
}
