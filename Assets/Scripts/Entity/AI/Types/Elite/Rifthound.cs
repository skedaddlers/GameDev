using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// this enemy will be an elite enemy that overrides the runAi method
// it will sometimes teleport to the player with offset 1 tile and attack
//it's attack will apply bleed to the player
public class Rifthound : EliteEnemy
{
    [SerializeField] private float teleportTimer = 0;
    [SerializeField] private float teleportRate = 7.5f;
    [SerializeField] private float bleedRate = 2f;
    [SerializeField] private int bleedDamage = 1;
    [SerializeField] private float bleedDuration = 6f;

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
                    BleedAttack(fighter.Target);
                    attackTimer = attackCooldown;
                }
                else{
                    MoveAlongPath(targetPos, GetComponent<Fighter>().MovementSpeed);
                }

                if(teleportTimer >= teleportRate){
                    TeleportToPlayer(fighter.Target.GetComponent<Actor>());
                    teleportTimer = 0;
                }
            }

            teleportTimer += Time.deltaTime;
            
        }
    }

    public void TeleportToPlayer(Actor target)
    {
        // teleport to player
        Vector3Int playerPos = MapManager.Instance.FloorMap.WorldToCell(target.transform.position);
        Vector3Int newPos = playerPos;
        newPos.x += Random.Range(-1, 2);
        newPos.y += Random.Range(-1, 2);
        transform.position = MapManager.Instance.FloorMap.GetCellCenterWorld(newPos);
    }

    private void BleedAttack(Actor target){
        target.GetComponent<Fighter>().TakeDamage(GetComponent<Fighter>().Power);

        GameObject bleedEffect = new GameObject("Bleed");
        Bleed bleed = bleedEffect.AddComponent<Bleed>();
        bleed.Duration = bleedDuration;
        bleed.ApplyRate = bleedRate;
        bleed.Damage = bleedDamage;
        bleed.Fighter = target.GetComponent<Fighter>();

        if(!target.GetComponent<Fighter>().IsUnderStatusEffect){
            bleedEffect.gameObject.SetActive(true);
            bleedEffect.transform.SetParent(target.transform, false);
            target.GetComponent<Fighter>().IsUnderStatusEffect = true;
        }
        else{
            Destroy(bleedEffect);
        }

        // GameObject bleedEffect = Instantiate(Resources.Load<GameObject>("Entities/Effect/Bleed"), target.transform.position, Quaternion.identity);
        // bleedEffect.GetComponent<Bleed>().Duration = bleedDuration;
        // bleedEffect.GetComponent<Bleed>().ApplyRate = bleedRate;
        // bleedEffect.GetComponent<Bleed>().Damage = bleedDamage;
        // bleedEffect.GetComponent<Bleed>().Fighter = target.GetComponent<Fighter>();
        // bleedEffect.name = "Bleed";
        // if(!target.GetComponent<Fighter>().IsUnderStatusEffect){
        //     bleedEffect.gameObject.SetActive(true);
        //     bleedEffect.transform.SetParent(target.transform, false);
        //     target.GetComponent<Fighter>().IsUnderStatusEffect = true;
        // }
        // else{
        //     Destroy(bleedEffect);
        // }
        // apply bleed effect to player
        UIManager.Instance.AddMessage("Rifthound attacked you and tried to bleed you!", "#FF0000");
    }
}
