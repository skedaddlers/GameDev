using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Actor))]
public class Fighter : MonoBehaviour
{
    [SerializeField] private int maxHp, hp, defense, power;
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private int shieldHp = 0;
    [SerializeField] private Actor target;
    [SerializeField] private bool isUnderStatusEffect = false;

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
    public int MaxHp { get => maxHp; set => maxHp = value;}
    public int Defense { get => defense; set => defense = value;}
    public int Power { get => power; set => power = value;}
    public int ShieldHp { get => shieldHp; set => shieldHp = value; }
    public float MovementSpeed { get => movementSpeed; set => movementSpeed = value;}
    public Actor Target { get => target; set => target = value; }
    public bool IsUnderStatusEffect { get => isUnderStatusEffect; set => isUnderStatusEffect = value; }
    private void Start(){
        if(GetComponent<Player>()){
            UIManager.Instance.SetHealthMax(maxHp);
            UIManager.Instance.SetHealth(hp, maxHp);
        }
    }

    void Update()
    {
        if(GetComponent<Player>()){
            UIManager.Instance.SetHealth(hp, maxHp);
        }
        else{
            UIManager.Instance.UpdateEnemyHealthBar(this.GetComponent<Actor>());
        }
    }

    public void ApplyEffect(StatusEffect effect){
        if(isUnderStatusEffect){
            UIManager.Instance.AddMessage("Already under a status effect!", "#FF0000");
            Destroy(effect.gameObject);
            return;
        }
    }

    public void TakeDamage(int damage){
        if(GetComponent<Player>()){
            if(GetComponent<Player>().IsDashing){
                UIManager.Instance.AddMessage("You are invulnerable while dashing!", "#FF0000");
                return;
            }
        }
        if(GetComponent<HostileEnemy>()){
            if(GetComponent<HostileEnemy>().CanTakeDamage == false){
                return;
            }
        }
        if(shieldHp > 0){
            shieldHp -= damage;
            if(shieldHp < 0){
                hp += shieldHp;
                shieldHp = 0;
            }
        }
        else{
            int damageDealt = damage;
            if(hp < damage){
                damageDealt = hp;
            }
            hp -= damageDealt;
        }
        if(hp <= 0){
            
            Die();
        }
    }
    public void Die(){
        if(GetComponent<Actor>().IsAlive){
            if(GetComponent<Player>()){
                UIManager.Instance.AddMessage("You died!", "#FF0000");
                UIManager.Instance.ShowDefeatScreen();
            }
            else{
                UIManager.Instance.AddMessage($"{name} died!", "#FFA500");
            }
            GetComponent<Actor>().IsAlive = false;
        }
        if(GetComponent<EliteEnemy>()){
            GetComponent<EliteEnemy>().MayDropWeapon();
        }
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = GameManager.Instance.DeadSprite;
        spriteRenderer.sortingOrder = 0;
        spriteRenderer.color = Color.yellow;

        name = $"Remains of {name}";
        GetComponent<Actor>().BlocksMovement = false;
        if(!GetComponent<Player>()){
            // add exp to player
            GameManager.Instance.Actors[0].GetComponent<Player>().Mora += GetComponent<HostileEnemy>().MoraGiven;
            GameManager.Instance.Actors[0].GetComponent<Player>().AddExp(GetComponent<HostileEnemy>().ExpGiven);
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

    public FighterState SaveState() => new FighterState(
        maxHp: maxHp,
        hp: hp,
        defense: defense,
        power: power,
        shieldHp: shieldHp,
        target: target != null ? target.name : null
    );

    public void LoadState(FighterState state){
        maxHp = state.MaxHp;
        hp = state.Hp;
        defense = state.Defense;
        power = state.Power;
        shieldHp = state.ShieldHp;
        target = GameManager.Instance.Actors.Find(x => x.name == state.Target);
    }
}

public class FighterState
{
    [SerializeField] private int maxHp, hp, defense, power;
    [SerializeField] private int shieldHp;
    [SerializeField] private string target;
    public int MaxHp { get => maxHp; set => maxHp = value; }
    public int Hp { get => hp; set => hp = value; }
    public int Defense { get => defense; set => defense = value; }
    public int Power { get => power; set => power = value; }
    public int ShieldHp { get => shieldHp; set => shieldHp = value; }
    public string Target { get => target; set => target = value; }

    public FighterState(int maxHp, int hp, int defense, int power, int shieldHp, string target){
        this.maxHp = maxHp;
        this.hp = hp;
        this.defense = defense;
        this.power = power;
        this.shieldHp = shieldHp;
        this.target = target;
    }
}
