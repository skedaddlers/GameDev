using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Entity
{
    [SerializeField] private string weaponName = "";
    [SerializeField] private int damage = 0;
    [SerializeField] private float radius = 0;
    [SerializeField] private Sprite icon;
    [SerializeField] private float attackSpeed = 0f;
    [SerializeField] private int cost = 0;
    [SerializeField] private string description = "";

    public string WeaponName { get => weaponName; }
    public int Damage { get => damage; }
    public float Radius { get => radius; }
    public Sprite Icon { get => icon; }
    public float AttackSpeed { get => attackSpeed; }
    public int Cost { get => cost; }
    public string Description { get => description; }

    private void Start() => AddToGameManager();
}
