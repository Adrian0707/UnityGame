
using UnityEngine;
using StateStuff;
using Pathfinding;
using MoonSharp.Interpreter;
using System;

public class ViligerInteraction : State<AIViliger>
{
    AIViliger thisViliger;
    private int times;
    private static ViligerInteraction _instance;
    Script script;
    private ViligerInteraction()

    {
        if (_instance != null)
        {
          //  return;
        }
        _instance = this;
        times = 0;
    }
    public static ViligerInteraction Instance
    {
        get
        {
           // if (_instance == null)
            //{
                new ViligerInteraction();
            //}

            return _instance;
        }
    }
    public override void EnterState(AIViliger _owner)
    {
        thisViliger = _owner;
        script = new Script();
        script.Globals["GetHeldToolName"] = (Func<string>)_owner.GetHeldToolName;
        script.Globals["IsTargetBuildingIsConstricted"] = (Func<bool>)thisViliger.IsTargetBuildingIsConstricted;
        script.Globals["MaxResurcesHeld"] = (Func<bool>)thisViliger.MaxResurcesHeld;
        script.Globals["TargetObjectExist"] = (Func<bool>)thisViliger.TargetObjectExist;
        script.Globals["IsViligerBusy"] = (Func<bool>)thisViliger.IsViligerBusy;
        script.Globals["ViligerBuild"] = (Action)thisViliger.ViligerBuild;
        script.Globals["ViligerGatherWood"] = (Action)thisViliger.ViligerGatherWood;
        script.Globals["ViligerGatherStone"] = (Action)thisViliger.ViligerGatherStone;
        script.Globals["TakeResources"] = (Action)thisViliger.TakeResources;
        script.Globals["ViligerStateChange"] = (Func<string, bool>)thisViliger.ViligerStateChange;
    }

    public override void ExitState(AIViliger _owner)
    {
    }

    public override void UpdateState(AIViliger _owner)
    {
        // if (_owner.targetGoTo != null) 
        //   { 
        // LookAt(_owner.targetGoTo.transform, _owner);
      
        // MoonSharpFactorial(_owner);
        // }
        // else
        // {
        //_owner.StartCoroutine(_owner.LookingforTarget());
        //  _owner.stateMachine.ChangeState(ViligerAttackAnim.Instance);
        //  }
        if (thisViliger.aiScripts.viligerInteractionNormalMode) {
            if (_owner.gameObject.GetComponent<Tool>().Item.name == "Axe")
            {
                ActionForAxe(_owner);
            }
            if (_owner.gameObject.GetComponent<Tool>().Item.name == "Hammer")
            {
                ActionForHammer(_owner);
            }
            if (_owner.gameObject.GetComponent<Tool>().Item.name == "Pick")
            {
                ActionForPick(_owner);
            }
        }
        else
        {
            string scriptCode = _owner.aiScripts.viligerInteraction;                
            string scriptCode2 = scriptCode;
            thisViliger.aiScripts.viligerInteraction = scriptCode2;
            try
            {
                script.DoString(scriptCode2);
            }
            catch (Exception e)
            {
                _owner.scriptsInfo.text = "Prblems in script " + this.GetType()+" "+e.StackTrace;
            }
            script.DoString(scriptCode2);
        }
      
    }
    public void ActionForPick()
    {
        //gameobject destroyed 
       // Debug.LogError("For pick luaaa");
        if (!thisViliger.MaxResurcesHeld() && thisViliger.TargetObjectExist())
        {
            thisViliger.ViligerGatherStone();

        }
        else
        {
            if (thisViliger.MaxResurcesHeld())
            {
                thisViliger.TakeResources();
            }
            else
            {
                if (!thisViliger.IsViligerBusy())
                {
                    thisViliger.ViligerStateChange("VilligerLookingForJobWithTool");
                }
            }
        }
    }
    void ActionForAxe()
    {
        if (!thisViliger.MaxResurcesHeld() && thisViliger.TargetObjectExist())
        {

            thisViliger.ViligerGatherWood();
        }
        else
        {
            if (thisViliger.MaxResurcesHeld())
            {
                // _owner.target1 = GameObject.FindGameObjectWithTag("breakable");
                thisViliger.TakeResources();
            }
            else
            {
                if (!thisViliger.IsViligerBusy())
                {
                    //thisViliger.StartCoroutine(thisViliger.LookingforTarget());
                    thisViliger.ViligerStateChange("VilligerLookingForJobWithTool");
                }
            }
        }
    }
    void ActionForHammer()
    {
        if (!thisViliger.IsTargetBuildingIsConstricted())
        {
            if (!thisViliger.IsViligerBusy())
            {
                thisViliger.ViligerBuild();
            }
        }
        else
        {
            thisViliger.ViligerStateChange("VilligerLookingForJobWithTool");
        }
    }
    void ActionForPick(AIViliger _owner)
    {
        //gameobject destroyed 
        if (_owner.resourcesHeld <= _owner.viligerStatistics.capicity.Value && _owner.targetGoTo != null)
        {
            if (_owner.resourcesHeld == 0)
            {
                _owner.resourceName = _owner.targetGoTo.name;
                _owner.resorceImage = _owner.targetGoTo.GetComponent<Stone>().dropp;
            }
            if (!_owner.corutineIsRunning)
            {


                _owner.StartCoroutine(_owner.AttackCo());
                _owner.resourcesHeld++;
                _owner.targetGoTo.GetComponent<Stone>().Damage(1);
            }

        }
        else
        {
            if (_owner.resourcesHeld != 0)
            {
                // _owner.target1 = GameObject.FindGameObjectWithTag("breakable");
                _owner.stateMachine.ChangeState(ViligerTake.Instance);
            }
            else
            {
                _owner.stateMachine.ChangeState(VilligerLookingForJobWithTool.Instance);
            }
        }
    }
    void ActionForAxe(AIViliger _owner)
    {
        if (_owner.resourcesHeld <= _owner.viligerStatistics.capicity.Value && _owner.targetGoTo != null)
        {
            if (_owner.resourcesHeld == 0)
            {
                _owner.resourceName = _owner.targetGoTo.name;
                _owner.resorceImage = _owner.targetGoTo.GetComponent<Tree>().dropp;
            }
            if (!_owner.corutineIsRunning)
            {


                _owner.StartCoroutine(_owner.AttackCo());
                _owner.resourcesHeld++;
                _owner.targetGoTo.GetComponent<Tree>().Damage(.5f);
            }

        }
        else
        {
            if (_owner.resourcesHeld != 0)
            {
                // _owner.target1 = GameObject.FindGameObjectWithTag("breakable");
                _owner.stateMachine.ChangeState(ViligerTake.Instance);
            }
            else
            {
                if (!_owner.corutineIsRunning)
                {
                    _owner.StartCoroutine(_owner.LookingforTarget());
                    _owner.stateMachine.ChangeState(VilligerLookingForJobWithTool.Instance);
                }
            }
        }
    }
    void ActionForHammer(AIViliger _owner)
    {
        if (!_owner.targetGoTo.GetComponent<Building>().isConstructed)
        {
            if (!_owner.corutineIsRunning)
            {
                _owner.StartCoroutine(_owner.AttackCo());
                _owner.targetGoTo.GetComponent<Building>().Construct(1);
            }
        }
        else
        {
            _owner.stateMachine.ChangeState(VilligerLookingForJobWithTool.Instance);
        }
    }


}



