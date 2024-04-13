using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour, Controls.IPlayerActions
{
    private Controls controls;
    private bool moveKeyHeld;

    [SerializeField] private int exp = 0;
    [SerializeField] private int level = 1;
    [SerializeField] private int maxLevel = 10;
    [SerializeField] private int expNeeded = 100;
    [SerializeField] private int mana = 100;
    [SerializeField] private int maxMana = 100;
    [SerializeField] private int stamina = 100;
    [SerializeField] private int maxStamina = 100;
    [SerializeField] private float critRate = 0.2f;
    [SerializeField] private float critDamage = 1.5f;
    [SerializeField] private float luck = 0.1f;
    [SerializeField] private int manaRegen = 1;
    [SerializeField] private float manaRegenRate = 1f;
    [SerializeField] private float manaRegenCounter = 2f;
    [SerializeField] private SkillManager skillManager;
    [SerializeField] private int enemiesKilled = 0;
    private float attackCd = 0.7f;
    private float timer = 0f;
    private float weaponCd = 0f;
    private float weaponTimer = 0f;
    private float dashCd = 0.8f;
    private float dashTimer = 0f;
    private float dashDuration = 0.15f;
    private float dashTimerDuration = 0f;
    private float staminaTimer = 1f;
    private int staminaRegen = 7;
    private float staminaRegenRate = 1f;
    private bool isDashing = false;

    public int Mana { get => mana; set => mana = value; }
    public int MaxMana { get => maxMana; }
    public int EnemiesKilled { get => enemiesKilled; set => enemiesKilled = value; }
    public int Exp { get => exp; set => exp = value; }
    public int Stamina { get => stamina; set => stamina = value; }
    public float CritRate { get => critRate; }
    public float CritDamage { get => critDamage; }
    public float Luck { get => luck; }
    public bool IsDashing { get => isDashing; set => isDashing = value; }
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
        if(context.performed){
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            Vector3Int gridPosition = MapManager.Instance.FloorMap.WorldToCell(mousePosition);
            Vector2 gridPosition2D = new Vector2(gridPosition.x, gridPosition.y);
            Vector2 playerPosition = transform.position;
            Vector2 direction = (gridPosition2D - playerPosition).normalized;
            // If the player has weapon, perform slash action, esle perform ranged attack
            if(GetComponent<Inventory>().Weapon != null ){
                if( weaponTimer <= 0 && GetComponent<Actor>().IsAlive && !UIManager.Instance.IsMenuOpen
                    && !UIManager.Instance.ContainsSkillButton(Mouse.current.position.ReadValue()))
                {
                    Action.SlashAction(GetComponent<Actor>(), direction);
                    weaponTimer = weaponCd;
                }
            }
            else{
                if(timer <= 0 && mana >= 2 && GetComponent<Actor>().IsAlive && !UIManager.Instance.IsMenuOpen &&
                    !UIManager.Instance.ContainsSkillButton(Mouse.current.position.ReadValue())){
                    Action.RangedAction(GetComponent<Actor>(), direction);
                    timer = attackCd;
                    mana -= 2;
                }
            }
        }
    }

    void Controls.IPlayerActions.OnDash(InputAction.CallbackContext context)
    {
        if(context.performed && !isDashing){
            if(stamina >= 20 && dashTimer <= 0 && GetComponent<Actor>().IsAlive){
                stamina -= 20;
                dashTimer = dashCd;
                isDashing = true;
            }
        }
    }

    void Controls.IPlayerActions.OnDropWeapon(InputAction.CallbackContext context)
    {
        // Drops currently equipped weapon
        if(context.performed)
        {
            if(GetComponent<Inventory>().Weapon != null){
                Weapon weapon = GetComponent<Inventory>().Weapon;
                GetComponent<Inventory>().DropWeapon();
                UIManager.Instance.AddMessage($"You dropped the {weapon.name}.", "#FF0000");
                UIManager.Instance.UpdateWeapon(GetComponent<Actor>());
            }
            else{
                UIManager.Instance.AddMessage("You have no weapon equipped!", "#FF0000");
            }
        }
    }


    void Controls.IPlayerActions.OnExit(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            if(!UIManager.Instance.IsEscapeMenuOpen && !UIManager.Instance.IsMenuOpen)
                UIManager.Instance.ToggleEscapeMenu();
            else if(UIManager.Instance.IsMenuOpen)
                UIManager.Instance.ToggleMenu();
            // UIManager.Instance.ToggleMenu(GetComponent<Actor>());
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
        if(GetComponent<Inventory>().Weapon != null){
            Weapon weapon = GetComponent<Inventory>().Weapon;
            weaponCd = weapon.AttackSpeed; 
            if(weaponTimer > 0)
                weaponTimer -= Time.fixedDeltaTime;
        }
        if(!UIManager.Instance.IsMenuOpen){
            if (GameManager.Instance.IsPlayerTurn && moveKeyHeld && GetComponent<Actor>().IsAlive)
            {
                if(!isDashing)
                    MovePlayer();
                else
                    MovePlayer(3f);
            }
        }
        UIManager.Instance.UpdateSkills(GetComponent<Actor>());

        if(stamina < maxStamina){
            staminaTimer -= Time.fixedDeltaTime;
            if(staminaTimer <= 0){
                if(stamina + staminaRegen > maxStamina)
                    stamina = maxStamina;
                else{
                    stamina += staminaRegen;
                }
                staminaTimer = staminaRegenRate;
            }
        }

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
        if(isDashing){
            dashTimerDuration += Time.fixedDeltaTime;
            if(dashTimerDuration >= dashDuration){
                isDashing = false;
                dashTimerDuration = 0;
            }
        }
        if(dashTimer > 0){
            dashTimer -= Time.fixedDeltaTime;
        }
        if(timer > 0){
            timer -= Time.fixedDeltaTime;
        }
        if(exp >= expNeeded){
            LevelUp();
        }
    }

    private void MovePlayer(float moveSpeed = 1f)
    {
        Vector2 direction = controls.Player.Movement.ReadValue<Vector2>();
        Vector3 roundedDirection = new Vector3(direction.x, direction.y, 0f) * GetComponent<Fighter>().MovementSpeed * Time.fixedDeltaTime * moveSpeed;
        Vector3 futurePosition = transform.position + roundedDirection;
        Action.MovementAction(GetComponent<Actor>(), roundedDirection);
        UpdateAnimation();
    }

    private void LevelUp()
    {
        if(level < maxLevel){
            level++;
            exp = 0;
            expNeeded = (int)(expNeeded * (1f + level * 0.1f));
            maxMana += 10;
            maxStamina += 10;
            mana = maxMana;
            stamina = maxStamina;
            UIManager.Instance.AddMessage($"You leveled up to level {level}!", "#00FF00");
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

    public void UpdateAnimation()
    {
        Vector2 movementInput = controls.Player.Movement.ReadValue<Vector2>();

        // Set animation parameters based on movement input
        animator.SetFloat("MoveX", movementInput.x);
        animator.SetFloat("MoveY", movementInput.y);
        // animator.SetBool("IsMoving", moveKeyHeld);
    }

    public PlayerState SaveState() => new PlayerState(
        mana: mana,
        maxMana: maxMana,
        enemiesKilled: enemiesKilled
    );

    public void LoadState(PlayerState state)
    {
        mana = state.Mana;
        enemiesKilled = state.EnemiesKilled;
    }
}

public class PlayerState
{
    public int mana;
    public int maxMana;
    public int enemiesKilled;
    public int Mana { get => mana; set => mana = value; }
    public int MaxMana { get => maxMana; }
    public int EnemiesKilled { get => enemiesKilled; set => enemiesKilled = value; }
    public PlayerState(int mana, int maxMana = 100, int enemiesKilled = 0)
    {
        this.mana = mana;
        this.maxMana = maxMana;
        this.enemiesKilled = enemiesKilled;
    }
   
}