using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// This skill will increase the player's attack power for a certain duration 
// while also decreasing the player's hp in a set of intervals
// Also the player will be healed if the player defeats an enemy
public class LetThePeopleRejoice : Skill
{
    [Header("Specific Attribute")]
    [SerializeField] private string skillName = "Let The People Rejoice";
    [SerializeField] private float duration = 10f;
    [SerializeField] private int manaCost = 30;
    [SerializeField] private float cooldown = 20f;
    [SerializeField] private bool onCooldown = false;
    [SerializeField] private float remainingCooldown = 0f;
    private float reduceHpInterval = 1f;
    private float remainingReduceHpInterval = 1f;
    private float healIfDefeatEnemyInterval = 2.5f;
    private float remainingHealIfDefeatEnemyInterval = 2.5f;
    private float remainingDuration = 10f;
    private bool isActive = false;
    private int initialAmountOfEnemiesKilled;

    public override string SkillName { get => skillName; }
    public override float Duration { get => duration; }
    public override int ManaCost { get => manaCost; }   
    public override float Cooldown { get => cooldown; }
    public override bool OnCooldown { get => onCooldown;}
    public override float RemainingCooldown { get => remainingCooldown; }
    public override bool IsActive { get => isActive; }

    public override void UpdateDuration()
    {
        remainingDuration -= Time.deltaTime;
        remainingReduceHpInterval -= Time.deltaTime;
        remainingHealIfDefeatEnemyInterval -= Time.deltaTime;
        if(remainingReduceHpInterval <= 0f)
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
            // Player can't die from this skill
            if(player.GetComponent<Fighter>().Hp > 1)
                player.GetComponent<Fighter>().Hp -= 1;
            remainingReduceHpInterval = reduceHpInterval;
        }
        if(remainingHealIfDefeatEnemyInterval <= 0f)
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
            // if the player has defeated an enemy logic
            if(player.EnemiesKilled > initialAmountOfEnemiesKilled)
            {
                initialAmountOfEnemiesKilled = player.EnemiesKilled;
                player.GetComponent<Fighter>().Heal(2);
                UIManager.Instance.AddMessage("You have been healed by Let The People Rejoice!", "#00FFFF");
            }
            remainingHealIfDefeatEnemyInterval = healIfDefeatEnemyInterval;
        }
        if(remainingDuration <= 0f)
        {
            remainingDuration = duration;
            UIManager.Instance.AddMessage(skillName + " has ended!", "#00FFFF");
            foreach(Entity entity in GameManager.Instance.Entities)
            {
                if(entity.GetComponent<Player>())
                {
                    entity.GetComponent<Fighter>().Power /= 2;
                    break;
                }
            }
            isActive = false;
        }

    }
    public override void Use()
    {
        UIManager.Instance.AddMessage("You used " + skillName + "!", "#00FFFF");
        isActive = true;
        Player player = null;
        foreach(Entity entity in GameManager.Instance.Entities)
        {
            if(entity.GetComponent<Player>())
            {
                player = entity.GetComponent<Player>();
                break;
            }
        }
        player.GetComponent<Fighter>().Power *= 2; 
        initialAmountOfEnemiesKilled = player.EnemiesKilled;
        MapManager.Instance.GenerateEffect("Let", player, duration, 1, 2);
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
