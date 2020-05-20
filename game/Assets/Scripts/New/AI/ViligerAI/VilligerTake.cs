
using UnityEngine;
using StateStuff;
using Pathfinding;
public class ViligerTake : State<AIViliger>
{


    private Vector3 change;
    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    Seeker seeker;
    Rigidbody2D rb;


    private static ViligerTake _instance;
    private ViligerTake()

    {
        if (_instance != null)
        {
            //  return;
        }
        _instance = this;
    }
    public static ViligerTake Instance
    {
        get
        {
            // if (_instance == null)
            // {
            new ViligerTake();
            // }
            return _instance;
        }
    }
    public override void EnterState(AIViliger _owner)
    {
        _owner.targetGoTo = null;
        _owner.StartCoroutine(_owner.TakeCo());
    }
    public override void ExitState(AIViliger _owner)
    {
    }
    public override void UpdateState(AIViliger _owner)
    {
        if (!_owner.corutineIsRunning)
        {
            if (_owner.Animator.GetBool("carry"))
            {
                foreach (GameObject item in GameObject.FindGameObjectsWithTag("Storage"))
                {
                    if (item.GetComponent<Building>().isConstructed)
                    {
                        _owner.targetGoTo = item;
                    }
                    if (_owner.targetGoTo != null)
                    {
                        _owner.stateChain.Push(ViligerTakeOf.Instance);
                        _owner.stateMachine.ChangeState(ViligerGo.Instance);
                    }
                }
            }
            else
            {
                _owner.stateMachine.ChangeState(VilligerLookingForJobWithTool.Instance);
            }
        }
    }
}



