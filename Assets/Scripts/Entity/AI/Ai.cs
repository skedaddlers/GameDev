using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Actor), typeof(AStar))]
public class Ai : MonoBehaviour
{
    [SerializeField] private AStar aStar;

    public AStar AStar { get => aStar; set => aStar = value; }

    public void OnValidate() => aStar = GetComponent<AStar>();

    public void MoveAlongPath(Vector3Int targetPos){
        Debug.Log("Moving along path");
        Vector3Int gridPos = MapManager.Instance.FloorMap.WorldToCell(transform.position); 
        Vector2 dir = aStar.Compute((Vector2Int)gridPos, (Vector2Int)targetPos);
        Action.MovementAction(GetComponent<Actor>(), dir);
    }
}