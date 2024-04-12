using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Fighter))]
public class HostileEnemy : Ai
{
    [SerializeField] private Fighter fighter;
    [SerializeField] private bool isFighting;

    // [SerializeField] private float movementCooldown = 0.5f; // Adjust as needed
    [SerializeField] private float attackCooldown = 0.1f; // Adjust as needed

    private float actionTimer = 0.1f;
    // private float movementTimer = 0.5f;
    private void OnValidateOverride(){
        fighter = GetComponent<Fighter>();
        AStar = GetComponent<AStar>();
    }

    private void Update(){
        if(actionTimer > 0){
            actionTimer -= Time.deltaTime;
        }
        // if(movementTimer > 0){
        //     movementTimer -= Time.deltaTime;
        // }
        // UIManager.Instance.UpdateEnemyHealthBar(this);
    }

    public void RunAI(){
        
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
                    if(actionTimer > 0){
                        // Debug.Log(actionTimer);
                        return;
                    }
                    // Debug.Log("Attacking");
                    Action.MeleeAction(GetComponent<Actor>(), fighter.Target);
                    actionTimer = attackCooldown;
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
