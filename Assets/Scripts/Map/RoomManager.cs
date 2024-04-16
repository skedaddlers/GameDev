using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public static RoomManager Instance;
    

    [SerializeField] private List<RectangularRoom> rooms;
    private bool hasAssignedEntities = false;
    private bool hasEnteredBossRoom = false;
    public List<RectangularRoom> Rooms { get => rooms; }

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
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
                room.ContainsPlayer = true;
                break;
            }
            else
            {
                room.ContainsPlayer = false;
            }
        }

        ActivateRooms();      
    }


    public RectangularRoom GetRoomWithEntity(Entity entity)
    {
        foreach (RectangularRoom room in rooms)
        {
            if (room.Entities.Contains(entity))
            {
                return room;
            }
        }
        return null;
    }


    public void AssignEntitiesToRooms()
    {
        foreach (RectangularRoom room in rooms)
        {
            room.Entities.Clear();
        }

        foreach (Entity entity in GameManager.Instance.Entities)
        {
            foreach (RectangularRoom room in rooms)
            {
                float entityX = entity.transform.position.x;
                float entityY = entity.transform.position.y;
                if (entityX >= room.X && entityX < room.X + room.Width && entityY >= room.Y && entityY < room.Y + room.Height)
                {
                    room.AddEntity(entity);
                    break;
                }
            }
        }
    }

    private void ActivateRooms()
    {
        foreach(RectangularRoom room in rooms)
        {
            if (room.ContainsPlayer)
            {
                //Close off the room if it hasn't been done yet
                if(!room.IsCleared){
                    if(room.IsBossRoom && !hasEnteredBossRoom){
                        AudioManager.Instance.PlaySound("BossMusic");
                        hasEnteredBossRoom = true;
                    }
                    CloseOffRoom(room);
                    foreach (Entity entity in room.Entities)
                    {
                        if (entity.GetComponent<Actor>() && entity.GetComponent<Actor>().IsAlive)
                        {
                            if (entity.GetComponent<HostileEnemy>())
                            {
                                entity.GetComponent<HostileEnemy>().CanTakeDamage = true;
                                entity.GetComponent<HostileEnemy>().RunAI();
                            }
                        }
                    }
                    if(AllEnemiesDead(room)){
                        room.IsCleared = true;
                        if(room.IsBossRoom){
                            AudioManager.Instance.PlaySound("VictoryMusic");
                            GameManager.Instance.HasWon = true;
                            UIManager.Instance.ShowVictoryScreen();
                        }
                    }
                }
                else{
                    OpenRoom(room);
                }
            }
        }
    }

    private bool AllEnemiesDead(RectangularRoom room)
    {
        foreach(Entity entity in room.Entities)
        {
            if(entity.GetComponent<Actor>() && entity.GetComponent<Actor>().IsAlive)
            {
                if(entity.GetComponent<Actor>().GetComponent<HostileEnemy>())
                {
                    return false;
                }
            }
        }
        return true;
    }

    private void CloseOffRoom(RectangularRoom room)
    {
        for (int x = room.X; x < room.X + room.Width; x++)
        {
            for (int y = room.Y; y < room.Y + room.Height; y++)
            {
                if (x == room.X || x == room.X + room.Width - 1 || y == room.Y || y == room.Y + room.Height - 1)
                {
                    if (MapManager.Instance.ObstacleMap.GetTile(new Vector3Int(x, y, 0)) == null)
                    {
                        MapManager.Instance.ObstacleMap.SetTile(new Vector3Int(x, y, 0), MapManager.Instance.WallTile[13]);
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
                    if (MapManager.Instance.ObstacleMap.GetTile(new Vector3Int(x, y, 0)) == MapManager.Instance.WallTile[13])
                    {
                        MapManager.Instance.ObstacleMap.SetTile(new Vector3Int(x, y, 0), null);
                    }
                }
            }
        }
    }

    private bool SetWallTileIfEmpty(Vector3Int position, int tileIndex)
    {
        if(MapManager.Instance.FloorMap.GetTile(new Vector3Int(position.x, position.y, 0)))
        {
            return true;
        }
        else
        {
            MapManager.Instance.ObstacleMap.SetTile(new Vector3Int(position.x, position.y, 0), MapManager.Instance.WallTile[tileIndex]);
            return false;
        }
    }


    public void AddRoom(RectangularRoom room)
    {
        rooms.Add(room);
    }

    public void RemoveRoom(RectangularRoom room)
    {
        rooms.Remove(room);
    }

}
