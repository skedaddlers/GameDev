using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ensnare : StatusEffect{
    public override void ApplyEffect()
    {
        fighter.MovementSpeed = 0;
        UIManager.Instance.AddMessage($"{fighter.name} is ensnared!", "#FF0000");
    }

    public override void EndEffect()
    {
        fighter.MovementSpeed = fighter.BaseMovementSpeed;
        UIManager.Instance.AddMessage($"{fighter.name} is no longer ensnared!", "#FF0000");
    }

}

