using UnityEngine;

public class Action
{
    
    static public void PickupAction(Actor actor)
    {
        for(int i = 0; i < GameManager.Instance.Entities.Count; i++){
            if(GameManager.Instance.Entities[i].GetComponent<Item>() || GameManager.Instance.Entities[i].GetComponent<Weapon>()){
                float offsetX = GameManager.Instance.Entities[i].transform.position.x - actor.transform.position.x;
                float offsetY = GameManager.Instance.Entities[i].transform.position.y - actor.transform.position.y;
                if(Mathf.Abs(offsetX) > 1 || Mathf.Abs(offsetY) > 1)
                    continue;
            }
            if(GameManager.Instance.Entities[i].GetComponent<Weapon>()){
                Weapon weapon = GameManager.Instance.Entities[i].GetComponent<Weapon>();
                actor.Inventory.EquipWeapon(weapon);
                actor.GetComponent<Fighter>().Power = weapon.Damage;
                weapon.GetComponent<SpriteRenderer>().enabled = false;
                UIManager.Instance.UpdateWeapon(actor);
                return;
            }
            else if(GameManager.Instance.Entities[i].GetComponent<Item>()){
                if(actor.Inventory.Items.Count >= actor.Inventory.Capacity){
                    UIManager.Instance.AddMessage("Your inventory is full!", Utilz.RED);
                    return;
                }
                Item item = GameManager.Instance.Entities[i].GetComponent<Item>();
                actor.Inventory.Add(item);
                item.GetComponent<SpriteRenderer>().enabled = false;
                UIManager.Instance.AddMessage($"You picked up the {item.name}.", "#00FF00");
            }
            // Debug.Log("Picking up item");
            
        }
    }

    static public void DropAction(Actor actor, Item item){
        actor.Inventory.Drop(item);
        // MapManager.Instance.CreateEntity(item.name, actor.transform.position);
        // GameManager.Instance.AddEntity(item);

        UIManager.Instance.ToggleDropMenu();
        // GameManager.Instance.EndTurn();
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
        // GameManager.Instance.EndTurn();

    }

    static public void InteractAction(Actor actor, Vector3 playerPos){
        foreach(Entity entity in GameManager.Instance.Entities){
            if(entity.GetComponent<Seller>())
            {
                float offsetX = entity.transform.position.x - playerPos.x;
                float offsetY = entity.transform.position.y - playerPos.y;
                if(Mathf.Abs(offsetX) > 1 || Mathf.Abs(offsetY) > 1)
                    continue;
                UIManager.Instance.ToggleShopMenu(entity.GetComponent<Seller>());
                return;
            }
        }
    }

    static public void BuySkill(Actor actor, Skill skill){
        if(actor.GetComponent<Player>().Mora < skill.Cost){
            UIManager.Instance.AddMessage("You don't have enough gold!", Utilz.RED);
            return;
        }
        skill.gameObject.SetActive(true);
        skill.GetComponent<SpriteRenderer>().enabled = false;
        skill.transform.SetParent(actor.transform);
        actor.GetComponent<Player>().Mora -= skill.Cost;
        actor.GetComponent<Player>().AddSkill(skill);
        UIManager.Instance.AddMessage($"You bought the {skill.SkillName} skill!", Utilz.GREEN);
        // UIManager.Instance.ToggleShopMenu(null);
    }

    static public void BuyWeapon(Actor actor, Weapon weapon){
        if(actor.GetComponent<Player>().Mora < weapon.Cost){
            UIManager.Instance.AddMessage("You don't have enough gold!", Utilz.RED);
            return;
        }
        UIManager.Instance.AddMessage($"You bought the {weapon.WeaponName}!", Utilz.GREEN);
        // weapon.gameObject.SetActive(true);
        actor.GetComponent<Player>().Mora -= weapon.Cost;
        GameObject newWeapon = MapManager.Instance.CreateEntity(weapon.name, actor.transform.position);
        // actor.Inventory.EquipWeapon(newWeapon.GetComponent<Weapon>());
        // UIManager.Instance.UpdateWeapon(actor);
        // UIManager.Instance.ToggleShopMenu(null);
    }

    static public void CheckForCollision(Projectile projectile){
        Actor target = null;
        if(projectile.GetComponent<Projectile>().IsPlayerProjectile){
            target = GameManager.Instance.GetBlockingActorAtLocation(projectile.transform.position);
        }
        else{
            target = GameManager.Instance.GetBlockingPlayerAtLocation(projectile.transform.position);
        }
        Vector3Int gridPosition = MapManager.Instance.FloorMap.WorldToCell(projectile.transform.position);
        if(!MapManager.Instance.InBounds((int)gridPosition.x, (int)gridPosition.y) || MapManager.Instance.ObstacleMap.HasTile(gridPosition)){
            GameManager.Instance.RemoveEntity(projectile);
            GameObject.Destroy(projectile.gameObject);
            return;
        }

        if(target != null){
            if(projectile.GetComponent<Projectile>().IsAOE){
                AOEAttack(projectile);
            }
            else{        
                int damage = projectile.GetComponent<Projectile>().Damage;
                target.GetComponent<Fighter>().TakeDamage(damage);
                // Debug.Log($"Original damage: {damage}, Damage dealt: {damageDealt}");
                // UIManager.Instance.AddMessage($"{target.name} takes a {damageDealt} damage from the {projectile.name}!", "#FF0000");
                if(projectile.GetComponent<Flame>()){
                    GameObject burnEffect = GameObject.Instantiate(Resources.Load<GameObject>("Entities/Effect/Burn"), target.transform.position, Quaternion.identity);
                    burnEffect.GetComponent<Burn>().Duration = projectile.GetComponent<Flame>().BurnDuration;
                    burnEffect.GetComponent<Burn>().Damage = projectile.GetComponent<Flame>().BurnDamage;
                    burnEffect.GetComponent<Burn>().ApplyRate = projectile.GetComponent<Flame>().BurnRate;
                    burnEffect.GetComponent<Burn>().Fighter = target.GetComponent<Fighter>();
                    target.GetComponent<Fighter>().ApplyEffect(burnEffect.GetComponent<StatusEffect>());
                }
            }
            GameManager.Instance.RemoveEntity(projectile);
            GameObject.Destroy(projectile.gameObject);
        }
    }

    static public void AOEAttack(Projectile projectile){
        foreach(Actor target in GameManager.Instance.Actors) {
            if ((projectile.GetComponent<Projectile>().IsPlayerProjectile && target.GetComponent<Player>()) 
                || (!projectile.GetComponent<Projectile>().IsPlayerProjectile && !target.GetComponent<Player>()))
                continue;

            float distance = Vector3.Distance(projectile.transform.position, target.transform.position);
            if (distance <= projectile.GetComponent<Projectile>().AoeRadius) {
                int damage = projectile.GetComponent<Projectile>().Damage;
                target.GetComponent<Fighter>().TakeDamage(damage);
                // Debug.Log($"Original damage: {damage}, Damage dealt: {damageDealt}");   
                UIManager.Instance.AddMessage($"{target.name} takes an AOE attack from the {projectile.name}!", Utilz.RED);
            }
        }
    }

    static public void SlashAction(Actor actor, Vector3 direction){
        // slashes in a fan shape area, dealing aoe damage to the enemies in the area
        Weapon weapon = actor.GetComponent<Inventory>().Weapon;
        int damage = weapon.Damage + actor.GetComponent<Fighter>().Power;
        float area = weapon.Radius;
        float fanAngle = 60f;

        for(int i = 0; i < GameManager.Instance.Actors.Count; i++){
            Actor target = GameManager.Instance.Actors[i];
            if(target == actor)
                continue;
            Vector3 targetDirection = target.transform.position - actor.transform.position;
            float angle = Vector3.Angle(direction, targetDirection);
            if(angle < fanAngle && targetDirection.magnitude < area){  
                int damageDealt = damage;
                if(actor.GetComponent<Player>()){
                    if(Random.value < actor.GetComponent<Player>().CritRate){
                        damageDealt = (int)(damageDealt * actor.GetComponent<Player>().CritDamage);
                        UIManager.Instance.AddMessage($"{actor.name} critically slashes {target.name} for {damageDealt} damage!", Utilz.ORANGE);
                    }
                }
                target.GetComponent<Fighter>().TakeDamage(damageDealt);
            }
        }

        UIManager.Instance.DrawFanSprite(actor.transform.position, direction, fanAngle, area);
    }

    static public void RangedAction(Actor actor, Vector3 direction){
        if(actor.GetComponent<Player>()){
            MapManager.Instance.CreateProjectile("Bubble" ,actor.transform.position, direction, actor.GetComponent<Fighter>().Power, true);
        }
        else{
            MapManager.Instance.CreateProjectile("Bubble", actor.transform.position, direction, actor.GetComponent<Fighter>().Power, false);
        }

    }


    static public void MeleeAction(Actor actor, Actor target){
        int targetDefense = target.GetComponent<Fighter>().Defense;
        int damage = actor.GetComponent<Fighter>().Power;

        string attackDesc = $"{actor.name} attacks {target.name}";

        if(damage > 0){
            if(target.GetComponent<Fighter>().ShieldHp > 0){
                int shieldDamage = Mathf.Min(target.GetComponent<Fighter>().ShieldHp, damage);
                int damageDealt = damage - shieldDamage;
                if(shieldDamage >= damage){
                    UIManager.Instance.AddMessage($"{attackDesc} but is blocked by the shield.", Utilz.RED);
                }
                else{
                    UIManager.Instance.AddMessage($"{attackDesc} and breaks the shield!", Utilz.ORANGE);
                }
            }
            else{
                // UIManager.Instance.AddMessage($"{attackDesc} for {damage} damage!", colorHex);
            }
            target.GetComponent<Fighter>().TakeDamage(damage);
        }
        else{
            // UIManager.Instance.AddMessage($"{attackDesc} but does no damage.", colorHex);
        }

        // GameManager.Instance.EndTurn();
    }

    static public void DashAction(Actor actor, Vector3 direction){
        actor.Move(direction * 3);
        // GameManager.Instance.EndTurn();
    }

    static public void MovementAction(Actor actor, Vector3 direction){
        if(!isValidPosition(actor, actor.transform.position + direction))
            return;
        actor.Move(direction);
        // GameManager.Instance.EndTurn();
        if(actor.GetComponent<Player>()){
            Camera.main.transform.position = new Vector3(actor.transform.position.x, actor.transform.position.y, -10);
        }
    }
    static private bool isValidPosition(Actor actor, Vector3 futurePosition)
    {
        Vector3Int gridPosition = MapManager.Instance.FloorMap.WorldToCell(futurePosition);
        if(!MapManager.Instance.InBounds(gridPosition.x, gridPosition.y) || MapManager.Instance.ObstacleMap.HasTile(gridPosition) || futurePosition == actor.transform.position)
            return false;
        return true;
    }

    static public void SkipAction(){
        // GameManager.Instance.EndTurn();
    }
}