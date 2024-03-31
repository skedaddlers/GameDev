using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{

    [SerializeField] private bool blocksMovement;
    // Start is called before the first frame update

    public bool BlocksMovement{ get => blocksMovement; set => blocksMovement = value; }
    // Update is called once per frame
    public void AddToGameManager(){
        GameManager.Instance.Entities.Add(this);
    }
    public void Move(Vector3 direction){
        transform.position += direction;
    }
}
