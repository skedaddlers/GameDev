using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class MapManager : MonoBehaviour
{
    public static MapManager Instance;

    [Header("Map Settings")]
    [SerializeField] private int Width = 80, Height = 45;

    [SerializeField] private int roomMaxSize = 15, roomMinSize = 8, maxRooms = 30;

    [Header("Colors")]
    [SerializeField] private Color32 darkColor = new Color32(0, 0, 0, 0), lightColor = new Color32(255, 255, 255, 255);

    [Header("Tiles")]
    [SerializeField] private List<TileBase> floorTile = new List<TileBase>();
    [SerializeField] private List<TileBase> wallTile = new List<TileBase>();

    [Header("Tilemaps")]
    [SerializeField] private Tilemap floorMap, obstacleMap;

    [Header("Features")]
    [SerializeField] private List<RectangularRoom> rooms = new List<RectangularRoom>();

    public List<TileBase> FloorTile { get => floorTile; }
    public List<TileBase> WallTile { get => wallTile; }
    public Tilemap FloorMap { get => floorMap; }
    public Tilemap ObstacleMap { get => obstacleMap; }
    public List<RectangularRoom> Rooms { get => rooms; }

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
        procGen.GenerateDungeon(Width, Height, roomMaxSize, roomMinSize, maxRooms, rooms);

        Instantiate(Resources.Load<GameObject>("Player"), new Vector3(4 + 0.5f, 2 + 0.5f, 0), Quaternion.identity).name = "Player";
        Instantiate(Resources.Load<GameObject>("NPC"), new Vector3(4 - 0.5f, 2 + 0.5f, 0), Quaternion.identity).name = "NPC";

        Camera.main.transform.position = new Vector3(20, 20.25f, -10);
        Camera.main.orthographicSize = 27f;
    }

    public bool InBounds(int x, int y)
    {
        return x >= -10 && x < Width && y >= -10 && y < Height;
    }
    
    public void CreatePlayer(Vector2 position)
    {
        Instantiate(Resources.Load<GameObject>("Player"), new Vector3(position.x + 0.5f, position.y + 0.5f, 0), Quaternion.identity).name = "Player";
    }
}
