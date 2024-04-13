using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Actor))]
public class Inventory : MonoBehaviour
{
    [SerializeField] private int capacity = 0;
    [SerializeField] private List<Item> items = new List<Item>();
    [SerializeField] private Weapon weapon;
    public int Capacity { get => capacity; }
    public List<Item> Items { get => items; }
    public Weapon Weapon { get => weapon; }

    public void Add(Item item)
    {
        Items.Add(item);
        item.transform.SetParent(transform);
        GameManager.Instance.RemoveEntity(item);
    }

    public void Drop(Item item)
    {
        items.Remove(item);
        item.transform.SetParent(null);
        item.GetComponent<SpriteRenderer>().enabled = true;
        item.AddToGameManager();
        
        UIManager.Instance.AddMessage($"You dropped the {item.name}.", "#FF0000");
    }

    public void EquipWeapon(Weapon wp)
    {
        if(weapon == null){
            weapon = wp;
            weapon.transform.SetParent(transform);
            GameManager.Instance.RemoveEntity(weapon);
            UIManager.Instance.AddMessage($"You equipped the {wp.name}.", "#00FF00");
            
        }
        else{
            weapon.transform.SetParent(null);
            weapon.GetComponent<SpriteRenderer>().enabled = true;
            weapon.AddToGameManager();
            UIManager.Instance.AddMessage($"You unequipped the {weapon.name}.", "#FF0000");
            weapon = wp;
            weapon.transform.SetParent(transform);
            GameManager.Instance.RemoveEntity(weapon);
            UIManager.Instance.AddMessage($"You equipped the {wp.name}.", "#00FF00");
        }
    }

    public void DropWeapon()
    {
        if(weapon != null){
            weapon.transform.SetParent(null);
            weapon.GetComponent<SpriteRenderer>().enabled = true;
            weapon.AddToGameManager();
            UIManager.Instance.AddMessage($"You dropped the {weapon.name}.", "#FF0000");
            weapon = null;
        }
    }
}
