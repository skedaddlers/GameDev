using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// this class is used to represent a seller in the game
// a seller is an entity that sells skills, weapons, and other items to the player
// the player can interact with the seller to buy items
public class Seller : Entity
{
    [SerializeField] private List<Skill> skillsForSale = new List<Skill>();
    [SerializeField] private int amountOfSkillsForSale = 3;
    [SerializeField] private List<Weapon> weaponsForSale = new List<Weapon>();
    [SerializeField] private int amountOfWeaponsForSale = 3;
    //check for sold out
    [SerializeField] private bool[] soldOutSkill = new bool[3];
    [SerializeField] private bool[] soldOutWeapon = new bool[3];

    public List<Skill> SkillsForSale { get => skillsForSale; set => skillsForSale = value; }
    public int AmountOfSkillsForSale { get => amountOfSkillsForSale; set => amountOfSkillsForSale = value; }
    public List<Weapon> WeaponsForSale { get => weaponsForSale; set => weaponsForSale = value; }
    public int AmountOfWeaponsForSale { get => amountOfWeaponsForSale; set => amountOfWeaponsForSale = value; }

    private void Start()
    {
        // add the seller to the game manager
        AddToGameManager();
    }

    public void AddSkillForSale(Skill skill)
    {
        skillsForSale.Add(skill);
    }
    public void AddWeaponForSale(Weapon weapon)
    {
        weaponsForSale.Add(weapon);
    }

    public void SetSoldOutSkill(int index, bool value)
    {
        soldOutSkill[index] = value;
    }

    public void SetSoldOutWeapon(int index, bool value)
    {
        soldOutWeapon[index] = value;
    }

    public bool GetSoldOutSkill(int index)
    {
        return soldOutSkill[index];
    }
    
    public bool GetSoldOutWeapon(int index)
    {
        return soldOutWeapon[index];
    }

    public bool AlreadyHasSkill(string name)
    {
        foreach(Skill skill in skillsForSale)
        {
            if(skill.SkillName == name)
            {
                return true;
            }
        }
        return false;
    }
    public bool AlreadyHasWeapon(string name)
    {
        foreach(Weapon weapon in weaponsForSale)
        {
            if(weapon.WeaponName == name)
            {
                return true;
            }
        }
        return false;
    }


}
