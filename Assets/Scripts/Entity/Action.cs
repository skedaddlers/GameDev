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

        string attackDesc = $"{actor.name} attacks {target.name}";

        string colorHex = "";

        if(actor.GetComponent<Player>()){
            colorHex = "#FFFFFF";
        }
        else{
            colorHex = "#d1a3a4";
        }
        if(damage > 0){
            UIManager.Instance.AddMessage($"{attackDesc} for {damage} damage!", colorHex);
            target.GetComponent<Fighter>().Hp -= damage;
        }
        else{
            UIManager.Instance.AddMessage($"{attackDesc} but does no damage.", colorHex);
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