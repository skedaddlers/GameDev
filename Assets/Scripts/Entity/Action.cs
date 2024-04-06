using UnityEngine;

static public class Action
{
    
    static public void PickupAction(Actor actor)
    {
        for(int i = 0; i < GameManager.Instance.Entities.Count; i++){
            if(GameManager.Instance.Entities[i].GetComponent<Actor>() || actor.transform.position != GameManager.Instance.Entities[i].transform.position){
                float offsetX = GameManager.Instance.Entities[i].transform.position.x - actor.transform.position.x;
                float offsetY = GameManager.Instance.Entities[i].transform.position.y - actor.transform.position.y;
                if(Mathf.Abs(offsetX) > 1 || Mathf.Abs(offsetY) > 1)
                    continue;
            }

            if(actor.Inventory.Items.Count >= actor.Inventory.Capacity){
                UIManager.Instance.AddMessage("Your inventory is full!", "#FF0000");
                return;
            }

            Item item = GameManager.Instance.Entities[i].GetComponent<Item>();
            item.transform.SetParent(actor.transform);
            actor.Inventory.Items.Add(item);

            UIManager.Instance.AddMessage($"You picked up the {item.name}.", "#00FF00");

            GameManager.Instance.RemoveEntity(item);
            GameManager.Instance.EndTurn();
        }
    }

    static public void DropAction(Actor actor, Item item){
        actor.Inventory.Drop(item);

        UIManager.Instance.ToggleDropMenu();
        GameManager.Instance.EndTurn();
    }

    static public void UseAction(Actor actor , int index){
        Item item = actor.Inventory.Items[index];

        bool itemUsed = false;

        if(item.GetComponent<Consumable>()){
            itemUsed = item.GetComponent<Consumable>().Activate(actor, item);
        }

        if(!itemUsed){
            return;
        }

        UIManager.Instance.ToggleInventory();
        GameManager.Instance.EndTurn();

    }
    static public bool BumpAction(Actor actor, Vector3 dir){
        Actor target = GameManager.Instance.GetBlockingActorAtLocation(actor.transform.position + (Vector3)dir);

        if(target != null){
            Debug.Log($"{actor.name} bumps into {target.name}!");
            MeleeAction(actor, target);
            return false;
        }
        else{
            MovementAction(actor, dir);
            // Make the camera moving code here
            Camera.main.transform.position = new Vector3(actor.transform.position.x, actor.transform.position.y, -10);
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