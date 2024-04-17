using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// This skill will increase the player's attack power for a certain duration 
// while also decreasing the player's hp in a set of intervals
// Also the player will be healed if the player defeats an enemy
public class LetThePeopleRejoice : Skill
{
    [Header("Specific Attribute")]
    [SerializeField] private float reduceHpInterval = 1f;
    [SerializeField] private float remainingReduceHpInterval = 1f;
    [SerializeField] private float healIfDefeatEnemyInterval = 2.5f;
    [SerializeField] private float remainingHealIfDefeatEnemyInterval = 2.5f;
    [SerializeField] private int initialAmountOfEnemiesKilled;
    [SerializeField] private int powerGain;

    public override void Update()
    {
        if(isActive)
        {
            remainingDuration -= Time.deltaTime;
            remainingReduceHpInterval -= Time.deltaTime;
            remainingHealIfDefeatEnemyInterval -= Time.deltaTime;
            Actor player = GameManager.Instance.Actors[0];
            if(remainingDuration <= 0f)
            {
                remainingDuration = duration;
                isActive = false;
                UIManager.Instance.AddMessage($"{skillName} has ended!", "#00FFFF");
                player.GetComponent<Fighter>().Power -= powerGain;
                powerGain = 0;
                if(player.GetComponent<Player>().EnemiesKilled > initialAmountOfEnemiesKilled)
                {
                    player.GetComponent<Fighter>().Heal(5);
                }
            }
            if(remainingReduceHpInterval <= 0f)
            {
                player.GetComponent<Fighter>().Hp -= 1;
                remainingReduceHpInterval = reduceHpInterval;
            }
            if(remainingHealIfDefeatEnemyInterval <= 0f)
            {
                if(player.GetComponent<Player>().EnemiesKilled > initialAmountOfEnemiesKilled)
                {
                    player.GetComponent<Fighter>().Heal(5);
                }
                remainingHealIfDefeatEnemyInterval = healIfDefeatEnemyInterval;
            }
        }
    }
    public override void Use()
    {
        AudioManager.Instance.PlayVoiceLine("Let The People Rejoice");
        UIManager.Instance.AddMessage("You used " + skillName + "!", "#00FFFF");
        isActive = true;
        Actor player = GameManager.Instance.Actors[0];
        powerGain = player.GetComponent<Fighter>().Power;
        player.GetComponent<Fighter>().Power += powerGain;
        initialAmountOfEnemiesKilled = player.GetComponent<Player>().EnemiesKilled;
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
