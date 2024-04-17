using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Creates an aura that deals damage to enemies within a certain radius
public class AuraOfTheFormerArchon : Skill
{
    [Header("Specific Attribute")]
    [SerializeField] private float radius = 3f;
    [SerializeField] private int damage = 4;
    private float damageInterval = 1f;
    private float remainingDamageInterval = 0f;

    public override void Update(){
        if(isActive)
        {
            remainingDuration -= Time.deltaTime;
            remainingDamageInterval -= Time.deltaTime;
            if(remainingDuration <= 0f)
            {
                remainingDuration = duration;
                isActive = false;
                UIManager.Instance.AddMessage($"{skillName} has ended!", "#00FFFF");
            }
            if(remainingDamageInterval <= 0f)
            {
                DealDamage();
                remainingDamageInterval = damageInterval;
            }
        }
    }

    private void DealDamage()
    {
        Actor player = GameManager.Instance.Actors[0];
        foreach(Entity entity in GameManager.Instance.Entities)
        {
            if(entity.GetComponent<HostileEnemy>())
            {
                if(Vector3.Distance(player.transform.position, entity.transform.position) <= radius && entity.GetComponent<Actor>().IsAlive)
                {
                    entity.GetComponent<Fighter>().TakeDamage(damage);
                }
            }
        }
    }

    public override void Use()
    {
        AudioManager.Instance.PlayVoiceLine("Aura Of The Former Archon");
        UIManager.Instance.AddMessage("You used " + skillName + "!", "#00FFFF");
        isActive = true;
        Actor player = GameManager.Instance.Actors[0];
        MapManager.Instance.GenerateEffect("Aura", player, duration, radius + 1, 2);
    }
}
