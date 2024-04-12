using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{

    [SerializeField] private bool blocksMovement;
    // Start is called before the first frame update

    public bool BlocksMovement{ get => blocksMovement; set => blocksMovement = value; }
    // Update is called once per frame
    public virtual void AddToGameManager(){
        if(GetComponent<Player>()){
            GameManager.Instance.InsertEntity(this, 0);
        }
        else{
            GameManager.Instance.AddEntity(this);
        }
    }
    public void Move(Vector3 direction){
        transform.position += direction;
    }

    public virtual EntityState SaveState() => new EntityState();
}

[System.Serializable]
public class EntityState {
    public enum EntityType {
        Actor,
        Item,
        Projectile,
        Other
    }
    [SerializeField] private EntityType type;
    [SerializeField] private string name;
    [SerializeField] private bool blocksMovement;
    [SerializeField] private Vector3 position;
    public EntityType Type { get => type; set => type = value; }
    public string Name { get => name; set => name = value; }   
    public bool BlocksMovement { get => blocksMovement; set => blocksMovement = value; }
    public Vector3 Position { get => position; set => position = value; }

    public EntityState(EntityType type = EntityType.Other, string name = "", bool blocksMovement = false, Vector3 position = new Vector3()){
        this.type = type;
        this.name = name;
        this.blocksMovement = blocksMovement;
        this.position = position;
    }
}
