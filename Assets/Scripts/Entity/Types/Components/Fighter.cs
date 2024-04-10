using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Actor))]
sealed class Fighter : MonoBehaviour
{
    [SerializeField] private int maxHp, hp, defense, power;
    [SerializeField] private int shieldHp = 0;
    [SerializeField] private Actor target;

    public int Hp{
        get => hp;
        set{
            hp = Mathf.Max(0, Mathf.Min(value, maxHp));

            if(GetComponent<Player>()){
                UIManager.Instance.SetHealth(hp, maxHp);
            }

            if(hp == 0){
                if(GetComponent<Player>() == null){
                    foreach(Entity entity in GameManager.Instance.Entities){
                        if(entity.GetComponent<Player>()){
                            entity.GetComponent<Player>().EnemiesKilled++;
                        }
                    }
                }
                Die();
            }
        }
    }
    public int MaxHp { get => maxHp; }
    public int Defense { get => defense; }
    public int Power { get => power; set => power = value;}
    public int ShieldHp { get => shieldHp; set => shieldHp = value; }
    public Actor Target { get => target; set => target = value; }

    private void Start(){
        if(GetComponent<Player>()){
            UIManager.Instance.SetHealthMax(maxHp);
            UIManager.Instance.SetManaMax(GetComponent<Player>().MaxMana);
            UIManager.Instance.SetHealth(hp, maxHp);
        }
    }

    void Update()
    {
        if(GetComponent<Player>()){
            UIManager.Instance.SetHealth(hp, maxHp);
            UIManager.Instance.SetMana(GetComponent<Player>().Mana, GetComponent<Player>().MaxMana);
        }
        else{
            UIManager.Instance.UpdateEnemyHealthBar(this.GetComponent<Actor>());
        }
    }

    public void TakeDamage(int damage){
        if(shieldHp > 0){
            shieldHp -= damage;
            if(shieldHp < 0){
                hp += shieldHp;
                shieldHp = 0;
            }
        }
        else{
            hp -= damage;
        }
        if(hp <= 0){
            Die();
        }
    }
    private void Die(){
        if(GetComponent<Player>()){
            UIManager.Instance.AddMessage("You died!", "#FF0000");
        }
        else{
            UIManager.Instance.AddMessage($"{name} died!", "#FFa500");
        }

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = GameManager.Instance.DeadSprite;
        spriteRenderer.sortingOrder = 0;
        spriteRenderer.color = Color.yellow;

        name = $"Remains of {name}";
        GetComponent<Actor>().BlocksMovement = false;
        GetComponent<Actor>().IsAlive = false;
        if(!GetComponent<Player>()){
            GameManager.Instance.RemoveActor(this.GetComponent<Actor>());
        }
    }

    public int Heal(int amount){
        if(hp == maxHp){
            return 0;
        }

        int newHPValue = hp + amount;

        if(newHPValue > maxHp){
            newHPValue = maxHp;
        }

        int amountHealed = newHPValue - hp;
        hp = newHPValue;
        return amountHealed;
    }
}
