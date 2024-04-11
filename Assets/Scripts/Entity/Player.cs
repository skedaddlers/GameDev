using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour, Controls.IPlayerActions
{
    private Controls controls;
    private bool moveKeyHeld;
    [SerializeField] private int mana = 100;
    [SerializeField] private int maxMana = 100;
    [SerializeField] private int manaRegen = 1;
    [SerializeField] private float manaRegenRate = 1f;
    [SerializeField] private float manaRegenCounter = 2f;
    [SerializeField] private SkillManager skillManager;
    [SerializeField] private int enemiesKilled = 0;
    private float rangedAttackCd = 0.7f;
    private float timer = 0f;

    public int Mana { get => mana; set => mana = value; }
    public int MaxMana { get => maxMana; }
    public int EnemiesKilled { get => enemiesKilled; set => enemiesKilled = value; }
    private Animator animator;

    private void Awake()
    {
        controls = new Controls();
        animator = GetComponent<Animator>();
        // Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y,0);
        skillManager = GetComponent<SkillManager>();
        InitializeSkills();
    }
    private void OnEnable()
    {
        controls.Player.SetCallbacks(this);
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Player.SetCallbacks(null);
        controls.Disable();
    }

    void Controls.IPlayerActions.OnMovement(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            moveKeyHeld = true;
        }
        else if (context.canceled)
        {
            moveKeyHeld = false;
        }

    }

    void Controls.IPlayerActions.OnClick(InputAction.CallbackContext context)
    {
        if(context.performed && GetComponent<Actor>().IsAlive && !UIManager.Instance.IsMenuOpen &&
            !UIManager.Instance.ContainsSkillButton(Mouse.current.position.ReadValue()) && timer <= 0
        )
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            Vector3Int gridPosition = MapManager.Instance.FloorMap.WorldToCell(mousePosition);
            Vector2 gridPosition2D = new Vector2(gridPosition.x, gridPosition.y);
            Vector2 playerPosition = transform.position;
            Vector2 direction = (gridPosition2D - playerPosition).normalized;
            MapManager.Instance.CreateProjectile(playerPosition, direction, GetComponent<Fighter>().Power);
            timer = rangedAttackCd;
            mana -= 2;
        }
    }

    void Controls.IPlayerActions.OnExit(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            UIManager.Instance.ToggleMenu(GetComponent<Actor>());
        }
    }

    public void OnView(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            if(!UIManager.Instance.IsMenuOpen || UIManager.Instance.IsMessageHistoryOpen)
                UIManager.Instance.ToggleMessageHistory();
        }
    }

    public void OnPickup(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            Action.PickupAction(GetComponent<Actor>());
        }
    }

    public void OnInventory(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            if(!UIManager.Instance.IsMenuOpen || UIManager.Instance.IsInventoryOpen){
                if(GetComponent<Inventory>().Items.Count > 0)
                    UIManager.Instance.ToggleInventory(GetComponent<Actor>());
                else
                    UIManager.Instance.AddMessage("Your inventory is empty!", "#FF0000");
            }
        }
    }

    public void OnDrop(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            if(!UIManager.Instance.IsMenuOpen || UIManager.Instance.IsDropMenuOpen){
                if(GetComponent<Inventory>().Items.Count > 0)
                    UIManager.Instance.ToggleDropMenu(GetComponent<Actor>());
                else
                    UIManager.Instance.AddMessage("Your inventory is empty!", "#FF0000");
            }
        }
    }

    private void FixedUpdate()
    {
        if(!UIManager.Instance.IsMenuOpen){
            if (GameManager.Instance.IsPlayerTurn && moveKeyHeld && GetComponent<Actor>().IsAlive)
            {
                MovePlayer();
            }
        }
        UIManager.Instance.UpdateSkills(GetComponent<Actor>());

        if(mana < maxMana){
            manaRegenCounter -= Time.fixedDeltaTime;
            if(manaRegenCounter <= 0){
                if(mana + manaRegen > maxMana)
                    mana = maxMana;
                else{
                    mana += manaRegen;
                }
                manaRegenCounter = manaRegenRate;
            }
        }
        if(timer > 0){
            timer -= Time.fixedDeltaTime;
        }
    }

    private void MovePlayer()
    {
        Vector2 direction = controls.Player.Movement.ReadValue<Vector2>();
        Vector3 roundedDirection = new Vector3(direction.x, direction.y, 0f) * GetComponent<Fighter>().MovementSpeed * Time.fixedDeltaTime;
        Vector3 futurePosition = transform.position + roundedDirection;

        // Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y,0);
        // Debug.Log(transform.position.x + " " + transform.position.y);
        if (isValidPosition(futurePosition))
        {
            moveKeyHeld = Action.BumpAction(GetComponent<Actor>(), roundedDirection);

            // Vector2 movementInput = controls.Player.Movement.ReadValue<Vector2>();
            // Vector3 movement = new Vector3(movementInput.x, movementInput.y, 0f) * movementSpeed * Time.fixedDeltaTime;
            // transform.position += movement;
            UpdateAnimation();
        }
    }

    private void InitializeSkills(){
        skillManager.AddSkill(new SalonSolitaire());
        skillManager.AddSkill(new LetThePeopleRejoice());
        skillManager.AddSkill(new SingerOfManyWaters());
        skillManager.AddSkill(new AuraOfTheFormerArchon());
        skillManager.AddSkill(new WatersAspirations());
        skillManager.AddSkill(new TearsOfTheSinners());

        
    }
    public void UseSkill(int index){
        skillManager.UseSkill(index);
    }
    private bool isValidPosition(Vector3 futurePosition)
    {
        Vector3Int gridPosition = MapManager.Instance.FloorMap.WorldToCell(futurePosition);
        if(!MapManager.Instance.InBounds(gridPosition.x, gridPosition.y) || MapManager.Instance.ObstacleMap.HasTile(gridPosition) || futurePosition == transform.position)
            return false;
        return true;
    }

    public void UpdateAnimation()
    {
        Vector2 movementInput = controls.Player.Movement.ReadValue<Vector2>();

        // Set animation parameters based on movement input
        animator.SetFloat("MoveX", movementInput.x);
        animator.SetFloat("MoveY", movementInput.y);
        // animator.SetBool("IsMoving", moveKeyHeld);
    }


}
