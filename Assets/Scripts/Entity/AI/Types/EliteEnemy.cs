using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EliteEnemyType
{
    Mitachurl,
    AbyssMage,
    RuinGuard,
    Rifthound,
    MirrorMaiden,
}
public class EliteEnemy : HostileEnemy
{
    // Start is called before the first frame update
    [SerializeField] private EliteEnemyType type;
    public EliteEnemyType Type { get => type; set => type = value; }
    public override void RunAI(){
        switch(type){
            case EliteEnemyType.Mitachurl:
                RunMitachurlAI();
                break;
            case EliteEnemyType.AbyssMage:
                RunAbyssMageAI();
                break;
            case EliteEnemyType.RuinGuard:
                RunRuinGuardAI();
                break;
            case EliteEnemyType.Rifthound:
                RunRifthoundAI();
                break;
            case EliteEnemyType.MirrorMaiden:  
                RunMirrorMaidenAI();
                break;
        }
    }

    private void RunMitachurlAI(){
        // just basic, tanky and strong
        base.RunAI();
    }

    private void RunAbyssMageAI(){
        // ranged attack every 3 seconds, teleport every 10 seconds, has shield, if
        // shield is broken, will teleport away and stunned for 6 seconds, after
        // that, regen shield
    }

    private void RunRuinGuardAI(){
        // generally tanky, has aoe ranged attack, 
    }

    private void RunRifthoundAI(){
        // Rifthound AI
    }

    private void RunMirrorMaidenAI(){
        // Mirror Maiden AI
    }
    
}
