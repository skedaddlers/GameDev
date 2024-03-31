using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Actor))]
sealed class Fighter : MonoBehaviour
{
    [SerializeField] private int maxHp, hp, defense, power;
    [SerializeField] private Actor target;

    public int Hp{
        get => hp;
        set{
            hp = Mathf.Max(0, Mathf.Min(value, maxHp));
            if(hp == 0){
                Die();
            }
        }
    }
    public int Defense { get => defense; }
    public int Power { get => power; }
    public Actor Target { get => target; set => target = value; }

    private void Die(){
        if(GetComponent<Player>()){
            Debug.Log($"You died!");
        }
        else{
            Debug.Log($"{name} died!");
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
}
