using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : Entity
{
    [SerializeField] private bool isAlive = true;
    [SerializeField] private int fieldOfViewRange = 16;
    [SerializeField] private List<Vector3Int> fieldOfView = new List<Vector3Int>();
    [SerializeField] private Ai ai;
    [SerializeField] Inventory inventory;
    [SerializeField] private Fighter fighter;
    AdamMilVisibility algorithm;
    public bool IsAlive { get => isAlive; set => isAlive = value; }
    public List<Vector3Int> FieldOfView { get => fieldOfView; }
    public Inventory Inventory { get => inventory; }
    private void OnValidate(){
        if(GetComponent<Ai>()){
            ai = GetComponent<Ai>();
        }

        if(GetComponent<Inventory>()){
            inventory = GetComponent<Inventory>();
        }

        if(GetComponent<Fighter>()){
            fighter = GetComponent<Fighter>();
        }
    }

    void Start()
    {
        AddToGameManager();
        if(isAlive){
            algorithm = new AdamMilVisibility();
            UpdateFieldOfView();
        }
        else if(fighter != null){
            fighter.Die();
        }       
    }

    private void Update() {
        if(isAlive){
            UpdateFieldOfView();
        }
    }

    public override void AddToGameManager(){
        base.AddToGameManager();

        if(GetComponent<Player>()){
            GameManager.Instance.InsertEntity(this, 0);
        }
        else{
            GameManager.Instance.AddEntity(this);
        }
    }

    public void UpdateFieldOfView() {
        Vector3Int gridPosition = MapManager.Instance.FloorMap.WorldToCell(transform.position);

        fieldOfView.Clear();
        algorithm.Compute(gridPosition, fieldOfViewRange, fieldOfView);

        if (GetComponent<Player>()) {
            MapManager.Instance.UpdateFogMap(fieldOfView);
            // MapManager.Instance.SetEntitiesVisibilities();
        }
    }

    public override EntityState SaveState() => new ActorState(
        name: name,
        blocksMovement: BlocksMovement,
        isAlive: IsAlive,
        position: transform.position,
        currentAI: ai != null ? ai.SaveState() : null,
        fighterState: fighter != null ? fighter.SaveState() : null
    );

    public void LoadState(ActorState state) {
        BlocksMovement = state.BlocksMovement;
        transform.position = state.Position;

        if(!isAlive){
            GameManager.Instance.RemoveActor(this);
        }

        if(state.CurrentAI != null){
            if(state.CurrentAI.Type == "HostileEnemy"){
                ai = GetComponent<HostileEnemy>();
            }
        }
        if(state.FighterState != null){
            fighter.LoadState(state.FighterState);
        }
    }

    
}

[System.Serializable]
public class ActorState : EntityState
{
    [SerializeField] private bool isAlive;
    [SerializeField] private AiState currentAI;
    [SerializeField] private FighterState fighterState;
    public bool IsAlive { get => isAlive; set => isAlive = value; }
    public AiState CurrentAI { get => currentAI; set => currentAI = value; }
    public FighterState FighterState { get => fighterState; set => fighterState = value; }

    public ActorState(EntityType type = EntityType.Actor, string name = "", bool blocksMovement = false, Vector3 position = new Vector3(), 
     bool isAlive = true, AiState currentAI = null, FighterState fighterState = null) : base(type, name, blocksMovement, position) {
        this.isAlive = isAlive;
        this.currentAI = currentAI;
        this.fighterState = fighterState;
    }
}
