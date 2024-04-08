using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour, Controls.IPlayerActions
{
    private Controls controls;
    private bool moveKeyHeld;
    [SerializeField] private float movementSpeed = 5f; // Adjust this value to change movement speed
    [SerializeField] private int mana = 100;

    private Animator animator;

    private void Awake()
    {
        controls = new Controls();
        animator = GetComponent<Animator>();
        // Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y,0);
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
        if(context.performed && GetComponent<Actor>().IsAlive && !UIManager.Instance.IsMenuOpen)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            Vector3Int gridPosition = MapManager.Instance.FloorMap.WorldToCell(mousePosition);
            Vector2 gridPosition2D = new Vector2(gridPosition.x, gridPosition.y);
            Vector2 playerPosition = transform.position;
            Vector2 direction = (gridPosition2D - playerPosition).normalized;
            MapManager.Instance.CreateProjectile(playerPosition, direction);
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
    }

    private void MovePlayer()
    {
        Vector2 direction = controls.Player.Movement.ReadValue<Vector2>();
        Vector3 roundedDirection = new Vector3(direction.x, direction.y, 0f) * movementSpeed * Time.fixedDeltaTime;
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
    
    // private void MovePlayer()
    // {
    //     Vector2 direction = controls.Player.Movement.ReadValue<Vector2>();
    //     Vector2 roundedDirection = new Vector2(Mathf.Round(direction.x), Mathf.Round(direction.y));
    //     Vector3 futurePosition = transform.position + (Vector3)roundedDirection * movementSpeed * Time.fixedDeltaTime;

    //     if(isValidPosition(futurePosition))
    //         Action.MovementAction(GetComponent<Entity>(), roundedDirection);
    //     // Vector2 movementInput = controls.Player.Movement.ReadValue<Vector2>();
    //     // Vector3 movement = new Vector3(movementInput.x, movementInput.y, 0f) * movementSpeed * Time.fixedDeltaTime;
    //     // transform.position += movement;
    //     // UpdateAnimation();
    // }

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
