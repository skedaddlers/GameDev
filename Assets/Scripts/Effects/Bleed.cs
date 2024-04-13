using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bleed : StatusEffect
{
    [SerializeField] private int damage;
    public int Damage { get => damage; set => damage = value; }

    public override void ApplyEffect()
    {
        fighter.Hp -= damage;
        UIManager.Instance.AddMessage($"{fighter.name} is bleeding for {damage} damage!", "#FF0000");
    }
}
