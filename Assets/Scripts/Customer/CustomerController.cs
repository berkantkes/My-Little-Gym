using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class CustomerController : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _navMeshAgent;
    [SerializeField] private Animator _animator;
    [SerializeField] private float _rotationSpeed = 5f;

    private CustomerStatus _customerStatus = CustomerStatus.WaitingArea; 
    private CustomersManager _customerManager;
    private bool _isExitLockerRoom = false;
    private bool _isEnterLockerRoom = false;
    private bool _isExitSportMachine = false;
    private bool _isExitWC = false;
    private bool _isMoving = false;
    public bool IsMoving => _isMoving;


    void Update()
    {
        bool moving = CheckIsMoving();
        if (moving != _isMoving)
        {
            _isMoving = moving;
            OnMovementStateChanged(_isMoving);
        }
        RotateTowardsMovementDirection();
    }

    private void OnMovementStateChanged(bool isMoving)
    {
        if (isMoving)
        {
            _animator.SetTrigger("Walking");
        }
        else
        {
            switch (_customerStatus)
            {
                case CustomerStatus.WaitingArea:
                    _animator.SetTrigger("Idle");
                    Debug.Log("WaitingArea");
                    break;
                case CustomerStatus.SportRunArea:
                    _animator.SetTrigger("Run");
                    Debug.Log("Run");
                    break;
                case CustomerStatus.WcArea:
                    _animator.SetTrigger("wc");
                    Debug.Log("Sitting");
                    break;
                case CustomerStatus.LockerRoomArea:
                    _animator.SetTrigger("LockerRoom");
                    Debug.Log("LockerRoomArea");
                    break;

            }
        }
    }

    public void Initialize(CustomersManager customersManager)
    {
        _customerManager = customersManager;
        OnMovementStateChanged(_isMoving);
    }

    private bool CheckIfReachedTarget()
    {
        if (!_navMeshAgent.pathPending)
        {
            if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
            {
                if (!_navMeshAgent.hasPath || _navMeshAgent.velocity.sqrMagnitude == 0f)
                {
                    return true;
                }
            }
        }
        return false;
    }


    public void SetTarget(Vector3 target)
    {
        _navMeshAgent.SetDestination(target);
    }

    public async void MoveToTargets(SportMachineController sportMachine, ChangingCubicle changingCubicle, WCManager wCManager)
    {
        _customerStatus = CustomerStatus.SportRunArea;
        SetTarget(sportMachine.transform.position);

        await UniTask.WaitUntil(() => CheckIfReachedTarget());
        sportMachine.PassingUseMachineTime();
        await UniTask.WaitUntil(() => _isExitSportMachine);

        bool shouldWaitWC = Random.value <= 1f;
        ThreeWCController threeWCController = wCManager.GetAvailableWCWC();
        if (shouldWaitWC && threeWCController != null)
        {
            _customerStatus = CustomerStatus.WcArea;
            threeWCController.AddCustomerQueue(this);
            await UniTask.WaitUntil(() => _isExitWC);
        }

        changingCubicle.AddCustomerQueue(this);

        await UniTask.WaitUntil(() => _isEnterLockerRoom && CheckIfReachedTarget());
        changingCubicle.PassingTime();
        _customerStatus = CustomerStatus.LockerRoomArea;

        await UniTask.WaitUntil(() => _isExitLockerRoom);

        SetTarget(new Vector3(-20, .1f, -20)); // exit area

        await UniTask.WaitUntil(() => CheckIfReachedTarget());
        _customerManager.ExitCustomer(this);
    }

    private void RotateTowardsMovementDirection()
    {
        if (_navMeshAgent.remainingDistance > _navMeshAgent.stoppingDistance)
        {
            // Obje hareket ediyorsa, gittiði yöne doðru döndür
            Vector3 direction = _navMeshAgent.velocity.normalized;
            Quaternion lookRotation = Quaternion.LookRotation(-direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
        //if (_navMeshAgent.velocity.sqrMagnitude > 0.1f)
        //{
        //    Vector3 direction = _navMeshAgent.steeringTarget - transform.position;
        //    direction.y = 0;
        //    if (direction.sqrMagnitude > 0.1f)
        //    {
        //        Quaternion targetRotation = Quaternion.LookRotation(-direction);
        //        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
        //    }
        //}
    }


    public void CustomerActivate()
    {
        gameObject.SetActive(true);

    }

    private bool CheckIsMoving()
    {
        return _navMeshAgent.hasPath && _navMeshAgent.velocity.sqrMagnitude > 0f;
    }

    public void SetExitLockerRoom(bool status)
    {
        _isExitLockerRoom = status;
    }
    public void SetEnterLockerRoom(bool status)
    {
        _isEnterLockerRoom = status;
    }
    public void SetExitSportMachine(bool status)
    {
        _isExitSportMachine = status;
    }
    public void SetExitWC(bool status)
    {
        _isExitWC = status;
    }
    public void SetNavMesh(bool status)
    {
        _navMeshAgent.enabled = status;
    }

    public void ExitCustomer()
    {
        _isExitLockerRoom = false;
        _isExitSportMachine = false;
        _isExitSportMachine = false;
    }
}

public enum CustomerStatus
{
    WaitingArea,
    SportRunArea,
    WcArea,
    LockerRoomArea
}
