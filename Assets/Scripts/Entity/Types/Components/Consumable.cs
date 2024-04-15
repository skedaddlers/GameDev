using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Item))]
public class Consumable : MonoBehaviour
{
    public enum ConsumableType
    {
        HealthPotion
        // ManaPotion,
        // StaminaPotion
    }
    [SerializeField] private ConsumableType consumableType;
    [SerializeField] private int amount = 0;

    public ConsumableType Type { get => consumableType; }
    public int Amount { get => amount; }

    public bool Activate(Actor actor, Item item){
        switch(consumableType){
            case ConsumableType.HealthPotion:
                return Healing(actor, item);
            // case ConsumableType.ManaPotion:
            //     actor.Fighter.Mana += amount;
            //     break;
            // case ConsumableType.StaminaPotion:
            //     actor.Fighter.Stamina += amount;
            //     break;
            default:
                return false;
        }
    }

    private bool Healing(Actor actor, Item item){
        int amountHealed = actor.GetComponent<Fighter>().Heal(amount);

        if(amountHealed > 0){
            UIManager.Instance.AddMessage($"You consumed the {name} and healed for {amountHealed} HP!", Utilz.GREEN);
            Consume(actor, item);
            return true;
        }
        else{
            UIManager.Instance.AddMessage("You are already at full health!", Utilz.RED);
            return false;
        }
    }
    
    public void Consume(Actor actor, Item item){
        actor.Inventory.Items.Remove(item);
        GameManager.Instance.RemoveEntity(GetComponent<Item>());
        Destroy(item.gameObject);
    }


}
