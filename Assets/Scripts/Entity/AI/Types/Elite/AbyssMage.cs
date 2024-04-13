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
    [SerializeField] private float attackTimer = 0;
    [SerializeField] private float attackRate = 2f;

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
                Teleport();
                teleportTimer = 0;
            }
            if(attackTimer >= attackRate)
            {
                Attack();
                attackTimer = 0;
            }
            teleportTimer += Time.deltaTime;
            attackTimer += Time.deltaTime;
    
        }
    }

    public void Teleport()
    {
        // teleport to player
        Player player = GameManager.Instance.Actors[0].GetComponent<Player>();
        Vector3Int playerPos = MapManager.Instance.FloorMap.WorldToCell(player.transform.position);
        Vector3Int newPos = playerPos;
        transform.position = MapManager.Instance.FloorMap.GetCellCenterWorld(newPos);
    }

    public void Attack()
    {
        // cast spell
        // spawn a projectile
        // deal damage to player
        // stun player
        // apply debuff
    }
}
