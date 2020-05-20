using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateStuff;
using UnityEngine.UI;
using TMPro;

public class AIViliger : MonoBehaviour
{
    public Scripts aiScripts;
    GameObject fireplace;
    public ViligerStatistics viligerStatistics;
    public AstarPath astar;
    public GameObject targetGoTo;
    public Vector3 targetGoPosition;
    public SpriteRenderer[] spriteRenderers;
   // public float speed = 200f;
    public float nextWaypountDistance = 1f;
    

    public int resourcesHeld;
    [HideInInspector] public Sprite resorceImage;
    [HideInInspector] public string resourceName;

    private Animator animator;
    public Animator Animator { get => animator; }

   // private bool switchState = false;
    private ViligearIA viliger;
    [HideInInspector]public bool corutineIsRunning=false;

    public string currentState;
    public StateMachine<AIViliger> stateMachine { get; set; }
    public Stack <State<AIViliger>> stateChain;
    public TextMeshProUGUI scriptsInfo;
    private void Start()
    {
        fireplace = GameObject.FindGameObjectWithTag("Fireplace");
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        stateChain = new Stack<State<AIViliger>>();
        viliger = GetComponent<ViligearIA>();
        stateMachine = new StateMachine<AIViliger>(this);
        stateMachine.ChangeState(VilligerIdle.Instance);
        animator = gameObject.transform.Find("Sprite").GetComponent<Animator>();
        resourcesHeld = 0;
    
    }
    private void Update()
    {
        
        /*if (Time.time > gameTimer + 1)
        {
            gameTimer = Time.time;
            seconds++;
            Debug.Log(seconds);
        }
        if (seconds == 1)
        {
            seconds = 0;
            switchState = !switchState;
        }*/
        //if(!bre)
        stateMachine.Update();
        currentState = stateMachine.curentState.ToString();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
      /*  if (collision.CompareTag("Enemies"))
        {
            //bre = true;
            Debug.LogError("Attttack");
            targetGoTo = GameObject.FindGameObjectWithTag("Fireplace");
            stateChain.Clear();
            //stateChain.Push(VilligerLookingForTool.Instance);
            stateMachine.ChangeState(ViligerGo.Instance);
            //bre = false;
        }*/
    }
    public void AddResource()
    {
        resourcesHeld += (int)viligerStatistics.gaderingEffectivnes.Value;
        if(resourcesHeld> (int)viligerStatistics.capicity.Value)
        {
            resourcesHeld = (int)viligerStatistics.capicity.Value;
        }
    }
    public bool MaxResurcesHeld()
    {
        if(resourcesHeld == (int)viligerStatistics.capicity.Value)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool NoResourcesHeld()
    {
        if (resourcesHeld == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool TargetObjectExist()
    {
        if(targetGoTo != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public string GetHeldToolName()
    {
        return gameObject.GetComponent<Tool>().Item.name;
    }
    public void Night()
    {
       // Debug.LogError("Night");
        stateChain.Push(stateMachine.curentState);
        stateChain.Push(ViligerNight.Instance);
        targetGoPosition = GameObject.FindGameObjectWithTag("Fireplace").GetComponent<Fireplace>().PositionInCircle();
        stateMachine.ChangeState(ViligerGoTPosition.Instance);
        //fireplace.GetComponent<Fireplace>().viligersNight.Add(this.gameObject);

 
    }
    public void Day()
    {
        // fireplace.GetComponent<Fireplace>().viligersNight.Remove(this.gameObject);
        //  Debug.LogError("Day");
        // stateChain.Clear();
        if (currentState == "ViligerNight")
        {
            try
            {
                stateMachine.ChangeState(stateChain.Pop());
            }
            catch (System.Exception)
            {
                if (GetComponent<Tool>().Item != null)
                {
                    if (resourcesHeld > 0)
                    {
                        Animator.SetBool("carry", true);
                    }
                    stateMachine.ChangeState(VilligerLookingForJobWithTool.Instance);
                }
                else
                {
                    //fireplace.GetComponent<Fireplace>().viligers.AddFirst(this.gameObject);
                }
            }
        }
       
            
            
        
    }
    public IEnumerator AttackCo()
    {
        /*int i = 0;
        while (tree.health >= 0 && i < 100)
        {*/
        corutineIsRunning = true;
            animator.SetBool("attacking", true);
            yield return null;
            yield return new WaitForSeconds(.3f);

            animator.SetBool("attacking", false);


            //yield return new WaitForSeconds(.3f);
            ///tree.Attack(1);
            //i++;
            yield return new WaitForSeconds(3f);
        corutineIsRunning = false;
       // }

    }
    public IEnumerator TakeCo()
    {
       
        corutineIsRunning = true;
        
        animator.SetBool("carry", true);
        yield return null;
        yield return new WaitForSeconds(1f);
        transform.Find("Sprite").Find("Received Item").GetComponent<SpriteRenderer>().sprite = resorceImage;
        //Destroy(target1.gameObject);
        yield return new WaitForSeconds(.2f);
        
       
        corutineIsRunning = false;
       

    }
    public IEnumerator TakeToolCo()
    {

        corutineIsRunning = true;

        animator.SetBool("receiveItem", true);
        transform.Find("Tool").GetComponent<SpriteRenderer>().sprite = targetGoTo.GetComponent<Tool>().Item.itemImage;
       //yield return null;
        yield return new WaitForSeconds(.2f);
        //yield return new WaitForSeconds(.3f);
        
        //yield return new WaitForSeconds(3f);
        //transform.FindChild("Received Item").GetComponent<SpriteRenderer>().sprite = null;
        gameObject.GetComponent<Tool>().Item = targetGoTo.GetComponent<Tool>().Item;
        yield return null;
        Destroy(targetGoTo.gameObject);
        yield return null;
        animator.SetBool("receiveItem", false);
        yield return new WaitForSeconds(.2f);
        
       
        


        corutineIsRunning = false;


    }
    public IEnumerator TakeCoOf()
    {

        corutineIsRunning = true;
        yield return null;
        animator.SetBool("carry", false);
        if (GetComponent<Tool>().Item.itemName == "Axe") { 
        GameObject.FindGameObjectWithTag("GUI").GetComponent<Gui>().ModifyWood(resourcesHeld);
        }
        else if(GetComponent<Tool>().Item.itemName == "Pick"){
         GameObject.FindGameObjectWithTag("GUI").GetComponent<Gui>().ModifyStone(resourcesHeld);
        }
        resourcesHeld = 0;
        resourceName = null;
        resorceImage = null;
        yield return null;
        yield return new WaitForSeconds(.4f);
        transform.Find("Sprite").Find("Received Item").GetComponent<SpriteRenderer>().sprite = null;
        yield return new WaitForSeconds(.4f);

       
        corutineIsRunning = false;
    

    }
    public IEnumerator WaitForS(float sec)
    {


        corutineIsRunning = true;
        yield return new WaitForSeconds(sec);


        corutineIsRunning = false;


    }
    public IEnumerator CantReachTarget()
    {


        corutineIsRunning = true;
        this.gameObject.transform.Find("Dialog").gameObject.SetActive(true);
        this.gameObject.transform.Find("Dialog").transform.Find("NoWay").gameObject.SetActive(true);
        animator.SetBool("moving", false);

       

        
        
        
        yield return new WaitForSeconds(5f);
        this.gameObject.transform.Find("Dialog").transform.Find("NoWay").gameObject.SetActive(false);
        this.gameObject.transform.Find("Dialog").gameObject.SetActive(false);
        stateMachine.ChangeState(ViligerGo.Instance);
        //yield return new WaitForSeconds(30f);
        corutineIsRunning = false;


    }
    public IEnumerator LookingforTarget()
    {


        corutineIsRunning = true;
        this.gameObject.transform.Find("Dialog").gameObject.SetActive(true);
        this.gameObject.transform.Find("Dialog").transform.Find("NoTarget").gameObject.SetActive(true);

        animator.SetBool("moving", false);






        yield return new WaitForSeconds(5f);
        this.gameObject.transform.Find("Dialog").transform.Find("NoTarget").gameObject.SetActive(false);
        this.gameObject.transform.Find("Dialog").gameObject.SetActive(false);
        stateMachine.ChangeState(ViligerGo.Instance);
        //yield return new WaitForSeconds(30f);
        corutineIsRunning = false;


    }

    public bool IsTargetBuildingIsConstricted()
    {
        if (this.targetGoTo.GetComponent<Building>() != null)
        {
            if (this.targetGoTo.GetComponent<Building>().isConstructed)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return true;
        }
    }
    public bool IsViligerBusy()
    {
        if (this.corutineIsRunning)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void ViligerBuild()
    {
        this.StartCoroutine(this.AttackCo());
        this.targetGoTo.GetComponent<Building>().Construct(1);
    }
    public bool ViligerStateChange(string state)
    {
        if (state == "ViligerGo")
        {
            this.stateMachine.ChangeState(ViligerGo.Instance);
            return true;
        }
        else if (state == "ViligerLookingForJobWithTool")
        {
            this.stateMachine.ChangeState(VilligerLookingForJobWithTool.Instance);
            return true;
        }

        return false;
    }
    public void ViligerGatherWood()
    {
        if (this.NoResourcesHeld())
        {
            this.resourceName = this.targetGoTo.name;
            this.resorceImage = this.targetGoTo.GetComponent<Tree>().dropp;
        }
        if (!this.corutineIsRunning)
        {


            this.StartCoroutine(this.AttackCo());
            this.AddResource();
            this.targetGoTo.GetComponent<Tree>().Damage(.5f);
        }
    }
    public void ViligerGatherStone()
    {
        if (this.NoResourcesHeld())
        {
            this.resourceName = this.targetGoTo.name;
            this.resorceImage = this.targetGoTo.GetComponent<Stone>().dropp;
        }
        if (!this.corutineIsRunning)
        {


            this.StartCoroutine(this.AttackCo());
            this.AddResource();
            this.targetGoTo.GetComponent<Stone>().Damage(.5f);
        }
    }
    public void TakeResources()
    {
        this.stateMachine.ChangeState(ViligerTake.Instance);
    }
    public void DestroyOb(GameObject a)
    {
        Destroy(a.gameObject);
    }
    public void TakeNextStateFromStateChain()
    {
        this.stateMachine.ChangeState(this.stateChain.Pop());

    }
    public float DitanceToTargetObject()
    {
        return Vector2.Distance(this.transform.position, new Vector2(this.targetGoTo.transform.position.x, this.targetGoTo.transform.position.y));
    }
    public void Walking(Vector2 force)
    {
        Vector3 change = Vector3.zero;
        change.x = force.x; //Input.GetAxisRaw("Horizontal");
        change.y = force.y; //Input.GetAxisRaw("Vertical");
        change.x = Mathf.Round(change.x);
        change.y = Mathf.Round(change.y);
        this.Animator.SetFloat("moveX", change.x);
        this.Animator.SetFloat("moveY", change.y);
        this.Animator.SetBool("moving", true);
    }
    public bool AddToStateChain(string state)
    {
        if (state == "ViligerInteraction")
        {
            this.stateChain.Push(ViligerInteraction.Instance);
            return true;
        }
        return false;
    }
}
