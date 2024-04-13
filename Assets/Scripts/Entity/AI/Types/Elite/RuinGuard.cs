using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this enemy will be an elite enemy that overrides the runAi method
// it's defense is tough and it's attack is strong
// it will move slowly and attack the player
// it will periodically use a special ranged aoe attack that will deal massive damage to the player
// if its below 50% health, it will be disabled for a few seconds
public class RuinGuard : EliteEnemy
{
    [SerializeField] private float specialAttackTimer = 0;
    [SerializeField] private float specialAttackRate = 10f;
    [SerializeField] private bool isPerformingSpecialAttack = false;
    [SerializeField] private float timeToPerformSpecialAttack = 2f;
    [SerializeField] private float performSpecialAttackTimer = 0;
    [SerializeField] private float disableTimer = 0;
    [SerializeField] private float disableDuration = 6.6f;
    [SerializeField] private bool isDisabled = false;
    [SerializeField] private bool hasDisabled = false;

    public override void RunAI(){

        
        if(GetComponent<Fighter>().Hp <= GetComponent<Fighter>().MaxHp / 2){
            if(!hasDisabled){
                isDisabled = true;
            }
            hasDisabled = true;
        }

        if(isDisabled){
            disableTimer += Time.deltaTime;
            if(disableTimer >= disableDuration){
                isDisabled = false;
                disableTimer = 0;
            }
            else{
                return;
            }
        }

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
                if(isPerformingSpecialAttack){
                    performSpecialAttackTimer += Time.deltaTime;
                    if(performSpecialAttackTimer >= timeToPerformSpecialAttack){
                        isPerformingSpecialAttack = false;
                        performSpecialAttackTimer = 0;
                        specialAttackTimer = 0;
                        PerformSpecialAttack(fighter.Target.GetComponent<Actor>());
                    }
                }
                else{
                    if(targetDistance <= 1f){
                        if(attackTimer > 0){
                            return;
                        }
                        Action.MeleeAction(GetComponent<Actor>(), fighter.Target);
                        attackTimer = attackCooldown;
                    }
                    else{
                        MoveAlongPath(targetPos, GetComponent<Fighter>().MovementSpeed);
                    }

                    if(specialAttackTimer >= specialAttackRate){
                        isPerformingSpecialAttack = true;
                    }
                }  
            }

            specialAttackTimer += Time.deltaTime;
            
        }
    }

    private void PerformSpecialAttack(Actor player){
        // perform special attack
        // ;aunch a missile projectile at the target position
        // deal massive damage to the player
        Vector3Int targetPos = MapManager.Instance.FloorMap.WorldToCell(player.transform.position);
        Vector2 direction = (new Vector2(targetPos.x - transform.position.x, targetPos.y - transform.position.y)).normalized;
        // MapManager.Instance.CreateProjectile("Missile", transform.position, direction, 8, false);
    }
}