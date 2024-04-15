using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flame : Projectile
{
    [SerializeField] private float burnDuration;
    [SerializeField] private float burnRate;
    [SerializeField] private int burnDamage;

    public override void CheckForCollision()
    {
        Actor target = null;
        Vector3Int gridPosition = MapManager.Instance.FloorMap.WorldToCell(transform.position);
        if(!MapManager.Instance.InBounds((int)gridPosition.x, (int)gridPosition.y) || MapManager.Instance.ObstacleMap.HasTile(gridPosition)){
            GameManager.Instance.RemoveEntity(this);
            if(isAOE){
                AOEAttack();
            }
            GameObject.Destroy(gameObject);
            return;
        }

        if(target != null){
            // Debug.Log($"{projectile.name} hits {target.name}!");
            if(GetComponent<Projectile>().IsAOE){
                AOEAttack();
            }
            else{
                int damage = GetComponent<Projectile>().Damage;
                int targetDefense = target.GetComponent<Fighter>().Defense;
                target.GetComponent<Fighter>().TakeDamage(damage * (int)(0.5f + (1 - (targetDefense / 20f))/2));
                GameObject burnEffect = Instantiate(Resources.Load<GameObject>("Prefabs/Effects/Burn"), target.transform.position, Quaternion.identity);
                burnEffect.GetComponent<Burn>().Duration = burnDuration;
                burnEffect.GetComponent<Burn>().Damage = burnDamage;
                burnEffect.GetComponent<Burn>().ApplyRate = burnRate;
                target.GetComponent<Fighter>().ApplyEffect(burnEffect.GetComponent<StatusEffect>());
            }
            GameManager.Instance.RemoveEntity(this);
            GameObject.Destroy(gameObject);
        }
    }
}
