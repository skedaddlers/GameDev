using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SalonSolitaire : Skill
{
    public override void Update()
    {
        if(isActive)
        {
            remainingDuration -= Time.deltaTime;
            if(remainingDuration <= 0f)
            {
                DestroySalonMembers();
                remainingDuration = duration;
                isActive = false;
                UIManager.Instance.AddMessage($"{skillName} has ended!", "#00FFFF");
            }
        }
    }
    public override void Use()
    {
        UIManager.Instance.AddMessage("You used " + skillName + "!", "#00FFFF");
        Actor player = GameManager.Instance.Actors[0];
        // MapManager.Instance.GenerateSalonMembers(player);
        Vector3 playerPosition = player.transform.position;
        GameObject usher = MapManager.Instance.CreateEntity("Gentilhomme Usher", playerPosition + new Vector3(0, 1.5f, 0));    
        GameObject chevalmarin = MapManager.Instance.CreateEntity("Surintendante Chevalmarin", playerPosition + new Vector3(1f, -1f, 0));
        GameObject crabaletta = MapManager.Instance.CreateEntity("Mademoiselle Crabaletta", playerPosition + new Vector3(-1f, -1f, 0));
        isActive = true;
    }

    private void DestroySalonMembers()
    {
        for(int i = 0; i < GameManager.Instance.Entities.Count; i++)
        {
            if(GameManager.Instance.Entities[i].GetComponent<SalonMember>())
            {
                GameObject.Destroy(GameManager.Instance.Entities[i].gameObject);
                GameManager.Instance.Entities.RemoveAt(i);
                i--;
            }
        }
    }
    // Override other properties and methods as needed
}

