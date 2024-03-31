using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : Entity
{
    [SerializeField] private bool isAlive = true;
    // Start is called before the first frame update
    [SerializeField] private Ai ai;
    public bool IsAlive { get => isAlive; set => isAlive = value; }

    private void OnValidate(){
        if(GetComponent<Ai>()){
            ai = GetComponent<Ai>();
        }
    }

    void Start()
    {
        AddToGameManager();
        if(GetComponent<Player>())
        {
            GameManager.Instance.InsertActor(this, 0);
        }
        else if(isAlive){
            GameManager.Instance.AddActor(this);
        }
    }
    // Update is called once per frame


}
