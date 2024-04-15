using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ensnare : StatusEffect{

    [SerializeField] private float fighterMovementSpeed;

    public override void Start(){
        base.Start();
        fighterMovementSpeed = fighter.MovementSpeed;
        fighter.MovementSpeed = 0.1f;
        UIManager.Instance.AddMessage($"{fighter.name} is ensnared!", Utilz.PURPLE);
    }

    public override void EndEffect()
    {
        fighter.MovementSpeed = fighterMovementSpeed;
        UIManager.Instance.AddMessage($"{fighter.name} is no longer ensnared!", Utilz.YELLOW);
    }

}

