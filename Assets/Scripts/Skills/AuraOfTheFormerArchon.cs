using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuraOfTheFormerArchon : Skill
{
    [Header("Specific Attribute")]
    [SerializeField] private string skillName = "Aura Of The Former Archon";
    [SerializeField] private float duration = 10f;
    [SerializeField] private int manaCost = 25;
    [SerializeField] private float cooldown = 10f;
    [SerializeField] private bool onCooldown = false;
    [SerializeField] private float remainingCooldown = 0f;

    public override string SkillName { get => skillName; }
    public override float Duration { get => duration; }
    public override int ManaCost { get => manaCost; }   
    public override float Cooldown { get => cooldown; }
    public override bool OnCooldown { get => onCooldown;}
    public override float RemainingCooldown { get => remainingCooldown; }


    void Start()
    {

    }

    void Update()
    {

    }


    public override void Use()
    {
        UIManager.Instance.AddMessage("You used " + skillName + "!", "#00FFFF");
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
