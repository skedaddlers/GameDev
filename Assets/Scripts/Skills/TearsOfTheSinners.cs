using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// THis skill will unleash a rain that will damage all enemies in the camera view
public class TearsOfTheSinners : Skill
{
    [Header("Specific Attribute")]
    [SerializeField] private int damage = 8;
    [SerializeField] private float damageInterval = 1f;
    private float remainingDamageInterval = 0f;
    
    public override void Update(){
        if(isActive){
            remainingDuration -= Time.deltaTime;
            remainingDamageInterval -= Time.deltaTime;
            if(remainingDuration <= 0f){
                remainingDuration = duration;
                isActive = false;
                UIManager.Instance.AddMessage($"{skillName} has ended!", "#00FFFF");
            }
            if(remainingDamageInterval <= 0f){
                DealDamage();
                remainingDamageInterval = damageInterval;
            }
        }
    }

    private void DealDamage(){
        Actor player = GameManager.Instance.Actors[0];
        foreach(Entity entity in GameManager.Instance.Entities){
            if(entity.GetComponent<HostileEnemy>()){   
                float xDistance = Mathf.Abs(player.transform.position.x - entity.transform.position.x);
                float yDistance = Mathf.Abs(player.transform.position.y - entity.transform.position.y);
                if(xDistance <= Camera.main.orthographicSize * Camera.main.aspect && yDistance <= Camera.main.orthographicSize && 
                entity.GetComponent<Actor>().IsAlive){
                    entity.GetComponent<Fighter>().Hp -= damage;
                }           
            }
        }
    }
    public override void Use()
    {
        AudioManager.Instance.PlayVoiceLine("Tears Of The Sinners");
        UIManager.Instance.AddMessage("You used " + skillName + "!", "#00FFFF");
        isActive = true;
        Actor player = GameManager.Instance.Actors[0];      
        MapManager.Instance.GenerateEffect("Rain", player, duration, 1, 3);
    }

}
