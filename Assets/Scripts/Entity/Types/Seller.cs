using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// this class is used to represent a seller in the game
// a seller is an entity that sells skills, weapons, and other items to the player
// the player can interact with the seller to buy items
public class Seller : MonoBehaviour
{
    [SerializeField] private List<Item> itemsForSale = new List<Item>();
    [SerializeField] private List<Skill> skillsForSale = new List<Skill>();
    [SerializeField] private List<Weapon> weaponsForSale = new List<Weapon>();

    public List<Item> ItemsForSale { get => itemsForSale; set => itemsForSale = value; }
    public List<Skill> SkillsForSale { get => skillsForSale; set => skillsForSale = value; }
    public List<Weapon> WeaponsForSale { get => weaponsForSale; set => weaponsForSale = value; }

    // this method is called when the player interacts with the seller
    // it opens the seller's shop and allows the player to buy items
    public void OpenShop()
    {
        // open the shop UI
        UIManager.Instance.OpenShop(this);
    }
}
