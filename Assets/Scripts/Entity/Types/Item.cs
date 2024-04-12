using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : Entity
{
    [SerializeField] private Consumable consumable;
    public Consumable Consumable { get => consumable; }
    // Start is called before the first frame update
    
    private void OnValidate(){
        if(GetComponent<Consumable>()){
            consumable = GetComponent<Consumable>();
        }
    }
    private void Start() => AddToGameManager();

    public override EntityState SaveState() => new ItemState(
        name: name,
        blocksMovement: BlocksMovement,
        position: transform.position,
        parent: transform.parent != null ? transform.parent.name : ""
    );

    public void LoadState(ItemState state){
        if(state.Parent != ""){
            GameObject parent = GameObject.Find(state.Parent);
            parent.GetComponent<Inventory>().Add(this);
        }
        transform.position = state.Position;
    }
}

[System.Serializable]
public class ItemState : EntityState {
    [SerializeField] private string parent;
    public string Parent { get => parent; set => parent = value; }
    public ItemState(EntityType type = EntityType.Item, string name = "", bool blocksMovement = false, Vector3 position = new Vector3(), 
     string parent = "") : base(type, name, blocksMovement, position)
    {
        this.parent = parent;
    }
}
