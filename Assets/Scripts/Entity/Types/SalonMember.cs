using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SalonMember : Entity
{
    [Header("Salon Member Settings")]
    [SerializeField] private int damage;
    [SerializeField] private float speed = 1.5f;
    [SerializeField] private float attackCooldown;
    private float moveCooldown = 0.5f;
    private float remainingCooldown = 1f;
    private float moveTimer = 0f;
    private Vector3 direction;

    public int Damage { get => damage; set => damage = value; }
    public float Speed { get => speed; }
    public float AttackCooldown { get => attackCooldown; set => attackCooldown = value; }

    // Start is called before the first frame update
    void Start()
    {
        AddToGameManager();
        BlocksMovement = false;
    }

    // Update is called once per frame
    void Update()
    {
        remainingCooldown -= Time.deltaTime;
        moveTimer -= Time.deltaTime;
        MoveRandomly();
        AttackEnemies();
    }

    private void MoveRandomly(){
        if(moveTimer > 0) {
            Vector3 newPos = transform.position + direction * speed * Time.deltaTime;
            if(isValidMove(newPos))
                transform.position += direction * speed * Time.deltaTime;
        }
        else{
            direction = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0).normalized;
            moveTimer = moveCooldown;
        }
    }
    private void AttackEnemies(){
        if(remainingCooldown <= 0){
            remainingCooldown = attackCooldown;
            foreach(Entity entity in GameManager.Instance.Entities){
                if(entity.GetComponent<HostileEnemy>() && Vector3.Distance(transform.position, entity.transform.position) < 8f 
                    && entity.GetComponent<Actor>().IsAlive)
                {
                    Vector3 enemyPosition = entity.transform.position;
                    Vector2 gridPosition2D = new Vector2(enemyPosition.x, enemyPosition.y);
                    Vector2 salonPos = transform.position;
                    Vector2 direction = (gridPosition2D - salonPos).normalized;
                    MapManager.Instance.CreateProjectile(salonPos, direction, damage, true);
                    break;
                }
            }
        }
    }

    private bool isValidMove(Vector3 newPos){
        Vector3Int gridPosition = MapManager.Instance.FloorMap.WorldToCell(newPos);
        if(!MapManager.Instance.InBounds(gridPosition.x, gridPosition.y) || MapManager.Instance.ObstacleMap.HasTile(gridPosition) || newPos == transform.position)
            return false;
        return true;
    }

}
