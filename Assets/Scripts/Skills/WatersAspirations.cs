using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This ckill creates a shield around the player that absorbs damage for a certain duration
public class WatersAspirations : Skill
{
    [Header("Specific Attribute")]
    [SerializeField] private string skillName = "Water's Aspirations";
    [SerializeField] private float duration = 10f;
    [SerializeField] private int manaCost = 15;
    [SerializeField] private float cooldown = 16f;
    [SerializeField] private bool onCooldown = false;
    [SerializeField] private float remainingCooldown = 0f;
    [SerializeField] private bool isActive = false;
    [SerializeField] private int shieldHp = 15;
    private float remainingDuration = 10f;

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
        Actor player = GameManager.Instance.Actors[0];
        if(player.GetComponent<Fighter>().ShieldHp <= 0)
        {
            UIManager.Instance.AddMessage(skillName + " is broken!", "#00FFFF");
            isActive = false;
            remainingDuration = duration;
            GameManager.Instance.RemoveVFXByNames("Shield");
            return;
        }
        if(remainingDuration <= 0)
        {
            remainingDuration = duration;
            UIManager.Instance.AddMessage(skillName + " has ended!", "#00FFFF");
            player.GetComponent<Fighter>().ShieldHp = 0;
            isActive = false;
        }
    }

    public override void Use()
    {
        isActive = true;
        UIManager.Instance.AddMessage("You used " + skillName + "!", "#00FFFF");
        Actor player = GameManager.Instance.Actors[0];
        player.GetComponent<Fighter>().ShieldHp = shieldHp;
        MapManager.Instance.GenerateEffect("Shield", player, duration, 1, 1);
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
