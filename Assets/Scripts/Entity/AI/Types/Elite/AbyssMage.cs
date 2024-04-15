using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// abyss mage will be an elite enemy that overrides the runAi method
// it will teleport to the player and cast spells
// has shield that needs to be broken before it can be damaged
// if the shield is broken, the abyss mage will be stunned for a few seconds
// the shield will regenerate after a few seconds
public class AbyssMage : EliteEnemy
{
    [SerializeField] private int shieldHp = 15;
    [SerializeField] private float shieldRegenTime = 7f;
    [SerializeField] private bool shieldBroken = false;
    [SerializeField] private float shieldRegenTimer = 0;
    [SerializeField] private float teleportTimer = 0;
    [SerializeField] private float teleportRate = 5f;

    void Start()
    {
        GetComponent<Fighter>().ShieldHp = shieldHp;
    }

    public override void RunAI()
    {
        if(!fighter.Target){
            fighter.Target = GameManager.Instance.Actors[0];
        }
        else if(fighter.Target && !fighter.Target.IsAlive){
            fighter.Target = null;
        }


        if(GetComponent<Fighter>().ShieldHp <= 0)
        {
            shieldBroken = true;
        }
        if(shieldBroken)
        {
            shieldRegenTimer += Time.deltaTime;
            if(shieldRegenTimer >= shieldRegenTime)
            {
                shieldBroken = false;
                shieldRegenTimer = 0;
                GetComponent<Fighter>().ShieldHp = shieldHp;
            }
        }
        else if(fighter.Target && fighter.Target.IsAlive){
            if(teleportTimer >= teleportRate)
            {
                TeleportToPlayer(fighter.Target.GetComponent<Actor>());
                teleportTimer = 0;
            }
            if(attackTimer >= attackCooldown)
            {
                FlameAttack(fighter.Target.GetComponent<Actor>());
                attackTimer = 0;
            }
            teleportTimer += Time.deltaTime;
            attackTimer += Time.deltaTime;
    
        }
    }

    public void TeleportToPlayer(Actor player)
    {
        // teleport to player
        Vector3Int playerPos = MapManager.Instance.FloorMap.WorldToCell(player.transform.position);
        Vector3Int newPos = playerPos;
        transform.position = MapManager.Instance.FloorMap.GetCellCenterWorld(newPos);
    }

    public void FlameAttack(Actor player)
    {
        Vector3Int targetPos = MapManager.Instance.FloorMap.WorldToCell(player.transform.position);
        Vector2 direction = new Vector2(targetPos.x - transform.position.x, targetPos.y - transform.position.y).normalized;
        int damage = GetComponent<Fighter>().Power;
        GameObject proj = MapManager.Instance.CreateProjectile("Flame", transform.position, direction, damage, false);
    }
}
