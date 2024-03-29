using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{

    [SerializeField] private bool isSentient = false;
    // Start is called before the first frame update

    public bool IsSentient{
        get => isSentient;
    }

    void Start()
    {
        if(GetComponent<Player>())
        {
            GameManager.Instance.InsertEntity(this, 0);
        }
        else if(isSentient){
            GameManager.Instance.AddEntity(this);
        }
    }
    // Update is called once per frame
    public void Move(Vector2 direction){
        transform.position += (Vector3)direction;
    }
}
