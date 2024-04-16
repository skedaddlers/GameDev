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

                        if (x == roomX)
                        {
                            if (y == roomY)
                                tileIndex = 0;
                            else if (y == roomY + roomHeight - 1)
                                tileIndex = 3;
                            else
                                tileIndex = 11;
                        }
                        else if (x == roomX + roomWidth - 1)
                        {
                            if (y == roomY)
                                tileIndex = 1;
                            else if (y == roomY + roomHeight - 1)
                                tileIndex = 2;
                            else
                                tileIndex = 9;
                        }
                        else if (y == roomY)
                        {
                            tileIndex = 8;
                        }
                        else if (y == roomY + roomHeight - 1)
                        {
                            tileIndex = 10;
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
            newRoom.RoomNumber = roomNum;
            RoomManager.Instance.AddRoom(newRoom);
        }
        // fill the rest of the map with walls
        for(int x = -10; x < mapWidth + 10; x++)
        {
            for(int y = -10; y < mapHeight + 10; y++)
            {
                if(MapManager.Instance.FloorMap.GetTile(new Vector3Int(x, y, 0)) || MapManager.Instance.ObstacleMap.GetTile(new Vector3Int(x, y, 0)))
                {
                    continue;
                }
                SetWallTileIfEmpty(new Vector3Int(x, y, 0), 12);
            }
        }

        RoomManager.Instance.Rooms[0].IsCleared = true;
        MapManager.Instance.CreateEntity("Player", RoomManager.Instance.Rooms[0].Center());
        MapManager.Instance.CreateEntity("Weapon0", RoomManager.Instance.Rooms[0].Center());
        CreateBossRoom(RoomManager.Instance.Rooms);
        CreateShopRooms(RoomManager.Instance.Rooms,  maxRooms/4);
        for(int i = 1; i < RoomManager.Instance.Rooms.Count; i++)
        {
            PlaceEntities(RoomManager.Instance.Rooms[i], minMonstersPerRoom, maxMonstersPerRoom, maxItemsPerRoom);
        }
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
                    if(SetWallTileIfEmpty(new Vector3Int(x, y, 0), 8))
                    {
                        continue;
                    }
                    // if(x == roomX || x == roomX + roomWidth - 1 || y == roomY || y == roomY + roomHeight - 1)
                    // {
                    //     int tileIndex = 0;
                    //     if (x == roomX){
                    //         if (y == roomY)
                    //             tileIndex = 3;
                    //         else if(y == roomY + roomHeight - 1)
                    //             tileIndex = 0;
                    //         else
                    //             tileIndex = 11;
                    //     }
                    //     else if(y == roomY){
                    //         tileIndex = 10;
                    //         if (x == roomX)
                    //             tileIndex = 3;
                    //         else if(x == roomX + roomWidth - 1)
                    //             tileIndex = 2;
                    //     }
                    //     else if(x == roomX + roomWidth - 1){
                    //         tileIndex = 9;
                    //         if (y == roomY)
                    //             tileIndex = 2;
                    //         else if(y == roomY + roomHeight - 1)
                    //             tileIndex = 1;
                    //     }
                    //     else if(y == roomY + roomHeight - 1){
                    //         tileIndex = 8;
                    //         if (x == roomX)
                    //             tileIndex = 0;
                    //         else if(x == roomX + roomWidth - 1)
                    //             tileIndex = 1;
                    //     }
                    //     if(SetWallTileIfEmpty(new Vector3Int(x, y, 0), tileIndex))
                    //     {
                    //         continue;
                    //     }
                    // }
                }
            }
        }
    }

    private void PlaceEntities(RectangularRoom newRoom, int minMonsters, int maxMonsters, int maxItems)
    {
        if(newRoom.IsBossRoom || newRoom.IsShopRoom )
        {
            return;
        }

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

            float random = Random.value;
            if(random < 0.2f){
                MapManager.Instance.CreateEntity("Slime", new Vector2(x, y));
            }
            else if(random < 0.4f){
                MapManager.Instance.CreateEntity("Fungi", new Vector2(x, y));
            }
            else if(random < 0.6f){
                MapManager.Instance.CreateEntity("Spectre", new Vector2(x, y));
            }
            else if(random < 0.8f){
                MapManager.Instance.CreateEntity("Treasure Hoarder", new Vector2(x, y));
            }
            else{
                MapManager.Instance.CreateEntity("Hilichurl", new Vector2(x, y));
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
        farthestRoom.IsCleared = false;
        GameObject boss = MapManager.Instance.CreateEntity("Focalors", farthestRoom.Center());
        Vector3 bossPos = new Vector3(farthestRoom.Center().x + 0.5f, farthestRoom.Center().y + 0.5f, 0);
        boss.GetComponent<BossEnemy>().Center = bossPos;
        //convert vector2int to vector3 
        
        // Debug.Log("Boss room created");
    }

    private void CreateShopRooms(List<RectangularRoom> rooms, int totalShopRooms)
    {
        bool allSkillsAvailable = false;
        //search for room nearest to the player
        RectangularRoom nearestRoom = rooms[0];
        float minDistance = Mathf.Infinity;
        foreach(RectangularRoom room in rooms)
        {
            if(room == rooms[0])
            {
                continue;
            }
            float distance = Vector2.Distance(RoomManager.Instance.Rooms[0].Center(), room.Center());
            if(distance < minDistance)
            {
                minDistance = distance;
                nearestRoom = room;
            }
        }
        nearestRoom.IsShopRoom = true;
        nearestRoom.IsCleared = true;

        GameObject firstSeller = MapManager.Instance.CreateEntity("Seller", nearestRoom.Center());
        Seller sellerComponent = firstSeller.GetComponent<Seller>();
        
        for(int i = 0; i < sellerComponent.AmountOfSkillsForSale; i++){
            int value = Random.Range(0, 6);
            string name = "Skills/Skill" + value;
            GameObject skill1 = Instantiate(Resources.Load<GameObject>(name));
            if(sellerComponent.AlreadyHasSkill(skill1.GetComponent<Skill>().SkillName))
            {
                i--;
                Destroy(skill1);
                continue;
            }
            sellerComponent.AddSkillForSale(skill1.GetComponent<Skill>());
            skill1.gameObject.SetActive(false);
            skill1.transform.SetParent(firstSeller.transform);
            if(sellerComponent.SkillsForSale.Count == sellerComponent.AmountOfSkillsForSale){
                break;
            }
        }

        for(int i = 0; i < sellerComponent.AmountOfWeaponsForSale; i++){
            int value = Random.Range(1, 5);
            string name = "Weapon" + value;
            GameObject weapon = MapManager.Instance.CreateEntity(name, nearestRoom.Center());
            if(sellerComponent.AlreadyHasWeapon(weapon.GetComponent<Weapon>().WeaponName))
            {
                i--;
                Destroy(weapon);
                continue;
            }
            sellerComponent.AddWeaponForSale(weapon.GetComponent<Weapon>());
            weapon.gameObject.SetActive(false);
            weapon.transform.SetParent(firstSeller.transform);
            if(sellerComponent.WeaponsForSale.Count == sellerComponent.AmountOfWeaponsForSale){
                break;
            }
        }

        for(int i = 1; i < totalShopRooms; i++){

            int randomIndex = Random.Range(0, rooms.Count);
            if(rooms[randomIndex].IsBossRoom || rooms[randomIndex].IsShopRoom)
            {
                i--;
                continue;
            }
            rooms[randomIndex].IsShopRoom = true;
            rooms[randomIndex].IsCleared = true;

            GameObject seller = MapManager.Instance.CreateEntity("Seller", rooms[randomIndex].Center());
            Seller sellerComponent2 = seller.GetComponent<Seller>();
            if(!allSkillsAvailable){
                for(int j = 0; j < 6; j++){
                    string name = "Skills/Skill" + j;
                    GameObject skill = Instantiate(Resources.Load<GameObject>(name));
                    if(sellerComponent.AlreadyHasSkill(skill.GetComponent<Skill>().SkillName))
                    {
                        Destroy(skill);
                        continue;
                    }
                    sellerComponent2.AddSkillForSale(skill.GetComponent<Skill>());
                    skill.gameObject.SetActive(false);
                    skill.transform.SetParent(seller.transform);

                }
                allSkillsAvailable = true;
            }
            else{
                for(int j = 0; j < sellerComponent2.AmountOfSkillsForSale; j++){
                    int value = Random.Range(0, 6);
                    string name = "Skills/Skill" + value;
                    GameObject skill = Instantiate(Resources.Load<GameObject>(name));
                    if(sellerComponent2.AlreadyHasSkill(skill.GetComponent<Skill>().SkillName))
                    {
                        j--;
                        Destroy(skill);
                        continue;
                    }
                    sellerComponent2.AddSkillForSale(skill.GetComponent<Skill>());
                    skill.gameObject.SetActive(false);
                    skill.transform.SetParent(seller.transform);
                    if(sellerComponent2.SkillsForSale.Count == sellerComponent2.AmountOfSkillsForSale){
                        break;
                    }
                }
            }

            for(int j = 0; j < sellerComponent2.AmountOfWeaponsForSale; j++){
                int value = Random.Range(1, 5);
                string name = "Weapon" + value;
                GameObject weapon = MapManager.Instance.CreateEntity(name, rooms[randomIndex].Center());
                if(sellerComponent2.AlreadyHasWeapon(weapon.GetComponent<Weapon>().WeaponName))
                {
                    j--;
                    Destroy(weapon);
                    continue;
                }
                sellerComponent2.AddWeaponForSale(weapon.GetComponent<Weapon>());
                weapon.gameObject.SetActive(false);
                weapon.transform.SetParent(seller.transform);
                if(sellerComponent2.WeaponsForSale.Count == sellerComponent2.AmountOfWeaponsForSale){
                    break;
                }
            }
        }
    }
}