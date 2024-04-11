using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SalonSolitaire : Skill
{
    [Header("Specific Attribute")]
    [SerializeField] private string skillName = "Salon Solitaire";
    [SerializeField] private float duration = 10f;
    [SerializeField] private int manaCost = 10;
    [SerializeField] private float cooldown = 15f;
    [SerializeField] private bool onCooldown = false;
    [SerializeField] private float remainingCooldown = 0f;
    private float remainingDuration = 10f;
    private bool isActive = false;

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
        if(remainingDuration <= 0f)
        {
            remainingDuration = duration;
            UIManager.Instance.AddMessage(skillName + " has ended!", "#00FFFF");
            isActive = false;
            DestroySalonMembers();
        }
    }

    public override void Use()
    {
        UIManager.Instance.AddMessage("You used " + skillName + "!", "#00FFFF");
        Actor player = GameManager.Instance.Actors[0];
        // MapManager.Instance.GenerateSalonMembers(player);
        Vector3 playerPosition = player.transform.position;
        MapManager.Instance.CreateEntity("Gentilhomme Usher", playerPosition + new Vector3(0, 1.5f, 0));
        MapManager.Instance.CreateEntity("Surintendante Chevalmarin", playerPosition + new Vector3(1f, -1f, 0));
        MapManager.Instance.CreateEntity("Mademoiselle Crabaletta", playerPosition + new Vector3(-1f, -1f, 0));
        isActive = true;
    }

    private void DestroySalonMembers()
    {
        for(int i = 0; i < GameManager.Instance.Entities.Count; i++)
        {
            if(GameManager.Instance.Entities[i].GetComponent<SalonMember>())
            {
                Destroy(GameManager.Instance.Entities[i].gameObject);
                GameManager.Instance.Entities.RemoveAt(i);
                i--;
            }
        }
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

    // Override other properties and methods as needed
}

