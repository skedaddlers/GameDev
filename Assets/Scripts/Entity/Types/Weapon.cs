using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Entity
{
    [SerializeField] private int damage = 0;
    [SerializeField] private float radius = 0;
    [SerializeField] private Sprite icon;
    [SerializeField] private float attackSpeed = 0f;
    public int Damage { get => damage; }
    public float Radius { get => radius; }
    public Sprite Icon { get => icon; }
    public float AttackSpeed { get => attackSpeed; }

    private void Start() => AddToGameManager();
}
