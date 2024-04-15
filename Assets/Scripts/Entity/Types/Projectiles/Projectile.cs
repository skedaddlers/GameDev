using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Entity
{
    // Start is called before the first frame update
    [SerializeField] protected float speed;
    [SerializeField] protected int damage = 5;
    [SerializeField] protected float rotation;
    [SerializeField] protected Vector3 direction;
    [SerializeField] protected bool isPlayerProjectile = false;
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
        Debug.Log("Projectile created");
        AddToGameManager();
        transform.rotation = Quaternion.Euler(0, 0, rotation);
    }
    
    void Update()
    {
        Debug.Log("Projectile updated");
        if(isPlayerProjectile){
            Actor player = GameManager.Instance.Actors[0];
            float xDistance = Mathf.Abs(player.transform.position.x - transform.position.x);
            float yDistance = Mathf.Abs(player.transform.position.y - transform.position.y);
            if(xDistance > Camera.main.orthographicSize * Camera.main.aspect || yDistance > Camera.main.orthographicSize){
                // Destroy(gameObject);
                return;
            }
        }
        
        transform.position += direction * speed * Time.deltaTime;
        if(targetPosition != Vector3.zero){
            if(Vector3.Distance(transform.position, targetPosition) < 0.1f){
                if(isAOE){
                    Debug.Log("AOE");
                    AOEAttack();
                }
            }
        }
        CheckForCollision();
    }

    
    
    public virtual void CheckForCollision(){
        Actor target = null;
        if(isPlayerProjectile){
            target = GameManager.Instance.GetBlockingActorAtLocation(transform.position);
        }
        else{
            target = GameManager.Instance.GetBlockingPlayerAtLocation(transform.position);
        }
        Vector3Int gridPos = MapManager.Instance.FloorMap.WorldToCell(transform.position);
        if(MapManager.Instance.InBounds((int)gridPos.x, (int)gridPos.y) || MapManager.Instance.ObstacleMap.HasTile(gridPos)){
            if(isAOE){
                AOEAttack();
            }
            GameManager.Instance.RemoveEntity(this);
            // GameObject.Destroy(gameObject);
            return;
        }

        if(target != null){
            if(isAOE){
                AOEAttack();
            }
            else{
                int targetDefense = target.GetComponent<Fighter>().Defense;
                target.GetComponent<Fighter>().TakeDamage(damage * (int)(0.5f + (1 - (targetDefense / 20f))/2));
            }
            GameManager.Instance.RemoveEntity(this);
            // GameObject.Destroy(gameObject);
        }
    }

    protected void AOEAttack(){
        foreach(Actor target in GameManager.Instance.Actors) {
            if ((isPlayerProjectile && target.GetComponent<Player>()) || (!isPlayerProjectile && !target.GetComponent<Player>()))
                continue;

            float distance = Vector3.Distance(transform.position, target.transform.position);
            if (distance <= aoeRadius) {
                int targetDefense = target.GetComponent<Fighter>().Defense;
                target.GetComponent<Fighter>().TakeDamage(damage * (int)(0.5f + (1 - (targetDefense / 20f)) / 2));
            }
        }
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