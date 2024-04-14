using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This ckill creates a shield around the player that absorbs damage for a certain duration
public class WatersAspirations : Skill
{
    [Header("Specific Attribute")]
    [SerializeField] private int shieldHp = 15;

    public override void Update()
    {
        if(isActive){
            remainingDuration -= Time.deltaTime;
            Actor player = GameManager.Instance.Actors[0];
            if(player.GetComponent<Fighter>().ShieldHp <= 0)
            {
                UIManager.Instance.AddMessage(skillName + " is broken!", "#00FFFF");
                isActive = false;
                remainingDuration = duration;
                GameManager.Instance.RemoveVFXByNames("Shield");
                return;
            }
            if(remainingDuration <= 0)
            {
                remainingDuration = duration;
                UIManager.Instance.AddMessage(skillName + " has ended!", "#00FFFF");
                player.GetComponent<Fighter>().ShieldHp = 0;
                isActive = false;
            }
        }
    }

    public override void Use()
    {
        isActive = true;
        UIManager.Instance.AddMessage("You used " + skillName + "!", "#00FFFF");
        Actor player = GameManager.Instance.Actors[0];
        player.GetComponent<Fighter>().ShieldHp = shieldHp;
        MapManager.Instance.GenerateEffect("Shield", player, duration, 1, 1);
    }
}
