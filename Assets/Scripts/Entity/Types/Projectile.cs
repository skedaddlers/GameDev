using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Entity
{
    // Start is called before the first frame update
    [SerializeField] private float speed;
    [SerializeField] private int damage = 5;
    [SerializeField] private Vector3 direction;
    [SerializeField] private bool isPlayerProjectile = false;

    public int Damage { get => damage; set => damage = value;}
    public Vector3 Direction { get => direction; set => direction = value; }
    public bool IsPlayerProjectile { get => isPlayerProjectile; set => isPlayerProjectile = value; }

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