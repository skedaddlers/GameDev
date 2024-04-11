using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Entity
{
    // Start is called before the first frame update
    [SerializeField] private float speed;
    [SerializeField] private int damage = 5;
    [SerializeField] private Vector3 direction;

    public int Damage { get => damage; set => damage = value;}
    public Vector3 Direction { get => direction; set => direction = value; }

    void Update()
    {
        // Destroy if off screen
        Actor player = GameManager.Instance.Actors[0];
        float xDistance = Mathf.Abs(player.transform.position.x - transform.position.x);
        float yDistance = Mathf.Abs(player.transform.position.y - transform.position.y);
        if(xDistance > Camera.main.orthographicSize * Camera.main.aspect || yDistance > Camera.main.orthographicSize){
            Destroy(gameObject);
            return;
        }
        transform.position += direction * speed * Time.deltaTime;
        Action.CheckForCollision(this);
    }

}
