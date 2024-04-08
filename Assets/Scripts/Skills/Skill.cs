using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
abstract public class Skill : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Skill Settings")]
    [SerializeField] private string skillName;
    [SerializeField] private int manaCost;
    [SerializeField] private float cooldown;


    public virtual string SkillName { get => skillName; }
    public virtual int ManaCost { get => manaCost; }
    public virtual float Cooldown { get => cooldown; }
    public virtual float Duration { get; set; }
    public virtual bool OnCooldown { get; set; }
    public virtual float RemainingCooldown { get; }
    public virtual void Use(){
        Debug.Log("Skill Used");
    }

    public virtual IEnumerator CooldownRoutine()
    {
        OnCooldown = true;
        yield return new WaitForSeconds(Cooldown);
        OnCooldown = false;
    }

}
