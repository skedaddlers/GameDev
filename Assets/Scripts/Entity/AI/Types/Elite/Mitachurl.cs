using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// mitachurl will be a type of elite enemy
// it will just be a typical bulky enemy with a lot of health
// it will periodically dash at the player
public class Mitachurl : EliteEnemy
{
    [SerializeField] private float dashTimer = 0;
    [SerializeField] private float dashRate = 6.5f;
    [SerializeField] private float speedMultiplier = 3.7f;
    [SerializeField] private float dashDuration = 0.41f;
    [SerializeField] private bool isDashing = false;
    [SerializeField] private float defaultSpeed;
    

    void Start()
    {
        defaultSpeed = GetComponent<Fighter>().MovementSpeed;
    }

    public override void RunAI()
    {
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
                        return;
                    }
                    Action.MeleeAction(GetComponent<Actor>(), fighter.Target);
                    attackTimer = attackCooldown;
                }
                else{
                    MoveAlongPath(targetPos, GetComponent<Fighter>().MovementSpeed);
                }
            }

            dashTimer += Time.deltaTime;
            if(dashTimer >= dashRate){
                Dash();
                isDashing = true;
                dashTimer = 0;
            }
            if(isDashing){
                dashDuration -= Time.deltaTime;
                if(dashDuration <= 0){
                    GetComponent<Fighter>().MovementSpeed = defaultSpeed;
                    isDashing = false;
                    dashDuration = 0.41f;
                }
            }
        }
    }

    private void Dash()
    {
        GetComponent<Fighter>().MovementSpeed = defaultSpeed * speedMultiplier;
    }
}
