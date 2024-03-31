using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Fighter))]
public class HostileEnemy : Ai
{
    [SerializeField] private Fighter fighter;
    [SerializeField] private bool isFighting;
    private void OnValidateOverride(){
        fighter = GetComponent<Fighter>();
        AStar = GetComponent<AStar>();
    }

    public void RunAI(){
        if(!fighter.Target){
            fighter.Target = GameManager.Instance.Actors[0];
        }
        else if(fighter.Target && !fighter.Target.IsAlive){
            fighter.Target = null;
        }

        if(fighter.Target){
            Vector3Int targetPos = MapManager.Instance.FloorMap.WorldToCell(fighter.Target.transform.position);
            // isFighting = true;
            if(isFighting){
                Debug.Log("Fighting");
                if(!isFighting){
                    isFighting = true;
                }

                float targetDistance = Vector3.Distance(transform.position, fighter.Target.transform.position);

                if(targetDistance <= 1.5f){
                    Action.MeleeAction(GetComponent<Actor>(), fighter.Target);
                    return;
                }
                else{
                    MoveAlongPath(targetPos);
                    return;
                }
            }
        }
        Action.SkipAction();
    }
}
