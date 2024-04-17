using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this class is for the boss enemy that only appears in the final room of the dungeon
// the boss enemy will have a normal attack, 4 skills, and 2 special attacks
// the boss enemy will have skill similar to the player's skills
// it's normal attack will just shoot a projectile at the player at a normal rate
// special attack 1 will be it running around in a circular motion around the center of the room and shooting projectiles in rapid succession
// special attack 2 will be it staying in the center of the room and shooting projectiles in all directions
// skill 1 will be a summoning skill that will summon a minion to fight for the boss
// skill 2 will be a healing skill that will heal the boss for a certain duration
// skill 3 will summon a shield that will protect the boss from all damage for a certain duration
// skill 4 will be it having an aura that will deal damage to the player if they get too close
// every few seconds the boss will use a random skill
// special attacks have their durations
// special attack 1 will be used first when the boss is at 80% health
// special attack 2 will be used first when the boss is at 60% health
// the boss will be enraged when it is at 50% health and will have increased attack speed and movement speed, skills and special attacks will be used more frequently
// after 50% health, special attack 1 will be used every 20 seconds and will switch between special attack 2 and special attack 1 after that
// when performing a special attack, the boss will be won't be doing normal attacks and skills

public class BossEnemy : HostileEnemy
{

    [SerializeField] private Vector3 center;
    [SerializeField] private float radius;
    [SerializeField] private float skillCooldown = 10;
    [SerializeField] private float skillTimer = 0;
    // [SerializeField] private float attackTimer = 0;
    // [SerializeField] private float attackCooldown = 3;
    [SerializeField] private float specialCooldown = 20;
    [SerializeField] private float specialTimer = 0;
    [SerializeField] private float specialDuration = 5;
    [SerializeField] private float skillDuration = 5;
    [SerializeField] private float skillDurationTimer;
    // [SerializeField] private float performSpecialAttackTimer = 0;
    [SerializeField] private bool hasUsedSkill = false;
    [SerializeField] private bool isEnraged = false;
    [SerializeField] private bool hasPerformed1 = false;
    [SerializeField] private bool hasPerformed2 = false;
    [SerializeField] private bool isPerformingSpecialAttack = false;
    [SerializeField] private bool toggleSpecial = false;
    // [SerializeField] private SpriteRenderer spriteRenderer;

    public Vector3 Center { get => center; set => center = value; }

    public override void RunAI(){
        if(!fighter.Target){
            fighter.Target = GameManager.Instance.Actors[0];
        }
        else if(fighter.Target && !fighter.Target.IsAlive){
            fighter.Target = null;
        }
        
        if(fighter.Target && fighter.Target.IsAlive){
            if(!isPerformingSpecialAttack){
                Vector3Int targetPos = MapManager.Instance.FloorMap.WorldToCell(fighter.Target.transform.position);
                float targetDistance = Vector3.Distance(transform.position, fighter.Target.transform.position);
                if(fighter.Hp <= fighter.MaxHp * 0.5){
                    if(!isEnraged){
                        EnrageBoss();
                    }
                }

                if(targetDistance > 1f){
                    MoveAlongPath(targetPos, fighter.MovementSpeed);
                }

                if(attackTimer >= attackCooldown){
                    Vector2 direction = (fighter.Target.transform.position - transform.position).normalized;
                    Attack(direction, fighter.Power);
                    attackTimer = 0;
                }
                else{
                    attackTimer += Time.deltaTime;
                }

                if(skillTimer >= skillCooldown){
                    hasUsedSkill = true;
                    UseSkill();
                    skillTimer = 0;
                }
                else{
                    skillTimer += Time.deltaTime;
                }

                if(hasUsedSkill){
                    skillDurationTimer += Time.deltaTime;
                    if(skillDurationTimer >= skillDuration){
                        EndAllSkills();
                        hasUsedSkill = false;
                        skillDurationTimer = 0;
                    }
                }

                if(fighter.Hp < fighter.MaxHp * 0.8){
                    if(!hasPerformed1){
                        EndAllSkills();
                        UIManager.Instance.AddMessage("Uh Oh... Focalors is going to do something crazy!", Utilz.PURPLE);
                        StartCoroutine(SpecialAttack1());
                        hasPerformed1 = true;
                    }
                }
                if(fighter.Hp < fighter.MaxHp * 0.6){
                    if(!hasPerformed2){
                        EndAllSkills();
                        UIManager.Instance.AddMessage("Uh Oh... Focalors is going to do something crazy!", Utilz.PURPLE);
                        StartCoroutine(SpecialAttack2());
                        hasPerformed2 = true;
                    }
                }

                if(isEnraged){
                    specialTimer += Time.deltaTime;
                    if(specialTimer >= specialCooldown){
                        if(toggleSpecial){
                            EndAllSkills();
                            UIManager.Instance.AddMessage("Uh Oh... Focalors is going to do something crazy!", Utilz.PURPLE);
                            StartCoroutine(SpecialAttack1());
                            toggleSpecial = false;
                        }
                        else{
                            EndAllSkills();
                            UIManager.Instance.AddMessage("Uh Oh... Focalors is going to do something crazy!", Utilz.PURPLE);
                            StartCoroutine(SpecialAttack2());
                            toggleSpecial = true;
                        }
                        specialTimer = 0;
                    }
                }
            }
        }
    }

    private void Attack(Vector2 direction, int damage){
        // shoot projectile at player
        AudioManager.Instance.PlaySFX("EnemyShoot");
        MapManager.Instance.CreateProjectile("Evil Bubble", transform.position, direction, damage, false);
    }

    private void UseSkill(){
        // use a random skill
        int skill = Random.Range(1, 5);
        switch(skill){
            case 1:
                Skill1();
                break;
            case 2:
                Skill2();
                break;
            case 3:
                Skill3();
                break;
            case 4:
                Skill4();
                break;
        }
    }

    private void Skill1(){
        // summon 3 minions to fight for the boss
        UIManager.Instance.AddMessage("Focalors summoned the whole salon members! Oh no... They're seems to be angry at you!", Utilz.PURPLE);
        MapManager.Instance.CreateEntity("Evil Crab", transform.position);
        MapManager.Instance.CreateEntity("Evil Squid", transform.position);
        MapManager.Instance.CreateEntity("Evil Seahorse", transform.position);
    }

    private void Skill2(){
        UIManager.Instance.AddMessage("Focalors is summoned The Singer of Many Waters... It seems to be healing her! How Frustrating!", Utilz.PURPLE);
        MapManager.Instance.CreateEntity("Evil Singer", transform.position);
    }
    private void Skill3(){
        UIManager.Instance.AddMessage("Focalors is summoning a shield to protect her! It was... Her aspirations to be the sinners all along!", Utilz.PURPLE);
        fighter.ShieldHp = 30;
        MapManager.Instance.GenerateEffect("Evil Shield", GetComponent<Actor>(), skillDuration, 1, 1, false);
    }
    private void Skill4(){
        // have an aura that will deal damage to the player if they get too close
        UIManager.Instance.AddMessage("Focalors is emitting an evil aura! But why??? She's not the archon anymore! Better not get too close!", Utilz.PURPLE);
        MapManager.Instance.GenerateEffect("Evil Aura", GetComponent<Actor>(), skillDuration, 6, 2, false);
        if(Vector3.Distance(transform.position, fighter.Target.transform.position) <= 5f){
            fighter.Target.GetComponent<Fighter>().TakeDamage(5);
        }
    }

    private void EndAllSkills(){
        // end all skills
        fighter.ShieldHp = 0;
        GameManager.Instance.RemoveVFXByNames("Evil Shield");
        GameManager.Instance.RemoveVFXByNames("Evil Aura");
        for(int i = 0; i < GameManager.Instance.Entities.Count; i++){
            if(GameManager.Instance.Entities[i].GetComponent<SalonMember>()){
                if(GameManager.Instance.Entities[i].GetComponent<SalonMember>().IsEvil){
                    GameObject.Destroy(GameManager.Instance.Entities[i].gameObject);
                    GameManager.Instance.Entities.RemoveAt(i);
                    i--;
                }
            }
        }

        for(int i = 0; i < GameManager.Instance.Entities.Count; i++){
            if(GameManager.Instance.Entities[i].GetComponent<Singer>()){
                if(GameManager.Instance.Entities[i].GetComponent<Singer>().IsEvil){
                    GameObject.Destroy(GameManager.Instance.Entities[i].gameObject);
                    GameManager.Instance.Entities.RemoveAt(i);
                    i--;
                }
            }
        }
    }

    private IEnumerator SpecialAttack1(){
        isPerformingSpecialAttack = true;
        Vector3 startPos = center + new Vector3(Mathf.Cos(90f * Mathf.Deg2Rad) * radius, Mathf.Sin(90f * Mathf.Deg2Rad) * radius, 0);
        Vector3 moveDirection = (startPos - transform.position).normalized;
        float moveDistance = Vector3.Distance(startPos, transform.position) - 1f;
                
        while(moveDistance > 0){
            transform.position += moveDirection * fighter.MovementSpeed * 0.75f *Time.deltaTime;
            moveDistance -= fighter.MovementSpeed * Time.deltaTime;
            canTakeDamage = false;
            if (fighter.Target && !fighter.Target.IsAlive) {
                isPerformingSpecialAttack = false;
                yield break; // Exit the coroutine
            }
            yield return null;

        }
        canTakeDamage = true;

        float angle = 0f;
        float angularSpeed = 30f;
        float projectileCooldown = 0.1f;

        while(angle < 360f){
            transform.position = center + new Vector3(Mathf.Cos((angle + 90f) * Mathf.Deg2Rad) * radius, Mathf.Sin((angle + 90f) * Mathf.Deg2Rad) * radius, 0);
            angle += angularSpeed * Time.deltaTime;

            if(Time.time % projectileCooldown < Time.deltaTime){
                Vector2 direction = (fighter.Target.transform.position - transform.position).normalized;
                Attack(direction, fighter.Power);
            }

            if (fighter.Target && !fighter.Target.IsAlive) {
                isPerformingSpecialAttack = false;
                yield break; // Exit the coroutine
            }

            yield return null;

        }

        isPerformingSpecialAttack = false;
    }

    private IEnumerator SpecialAttack2(){
        transform.position = center;
        isPerformingSpecialAttack = true;

        Vector3 moveDirection = (center - transform.position).normalized;
        float moveDistance = Vector3.Distance(center, transform.position) - 1f;
        
        // move towards the center of the room
        
        while(moveDistance > 0){
            transform.position += moveDirection * fighter.MovementSpeed * 0.75f * Time.deltaTime;
            moveDistance -= fighter.MovementSpeed * Time.deltaTime;
            canTakeDamage = false;
            if (fighter.Target && !fighter.Target.IsAlive) {
                isPerformingSpecialAttack = false;
                yield break; // Exit the coroutine
            }
            yield return null;

        }
        canTakeDamage = true;

        float angle = 0f;
        float angleIncrement = 40f;
        float projectileCooldown = 0.5f;

        float elapsedTime = 0f;
        float currentAngle = 0f;
        while(elapsedTime < specialDuration){
            for(int i = 0; i < 9; i++){
                Vector2 direction = new Vector2(Mathf.Cos((currentAngle + angle) * Mathf.Deg2Rad), Mathf.Sin((currentAngle + angle) * Mathf.Deg2Rad));
                Attack(direction, fighter.Power);
                currentAngle += angleIncrement;
            }
            angle += 10f;
            elapsedTime += projectileCooldown;
            if (fighter.Target && !fighter.Target.IsAlive) {
                isPerformingSpecialAttack = false;
                yield break; // Exit the coroutine
            }
            yield return new WaitForSeconds(projectileCooldown);

        }
        isPerformingSpecialAttack = false;
    }
    private void EnrageBoss(){
        // increase attack speed and movement speed, skills and special attacks will be used more frequently
        UIManager.Instance.AddMessage("Why... WHY??? I JUST WANNA MAKE MY PEOPLE HAPPY! YOU JUST DON'T UNDERSTAND DO YOU??!!", Utilz.PURPLE);
        isEnraged = true;
        fighter.MovementSpeed *= 1.5f;
        skillCooldown *= 0.5f;
        attackCooldown *= 0.5f;
    }
}

