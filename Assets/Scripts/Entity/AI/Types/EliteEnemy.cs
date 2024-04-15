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
    [SerializeField] private float chanceToDropWeapon;
    [SerializeField] private float highRareDropChance;
    public EliteEnemyType Type { get => type; set => type = value; }
    public float ChanceToDropWeapon { get => chanceToDropWeapon; set => chanceToDropWeapon = value; }
    public float HighRareDropChance { get => highRareDropChance; set => highRareDropChance = value; }

    public void MayDropWeapon()
    {
        float commonDropChance = 0.5f;
        float rareDropChance = 0.25f;
        float epicDropChance = 0.15f;
        float legendaryDropChance = 0.10f;
        float playersLuck = GameManager.Instance.Actors[0].GetComponent<Player>().Luck;
        float dropChance = chanceToDropWeapon * (1 + playersLuck / 10);
        if (Random.value <= dropChance)
        {
            float rarityChance = highRareDropChance * (1 + playersLuck / 10);
            switch(rarityChance){
                case < 0.2f:
                    break;
                case < 0.4f:
                    commonDropChance = 0.35f;
                    rareDropChance = 0.3f;
                    epicDropChance = 0.2f;
                    legendaryDropChance = 0.15f;
                    break;
                case < 0.6f:
                    commonDropChance = 0.25f;
                    rareDropChance = 0.25f;
                    epicDropChance = 0.25f;
                    legendaryDropChance = 0.25f;
                    break;
                case < 0.8f:
                    commonDropChance = 0.2f;
                    rareDropChance = 0.2f;
                    epicDropChance = 0.3f;
                    legendaryDropChance = 0.3f;
                    break;
                default:
                    commonDropChance = 0.1f;
                    rareDropChance = 0.15f;
                    epicDropChance = 0.35f;
                    legendaryDropChance = 0.4f;
                    break;
            }   
                float totalDropChance = commonDropChance + rareDropChance + highRareDropChance + legendaryDropChance;
                float randomValue = Random.Range(0, totalDropChance);
                if(randomValue <= commonDropChance)
                {
                    MapManager.Instance.CreateEntity("Weapon1", transform.position);
                    UIManager.Instance.AddMessage($"The {name} dropped a Fleuve Cendre Ferryman!", Utilz.GREEN);
                }
                else if(randomValue <= commonDropChance + rareDropChance)
                {
                    MapManager.Instance.CreateEntity("Weapon2", transform.position);
                    UIManager.Instance.AddMessage($"The {name} dropped a Festering Desire!", Utilz.BLUE);
                }
                else if(randomValue <= commonDropChance + rareDropChance + epicDropChance)
                {
                    MapManager.Instance.CreateEntity("Weapon3", transform.position);
                    UIManager.Instance.AddMessage($"The {name} dropped a Primordial Jade Cutter!", Utilz.PURPLE);
                }
                else
                {
                    MapManager.Instance.CreateEntity("Weapon4", transform.position);
                    UIManager.Instance.AddMessage($"The {name} dropped a Splendor of Tranquil Waters!", Utilz.ORANGE);
                    UIManager.Instance.AddMessage($"IT'S THE WEAPON OF FURINA DE FONTAINE!", Utilz.ORANGE);
                }
        }
    }


}
