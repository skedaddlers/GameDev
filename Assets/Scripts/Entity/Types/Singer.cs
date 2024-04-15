using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Entity from the skill singer of many waters, it will heal the player in a certain interval and radius
public class Singer : Entity
{
    [Header("Singer Settings")]
    [SerializeField] private int healAmount = 3;
    [SerializeField] private float healCooldown = 3f;
    [SerializeField] private float healRadius = 5f;
    [SerializeField] private bool isEvil;
    public bool IsEvil { get => isEvil; set => isEvil = value; }
    public int HealAmount { get => healAmount; set => healAmount = value; }
    private float remainingCooldown = 0f;
    // Start is called before the first frame update
    void Start()
    {
        AddToGameManager();
        BlocksMovement = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(isEvil){
            HealEnemy();
        }
        else{
            HealPlayer();
        }
    }

    private void HealPlayer(){
        remainingCooldown -= Time.deltaTime;
        if(remainingCooldown <= 0){
            remainingCooldown = healCooldown;
            Actor player = GameManager.Instance.Actors[0];
            if(Vector3.Distance(transform.position, player.transform.position) < healRadius){
                player.GetComponent<Fighter>().Heal(healAmount);
                UIManager.Instance.AddMessage("You were healed by the Singer of Many Waters!", Utilz.BLUE);
            }
        }
    }

    private void HealEnemy(){
        remainingCooldown -= Time.deltaTime;
        if(remainingCooldown <= 0){
            remainingCooldown = healCooldown;
            foreach(Entity entity in GameManager.Instance.Entities){
                if(entity.GetComponent<HostileEnemy>() && Vector3.Distance(transform.position, entity.transform.position) < healRadius){
                    entity.GetComponent<Fighter>().Heal(healAmount);
                }
            }
        }
    }
}
