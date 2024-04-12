using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This skill summons a water singer that will heal the player for a certain duration
public class SingerOfManyWaters : Skill
{
    [Header("Specific Attribute")]
    [SerializeField] private string skillName = "Singer Of Many Waters";
    [SerializeField] private float duration = 12f;
    [SerializeField] private int manaCost = 50;
    [SerializeField] private float cooldown = 25f;
    [SerializeField] private bool onCooldown = false;
    [SerializeField] private float remainingCooldown = 0f;
    [SerializeField] private bool isActive = false;
    private float remainingDuration = 12f;

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
        if(remainingDuration <= 0f)
        {
            remainingDuration = duration;
            UIManager.Instance.AddMessage(skillName + " has ended!", "#00FFFF");
            isActive = false;
            DestroySinger();
        }
    }

    private void DestroySinger()
    {
        for(int i = 0; i < GameManager.Instance.Entities.Count; i++)
        {
            if(GameManager.Instance.Entities[i].GetComponent<Singer>())
            {
                GameObject.Destroy(GameManager.Instance.Entities[i].gameObject);
                GameManager.Instance.Entities.RemoveAt(i);
                i--;
            }
        }
    }
    public override void Use()
    {
        UIManager.Instance.AddMessage("You used " + skillName + "!", "#00FFFF");
        isActive = true;
        Actor player = GameManager.Instance.Actors[0];
        MapManager.Instance.CreateEntity("Singer", player.transform.position);
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
