using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RectangularRoom : MonoBehaviour
{
    // Start is called before the first frame update
    private int x, y, width, height;
    public int X { get => x; set => x = value; }
    public int Y { get => y; set => y = value; }
    public int Width { get => width; set => width = value; }    
    public int Height { get => height; set => height = value; }
    public RectangularRoom(int x, int y, int width, int height)
    {
        this.x = x;
        this.y = y;
        this.width = width;
        this.height = height;
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
