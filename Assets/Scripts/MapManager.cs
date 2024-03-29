using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;


public class MapManager : MonoBehaviour
{

    public static MapManager Instance;

    [SerializeField] private int Width = 80, Height = 45;

    [SerializeField] private Color32 darkColor = new Color32(0, 0, 0, 0), lightColor = new Color32(255, 255, 255, 255);

    [SerializeField] private List<TileBase> floorTile = new List<TileBase>();
    [SerializeField] private List<TileBase> wallTile = new List<TileBase>();

    [SerializeField] private Tilemap floorMap, obstacleMap;

    public Tilemap FloorMap
    {
        get => floorMap;
    }

    public Tilemap ObstacleMap
    {
        get => obstacleMap;
    }

    private void Awake(){
        if(Instance == null)
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
        Vector3Int centerTile = new Vector3Int(Width / 2, Height / 2, 0);

        BoundsInt wallBounds = new BoundsInt(new Vector3Int(29, 28, 0), new Vector3Int(-30, -10, 0));

        // for(int i = 0; i < wallBounds.size.x; i++)
        // {
        //     for(int j = 0; j < wallBounds.size.y; j++)
        //     {
        //         Vector3Int wallPosition = new Vector3Int(wallBounds.min.x + i, wallBounds.min.y + j, 0);
        //         obstacleMap.SetTile(wallPosition, wallTile);
        //     }
        // }

        Instantiate(Resources.Load<GameObject>("Player"), new Vector3(4 + 0.5f, 2 + 0.5f, 0), Quaternion.identity).name = "Player";
        Instantiate(Resources.Load<GameObject>("NPC"), new Vector3(4 - 0.5f, 2 + 0.5f, 0), Quaternion.identity).name = "NPC";

        Camera.main.transform.position = new Vector3(4, 20.25f, -10);
        Camera.main.orthographicSize = 27f;
    
    }

    public bool inBounds(int x, int y)
    {
        return x >= -10 && x < Width && y >= -10 && y < Height;
    }

}
