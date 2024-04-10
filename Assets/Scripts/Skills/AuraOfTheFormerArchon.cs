using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Creates an aura that deals damage to enemies within a certain radius
public class AuraOfTheFormerArchon : Skill
{
    [Header("Specific Attribute")]
    [SerializeField] private string skillName = "Aura Of The Former Archon";
    [SerializeField] private float duration = 10f;
    [SerializeField] private float radius = 3f;
    [SerializeField] private int manaCost = 25;
    [SerializeField] private float cooldown = 15f;
    [SerializeField] private bool onCooldown = false;
    [SerializeField] private float remainingCooldown = 0f;
    [SerializeField]private float remainingDuration = 10f;
    [SerializeField]private bool isActive = false;
    [SerializeField] private int damage = 4;
    private float damageInterval = 1f;
    private float remainingDamageInterval = 0f;
    public override string SkillName { get => skillName; }
    public override float Duration { get => duration; }
    public override int ManaCost { get => manaCost; }   
    public override float Cooldown { get => cooldown; }
    public override bool OnCooldown { get => onCooldown;}
    public override float RemainingCooldown { get => remainingCooldown; }
    public override bool IsActive { get => isActive; }

    public override void UpdateDuration()
    {
        // Debug.Log("Update Duration salon solitaire");
        remainingDuration -= Time.deltaTime;
        remainingDamageInterval -= Time.deltaTime;
        // Deal damage to enemies within a certain radius
        if(remainingDamageInterval <= 0f)
        {
            DealDamage();
            remainingDamageInterval = damageInterval;
        }
        if(remainingDuration <= 0f)
        {
            remainingDuration = duration;
            UIManager.Instance.AddMessage(skillName + " has ended!", "#00FFFF");
            isActive = false;
        }
    }

    private void DealDamage()
    {
        Player player = null;
        foreach(Entity entity in GameManager.Instance.Entities)
        {
            if(entity.GetComponent<Player>())
            {
                player = entity.GetComponent<Player>();
                break;
            }
        }
        foreach(Entity entity in GameManager.Instance.Entities)
        {
            if(entity.GetComponent<HostileEnemy>())
            {
                if(Vector3.Distance(player.transform.position, entity.transform.position) <= radius)
                {
                    entity.GetComponent<Fighter>().TakeDamage(damage);
                }
            }
        }
    }

    public override void Use()
    {
        UIManager.Instance.AddMessage("You used " + skillName + "!", "#00FFFF");
        isActive = true;
        // Creates a ring of water around the player
        Player player = null;
        foreach(Entity entity in GameManager.Instance.Entities)
        {
            if(entity.GetComponent<Player>())
            {
                player = entity.GetComponent<Player>();
                break;
            }
        }
        MapManager.Instance.GenerateEffect("Aura", player, duration, radius + 1, 2);
    }

    public override IEnumerator CooldownRoutine()
    {
        onCooldown = true;
        remainingCooldown = cooldown;
        while (remainingCooldown > 0f)
        {
            remainingCooldown -= Time.deltaTime;
            yield return null;
        }
        onCooldown = false;
    }
}
