using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RoomGenerator : MonoBehaviour
{
    [SerializeField] public int mapSize;
    [SerializeField] public GameObject gridMapPrefab;

    public Room[,] rooms;
    public int[] startingRoomPos = new int[2];
    public int[] exitRoomPos = new int[2];
    public int[] secretRoomPos = new int[2];
    public List<int[]> medallionPieceRoomPos = new List<int[]>()
    {
        new int[2],
        new int[2],
        new int[2]
    };
    public List<int[]> rabbitMonsterRoomPos = new List<int[]>()
    {
        new int[2],
        new int[2]
    };
    List<Room> routeToExit = new List<Room>();
    List<Room> routeToSecret = new List<Room>();
    List<Room> unusedRooms = new List<Room>();
    List<Room> backTraceRoom = new List<Room>();

    public GameObject player;
    public GameObject instructionMenu;

    void Start()
    {
        CreateRooms();
    }

    private void CreateRooms()
    {
        rooms = new Room[mapSize, mapSize];

        for (int i = 0; i < mapSize; i++)
        {
            for (int j = 0; j < mapSize; j++)
            {
                rooms[i, j] = new Room(new Vector2(i, j), RoomType.Normal);
            }
        }

        SetStartingRoom();
        SetExitRoom();
        SetSecretRoom();
        SetInitialRouteToEscape();
        InstantiateGridMap();

    }

    private void SetStartingRoom()
    {
        startingRoomPos[0] = Random.Range(0, mapSize);
        startingRoomPos[1] = Random.Range(0, mapSize);
        rooms[startingRoomPos[0], startingRoomPos[1]].Type = RoomType.Start;
    }

    private void SetExitRoom()
    {
        bool same;
        do
        {
            same = false;
            exitRoomPos[0] = Random.Range(0, mapSize);
            exitRoomPos[1] = Random.Range(0, mapSize);
            if (exitRoomPos[0] == startingRoomPos[0] && exitRoomPos[1] == startingRoomPos[1])
            {
                same = true;
            }
            if (exitRoomPos[0] == secretRoomPos[0] && exitRoomPos[1] == secretRoomPos[1])
            {
                same = true;
            }

        } while (same);
        rooms[exitRoomPos[0], exitRoomPos[1]].Type = RoomType.Exit;
    }

    private void SetSecretRoom()
    {
        bool same;
        do
        {
            same = false;
            secretRoomPos[0] = Random.Range(0, mapSize);
            secretRoomPos[1] = Random.Range(0, mapSize);
            if (secretRoomPos[0] == startingRoomPos[0] && secretRoomPos[1] == startingRoomPos[1])
            {
                same = true;
            }
            if (secretRoomPos[0] == exitRoomPos[0] && secretRoomPos[1] == exitRoomPos[1])
            {
                same = true;
            }

        } while (same);
        rooms[secretRoomPos[0], secretRoomPos[1]].Type = RoomType.Secret;
    }

    public void SetInitialRouteToEscape()
    {
        float minimunRouteToEscape = mapSize * mapSize * 0.6f;
        int max = 0;
        int backCount = 0;
        Room currentRoom;
        Room previousRoom;
        List<Room> neighboorRooms;
        List<Room> exitNeighboorRooms;

        bool reachExit = false;
        bool reachSecret = false;
        bool inBacktrack = false;

        Room startRoom = rooms[startingRoomPos[0], startingRoomPos[1]];
        Room exitRoom = rooms[exitRoomPos[0], exitRoomPos[1]];
        Room secretRoom = rooms[secretRoomPos[0], secretRoomPos[1]];


        for (int i = 0; i < mapSize; i++)
        {
            for (int j = 0; j < mapSize; j++)
            {
                unusedRooms.Add(rooms[i, j]);
            }
        }

        unusedRooms.Remove(startRoom);

        routeToExit.Add(startRoom);
        currentRoom = startRoom;

        while (unusedRooms.Count > 0 && max < (mapSize * mapSize * mapSize * mapSize))
        {
            max++;
            if (inBacktrack)
            {
                if (routeToExit.Count != 0)
                {
                    backCount++;
                    currentRoom = routeToExit[routeToExit.Count - backCount];
                }
            }

            neighboorRooms = GetNeighbourRooms(currentRoom);

            if (neighboorRooms.Count != 0)
            {
                inBacktrack = false;
                if (backTraceRoom.Count != 0)
                {
                    backCount = 0;
                }

                int index = Random.Range(0, neighboorRooms.Count);
                previousRoom = currentRoom;
                currentRoom = neighboorRooms[index];

                if (currentRoom != exitRoom)
                {
                    SetRoomsConnection(previousRoom, currentRoom);
                    routeToExit.Add(currentRoom);
                }
                else
                {
                    exitNeighboorRooms = GetNeighbourRooms(exitRoom);
                    if (routeToExit.Count + 1 > minimunRouteToEscape || !unusedRooms.Any(x => exitNeighboorRooms.Any(y => y == x)))
                    {
                        if (!reachExit)
                        {
                            SetRoomsConnection(previousRoom, currentRoom);
                            routeToExit.Add(currentRoom);
                            reachExit = true;
                        }
                    }
                    else
                    {
                        inBacktrack = true;
                    }
                }

                if (!inBacktrack)
                {
                    if (currentRoom != secretRoom && !reachSecret)
                    {
                        routeToSecret.Add(currentRoom);
                    }
                    else
                    {
                        reachSecret = true;
                    }
                    unusedRooms.Remove(currentRoom);
                }
            }
            else
            {
                inBacktrack = true;
            }
        }
    }

    public List<Room> GetConnectedRooms(Room room)
    {
        List<Room> connectedRooms = new List<Room>();

        if (room.DoorTop)
        {
            connectedRooms.Add(rooms[(int)room.GridPos.x, (int)room.GridPos.y+1]);
        }
        if (room.DoorBot)
        {
            connectedRooms.Add(rooms[(int)room.GridPos.x, (int)room.GridPos.y-1]);
        }
        if (room.DoorLeft)
        {
            connectedRooms.Add(rooms[(int)room.GridPos.x-1, (int)room.GridPos.y]);
        }
        if (room.DoorRight)
        {
            connectedRooms.Add(rooms[(int)room.GridPos.x+1, (int)room.GridPos.y]);
        }

        return connectedRooms;
    }

    public List<Room> GetNeighbourRooms(Room room)
    {
        List<Room> neighbourRooms = new List<Room>();
        int x = (int)room.GridPos.x;
        int y = (int)room.GridPos.y;

        if (y < (int)(mapSize - 1))
        {
            if (!routeToExit.Contains(rooms[x, y + 1]) && !backTraceRoom.Contains(rooms[x, y + 1]))
            {
                neighbourRooms.Add(rooms[x, y + 1]);
            }
        }
        if (y > 0)
        {
            if (!routeToExit.Contains(rooms[x, y - 1]) && !backTraceRoom.Contains(rooms[x, y - 1]))
            {
                neighbourRooms.Add(rooms[x, y - 1]);
            }
        }
        if (x > 0)
        {
            if (!routeToExit.Contains(rooms[x - 1, y]) && !backTraceRoom.Contains(rooms[x - 1, y]))
            {
                neighbourRooms.Add(rooms[x - 1, y]);
            }
        }
        if (x < (mapSize - 1))
        {
            if (!routeToExit.Contains(rooms[x + 1, y]) && !backTraceRoom.Contains(rooms[x + 1, y]))
            {
                neighbourRooms.Add(rooms[x + 1, y]);
            }
        }

            return neighbourRooms;
    }

    public void SetRoomsConnection(Room initialRoom, Room endingRoom)
    {
        int initialX = (int)initialRoom.GridPos.x;
        int initialY = (int)initialRoom.GridPos.y;
        int endingX = (int)endingRoom.GridPos.x;
        int endingY = (int)endingRoom.GridPos.y;

        if (initialY < endingY)
        {
            initialRoom.DoorTop = true;
            endingRoom.DoorBot = true;
            return;
        }
        if (initialY > endingY)
        {
            initialRoom.DoorBot = true;
            endingRoom.DoorTop = true;
            return;
        }
        if (initialX > endingX)
        {
            initialRoom.DoorLeft = true;
            endingRoom.DoorRight = true;
            return;
        }
        if (initialX < endingX)
        {
            initialRoom.DoorRight = true;
            endingRoom.DoorLeft = true;
            return;
        }
        return;
    }

    public void InstantiateGridMap()
    {
        SetMedallionPieces();
        SetRabbitMonsterRoomPos();
        int medallionSpawnCount = 0;
        for (int i = 0; i < mapSize; i++)
        {
            for (int j = 0; j < mapSize; j++)
            {
                GameObject gameObject = Instantiate(gridMapPrefab, new Vector2(i * 20.40f, j * 16.60f), Quaternion.identity);
                
                if (rooms[i, j].Type == RoomType.Start)
                {
                    gameObject.GetComponent<GridMapRenderer>().GenerateStartRoomsObejct();
                    player.transform.position = gameObject.transform.position;
                }
                else if (rooms[i, j].Type == RoomType.Exit)
                {
                    gameObject.GetComponent<GridMapRenderer>().GenerateEscapeRoom();
                }
                else if (rooms[i, j].Type == RoomType.Secret)
                {
                    gameObject.GetComponent<GridMapRenderer>().GenerateSecretRoomObject();
                }
                else
                {
                    int randNumOfObj = Random.Range(0, 4);
                    gameObject.GetComponent<GridMapRenderer>().GenerateRandomObjects(randNumOfObj);
                }

                foreach (var medRoom in medallionPieceRoomPos)
                {
                    if (medRoom[0] == i && medRoom[1] == j)
                    {
                        gameObject.GetComponent<GridMapRenderer>().GenerateMedallionPiece(medallionSpawnCount);
                        medallionPieceRoomPos.Remove(medRoom);
                        medallionSpawnCount++;
                        break;
                    }
                }

                foreach (var rabRoom in rabbitMonsterRoomPos)
                {
                    if (rabRoom[0] == i && rabRoom[1] == j)
                    {
                        gameObject.GetComponent<GridMapRenderer>().SpawnMonster();
                        rabbitMonsterRoomPos.Remove(rabRoom);
                        break;
                    }
                }

                if (rooms[i,j].DoorTop)
                {
                    //Remove RG that's blocking the passage
                    gameObject.transform.GetChild(1).transform.GetChild(2).gameObject.SetActive(false);
                    //Reveal Door Sprite
                    gameObject.transform.GetChild(5).gameObject.SetActive(true);
                }

                if (rooms[i, j].DoorBot)
                {
                    //Remove RG that's blocking the passage
                    gameObject.transform.GetChild(2).transform.GetChild(2).gameObject.SetActive(false);
                    //Reveal Door Sprite
                    gameObject.transform.GetChild(6).gameObject.SetActive(true);
                }

                if (rooms[i, j].DoorLeft)
                {
                    //Remove RG that's blocking the passage
                    gameObject.transform.GetChild(3).transform.GetChild(2).gameObject.SetActive(false);
                    //Reveal Door Sprite
                    gameObject.transform.GetChild(7).gameObject.SetActive(true);
                }

                if (rooms[i, j].DoorRight)
                {
                    //Remove RG that's blocking the passage
                    gameObject.transform.GetChild(4).transform.GetChild(2).gameObject.SetActive(false);
                    //Reveal Door Sprite
                    gameObject.transform.GetChild(8).gameObject.SetActive(true);
                }
            }
        }
        instructionMenu.SetActive(true);
    }

    private void SetMedallionPieces()
    {
        for (int i = 0; i < 3; i++)
        {
            int[] roomPos = new int[2];
            bool same;
            do
            {
                same = false;
                
                roomPos[0] = Random.Range(0, mapSize);
                roomPos[1] = Random.Range(0, mapSize);

                foreach (var medRoomPos in medallionPieceRoomPos)
                {
                    if (medRoomPos[0] == roomPos[0] && medRoomPos[1] == roomPos[1])
                    {
                        same = true;
                    }
                }

            } while (same);
            medallionPieceRoomPos[i] = roomPos;
        }
    }

    private void SetRabbitMonsterRoomPos()
    {
        for (int i = 0; i < 2; i++)
        {
            int[] roomPos = new int[2];
            bool same;
            do
            {
                same = false;

                roomPos[0] = Random.Range(0, mapSize);
                roomPos[1] = Random.Range(0, mapSize);

                foreach (var rabRoomPos in rabbitMonsterRoomPos)
                {
                    if (roomPos[0] == startingRoomPos[0] && roomPos[1]==startingRoomPos[1])
                    {
                        same = true;
                    }
                    if (roomPos[0] == exitRoomPos[0] && roomPos[1] == exitRoomPos[1])
                    {
                        same = true;
                    }
                    if (roomPos[0] == secretRoomPos[0] && roomPos[1] == secretRoomPos[1])
                    {
                        same = true;
                    }
                    if (roomPos[0] == rabRoomPos[0] && roomPos[1] == rabRoomPos[1])
                    {
                        same = true;
                    }
                }
            } while (same);
            rabbitMonsterRoomPos[i] = roomPos;
        }
    }
}
