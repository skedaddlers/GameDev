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
        float highRareDropChance = 0.15f;
        float ultraRareDropChance = 0.10f;
        if (Random.value <= chanceToDropWeapon)
        {
            switch(HighRareDropChance){
                case < 0.25f:
                    break;
                case < 0.50f:
                    commonDropChance = 0.35f;
                    rareDropChance = 0.3f;
                    highRareDropChance = 0.2f;
                    ultraRareDropChance = 0.15f;
                    break;
                case < 0.75f:
                    commonDropChance = 0.25f;
                    rareDropChance = 0.25f;
                    highRareDropChance = 0.25f;
                    ultraRareDropChance = 0.25f;
                    break;
                default:
                    commonDropChance = 0.15f;
                    rareDropChance = 0.2f;
                    highRareDropChance = 0.3f;
                    ultraRareDropChance = 0.35f;
                    break;
                }
                float totalDropChance = commonDropChance + rareDropChance + highRareDropChance + ultraRareDropChance;
                float randomValue = Random.Range(0, totalDropChance);
                if(randomValue <= commonDropChance)
                {
                    MapManager.Instance.CreateEntity("Fleuve Cendre Ferryman", transform.position);
                    UIManager.Instance.AddMessage($"The {name} dropped a Fleuve Cendre Ferryman!", "#17EA16");
                }
                else if(randomValue <= commonDropChance + rareDropChance)
                {
                    MapManager.Instance.CreateEntity("Festering Desire", transform.position);
                    UIManager.Instance.AddMessage($"The {name} dropped a Festering Desire!", "#18C0E5");
                }
                else if(randomValue <= commonDropChance + rareDropChance + highRareDropChance)
                {
                    MapManager.Instance.CreateEntity("Primordial Jade Cutter", transform.position);
                    UIManager.Instance.AddMessage($"The {name} dropped a Primordial Jade Cutter!", "#B818E5");
                }
                else
                {
                    MapManager.Instance.CreateEntity("Splendor of Tranquil Waters", transform.position);
                    UIManager.Instance.AddMessage($"The {name} dropped a Splendor of Tranquil Waters!", "#F3B714");
                }
            // if(Random.value <= 0.50f)
            // {
            //     MapManager.Instance.CreateEntity("Fleuve Cendre Ferryman", transform.position);
            // }
            // else if(Random.value <= 0.75f)
            // {
            //     MapManager.Instance.CreateEntity("Festering Desire", transform.position);
            // }
            // else if(Random.value <= 0.90f)
            // {
            //     MapManager.Instance.CreateEntity("Primordial Jade Cutter", transform.position);
            // }
            // else
            // {
            //     MapManager.Instance.CreateEntity("Splendor of Tranquil Waters", transform.position);
            // }
        }
    }


}
