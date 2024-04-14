using System.Collections;
using System.Collections.Generic;
using UnityEngine;

sealed class ProcGen : MonoBehaviour
{
    public void GenerateDungeon(int mapWidth, int mapHeight, int maxRoomSize, int minRoomSize, int maxRooms, 
    int minMonstersPerRoom, int maxMonstersPerRoom, int maxItemsPerRoom)
    {
        for(int roomNum = 0; roomNum < maxRooms; roomNum++)
        {
            int roomWidth = Random.Range(minRoomSize, maxRoomSize);
            int roomHeight = Random.Range(minRoomSize, maxRoomSize);
            int roomX = Random.Range(0, mapWidth - roomWidth - 1);
            int roomY = Random.Range(0, mapHeight - roomHeight - 1);

            RectangularRoom newRoom = new RectangularRoom(roomX, roomY, roomWidth, roomHeight);
            

            if(newRoom.Overlaps(RoomManager.Instance.Rooms))
            {
                roomNum--;
                continue;
            }

            for(int x = roomX; x < roomX + roomWidth; x++)
            {
                for(int y = roomY; y < roomY + roomHeight; y++)
                {
                    if(x == roomX || x == roomX + roomWidth - 1 || y == roomY || y == roomY + roomHeight - 1)
                    {
                        int tileIndex = 0;
                        if (x == roomX){
                            if (y == roomY)
                                tileIndex = 3;
                            else if(y == roomY + roomHeight - 1)
                                tileIndex = 0;
                            else
                                tileIndex = 11;
                        }
                        else if(y == roomY){
                            tileIndex = 10;
                            if (x == roomX)
                                tileIndex = 3;
                            else if(x == roomX + roomWidth - 1)
                                tileIndex = 2;
                        }
                        else if(x == roomX + roomWidth - 1){
                            tileIndex = 9;
                            if (y == roomY)
                                tileIndex = 2;
                            else if(y == roomY + roomHeight - 1)
                                tileIndex = 1;
                        }
                        else if(y == roomY + roomHeight - 1){
                            tileIndex = 8;
                            if (x == roomX)
                                tileIndex = 0;
                            else if(x == roomX + roomWidth - 1)
                                tileIndex = 1;
                        }
                        if(SetWallTileIfEmpty(new Vector3Int(x, y, 0), tileIndex))
                        {
                            continue;
                        }
                    }
                    else
                    {
                        if(MapManager.Instance.ObstacleMap.GetTile(new Vector3Int(x, y, 0)))
                        {
                            MapManager.Instance.ObstacleMap.SetTile(new Vector3Int(x, y, 0), null);
                        }
                        MapManager.Instance.FloorMap.SetTile(new Vector3Int(x, y, 0), MapManager.Instance.FloorTile[Random.Range(0, MapManager.Instance.FloorTile.Count)]);
                    }
                }
            }
            if(RoomManager.Instance.Rooms.Count != 0){
                TunnelBetween(RoomManager.Instance.Rooms[RoomManager.Instance.Rooms.Count - 1], newRoom);
            }
            else{

            }
            PlaceEntities(newRoom, minMonstersPerRoom, maxMonstersPerRoom, maxItemsPerRoom);
            
            newRoom.RoomNumber = roomNum;
            RoomManager.Instance.AddRoom(newRoom);
            
        }
        RoomManager.Instance.Rooms[0].IsCleared = true;
        MapManager.Instance.CreateEntity("Player", RoomManager.Instance.Rooms[0].Center());
        MapManager.Instance.CreateEntity("Dull Blade", RoomManager.Instance.Rooms[0].Center());
        CreateBossRoom(RoomManager.Instance.Rooms);
        RoomManager.Instance.AssignEntitiesToRooms();
    }

    private void TunnelBetween(RectangularRoom oldRoom, RectangularRoom newRoom)
    {
        Vector2Int oldRoomCenter = oldRoom.Center();
        Vector2Int newRoomCenter = newRoom.Center();
        Vector2Int tunnelCenter;

        if(Random.value < 0.5f){
            tunnelCenter = new Vector2Int(oldRoomCenter.x, newRoomCenter.y);
        }
        else{
            tunnelCenter = new Vector2Int(newRoomCenter.x, oldRoomCenter.y);
        }
        
        List<Vector2Int> tunnelCoords = new List<Vector2Int>();
        BressenhamLine(oldRoomCenter, tunnelCenter, tunnelCoords);
        BressenhamLine(tunnelCenter, newRoomCenter, tunnelCoords);

        for (int i = 0; i < tunnelCoords.Count; i++)
        {
            if (MapManager.Instance.ObstacleMap.HasTile(new Vector3Int(tunnelCoords[i].x, tunnelCoords[i].y, 0)))
            {
                MapManager.Instance.ObstacleMap.SetTile(new Vector3Int(tunnelCoords[i].x, tunnelCoords[i].y, 0), null);
            }

            MapManager.Instance.FloorMap.SetTile(new Vector3Int(tunnelCoords[i].x, tunnelCoords[i].y, 0), MapManager.Instance.FloorTile[Random.Range(0, MapManager.Instance.FloorTile.Count)]);

            foreach (var tunnelCoord in tunnelCoords)
            {
                for (int x = tunnelCoord.x - 1; x <= tunnelCoord.x + 1; x++)
                {
                    for (int y = tunnelCoord.y - 1; y <= tunnelCoord.y + 1; y++)
                    {
                        if (MapManager.Instance.ObstacleMap.HasTile(new Vector3Int(x, y, 0)))
                        {
                            MapManager.Instance.ObstacleMap.SetTile(new Vector3Int(x, y, 0), null);
                        }

                        MapManager.Instance.FloorMap.SetTile(new Vector3Int(x, y, 0), MapManager.Instance.FloorTile[Random.Range(0, MapManager.Instance.FloorTile.Count)]);
                    }
                }
            }

            for (int x = tunnelCoords[i].x - 2; x <= tunnelCoords[i].x + 2; x++) {
                for (int y = tunnelCoords[i].y - 2; y <= tunnelCoords[i].y + 2; y++) {
                    if (SetWallTileIfEmpty(new Vector3Int(x, y, 0), 8)) {
                        continue;
                    }
                }
            }
        }
    }

    private void PlaceEntities(RectangularRoom newRoom, int minMonsters, int maxMonsters, int maxItems)
    {
        //Check if the room is the first room
        if(RoomManager.Instance.Rooms.Count == 0)
        {
            return;
        }
        // if(newRoom == MapManager.Instance.Rooms[0])
        // {
        //     return;
        // }
        int numMonsters = Random.Range(minMonsters, maxMonsters + 1);
        int numItems = Random.Range(0, maxItems + 1);

        for(int monster = 0; monster < numMonsters;)
        {
            int x = Random.Range(newRoom.X + 1, newRoom.X + newRoom.Width - 1);
            int y = Random.Range(newRoom.Y + 1, newRoom.Y + newRoom.Height - 1);

            if(x == newRoom.X || x == newRoom.X + newRoom.Width - 1 || y == newRoom.Y || y == newRoom.Y + newRoom.Height - 1)
            {
                continue;
            }

            for(int entity = 0; entity < GameManager.Instance.Entities.Count; entity++)
            {
                Vector3Int pos = MapManager.Instance.FloorMap.WorldToCell(GameManager.Instance.Entities[entity].transform.position);
                if(x == pos.x && y == pos.y)
                {
                    continue;
                }
            }

            if(Random.value < 0.5f){
                MapManager.Instance.CreateEntity("Skeleton", new Vector2(x, y));
            }
            else{
                MapManager.Instance.CreateEntity("Zombie", new Vector2(x, y));
            }
            monster++;
        }

        //Chance for elite monster, the farthest room from the player, the higher the chance
        float distance = Vector2.Distance(RoomManager.Instance.Rooms[0].Center(), newRoom.Center());
        float mapDiagonal = Mathf.Sqrt(Mathf.Pow(MapManager.Instance.Width, 2) + Mathf.Pow(MapManager.Instance.Height, 2));
        float distanceChance = distance / mapDiagonal;
        if(Random.value < 0.5f + (distanceChance / 2)){
            while(true){
                int x = Random.Range(newRoom.X + 1, newRoom.X + newRoom.Width - 1);
                int y = Random.Range(newRoom.Y + 1, newRoom.Y + newRoom.Height - 1);
                // check for empty tiles
                for(int entity = 0; entity < GameManager.Instance.Entities.Count; entity++)
                {
                    Vector3Int pos = MapManager.Instance.FloorMap.WorldToCell(GameManager.Instance.Entities[entity].transform.position);
                    if(x == pos.x && y == pos.y)
                    {
                        continue;
                    }
                }
                float random = Random.value;
                if(random < 0.2f){
                    GameObject mitachurl = MapManager.Instance.CreateEntity("Mitachurl", new Vector2(x, y));
                    mitachurl.GetComponent<EliteEnemy>().Type = EliteEnemyType.Mitachurl;
                    // assign chance to drop weapon to be higher the farther the room is from the player
                    mitachurl.GetComponent<EliteEnemy>().HighRareDropChance = (distanceChance);
                }
                else if(random < 0.4f){
                    GameObject abyssMage = MapManager.Instance.CreateEntity("Abyss Mage", new Vector2(x, y));
                    abyssMage.GetComponent<EliteEnemy>().Type = EliteEnemyType.AbyssMage;
                    abyssMage.GetComponent<EliteEnemy>().HighRareDropChance = (distanceChance);
                }
                else if(random < 0.6f){
                    GameObject ruinGuard = MapManager.Instance.CreateEntity("Ruin Guard", new Vector2(x, y));
                    ruinGuard.GetComponent<EliteEnemy>().Type = EliteEnemyType.RuinGuard;
                    ruinGuard.GetComponent<EliteEnemy>().HighRareDropChance = (distanceChance);
                }
                else if(random < 0.8f){
                    GameObject ruinHunter = MapManager.Instance.CreateEntity("Rifthound", new Vector2(x, y));
                    ruinHunter.GetComponent<EliteEnemy>().Type = EliteEnemyType.Rifthound;
                    ruinHunter.GetComponent<EliteEnemy>().HighRareDropChance = (distanceChance);
                }
                else{
                    GameObject ruinGrader = MapManager.Instance.CreateEntity("Mirror Maiden", new Vector2(x, y));
                    ruinGrader.GetComponent<EliteEnemy>().Type = EliteEnemyType.MirrorMaiden;
                    ruinGrader.GetComponent<EliteEnemy>().HighRareDropChance = (distanceChance);
                }
                break;
            }      
        }


        for(int item = 0; item < numItems;)
        {
            int x = Random.Range(newRoom.X + 1, newRoom.X + newRoom.Width - 1);
            int y = Random.Range(newRoom.Y + 1, newRoom.Y + newRoom.Height - 1);

            if(x == newRoom.X || x == newRoom.X + newRoom.Width - 1 || y == newRoom.Y || y == newRoom.Y + newRoom.Height - 1)
            {
                continue;
            }

            for(int entity = 0; entity < GameManager.Instance.Entities.Count; entity++)
            {
                Vector3Int pos = MapManager.Instance.FloorMap.WorldToCell(GameManager.Instance.Entities[entity].transform.position);
                if(x == pos.x && y == pos.y)
                {
                    continue;
                }
            }

            MapManager.Instance.CreateEntity("HpPotion", new Vector2(x, y));
            item++;
        }
    }
    

    private void BressenhamLine(Vector2Int roomCenter, Vector2Int tunnelCorner, List<Vector2Int> tunnelCoords)
    {
        int x = roomCenter.x;
        int y = roomCenter.y;
        int dx = Mathf.Abs(tunnelCorner.x - roomCenter.x), dy = Mathf.Abs(tunnelCorner.y - roomCenter.y);
        int sx = roomCenter.x < tunnelCorner.x ? 1 : -1, sy = roomCenter.y < tunnelCorner.y ? 1 : -1;
        int err = dx - dy;
        while(true)
        {
            tunnelCoords.Add(new Vector2Int(x, y));
            if(x == tunnelCorner.x && y == tunnelCorner.y)
            {
                break;
            }
            int e2 = 2 * err;
            if(e2 > -dy)
            {
                err -= dy;
                x += sx;
            }
            if(e2 < dx)
            {
                err += dx;
                y += sy;
            }
        }
    }

    private bool SetFloorTileIfEmpty(Vector3Int position)
    {
        if (MapManager.Instance.FloorMap.GetTile(position))
        {
            return true;
        }
        else
        {
            MapManager.Instance.FloorMap.SetTile(position, MapManager.Instance.FloorTile[Random.Range(0, MapManager.Instance.FloorTile.Count)]);
            return false;
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

    private void CreateBossRoom(List<RectangularRoom> rooms)
    {
        RectangularRoom firstRoom = rooms[0];
        RectangularRoom farthestRoom = rooms[0];
        float maxDistance = 0;
        foreach(RectangularRoom room in rooms)
        {
            float distance = Vector2.Distance(firstRoom.Center(), room.Center());
            if(distance > maxDistance)
            {
                maxDistance = distance;
                farthestRoom = room;
            }
        }
        farthestRoom.IsBossRoom = true;
        MapManager.Instance.CreateEntity("Boss", farthestRoom.Center());
        Debug.Log("Boss room created");
    }
}
