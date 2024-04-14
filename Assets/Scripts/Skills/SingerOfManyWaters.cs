using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This skill summons a water singer that will heal the player for a certain duration
public class SingerOfManyWaters : Skill
{
    public override void Update()
    {
        if (isActive)
        {
            remainingDuration -= Time.deltaTime;
            if (remainingDuration <= 0f)
            {
                remainingDuration = duration;
                isActive = false;
                UIManager.Instance.AddMessage($"{skillName} has ended!", "#00FFFF");
                DestroySinger();
            }
        }
    }

    private void DestroySinger()
    {
        for (int i = GameManager.Instance.Entities.Count - 1; i >= 0; i--)
        {
            Entity entity = GameManager.Instance.Entities[i];
            if (entity.GetComponent<Singer>())
            {
                GameObject.Destroy(entity.gameObject);
                GameManager.Instance.Entities.RemoveAt(i);
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

}
