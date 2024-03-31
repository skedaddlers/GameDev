using UnityEngine;

static public class Action
{
    static public void EscapeAction(){
        Debug.Log("Quit");
    }

    static public bool BumpAction(Entity entity, Vector2 dir){
        Entity target = GameManager.Instance.GetBlockingEntityAtLocation(entity.transform.position + (Vector3)dir);

        if(target){
            MeleeAction(target);
            return false;
        }
        else{
            // MovementAction(entity, dir);
            return true;
        }
    }

    static public void MeleeAction(Entity target){
        Debug.Log("Auwo AWOK! You hit me for 1 damage!");
        GameManager.Instance.EndTurn();
    }

    static public void MovementAction(Entity entity, Vector2 direction){
        entity.Move(direction);
        GameManager.Instance.EndTurn();
    }

    static public void SkipAction(Entity entity){
        if(entity.GetComponent<Player>())
        {

        }
        else if(entity.IsSentient)
        {

        }
        GameManager.Instance.EndTurn();
    }
}