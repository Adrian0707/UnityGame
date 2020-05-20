using System.Collections;
using Pathfinding;
using UnityEngine;

public enum PlayerState
{
    walk,
    attack,
    interact,
    stagger,
    idle,
    ability
}

public class Movement : MonoBehaviour
{
    [Header("Movment")]
    public PlayerStatistics playerStatistics;
    public Joystick joystick;
    public PlayerState currentState;
    public PlayerHealth playerHealth;
    private Rigidbody2D myRigidbody;
    private Vector3 change;
    private Animator animator;
    public GenericMana GenericMana;
    //   public SpriteRenderer receivedItemSprite;
    public Signal2 playerHit;
    public Signal2 decreaseMagic;
    [Header("Projectile tuff")]
    public GameObject projectile;
    //  public Item bow;
    [Header("IFrame Stuff")]
    public Color flashColor;
    public Color regularColor;
    public float flashDuration;
    public int numberOfFlashes;
    public Collider2D triggerColider;
    public SpriteRenderer mySprite;
    [SerializeField] private GenericAbility currentAbility;
    // private Vector2 tempMovment = Vector2.down;
    private Vector2 facingDirection = Vector2.down;
    private SpriteRenderer spriteRenderer;


    // Start is called before the first frame update
    void Start()
    {
        currentState = PlayerState.walk;
        animator = GetComponent<Animator>();
        myRigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator.SetFloat("moveX", 0);
        animator.SetFloat("moveY", -1);
        // transform.position = startingPosition.initialValue;
    }
    public void Attack()
    {
        if (currentState != PlayerState.attack && currentState != PlayerState.stagger)
        {
            StartCoroutine(AttackCo());
        }
    }
    public void SecoundWeapon()
    {
        if (currentState != PlayerState.attack && currentState != PlayerState.stagger)
        {
            StartCoroutine(SecondAttackCo());
        }
    }
    public void Ability()
    {
        if (currentAbility)
        {
            if (GenericMana.currentMana >= 2)
            {
                StartCoroutine(AbilityCo(currentAbility.duration));
                GenericMana.DecreseMana(2);
            }
        }
    }
    void Update()
    {
        change = Vector3.zero;
        change.x = joystick.Horizontal;
        change.y = joystick.Vertical;
        spriteRenderer.sortingOrder = -(int)transform.position.y + 3;
        if (Input.GetButtonDown("attack") && currentState != PlayerState.attack
            && currentState != PlayerState.stagger)
        {
            Attack();
        }
        else if (Input.GetButtonDown("second weapon") && currentState != PlayerState.attack
            && currentState != PlayerState.stagger)
        {
            SecoundWeapon();
        }
        else if (Input.GetButtonDown("Ability"))
        {
            Ability();
        }
        else if (currentState == PlayerState.walk || currentState == PlayerState.idle)
        {
            UpadeAnimationAndMove();
        }

    }
    private IEnumerator AttackCo()
    {
        animator.SetBool("attacking", true);
        currentState = PlayerState.attack;
        yield return null;
        animator.SetBool("attacking", false);
        yield return new WaitForSeconds(.3f);
        if (currentState != PlayerState.interact)
        {
            currentState = PlayerState.walk;
        }

    }
    public IEnumerator AbilityCo(float abilityDuration)
    {
        currentState = PlayerState.ability;
        currentAbility.Ability(transform.position, facingDirection, animator, myRigidbody);
        playerHealth.gameObject.SetActive(false);
        yield return new WaitForSeconds(abilityDuration);
        playerHealth.gameObject.SetActive(true);
        currentState = PlayerState.idle;

    }
    private IEnumerator SecondAttackCo()
    {
        currentState = PlayerState.attack;
        yield return null;
        MakeArrow();
        yield return new WaitForSeconds(.3f);
        if (currentState != PlayerState.interact)
        {
            currentState = PlayerState.walk;
        }
    }
    private void MakeArrow()
    {
        if (GenericMana.currentMana >= projectile.GetComponent<Arrow>().magicCost)
        {
            Vector2 temp = new Vector2(animator.GetFloat("moveX"), animator.GetFloat("moveY"));
            Arrow arrow = Instantiate(projectile, transform.transform.position, Quaternion.identity).GetComponent<Arrow>();
            arrow.Setup(temp, ChooseArrowDirection());
            GenericMana.DecreseMana(arrow.magicCost);
            decreaseMagic.Raise();
        }
    }
    Vector3 ChooseArrowDirection()
    {
        float temp = Mathf.Atan2(animator.GetFloat("moveY"), animator.GetFloat("moveX")) * Mathf.Rad2Deg;
        return new Vector3(0, 0, temp);
    }
    void UpadeAnimationAndMove()
    {
        if (change != Vector3.zero)
        {
            MoveCharacter();
            change.x = Mathf.Round(change.x);
            change.y = Mathf.Round(change.y);
            animator.SetFloat("moveX", change.x);
            animator.SetFloat("moveY", change.y);
            animator.SetBool("moving", true);
            facingDirection = change;
        }
        else
        {
            animator.SetBool("moving", false);
        }
    }
    void MoveCharacter()
    {
        change.Normalize();
        if (change.x * change.y == 0)
            myRigidbody.MovePosition(transform.position + change * playerStatistics.speed.Value * Time.deltaTime);
        else
            myRigidbody.MovePosition(transform.position + change * playerStatistics.speed.Value / (float)1.4 * Time.deltaTime);
    }
    public void Knock(float knockTime)
    {
        if (currentState != PlayerState.stagger)
        {
            currentState = PlayerState.stagger;
            StartCoroutine(KnockCo(knockTime));
        }
    }
    private IEnumerator KnockCo(float knockTime)
    {
        if (myRigidbody != null)
        {
            StartCoroutine(FlashCo());
            yield return new WaitForSeconds(knockTime);
            myRigidbody.velocity = Vector2.zero;
            myRigidbody.velocity = Vector2.zero;
        }
    }
    private IEnumerator FlashCo()
    {
        int temp = 0;
        triggerColider.enabled = false;
        while (temp < numberOfFlashes)
        {
            mySprite.color = flashColor;
            yield return new WaitForSeconds(flashDuration);
            mySprite.color = regularColor;
            yield return new WaitForSeconds(flashDuration);
            temp++;
        }
        triggerColider.enabled = true;
        currentState = PlayerState.idle;
    }
}
