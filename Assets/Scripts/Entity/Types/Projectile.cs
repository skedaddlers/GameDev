using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Entity
{
    // Start is called before the first frame update
    [SerializeField] private float speed;
    [SerializeField] private int damage = 5;
    [SerializeField] private Vector3 direction;

    public int Damage { get => damage; }
    public Vector3 Direction { get => direction; set => direction = value; }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
        Action.CheckForCollision(this);
    }

}
