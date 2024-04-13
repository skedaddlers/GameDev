using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFX : MonoBehaviour
{
    private Transform player; // Reference to the player's transform
    private float duration; // The duration the sprite will be displayed for
    private float timer = 0f; // The timer to keep track of how long the sprite has been displayed for    // getter
    [SerializeField] private float size;
    //list of sprites to do animation
    [SerializeField] private List<Sprite> sprites;
    private int currentSpriteIndex = 0;
    private float spriteTimer = 0f;
    private float spriteInterval = 0.5f;
    private int maxSprteIndex;
    private float remainingDuration;
    private Camera mainCamera; // Reference to the main camera
    public float Size { get => size; set => size = value; }
    public float Duration { get => duration; set => duration = value; }

    private void Start()
    {
        //enlarge it as the radius
        mainCamera = Camera.main; // Get the main camera
        transform.localScale = new Vector3(size, size, 1);
        foreach(Entity entity in GameManager.Instance.Entities){
            if(entity.GetComponent<Player>()){
                player = entity.GetComponent<Player>().transform;
            }
        }
        mainCamera = Camera.main;
        if (player == null)
        {
            Debug.LogError("Player reference not set for SkillEffect script on " + gameObject.name);
        }
        GameManager.Instance.AddVFX(this);
    }

    private void Update()
    {
        
        if (player != null)
        {
            // Set the position of the skill effect sprite to follow the player
            transform.position = player.position;
        }
        timer += Time.deltaTime;
        spriteTimer += Time.deltaTime;
        UpdateSprite();
        if(timer >= duration)
        {
            GameManager.Instance.RemoveVFX(this);
            Destroy(gameObject);
        }
        // Scale to camera size
        // ScaleToCamera();

    }

    private void UpdateSprite()
    {
        if(spriteTimer >= spriteInterval)
        {
            currentSpriteIndex++;
            if(currentSpriteIndex >= maxSprteIndex)
            {
                currentSpriteIndex = 0;
            }
            GetComponent<SpriteRenderer>().sprite = sprites[currentSpriteIndex];
            spriteTimer = 0f;
        }
    }

    public void GetSprites(string spritename, int totalSprites)
    {
        maxSprteIndex = totalSprites;
        // sprites = new List<Sprite>();
        for(int i = 0; i < totalSprites; i++)
        {
            string path = "Sprites/" + spritename + i;
            sprites.Add(Resources.Load<Sprite>(path));
        }
        // Set the sprite to be displayed
        GetComponent<SpriteRenderer>().sprite = sprites[0];
    }
}
