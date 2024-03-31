using UnityEngine;

static public class Action
{
    static public void EscapeAction(){
        Debug.Log("Quit");
    }

    static public bool BumpAction(Actor actor, Vector3 dir){
        // Debug.Log($"{actor.name} moves {dir}.");
        Actor target = GameManager.Instance.GetBlockingActorAtLocation(actor.transform.position + (Vector3)dir);

        if(target != null){
            Debug.Log($"{actor.name} bumps into {target.name}!");
            MeleeAction(actor, target);
            return false;
        }
        else{
            // Debug.Log($"{actor.name} moves {dir}.");
            MovementAction(actor, dir);
            return true;
        }
    }

    static public void MeleeAction(Actor actor, Actor target){
        int damage = actor.GetComponent<Fighter>().Power - target.GetComponent<Fighter>().Defense;

        string attackDesc = $"{actor.name} attacks {target.name} for {damage} damage!";
        if(damage > 0){
            Debug.Log(attackDesc);
            target.GetComponent<Fighter>().Hp -= damage;
        }
        else{
            attackDesc = $"{actor.name} attacks {target.name} but does no damage...";
            Debug.Log(attackDesc);
        }
        GameManager.Instance.EndTurn();
    }

    static public void MovementAction(Actor actor, Vector3 direction){
        actor.Move(direction);
        GameManager.Instance.EndTurn();
    }

    static public void SkipAction(){
        GameManager.Instance.EndTurn();
    }
}