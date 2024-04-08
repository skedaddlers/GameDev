using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    [Header("Time")]
    [SerializeField] private float baseTime = 0.075f;
    [SerializeField] private float delayTime;

    [Header("Entities")]
    [SerializeField] private bool isPlayerTurn = true;
    [SerializeField] private int actorNum = 0;
    [SerializeField] private List<Entity> entities = new List<Entity>();
    [SerializeField] private List<Actor> actors = new List<Actor>();

    [Header("Death")]
    [SerializeField] private Sprite deadSprite;
    public bool IsPlayerTurn { get => isPlayerTurn; }
    public List<Entity> Entities { get => entities; }
    public List<Actor> Actors { get => actors; }
    public Sprite DeadSprite { get => deadSprite; }
    float deltaTime = 0.0f;


    void OnGUI()
    {
        int w = Screen.width, h = Screen.height;

        GUIStyle style = new GUIStyle();

        Rect rect = new Rect(0, 0, w, h * 2 / 100);
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = h * 2 / 50;
        style.normal.textColor = new Color (1.0f, 1.0f, 1.0f, 1.0f);
        float msec = deltaTime * 1000.0f;
        float fps = 1.0f / deltaTime;
        string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
        GUI.Label(rect, text, style);
    }
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

    private void Start(){
        Application.targetFrameRate = 60;
    }

    private void Update(){
        foreach(Actor actor in actors){
            if(actor.GetComponent<Player>()){
                isPlayerTurn = true;
            }
            else 
                actor.GetComponent<HostileEnemy>().RunAI();
        }
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
    }

    // private void StartTurn(){
    //     if(actors[actorNum].GetComponent<Player>())
    //         isPlayerTurn = true;
    //     else {
    //         if (actors[actorNum].GetComponent<HostileEnemy>()){
    //             actors[actorNum].GetComponent<HostileEnemy>().RunAI();
    //         }
    //         else{
    //             Action.SkipAction();
    //         }
    //     }
    // }

    // public void EndTurn(){
    //     if(actors[actorNum].GetComponent<Player>())
    //         isPlayerTurn = false;

    //     if (actorNum == actors.Count - 1)
    //         actorNum = 0;
    //     else
    //         actorNum++;
    //     StartCoroutine(TurnDelay());
    // }

    // private IEnumerator TurnDelay(){
    //     yield return new WaitForSeconds(delayTime);
    //     StartTurn();
    // }

    public Actor GetBlockingActorAtLocation(Vector3 location){
        foreach(Actor actor in Actors){
            if(actor.BlocksMovement && actor.GetComponent<Player>() == null){
                float offsetX = actor.transform.position.x - location.x;
                float offsetY = actor.transform.position.y - location.y;
                // if(Mathf.Abs(offsetX) < 2 && Mathf.Abs(offsetY) < 2)
                    // Debug.Log("Offset X: " + offsetX + " Offset Y:" + offsetY);
                if(Mathf.Abs(offsetX) < 0.5f && Mathf.Abs(offsetY) < 0.5f){
                    return actor;
                }
            }
        }
        return null;
    }

    public void AddEntity(Entity entity){
        if(!entity.gameObject.activeSelf)
            entity.gameObject.SetActive(true);
        entities.Add(entity);
    }

    public void RemoveEntity(Entity entity){
        entity.gameObject.SetActive(false);
        entities.Remove(entity);
    }

    public void AddActor(Actor actor){
        actors.Add(actor);
        // delayTime = SetTime();
    }

    public void InsertActor(Actor actor, int index){
        actors.Insert(index, actor);
        // delayTime = SetTime();
    }

    public void RemoveActor(Actor actor){
        actors.Remove(actor);
        // delayTime = SetTime();
    }

    // private float SetTime() => baseTime / actors.Count;
}
