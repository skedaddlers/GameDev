using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// THis skill will unleash a rain that will damage all enemies in the camera view
public class TearsOfTheSinners : Skill
{
    [Header("Specific Attribute")]
    [SerializeField] private string skillName = "Tears Of The Sinners";
    [SerializeField] private float duration = 3f;
    [SerializeField] private int manaCost = 50;
    [SerializeField] private float cooldown = 30f;
    [SerializeField] private bool onCooldown = false;
    [SerializeField] private float remainingCooldown = 0f;
    [SerializeField] private bool isActive = false;
    [SerializeField] private int damage = 8;
    [SerializeField] private float damageInterval = 1f;
    private float remainingDamageInterval = 0f;
    private float remainingDuration = 3f;

    public override string SkillName { get => skillName; }
    public override float Duration { get => duration; }
    public override int ManaCost { get => manaCost; }   
    public override float Cooldown { get => cooldown; }
    public override bool OnCooldown { get => onCooldown;}
    public override float RemainingCooldown { get => remainingCooldown; }
    public override bool IsActive { get => isActive; }

    public override void UpdateDuration(){
        remainingDuration -= Time.deltaTime;
        remainingDamageInterval -= Time.deltaTime;
        if(remainingDamageInterval <= 0f){
            DealDamage();
            remainingDamageInterval = damageInterval;
        }
        if(remainingDuration <= 0f){
            remainingDuration = duration;
            UIManager.Instance.AddMessage(skillName + " has ended!", "#00FFFF");
            isActive = false;
        }
    }

    private void DealDamage(){
        Player player = null;
        foreach(Entity entity in GameManager.Instance.Entities){
            if(entity.GetComponent<Player>()){
                player = entity.GetComponent<Player>();
                break;
            }
        }
        foreach(Entity entity in GameManager.Instance.Entities){
            if(entity.GetComponent<HostileEnemy>()){   
                float xDistance = Mathf.Abs(player.transform.position.x - entity.transform.position.x);
                float yDistance = Mathf.Abs(player.transform.position.y - entity.transform.position.y);
                if(xDistance <= Camera.main.orthographicSize * Camera.main.aspect && yDistance <= Camera.main.orthographicSize){
                    entity.GetComponent<Fighter>().TakeDamage(damage);
                }           
                // if(Vector3.Distance(player.transform.position, entity.transform.position) <= Camera.main.orthographicSize){
                //     entity.GetComponent<Fighter>().Hp -= damage;
                // }
            }
        }
    }
    public override void Use()
    {
        UIManager.Instance.AddMessage("You used " + skillName + "!", "#00FFFF");
        isActive = true;
        Player player = null;
        foreach(Entity entity in GameManager.Instance.Entities)
        {
            if(entity.GetComponent<Player>())
            {
                player = entity.GetComponent<Player>();
                break;
            }
        }        
        MapManager.Instance.GenerateEffect("Rain", player, duration, 1, 3);
    }

    public override IEnumerator CooldownRoutine()
    {
        onCooldown = true;
        remainingCooldown = cooldown;
        while (remainingCooldown > 0f)
        {
            remainingCooldown -= Time.deltaTime;
            yield return null;
        }
        onCooldown = false;
    }
}
