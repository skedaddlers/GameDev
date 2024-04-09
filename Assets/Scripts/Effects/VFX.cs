using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFX : MonoBehaviour
{
    private Transform player; // Reference to the player's transform
    private float duration; // The duration the sprite will be displayed for
    private float timer = 0f; // The timer to keep track of how long the sprite has been displayed for    // getter
    [SerializeField] private float size;
    private float remainingDuration;
    public float Size { get => size; set => size = value; }
    public float Duration { get => duration; set => duration = value; }

    private void Start()
    {
        //enlarge it as the radius
        transform.localScale = new Vector3(size, size, 1);
        foreach(Entity entity in GameManager.Instance.Entities){
            if(entity.GetComponent<Player>()){
                player = entity.GetComponent<Player>().transform;
            }
        }
        if (player == null)
        {
            Debug.LogError("Player reference not set for SkillEffect script on " + gameObject.name);
        }
    }

    private void Update()
    {
        
        if (player != null)
        {
            // Set the position of the skill effect sprite to follow the player
            transform.position = player.position;
        }
        timer += Time.deltaTime;
        if(timer >= duration)
        {
            Destroy(gameObject);
        }
    }

    public void SetSprite(Sprite sprite)
    {
        // Set the sprite to be displayed
        GetComponent<SpriteRenderer>().sprite = sprite;
    }
}
