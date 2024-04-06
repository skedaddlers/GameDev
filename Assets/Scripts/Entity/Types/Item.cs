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
    void Start()
    {
        AddToGameManager();
    }

}
