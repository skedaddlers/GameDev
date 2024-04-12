using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    [Header("Time")]
    [SerializeField] private float delayTime;

    [Header("Entities")]
    [SerializeField] private bool isPlayerTurn = true;
    [SerializeField] private List<Entity> entities;
    [SerializeField] private List<Actor> actors;
    [SerializeField] private List<VFX> vfx;


    [Header("Death")]
    [SerializeField] private Sprite deadSprite;
    public bool IsPlayerTurn { get => isPlayerTurn; }
    public List<Entity> Entities { get => entities; }
    public List<Actor> Actors { get => actors; }
    public Sprite DeadSprite { get => deadSprite; }
    public List<VFX> VFX { get => vfx; }
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

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode){
        SceneState sceneState = SaveManager.Instance.Save.Scenes.Find(x => x.FloorNumber == SaveManager.Instance.CurrentFloor);
        if(sceneState != null){
            LoadState(sceneState.GameState);
        }
        else{
            entities = new List<Entity>();
            actors = new List<Actor>();
            vfx = new List<VFX>();
        }
    }

    private void Start(){
        Application.targetFrameRate = 60;
    }

    private void Update(){
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
            // if the actor is not the player and blocks movement

            if(actor.GetComponent<Player>() == null){
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

    public void InsertEntity(Entity entity, int index){
        if(!entity.gameObject.activeSelf)
            entity.gameObject.SetActive(true);
        entities.Insert(index, entity);
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

    public void AddVFX(VFX effect){
        vfx.Add(effect);
    }
    public void RemoveVFX(VFX effect){
        vfx.Remove(effect);
        Destroy(effect.gameObject);
    }
    public void RemoveVFXByNames(string name){
        for(int i = 0; i < vfx.Count; i++){
            if(vfx[i].name == name){
                Destroy(vfx[i].gameObject);
                vfx.RemoveAt(i);
                i--;
            }
        }
    } 
    // private float SetTime() => baseTime / actors.Count;

    public GameState SaveState(){
        
        foreach(Item item in actors[0].Inventory.Items){
            if(entities.Contains(item)){
                continue;
            }
            AddEntity(item);
        }
        GameState gameState = new GameState(entities: entities.ConvertAll(x => x.SaveState()));

        foreach(Item item in actors[0].Inventory.Items){
            RemoveEntity(item);
        }
        return gameState;
    }

    public void LoadState(GameState gameState){
        if(entities.Count > 0){
            foreach(Entity entity in entities){
                Destroy(entity.gameObject);
            }
            foreach(VFX effect in vfx){
                Destroy(effect.gameObject);
            }
            foreach(Actor actor in actors){
                Destroy(actor.gameObject);
            }
            entities.Clear();
            actors.Clear();
            vfx.Clear();
        }

        StartCoroutine(LoadEntityStates(gameState.Entities));
    }

    private IEnumerator LoadEntityStates(List<EntityState> entityStates){
        int entityState = 0;
        while(entityState < entityStates.Count){
            yield return new WaitForEndOfFrame();
            string entityName = entityStates[entityState].Name.Contains("Remains of") ?
                entityStates[entityState].Name.Substring(entityStates[entityState].Name.LastIndexOf(' ') + 1) : entityStates[entityState].Name;

            if(entityStates[entityState].Type == EntityState.EntityType.Actor){
                ActorState actorState = entityStates[entityState] as ActorState;
                Actor actor = MapManager.Instance.CreateEntity(entityName, actorState.Position).GetComponent<Actor>();
                actor.LoadState(actorState);

                // // Check if the actor is the player
                // if (actor.GetComponent<Player>() != null)
                // {
                //     // Save and load the player's mana
                //     Player player = actor.GetComponent<Player>();
                //     player.Mana = actorState.Mana;
                // }
            }
            else if(entityStates[entityState].Type == EntityState.EntityType.Item){
                ItemState itemState = entityStates[entityState] as ItemState;
                Item item = MapManager.Instance.CreateEntity(entityName, itemState.Position).GetComponent<Item>();
                item.LoadState(itemState);
            }
            else if(entityStates[entityState].Type == EntityState.EntityType.Projectile){
                ProjectileState projectileState = entityStates[entityState] as ProjectileState;
                Projectile projectile = MapManager.Instance.CreateEntity(entityName, projectileState.Position).GetComponent<Projectile>();
                projectile.LoadState(projectileState);
            }

            entityState++;

        }
    }
}


[System.Serializable]
public class GameState
{
    [SerializeField] private List<EntityState> entities;
    public List<EntityState> Entities { get => entities; set => entities = value; }
    public GameState(List<EntityState> entities){
        this.entities = entities;
    }
}
