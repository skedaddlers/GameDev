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

        algorithm = new AdamMilVisibility();
        UpdateFieldOfView();
    }

    private void Update() {
        if(isAlive){
            UpdateFieldOfView();
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
    // Update is called once per frame


}
