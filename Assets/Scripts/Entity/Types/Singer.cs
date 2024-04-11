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
        HealPlayer();
    }

    private void HealPlayer(){
        remainingCooldown -= Time.deltaTime;
        if(remainingCooldown <= 0){
            remainingCooldown = healCooldown;
            Actor player = GameManager.Instance.Actors[0];
            if(Vector3.Distance(transform.position, player.transform.position) < healRadius){
                player.GetComponent<Fighter>().Heal(healAmount);
                UIManager.Instance.AddMessage("You were healed by the Singer of Many Waters!", "#00FFFF");
            }
        }
    }
}
