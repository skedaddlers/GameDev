using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Fighter))]
public class HostileEnemy : Ai
{
    [SerializeField] protected Fighter fighter;
    [SerializeField] protected bool isFighting;
    [SerializeField] protected bool canTakeDamage = false;


    // [SerializeField] private float movementCooldown = 0.5f; // Adjust as needed
    [SerializeField] protected float attackCooldown = 0.1f; // Adjust as needed
    [SerializeField] int expGiven;

    [SerializeField]protected float attackTimer = 0.1f;

    public int ExpGiven { get => expGiven; set => expGiven = value; }
    public bool IsFighting { get => isFighting; set => isFighting = value; }
    public bool CanTakeDamage { get => canTakeDamage; set => canTakeDamage = value; }
    // private float movementTimer = 0.5f;
    private void OnValidateOverride(){
        fighter = GetComponent<Fighter>();
        AStar = GetComponent<AStar>();
    }

    private void Update(){
        if(attackTimer > 0){
            attackTimer -= Time.deltaTime;
        }
    }

    public virtual void RunAI(){
        
        if(!fighter.Target){
            fighter.Target = GameManager.Instance.Actors[0];
        }
        else if(fighter.Target && !fighter.Target.IsAlive){
            fighter.Target = null;
        }

        if(fighter.Target && fighter.Target.IsAlive){
            Vector3Int targetPos = MapManager.Instance.FloorMap.WorldToCell(fighter.Target.transform.position);
            // isFighting = true;
            if(isFighting || GetComponent<Actor>().FieldOfView.Contains(targetPos)){
                // Debug.Log("Fighting");
                if(!isFighting){
                    isFighting = true;
                }

                float targetDistance = Vector3.Distance(transform.position, fighter.Target.transform.position);

                if(targetDistance <= 1f){
                    if(attackTimer > 0){
                        // Debug.Log(attackTimer);
                        return;
                    }
                    // Debug.Log("Attacking");
                    Action.MeleeAction(GetComponent<Actor>(), fighter.Target);
                    attackTimer = attackCooldown;
                    return;
                }
                else{
                    
                    // if(movementTimer > 0){
                    //     // Debug.Log(movementTimer);
                    //     return;
                    // }
                    // movementTimer = movementCooldown;
                    MoveAlongPath(targetPos, GetComponent<Fighter>().MovementSpeed);
                    return;
                }
            }
        }
    }

    public override AiState SaveState() => new AiState(type: "HostileEnemy");
}
