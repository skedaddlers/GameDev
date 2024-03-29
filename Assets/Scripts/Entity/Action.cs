using UnityEngine;

static public class Action
{
    static public void EscapeAction(){
        Debug.Log("Quit");
    }

    static public void MovementAction(Entity entity, Vector2 direction){
        entity.Move(direction);
        GameManager.Instance.EndTurn();
    }

    static public void SkipAction(Entity entity){
        GameManager.Instance.EndTurn();
    }
}