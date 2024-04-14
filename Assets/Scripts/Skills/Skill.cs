using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour {
    // Start is called before the first frame update
    [Header("Skill Settings")]
    [SerializeField] protected string skillName;
    [SerializeField] protected int manaCost;
    [SerializeField] protected float cooldown;
    [SerializeField] protected float duration;
    [SerializeField] protected bool onCooldown = false;
    [SerializeField] protected bool isActive = false;
    [SerializeField] protected float remainingCooldown;
    [SerializeField] protected float remainingDuration;

    public virtual string SkillName { get => skillName; }
    public virtual int ManaCost { get => manaCost; }
    public virtual float Cooldown { get => cooldown; }
    public virtual float Duration { get => duration; }
    public virtual bool OnCooldown { get => onCooldown; }
    public virtual bool IsActive { get => isActive; }
    public virtual float RemainingCooldown { get => remainingCooldown; }
    public virtual float RemainingDuration { get => remainingDuration; }


    public virtual void Update()
    {
        if(isActive)
        {
            remainingDuration -= Time.deltaTime;
            if(remainingDuration <= 0f)
            {
                remainingDuration = duration;
                isActive = false;
                UIManager.Instance.AddMessage($"{SkillName} has ended!", "#00FFFF");
            }
        }
    }

    public virtual void Use(){
        Debug.Log("Skill Used");
    }

    public virtual IEnumerator CooldownRoutine()
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
