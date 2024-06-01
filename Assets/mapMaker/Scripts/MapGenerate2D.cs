using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Xml.Schema;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Photon.Pun;
/*using static UnityEditor.FilePathAttribute;
using static UnityEditor.PlayerSettings;*/

public class MapGenerate2D : MonoBehaviourPun
{
    enum CellType //grid방식을 응용하기위해서 cell의 타입을 room(방) none(아무것도없음) hallway(복도) 3가지로 나누었다.
    {
        None, Room , Hallway, Wall
    }

    public delegate void OnFinishedMapGenerate();
    private OnFinishedMapGenerate onFinishedMapGenerateCallback = null;
    public OnFinishedMapGenerate OnFinishedMapGenerateCallback
    {
        set { onFinishedMapGenerateCallback = value; }
    }
    
    private Transform[] transforms;

    private List<Vector3> points = new List<Vector3>();

    private List<int> roomIndexList = new List<int>();

    private Vector3 size;
    [SerializeField]
    private GameObject map;



    [SerializeField]
    private int sizeX;
    [SerializeField]
    private int sizeZ;


    [SerializeField]
    private GameObject[] RoomsPrefab;
    [SerializeField]
    private GameObject WallPrefab;



    private Transform[] Tile;

    [SerializeField]
    private int RoomCount;
    private List<Room> rooms;
    private List<Room> RoomTrList =  new List<Room>();
    private BoywerWatson2D delaunay;


    System.Random random = new System.Random();

    [SerializeField]
    GameObject roadPrefab;


    Grid2D<CellType> grid;
    // using incremental algorithm

    private List<MapTransmission> RecivedData = new List<MapTransmission>();
    private bool isFinished = false;

    public List<Room> getRooms()
    {
        return rooms;
    }
    public void SetRoomData(List<MapTransmission> _Input)
    {
        RecivedData = _Input;
    }

    public List<int> getRoomidx()
    {
        return roomIndexList;
    }

    private IEnumerator WaitForMaster()
    {
        while (isFinished == false)
        {
            Debug.Log("Photon is NotReady");
        }

        yield break;
    }

    public void StartMapGenerator()
    {
        StartCoroutine(MapGenerate());
    }

    private IEnumerator MapGenerate()
    {
        rooms = new List<Room>(); //방의 크기와 위치 저장하는곳
        transforms = GetComponentsInChildren<Transform>();


        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogWarning("MasterClient?: " + PhotonNetwork.IsMasterClient);
            yield return StartCoroutine(PlaceRooms());

            onFinishedMapGenerateCallback?.Invoke(); // 맵 생성이 끝나면 MapManager에게 알려줌

            isFinished = true;
        }
        else
        {
            Debug.LogWarning("MasterClient?: " + PhotonNetwork.IsMasterClient);
            yield return StartCoroutine(WaitForMaster());

            foreach (MapTransmission t in RecivedData)
            {
                yield return StartCoroutine(PlaceRoom(new Vector3(t.posX, 0, t.posZ), new Vector3(t.sizeX, 0, t.sizeZ), RoomsPrefab[t.index]));
            }
        }

        // 아래는 기존 진행 코드
        // yield return StartCoroutine(PlaceRooms());

        //onFinishedMapGenerateCallback?.Invoke(); // 맵 생성이 끝나면 MapManager에게 알려줌

        // << 길 생성 시작
        //delaunay에 델로니 삼각함수의 결과값을 가져옴
        delaunay = BoywerWatson2D.Triangulate(rooms); // 삼각분할


        //delaunay한값을 MST로 경로를 바꾸고, 거기에 랜덤한 경로를 20%확률로 추가
     

        

        
            //그리드형식으로 도로를 깔아줌
        GenerateRoads(PlayMST());

            //방의 위치가 맞는지 재확인


        }

    private IEnumerator PlaceRooms() {



        for (int i =0; i< RoomCount; i++)
        {
         
            //room 랜덤배치
            Vector3 location = new Vector3(
                random.Next(20, 250),
                0,
                random.Next(20, 250)
                );

            int randomRoomindex = random.Next(2);
            roomIndexList.Add(randomRoomindex);
            GameObject RoomPrefab = RoomsPrefab[randomRoomindex];
            
            var sqr = RoomPrefab.gameObject.GetComponent<BoxCollider>();
            Vector3 roomSize = new Vector3(sqr.size.x, 0, sqr.size.z);


            Debug.Log(location);

            //grid에 해당 방의 위치와 크기를 고려한 방을 담는다.            


            yield return StartCoroutine(PlaceRoom(location, roomSize, RoomPrefab));
        }
        Debug.Log("get rooms information..");

        Map mm = map.GetComponent<Map>();
        RoomTrList = mm.returnList();

        Debug.Log(RoomTrList.Count);
        float minX = 9999;
        float minZ = 9999;
        float maxX = 0;
        float maxZ = 0;

        
        

        foreach(Room room in RoomTrList)
        {
            if(room.tr.position.x%2 == 1)
            {
                room.RoomMove(new Vector3(1, 0, 0));
            }
            if(room.tr.position.z%2 == 1)
            {
                room.RoomMove(new Vector3(0, 0, 1));
            }

            if (room.xMin <= minX)
            {
                minX = room.xMin;
            }
            if (room.zMin <= minZ)
            {
                minZ = room.zMin;
            }
            if (room.xMax >= maxX)
            {
                maxX = room.xMax;
            }
            if (room.zMax >= maxZ)
            {
                maxZ = room.zMax;
            }

            Destroy(room.tr.gameObject.GetComponent<Rigidbody>());
            Destroy(room.tr.gameObject.GetComponent<BoxCollider>());

        }
        
        if (minX < 0) {
            if (minX % 2 == 1)
            {
                minX--;
            }
            foreach (var room in RoomTrList)
            {
                
                room.RoomMove(new Vector3(Mathf.Abs(minX), 0,0));

            }
            maxX = maxX + Mathf.Abs(minX);
            minX = 0; }
        if(minZ < 0) {
            if (minZ % 2 == 1)
            {
                minZ--;
            }
            foreach (var room in RoomTrList)
            {
                 //여기서 13f는 방의 크기 (여러방을 쓸경우 13f를 바꿀거임)
                room.RoomMove(new Vector3(0, 0, Mathf.Abs(minZ)));
            }
            maxZ = maxZ + Mathf.Abs(minZ);
            minZ = 0; }
        

        

        size = new Vector3(maxX + 1, 0, maxZ + 1);

        grid = new Grid2D<CellType>(size, Vector3.zero);

        for (int i = 0; i < RoomTrList.Count; i++)
        {
            Debug.Log(RoomTrList[i].xMax + " "+ RoomTrList[i].xMin + " " + RoomTrList[i].zMax + " " + RoomTrList[i].zMin);
            for (int x = (int)RoomTrList[i].xMin; x < (int)RoomTrList[i].xMax; x++)
            {
                for (int z = (int)RoomTrList[i].zMin; z < (int)RoomTrList[i].zMax; z++)
                {
                    
                    grid[new Vector3(x, 0, z)] = CellType.Room;
                }
            }

            rooms.Add(RoomTrList[i]);
            



        }


    }



    private HashSet<Edge> PlayMST()
    {
        List<Edge> edges = new List<Edge>();
        edges = delaunay.Edges;
        List<Edge> edges2 = new List<Edge>();

        

        edges2 = primMst.MST(edges, delaunay.pointList);


        HashSet<Edge> remainEdges = new HashSet<Edge>(edges);
        HashSet<Edge> selectedEdges = new HashSet<Edge>(edges2);

        remainEdges.ExceptWith(selectedEdges);



        foreach (var edge in remainEdges)
        {
            
            double a = random.NextDouble();
            if (a < 0.3)
            {
                selectedEdges.Add(edge);
            }
            //edge 길이 검사헤서 제일 긴거 2개정도 자르는 함수
        }

        foreach(var edge in selectedEdges)
        {
            Debug.DrawLine(edge.U.Position , edge.V.Position, UnityEngine.Color.blue);
        }



        
        return selectedEdges;
    }

    private void GenerateRoads(HashSet<Edge> edges)
    {
        
        PathFinder2D astar = new PathFinder2D(size);

        foreach (Edge edge in edges)
        {
            Room startRoom = null;
            Room endRoom = null;
            foreach (Room r in rooms)
            {
                if (r.point == edge.U)
                {
                    startRoom = r;
                }
                if (r.point == edge.V)
                {
                    endRoom = r;
                }
            }
            Vector3 startInt = new Vector3((int)startRoom.point.Position.x, 0 , (int)startRoom.point.Position.z);
            Vector3 endInt = new Vector3((int)endRoom.point.Position.x, 0, (int)endRoom.point.Position.z);

            Point startPoint = new Point(startInt);
            Point endPoint = new Point(endInt);

            var path = astar.FindPath(startPoint, endPoint, (PathFinder2D.Node a , PathFinder2D.Node b) =>
            {
                var pathCost = new PathFinder2D.PathCost();

                pathCost.cost = Vector3.Distance(b.Position.Position, endPoint.Position);

                


                if (grid[b.Position.Position] == CellType.Room)
                {
                    
                    pathCost.cost += 10;
                }
                else if (grid[b.Position.Position] == CellType.None)
                {
                    
                    pathCost.cost += 5;
                }
                else if (grid[b.Position.Position] == CellType.Hallway)
                {
                    
                    pathCost.cost += 1;
                }

                pathCost.traversable = true;

                return pathCost;

            });
           
            if(path != null)
            {
                for (int i =0; i < path.Count; i++)
                {
                    var current = path[i];
                    Debug.Log(path[i] + " " + grid[current]);
                    if (grid[current] == CellType.None)
                    {
                        grid[current] = CellType.Hallway;
                    }

                    if (i > 0)
                    {
                        var prev = path[i - 1];
                        var delta = current - prev;

                        Debug.DrawLine(prev, current, UnityEngine.Color.red, 100, true);
                    }
                }

                foreach(var pos in path)
                {
                    if (grid[pos] == CellType.Hallway)
                    {
                        PlaceHallway(pos);
                        
                        
                    }
                }
            }



        }

    }
     

    private IEnumerator PlaceRoom(Vector3 location , Vector3 Size , GameObject RoomPrefab)
    {
        
        
        GameObject r = Instantiate(RoomPrefab, location, Quaternion.identity);

        

        r.transform.parent = map.transform;
        

        Map Map = map.GetComponent<Map>();

        if (!PhotonNetwork.IsMasterClient)
        {
            rooms.Add(r.GetComponent<Room>());
        }
        
        while (Map.returnBool())
        {
            Debug.Log("wating...");
            yield return null;
        }
        
        
        
        


    }
    //  
    private void PlaceHallway(Vector3 pos)
{
    Debug.Log(grid[pos] + " " + pos.x + " " + pos.z);
    Vector3 adjustedLocation = new Vector3(pos.x, 0, pos.z);
    Vector3 additionxPlusPos = pos - new Vector3(2, 0, 0);
    Vector3 additionxMinusPos = pos + new Vector3(2, 0, 0);
    Vector3 additionzPlusPos = pos - new Vector3(0, 0, 2);
    Vector3 additionzMinusPos = pos + new Vector3(0, 0, 2);
    Vector3 hallwaySize = new Vector3(1, 1, 1);

    
    PlaceHallwayAndWalls(additionxPlusPos);
    PlaceHallwayAndWalls(additionxMinusPos);
    PlaceHallwayAndWalls(additionzPlusPos);
    PlaceHallwayAndWalls(additionzMinusPos);
}

private void PlaceHallwayAndWalls(Vector3 pos)
{
        

    if (grid[pos] == CellType.Hallway)
        {
            GameObject hallway = Instantiate(roadPrefab, pos, Quaternion.identity);
            hallway.GetComponent<Transform>().localScale = new Vector3(1, 1, 1);
        }
    else if (grid[pos] == CellType.None)
    {
        grid[pos] = CellType.Hallway; 
        GameObject hallway = Instantiate(roadPrefab, pos, Quaternion.identity);
        hallway.GetComponent<Transform>().localScale = new Vector3(1, 1, 1);
        
    }
}

private void PlaceWallsAroundHallway(Vector3 pos)
{
    Vector3[] adjacentPositions = {
        pos + new Vector3(1, 0, 0),
        pos - new Vector3(0, 0, 1),
        pos - new Vector3(1, 0, 0),
        pos + new Vector3(0, 0, 1),
        
    };
        float rotate = -90f;
    for(int i =0; i<adjacentPositions.Length; i++)
    {
            rotate = rotate + (i * 90);
    
        if (grid[adjacentPositions[i]] == CellType.None)
        {
            grid[adjacentPositions[i]] = CellType.Wall;
            GameObject wall = Instantiate(WallPrefab, adjacentPositions[i], Quaternion.Euler(0,rotate,0));
            wall.GetComponent<Transform>().localScale = new Vector3(1, 1, 1);
        }
    }
}
   

    
}
