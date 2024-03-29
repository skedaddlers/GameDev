using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;
    [SerializeField] private float time = 0.1f;

    [SerializeField] private bool isPlayerTurn = true;

    [SerializeField] private int entityNum = 0;
    [SerializeField] private List<Entity> entities = new List<Entity>();

    public bool IsPlayerTurn
    {
        get => isPlayerTurn;
        set => isPlayerTurn = value; // Add a setter for IsPlayerTurn
    }
    // Start is called before the first frame update
    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void StartTurn(){
        if(entities[entityNum].GetComponent<Player>())
            IsPlayerTurn = true;
        else if (entities[entityNum].IsSentient)
            Action.SkipAction(entities[entityNum]);
    }

    public void EndTurn(){
        if(entities[entityNum].GetComponent<Player>())
            IsPlayerTurn = false;

        if (entityNum == entities.Count - 1)
            entityNum = 0;
        else
            entityNum++;
        StartCoroutine(TurnDelay());
    }

    private IEnumerator TurnDelay(){
        yield return new WaitForSeconds(time);
        StartTurn();
    }

    public void AddEntity(Entity entity){
        entities.Add(entity);
    }

    public void InsertEntity(Entity entity, int index){
        entities.Insert(index, entity);
    }
}
