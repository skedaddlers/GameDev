using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Entity
{
    // Start is called before the first frame update
    [SerializeField] private float speed;
    [SerializeField] private int damage = 5;
    //rotation var
    [SerializeField] private float rotation;
    [SerializeField] private Vector3 direction;
    [SerializeField] private bool isPlayerProjectile = false;
    [SerializeField] protected bool isAOE = false;
    [SerializeField] protected float aoeRadius = 1f;
    [SerializeField] protected Vector3 targetPosition = Vector3.zero;
    public int Damage { get => damage; set => damage = value;}
    public Vector3 Direction { get => direction; set => direction = value; }
    public bool IsPlayerProjectile { get => isPlayerProjectile; set => isPlayerProjectile = value; }
    public float Rotation { get => rotation; set => rotation = value; }
    public bool IsAOE { get => isAOE; set => isAOE = value; }
    public float AoeRadius { get => aoeRadius; set => aoeRadius = value; }
    public Vector3 TargetPosition { get => targetPosition; set => targetPosition = value; }
    void Start()
    {
        transform.rotation = Quaternion.Euler(0, 0, rotation);
    }
    
    void Update()
    {
        if(isPlayerProjectile){
            Actor player = GameManager.Instance.Actors[0];
            float xDistance = Mathf.Abs(player.transform.position.x - transform.position.x);
            float yDistance = Mathf.Abs(player.transform.position.y - transform.position.y);
            if(xDistance > Camera.main.orthographicSize * Camera.main.aspect || yDistance > Camera.main.orthographicSize){

                Destroy(gameObject);
                return;
            }
        }
        if(targetPosition != Vector3.zero){
            if(Vector3.Distance(transform.position, targetPosition) < 0.1f){
                if(isAOE){
                    Debug.Log("AOE");
                    Action.AOEAttack(this);
                }
                else{
                    Action.CheckForCollision(this);
                }
                Destroy(gameObject);
            }
        }

        transform.position += direction * speed * Time.deltaTime;
        Action.CheckForCollision(this);
    }

    public override EntityState SaveState() => new ProjectileState(
        name: name,
        blocksMovement: BlocksMovement,
        position: transform.position,
        direction: direction
    );

    public void LoadState(ProjectileState state){
        transform.position = state.Position;
        direction = state.Direction;
    }

}

[System.Serializable]
public class ProjectileState : EntityState {
    [SerializeField] private Vector3 direction;
    public Vector3 Direction { get => direction; set => direction = value; }
    public ProjectileState(EntityType type = EntityType.Projectile, string name = "", bool blocksMovement = false, Vector3 position = new Vector3(), 
    Vector3 direction = new Vector3()) : base(type, name, blocksMovement, position)
    {
        this.direction = direction;
    }
}