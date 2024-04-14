using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// status effect will be a component that can be added to an entity
// effects such as ensnare, burn, bleed, etc. will be implemented as status effects
// status effects will have a duration and a timer
public class StatusEffect : MonoBehaviour
{
    [SerializeField] protected float duration;
    [SerializeField] private float timer;
    [SerializeField] protected float applyRate;
    [SerializeField] private float applyTimer;
    [SerializeField] protected Fighter fighter = null;

    public float Duration { get => duration; set => duration = value; }
    public float Timer { get => timer; set => timer = value; }
    public float ApplyRate { get => applyRate; set => applyRate = value; }
    public float ApplyTimer { get => applyTimer; set => applyTimer = value; }
    public Fighter Fighter { get => fighter; set => fighter = value; }


    void Update()
    {
        if (timer >= duration)
        {
            EndEffect();
            fighter.IsUnderStatusEffect = false;
            Destroy(gameObject);
        }
        else
        {
            timer += Time.deltaTime;
        }

        if (applyTimer >= applyRate)
        {
            ApplyEffect();
            applyTimer = 0;
        }
        else
        {
            applyTimer += Time.deltaTime;
        }
    }

    public virtual void Start()
    {
        
    }

    public virtual void ApplyEffect()
    {
        // this method will be overridden by the child class
    }

    public virtual void EndEffect()
    {
        // this method will be overridden by the child class
    }

}
