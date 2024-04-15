using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this enemy will override the runAI method
// It will stay in place and attack the player
// it will periodically use a special attack that will 
// ensnare the player and make them unable to move for a few seconds
// it will also teleport away if the player goes near it
public class MirrorMaiden : EliteEnemy
{
    [SerializeField] private float specialAttackTimer = 0;
    [SerializeField] private float specialAttackRate = 9f;
    [SerializeField] private float ensnareDuration = 3f;
    [SerializeField] private float teleportTimer = 0;
    [SerializeField] private float teleportRate = 4.5f;

    public override void RunAI(){
        if(!fighter.Target){
            fighter.Target = GameManager.Instance.Actors[0];
        }
        else if(fighter.Target && !fighter.Target.IsAlive){
            fighter.Target = null;
        }

        if(fighter.Target && fighter.Target.IsAlive){
            float targetDistance = Vector3.Distance(transform.position, fighter.Target.transform.position);
            if(targetDistance <= 3f && teleportTimer >= teleportRate){
                TeleportAway(fighter.Target.GetComponent<Actor>());
                teleportTimer = 0;
            }
            if(teleportTimer < teleportRate){
                teleportTimer += Time.deltaTime;
            }
            specialAttackTimer += Time.deltaTime;
            attackTimer += Time.deltaTime;
            if(specialAttackTimer >= specialAttackRate){
                SpecialAttack(fighter.Target.GetComponent<Actor>());
                specialAttackTimer = 0;
            }
            if(attackTimer >= attackCooldown){
                RangedAttack(fighter.Target.GetComponent<Actor>());
                attackTimer = 0;
            }
        }
    }

    private void SpecialAttack(Actor target){

        GameObject ensnare = Instantiate(Resources.Load<GameObject>("Entities/Effect/Ensnare"), target.transform.position, Quaternion.identity);
        ensnare.GetComponent<Ensnare>().Duration = ensnareDuration;
        ensnare.GetComponent<Ensnare>().Fighter = target.GetComponent<Fighter>();
        ensnare.name = "Ensnare";
        
        target.GetComponent<Fighter>().ApplyEffect(ensnare.GetComponent<StatusEffect>());

        UIManager.Instance.AddMessage("Mirror Maiden used Ensnare! Oh no, You're stuck!", "#FF0000");
    }

    private void TeleportAway(Actor target){
        // Debug.Log("Teleporting away");
        RectangularRoom room = RoomManager.Instance.GetRoomWithEntity(GetComponent<Entity>());    

        while(true){
            Vector3Int randomPos = new Vector3Int(Random.Range(room.X + 1, room.X + room.Width - 1), Random.Range(room.Y - 1, room.Y + room.Height + 1), 0);
            float distanceToPlayer = Vector3.Distance(MapManager.Instance.FloorMap.CellToWorld(randomPos), target.transform.position);
            if(distanceToPlayer >= 1f){
                transform.position = MapManager.Instance.FloorMap.CellToWorld(randomPos);
                break;
            }
        }
    }

    private void RangedAttack(Actor target){
        Vector3Int targetPos = MapManager.Instance.FloorMap.WorldToCell(target.transform.position);
        Vector2 direction = new Vector2(targetPos.x - transform.position.x, targetPos.y - transform.position.y).normalized;
        int damage = GetComponent<Fighter>().Power; 
        GameObject proj = MapManager.Instance.CreateProjectile("Jet", transform.position, direction, damage, false);
    }
}


