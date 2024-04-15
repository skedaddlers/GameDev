using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flame : Projectile
{
    [SerializeField] private float burnDuration;
    [SerializeField] private float burnRate;
    [SerializeField] private int burnDamage;

    public float BurnDuration { get => burnDuration; set => burnDuration = value; }
    public float BurnRate { get => burnRate; set => burnRate = value; }
    public int BurnDamage { get => burnDamage; set => burnDamage = value; }
}
