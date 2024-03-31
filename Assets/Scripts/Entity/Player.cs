using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour, Controls.IPlayerActions
{
    private Controls controls;
    private bool moveKeyHeld;
    [SerializeField] private float movementSpeed = 5f; // Adjust this value to change movement speed

    private Animator animator;

    private void Awake()
    {
        controls = new Controls();
        animator = GetComponent<Animator>();
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

    void Controls.IPlayerActions.OnExit(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            Action.EscapeAction();
        }
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.IsPlayerTurn && moveKeyHeld && GetComponent<Actor>().IsAlive)
        {
            // Debug.Log("Player is moving");
            MovePlayer();
        }
    }

    private void MovePlayer()
    {
        Vector2 direction = controls.Player.Movement.ReadValue<Vector2>();
        Vector3 roundedDirection = new Vector3(direction.x, direction.y, 0f) * movementSpeed * Time.fixedDeltaTime;
        Vector3 futurePosition = transform.position + roundedDirection;
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
