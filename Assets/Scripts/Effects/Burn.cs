using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burn : StatusEffect
{
    [SerializeField] private int damage;
    public int Damage { get => damage; set => damage = value; }

    public override void ApplyEffect()
    {
        fighter.TakeDamage(damage);
        UIManager.Instance.AddMessage($"{fighter.name} is burning for {damage} damage!", Utilz.RED);
    }

    public override void EndEffect()
    {
        UIManager.Instance.AddMessage($"{fighter.name} has stopped burning.", Utilz.YELLOW);
    }

}

