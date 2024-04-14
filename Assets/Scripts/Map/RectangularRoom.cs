using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RectangularRoom
{
    // Start is called before the first frame update
    [SerializeField] private int roomNumber;
    [SerializeField] private bool isBossRoom = false;
    [SerializeField] private bool isShopRoom = false;
    [SerializeField] private bool containsPlayer = false;
    [SerializeField] private bool isCleared = false;
    [SerializeField] private int x, y, width, height;
    [SerializeField] private List<Entity> entities = new List<Entity>();
    public int X { get => x; set => x = value; }
    public int Y { get => y; set => y = value; }
    public int Width { get => width; set => width = value; }    
    public int Height { get => height; set => height = value; }
    public int RoomNumber { get => roomNumber; set => roomNumber = value; }
    public bool IsBossRoom { get => isBossRoom; set => isBossRoom = value; }
    public bool IsShopRoom { get => isShopRoom; set => isShopRoom = value; }
    public bool ContainsPlayer { get => containsPlayer; set => containsPlayer = value; }
    public bool IsCleared { get => isCleared; set => isCleared = value; }
    public List<Entity> Entities { get => entities; set => entities = value; }
    public RectangularRoom(int x, int y, int width, int height)
    {
        this.x = x;
        this.y = y;
        this.width = width;
        this.height = height;
    }

    public void AddEntity(Entity entity)
    {
        entities.Add(entity);
    }
    public Vector2Int Center() => new Vector2Int(x + width / 2, y + height / 2);

    /// <summary>
    /// 
    /// </summary>
    
    public Bounds GetBounds() => new Bounds(new Vector3(x, y, 0), new Vector3(width, height, 0));

    /// <summary>
    /// 
    /// </summary>
    
    public BoundsInt GetBoundsInt() => new BoundsInt(new Vector3Int(x, y, 0), new Vector3Int(width, height, 0));

    public bool Overlaps(List<RectangularRoom> rooms)
    {
        foreach (RectangularRoom room in rooms)
        {
            if (GetBounds().Intersects(room.GetBounds()))
            {
                return true;
            }
        }
        return false;
    }
}
