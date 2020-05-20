
using UnityEngine;
using StateStuff;
using Pathfinding;
using MoonSharp.Interpreter;
using System;

public class ViligerGo : State<AIViliger>
{
    AIViliger thisViliger;
    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;
    private float nextUpdate = 1f;
    Seeker seeker;
    Rigidbody2D rb;
    Script script;
    private static ViligerGo _instance;
    private ViligerGo()
    {
        if (_instance != null)
        {
            //  return;
        }
        _instance = this;
    }
    public static ViligerGo Instance
    {
        get
        {
            // if (_instance == null)
            // {
            new ViligerGo();
            // }

            return _instance;
        }
    }
    public override void EnterState(AIViliger _owner)
    {
        thisViliger = _owner;
        reachedEndOfPath = false;
        currentWaypoint = 0;
        seeker = _owner.GetComponent<Seeker>();
        rb = _owner.GetComponent<Rigidbody2D>();
        UpdatePath(_owner);

    }
    public override void ExitState(AIViliger _owner)
    {
        _owner.Animator.SetBool("moving", false);

    }
    public override void UpdateState(AIViliger _owner)
    {
        if (Time.time >= nextUpdate)
        {
            nextUpdate = Mathf.FloorToInt(Time.time) + 1;
            UpdatePath(_owner);
        }



        if (path == null)
        {
            //Debug.Log("ViligergGo null path");
            return;
        }

        if (_owner.targetGoTo != null)
        {

            if (currentWaypoint >= path.vectorPath.Count)
            {

                reachedEndOfPath = true;

                if (Vector2.Distance(_owner.transform.position, new Vector2(_owner.targetGoTo.transform.position.x, _owner.targetGoTo.transform.position.y)) <= 3)
                {

                    _owner.Animator.SetBool("moving", false);
                    if (_owner.stateChain.Count > 0)
                    {
                        _owner.stateMachine.ChangeState(_owner.stateChain.Pop());
                    }
                }
                else
                {
                    _owner.stateMachine.ChangeState(VilligerLookingForJobWithTool.Instance);
                    reachedEndOfPath = true;
                }
            }
        }
        else
        {
            _owner.stateMachine.ChangeState(VilligerLookingForJobWithTool.Instance);
            reachedEndOfPath = true;
        }
        if (!reachedEndOfPath)
        {          
            Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
            Vector2 force = direction * _owner.viligerStatistics.speed.Value / 20 * Time.deltaTime;
            _owner.Walking(force * 1000);
            rb.MovePosition(new Vector2(rb.transform.position.x, rb.transform.position.y) + force);
            foreach (SpriteRenderer spriteRenderer in _owner.spriteRenderers)
            {
                spriteRenderer.sortingOrder = -(int)_owner.transform.position.y + 2;
            }
            float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
            if (distance < 1f)
            {
                currentWaypoint++;
            }
        }
    }

    public float DistanceToCurrentWaypoint()
    {
        return Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
    }
    public void MoveViligerToCurrentWaypoint()
    {
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * thisViliger.viligerStatistics.speed.Value / 20 * Time.deltaTime;
        thisViliger.Walking(force * 1000);
        rb.MovePosition(new Vector2(rb.transform.position.x, rb.transform.position.y) + force);
        foreach (SpriteRenderer spriteRenderer in thisViliger.spriteRenderers)
        {
            spriteRenderer.sortingOrder = -(int)thisViliger.transform.position.y + 2;
        }
    }
    public bool IsThisLastWaypoint()
    {
        if (currentWaypoint >= path.vectorPath.Count)
        {
            return true;

        }
        else
        {
            return false;
        }
    }
    public void UpdatePath()
    {
        if (Time.time >= nextUpdate)
        {
            nextUpdate = Mathf.FloorToInt(Time.time) + 1;
            UpdatePath(thisViliger);
        }
    }
    public bool IsPathFinded()
    {
        if (path != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    void UpdatePath(AIViliger _owner)
    {
        if (_owner.targetGoTo != null)
        {
            if (seeker.IsDone())
                seeker.StartPath(rb.position, _owner.targetGoTo.transform.position, OnPathComplete);
        }
        else { Debug.LogError("null cant create path"); }
    }
    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    /*  void LookAt(Transform at, AIViliger _owner)
      {
          change = Vector3.zero;
          change.x = (at.position.x-_owner.transform.position.x); //Input.GetAxisRaw("Horizontal");
          change.y = (at.position.y - _owner.transform.position.y); //Input.GetAxisRaw("Vertical");
          //change.x = Mathf.Round(change.x);
          //change.y = Mathf.Round(change.y);
          if (change.x * change.x > change.y * change.y)
              change.y = 0;
          else
              change.x = 0;
          _owner.Animator.SetFloat("moveX", change.x);
          _owner.Animator.SetFloat("moveY", change.y);
          _owner.Animator.SetBool("moving", false);
      }*/
}



