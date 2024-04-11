using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance;
    [SerializeField] private bool hasAssignedEntities = false;

    [Header("Map Settings")]
    [SerializeField] private int width = 80;
    [SerializeField] private int height = 45;
    [SerializeField] private int roomMaxSize = 25, roomMinSize = 8, maxRooms = 30;
    [SerializeField] private int minMonsterPerRoom = 2;
    [SerializeField] private int maxMonsterPerRoom = 6;
    [SerializeField] private int maxItemsPerRoom = 2;

    [Header("Colors")]
    [SerializeField] private Color32 darkColor = new Color32(0, 0, 0, 0), lightColor = new Color32(255, 255, 255, 255);

    [Header("Tiles")]
    [SerializeField] private List<TileBase> floorTile = new List<TileBase>();
    [SerializeField] private List<TileBase> wallTile = new List<TileBase>();

    [Header("Tilemaps")]
    [SerializeField] private Tilemap floorMap, obstacleMap;
    // [SerializeField] private Tilemap fogMap;

    [Header("Features")]
    [SerializeField] private List<RectangularRoom> rooms = new List<RectangularRoom>();
    [SerializeField] private List<Vector3Int> visibleTiles = new List<Vector3Int>();
    private Dictionary<Vector3Int, TileData> tiles = new Dictionary<Vector3Int, TileData>();
    private Dictionary<Vector2Int, Node> nodes = new Dictionary<Vector2Int, Node>();
    
    private int Width { get => width; }
    private int Height { get => height; }
    public List<TileBase> FloorTile { get => floorTile; }
    public List<TileBase> WallTile { get => wallTile; }
    public Tilemap FloorMap { get => floorMap; }
    public Tilemap ObstacleMap { get => obstacleMap; }
    public List<RectangularRoom> Rooms { get => rooms; }
    public Dictionary<Vector2Int, Node> Nodes { get => nodes; set => nodes = value;}
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        ProcGen procGen = new ProcGen();
        procGen.GenerateDungeon(Width, Height, roomMaxSize, roomMinSize, maxRooms, minMonsterPerRoom, maxMonsterPerRoom, maxItemsPerRoom, rooms);

        AddTileMapToDictionary(floorMap);
        AddTileMapToDictionary(obstacleMap);
        
    }

    void Update(){
        // iterate every room to check if player is in it
        if(!hasAssignedEntities){
            AssignEntitiesToRooms();
            hasAssignedEntities = true;
        }
        Actor player = GameManager.Instance.Actors[0];
        foreach (RectangularRoom room in rooms)
        {
            float playerX = player.transform.position.x;
            float playerY = player.transform.position.y;
            if(playerX - 1f >= room.X  && playerX + 1f < room.X + room.Width && playerY - 1f >= room.Y && playerY + 1f < room.Y + room.Height)
            {
                // Debug.Log("Player is in room " + room.RoomNumber);
                room.ContainsPlayer = true;
                break;
            }
            else
            {
                room.ContainsPlayer = false;
            }
        }
        // Run AI on enemies in the current room
        foreach(RectangularRoom room in rooms)
        {
            if (room.ContainsPlayer)
            {
                //Close off the room if it hasn't been done yet
                if(!room.IsCleared){
                    CloseOffRoom(room);
                    foreach (Entity entity in room.Entities)
                    {
                        if (entity.GetComponent<Actor>())
                        {
                            if (entity.GetComponent<Actor>().IsAlive)
                            {
                                if (entity.GetComponent<Actor>().GetComponent<HostileEnemy>())
                                {
                                    entity.GetComponent<Actor>().GetComponent<HostileEnemy>().RunAI();
                                }
                            }
                            else
                            {
                                room.Entities.Remove(entity);
                            }
                        }
                        else{
                            room.Entities.Remove(entity);
                        }
                    }
                    if(room.Entities.Count == 0){
                        room.IsCleared = true;
                    }
                }
                else{
                    OpenRoom(room);
                }
            }
        }
        
        
    }

    public bool InBounds(int x, int y)
    {
        return x >= -10 && x < Width && y >= -10 && y < Height;
    }

    public void CreateEntity(string entity, Vector2 position)
    {
        string path = "Entities/" + entity;
        switch (entity)
        {
            case "Player":
                GameObject player = Instantiate(Resources.Load<GameObject>(path), new Vector3(position.x + 0.5f, position.y + 0.5f, 0), Quaternion.identity);
                player.name = "Player";
                Camera.main.transform.position = new Vector3(position.x + 0.5f, position.y + 0.5f, -10);
                Camera.main.orthographicSize = 6f;
                break;
            case "Skeleton":
                Instantiate(Resources.Load<GameObject>(path), new Vector3(position.x + 0.5f, position.y + 0.5f, 0), Quaternion.identity).name = "Skeleton";
                break;
            case "Zombie":
                Instantiate(Resources.Load<GameObject>(path), new Vector3(position.x + 0.5f, position.y + 0.5f, 0), Quaternion.identity).name = "Zombie";
                break;
            case "HpPotion":
                Instantiate(Resources.Load<GameObject>(path), new Vector3(position.x + 0.5f, position.y + 0.5f, 0), Quaternion.identity).name = "HpPotion";
                break;
            case "Gentilhomme Usher":
                GameObject octo = Instantiate(Resources.Load<GameObject>(path), position, Quaternion.identity);
                octo.name = "Gentilhomme Usher";
                octo.GetComponent<SalonMember>().Damage = 4;
                octo.GetComponent<SalonMember>().AttackCooldown = 2;
                break;
            case "Surintendante Chevalmarin":
                GameObject seahorse = Instantiate(Resources.Load<GameObject>(path), position, Quaternion.identity);
                seahorse.name = "Surintendante Chevalmarin";
                seahorse.GetComponent<SalonMember>().Damage = 3;
                seahorse.GetComponent<SalonMember>().AttackCooldown = 1.5f;
                break;
            case "Mademoiselle Crabaletta":
                GameObject crab = Instantiate(Resources.Load<GameObject>(path), position, Quaternion.identity);
                crab.name = "Mademoiselle Crabaletta"; 
                crab.GetComponent<SalonMember>().Damage = 6;
                crab.GetComponent<SalonMember>().AttackCooldown = 3f;
                break;
            case "Singer":
                GameObject singer = Instantiate(Resources.Load<GameObject>(path), position, Quaternion.identity);
                singer.name = "Singer";
                break;
            case "Boss":
                GameObject boss = Instantiate(Resources.Load<GameObject>(path), new Vector3(position.x + 0.5f, position.y + 0.5f, 0), Quaternion.identity);
                boss.name = "Boss";
                break;
            default:
                break;
        }
    }
    public void AssignEntitiesToRooms()
    {
        Debug.Log("Assigning entities to rooms");
        foreach (RectangularRoom room in rooms)
        {
            // Debug.Log("Room " + room.RoomNumber);
            foreach(Entity entity in GameManager.Instance.Entities)
            {
                // Debug.Log(entity.name);
                float entityX = entity.transform.position.x;
                float entityY = entity.transform.position.y;
                if(entityX >= room.X && entityX < room.X + room.Width && entityY >= room.Y && entityY < room.Y + room.Height)
                {
                    room.AddEntity(entity);
                }
            }
            Debug.Log("Entities in room " + room.RoomNumber + ": " + room.Entities.Count);
        }
    }

    private void CloseOffRoom(RectangularRoom room)
    {
        for (int x = room.X; x < room.X + room.Width; x++)
        {
            for (int y = room.Y; y < room.Y + room.Height; y++)
            {
                if (x == room.X || x == room.X + room.Width - 1 || y == room.Y || y == room.Y + room.Height - 1)
                {
                    if (obstacleMap.GetTile(new Vector3Int(x, y, 0)) == null)
                    {
                        // set wall tile number 13
                        obstacleMap.SetTile(new Vector3Int(x, y, 0), wallTile[13]);
                    }
                }
            }
        }
    }

    private void OpenRoom(RectangularRoom room)
    {
        for (int x = room.X; x < room.X + room.Width; x++)
        {
            for (int y = room.Y; y < room.Y + room.Height; y++)
            {
                if (x == room.X || x == room.X + room.Width - 1 || y == room.Y || y == room.Y + room.Height - 1)
                {
                    // check if the tile is tile number 13
                    if (obstacleMap.GetTile(new Vector3Int(x, y, 0)) == wallTile[13])
                    {
                        obstacleMap.SetTile(new Vector3Int(x, y, 0), null);
                    }
                }
            }
        }
        room.IsCleared = true;
    }

    public void CreateProjectile(Vector2 position, Vector2 direction, int damage)
    {
        GameObject projectile = Instantiate(Resources.Load<GameObject>("Entities/Projectile"), new Vector3(position.x, position.y, 0), Quaternion.identity);
        projectile.GetComponent<Projectile>().Direction = direction;
        projectile.GetComponent<Projectile>().Damage = damage;
        projectile.name = "Projectile";
    }

    public void UpdateFogMap(List<Vector3Int> playerFOV) {
        // foreach (Vector3Int pos in visibleTiles) {
            // if (!tiles[pos].IsExplored) {
            //     tiles[pos].IsExplored = true;
            // }

            // tiles[pos].IsVisible = false;
            // fogMap.SetColor(pos, new Color(1.0f, 1.0f, 1.0f, 0.5f));
        // }

        visibleTiles.Clear();

        foreach (Vector3Int pos in playerFOV) {
            // tiles[pos].IsVisible = true;
            // fogMap.SetColor(pos, Color.clear);
            visibleTiles.Add(pos);
        }
    }
    public void SetEntitiesVisibilities() {
        foreach (Entity entity in GameManager.Instance.Entities) {
            if (entity.GetComponent<Player>()) {
                continue;
            }

            Vector3Int entityPosition = floorMap.WorldToCell(entity.transform.position);

            if (visibleTiles.Contains(entityPosition)) {
                entity.GetComponent<SpriteRenderer>().enabled = true;
            } else {
                entity.GetComponent<SpriteRenderer>().enabled = false;
            }
        }
  }

    private void AddTileMapToDictionary(Tilemap tilemap) {
        foreach (Vector3Int pos in tilemap.cellBounds.allPositionsWithin) {
            if (!tilemap.HasTile(pos)) {
                continue;
            }

            TileData tile = new TileData();
            tiles.Add(pos, tile);
        }
    }

    public void GenerateEffect(string Effectname, Actor player, float duration, float radius, int totalSprites){
        GameObject effect = Instantiate(Resources.Load<GameObject>("Effect"), new Vector3(player.transform.position.x, player.transform.position.y, 0), Quaternion.identity);
        effect.name = Effectname;
        effect.GetComponent<VFX>().GetSprites(Effectname, totalSprites);
        effect.GetComponent<VFX>().Duration = duration;
        effect.GetComponent<VFX>().Size = radius;

    }
    

    

    // private void SetupFogMap() {
    //     foreach (Vector3Int pos in tiles.Keys) {
    //         fogMap.SetTile(pos, fogTile);
    //         fogMap.SetTileFlags(pos, TileFlags.None);
    //     }
    // }
}
