using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Xml.Schema;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Photon.Pun;
public class MapGenerate2D : MonoBehaviourPun
{

    enum CellType //grid방식을 응용하기위해서 cell의 타입을 room(방) none(아무것도없음) hallway(복도) 3가지로 나누었다.
    {
        None, Room , Hallway, Wall
    }
    private List<Vector3> roadpath = new List<Vector3>(); 
    
    private Transform[] transforms;

    private List<Vector3> points = new List<Vector3>();
    private List<int> roomIndexList = new List<int>();
    private Vector3 size;
    [SerializeField]
    private GameObject map;
    [SerializeField]
    private GameObject route;


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
    [SerializeField]
    GameObject roadrotatePrefab;

    Grid2D<CellType> grid;
    // using incremental algorithm

    private bool isFinished = false;
    public void StartMapGenerator()
    {
        StartCoroutine(MapGenerate());  
    }

    private IEnumerator WaitForMaster()
    {
        Debug.Log("Waiting Acitive");
        while (isFinished == false)
        {
            Debug.Log("Waiting...");
            yield return null;
        }
        yield break;
    }
    [PunRPC]
    public void SetStatus(bool _input)
    {
        Debug.Log("SetStatus Active");
        isFinished = _input;
    }
    private IEnumerator MapGenerate()
    {
        Debug.Log("Start Generate Coroutine");
        rooms = new List<Room>(); //방의 크기와 위치 저장하는곳
        transforms = GetComponentsInChildren<Transform>();

        //방의 위치배치만 하는 함수
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Master Start Generate");
            yield return StartCoroutine(PlaceRooms());

            for (int i = 0; i < RoomTrList.Count; i++)
            {
                GameObject A = PhotonNetwork.Instantiate(RoomsPrefab[roomIndexList[i]].name, RoomTrList[i].point.Position, Quaternion.identity);
                
            }
            photonView.RPC("SetStatus", RpcTarget.AllBuffered, true);
        }
        else
        {
            Debug.Log("Normal Player Wait For Master");
            yield return StartCoroutine(WaitForMaster());
        }
        // 여기서 일반 클라이언트는 기다려함 호스트가 인스턴스 할때까지
        MakeRooms();

        foreach(Room r in rooms)
        {
            Debug.Log(r + " " + r.point.Position);
        }
        delaunay = BoywerWatson2D.Triangulate(rooms);
        /////////////////////////////////////////////////(destroy)

        GenerateRoads(PlayMST());

        
        //delaunay에 델로니 삼각함수의 결과값을 가져옴
        // 삼각분할


        //delaunay한값을 MST로 경로를 바꾸고, 거기에 랜덤한 경로를 20%확률로 추가
     

        

        
            //그리드형식으로 도로를 깔아줌
        

        //방의 위치가 맞는지 재확인
        makeroad(roadpath);
        }


    private void MakeRooms()
    {

        GameObject[] roomobjectarr =  GameObject.FindGameObjectsWithTag("Room");
        
        foreach(var r in roomobjectarr)
        {
            r.transform.parent = map.transform;
        }

        colidRoomParents[] LM = map.GetComponentsInChildren<colidRoomParents>();

        foreach (colidRoomParents t in LM)
        {

            Room newRoom = new Room(t.getTr().position, t.getBX());
            newRoom.tr = t.getTr();

            t.destroyCollidRoom();
            rooms.Add(newRoom);
            Destroy(t);
        }
        float maxX = 0f;
        float maxZ = 0f;
        foreach (Room r in rooms)
        {
            if (maxX < r.xMax)
            {
                maxX = r.xMax;
            }
            if (maxZ < r.zMax)
            {
                maxZ = r.zMax;
            }
        }

        size = new Vector3(maxX + 1, 0, maxZ + 1);

        grid = new Grid2D<CellType>(size, Vector3.zero);

        for (int i = 0; i < rooms.Count; i++)
        {

            for (int x = (int)rooms[i].xMin; x <= (int)rooms[i].xMax; x++)
            {
                for (int z = (int)rooms[i].zMin; z <= (int)rooms[i].zMax; z++)
                {
                    grid[new Vector3(x, 0, z)] = CellType.Room;
                }
            }





        }
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

            var sqr = RoomPrefab.gameObject.GetComponent<colidRoomParents>();
            Vector3 roomSize = sqr.getBX();



            //grid에 해당 방의 위치와 크기를 고려한 방을 담는다.            


            yield return StartCoroutine(PlaceRoom(location, roomSize, RoomPrefab));
        }
        Debug.Log("get rooms information..");

        Map mm = map.GetComponent<Map>();
        RoomTrList = mm.returnList();

        float minX = 9999;
        float minZ = 9999;
        float maxX = 0;
        float maxZ = 0;

        
        

        foreach(Room room in RoomTrList)
        {
            if (Mathf.Abs(room.point.Position.x) % 2 == 1)
            {
                room.RoomMove(new Vector3(1, 0, 0));
            }
            if (Mathf.Abs(room.point.Position.z) % 2 == 1)
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



        }
        
        if (minX < 0) {
 
            foreach (var room in RoomTrList)
            {
                
                room.RoomMove(new Vector3(Mathf.Abs(minX), 0,0));

            }
            maxX = maxX + Mathf.Abs(minX);
            minX = 0; }
        if(minZ < 0) {

            foreach (var room in RoomTrList)
            {
                 //여기서 13f는 방의 크기 (여러방을 쓸경우 13f를 바꿀거임)
                room.RoomMove(new Vector3(0, 0, Mathf.Abs(minZ)));
            }
            maxZ = maxZ + Mathf.Abs(minZ);
            minZ = 0; }

        Transform[] mmarr = map.GetComponentsInChildren<Transform>();


        foreach (Transform t in mmarr)
        {
            if (t != map.transform)
            {
                Destroy(t.gameObject);
            }
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
                    
                    if (grid[current] == CellType.None)
                    {
                        grid[current] = CellType.Hallway;
                    }
                   
                    if (i > 0)
                    {
                        var prev = path[i - 1];
                        var delta = current - prev;

                        Debug.DrawLine(prev, current, UnityEngine.Color.red, 10000, true);
                    }
                }

                foreach(var pos in path)
                {
                    if (grid[pos] == CellType.Hallway)
                    {
                        roadpath.Add(pos);
                        
                        
                    }
                }
            }



        }

    }
   
    //collider를 생성하여 각 좌표를 찍는단계
    private IEnumerator PlaceRoom(Vector3 location , Vector3 Size , GameObject RoomPrefab)
    {

        GameObject r = new GameObject("RoomColid");
        CollidRoom Cr = r.AddComponent<CollidRoom>();
        Rigidbody Rigid = r.AddComponent<Rigidbody>();
        Rigid.useGravity = false;
        Rigid.constraints = RigidbodyConstraints.FreezeAll;
        BoxCollider Bxcol = r.AddComponent<BoxCollider>();

        
        Bxcol.size = Size;
        

        Map mapManager = map.GetComponent<Map>();
        r.transform.parent = map.transform;

        Debug.Log("create!");

        while (true)
        {
            Debug.Log("wating...");
            yield return new WaitForSeconds(1f);
            if (mapManager.returnBool() == false)
            {
                break;
            }
        }
        
        
        
        


    }
    //  
    private void makeroad(List<Vector3> road)
    {
       foreach(var pos in road)
        {
            PlaceHallway(pos);
        }
        foreach (var pos in road)
        {
            PlaceHallway2(pos);
        }
    }

    private void PlaceHallway(Vector3 pos)
{
    Vector3 adjustedLocation = new Vector3(pos.x, 0, pos.z);
    Vector3 additionxPPPos = pos + new Vector3(2, 0, 2);
    Vector3 additionxPMPos = pos + new Vector3(2, 0, -2);
    Vector3 additionzMPPos = pos + new Vector3(-2, 0, 2);
    Vector3 additionzMMPos = pos + new Vector3(-2, 0, -2);

    Vector3 additionxPlusPos = pos - new Vector3(2, 0, 0);
    Vector3 additionxMinusPos = pos + new Vector3(2, 0, 0);
    Vector3 additionzPlusPos = pos - new Vector3(0, 0, 2);
    Vector3 additionzMinusPos = pos + new Vector3(0, 0, 2);
    Vector3 hallwaySize = new Vector3(1, 1, 1);


    PlaceHallwayAndWalls(additionxPPPos);
    PlaceHallwayAndWalls(additionxPMPos);
    PlaceHallwayAndWalls(additionzMPPos);
    PlaceHallwayAndWalls(additionzMMPos);



    PlaceHallwayAndWalls(additionxPlusPos);
    PlaceHallwayAndWalls(additionxMinusPos);
    PlaceHallwayAndWalls(additionzPlusPos);
    PlaceHallwayAndWalls(additionzMinusPos);
    
    }
    private void PlaceHallway2(Vector3 pos)
    {
        PlaceHallwayPos(pos);
    }

    private void PlaceHallwayPos(Vector3 pos)
    {
        GameObject hallway = Instantiate(roadPrefab, pos, Quaternion.identity);
        hallway.GetComponent<Transform>().localScale = new Vector3(1, 1, 1);
        hallway.transform.parent = route.transform;

        Collider[] colliders = Physics.OverlapBox(pos+ new Vector3(0, 1 , 0), new Vector3(0.4f, 0.4f, 0.4f));
        foreach (var collider in colliders)
        {
            // 오브젝트 제거
            Destroy(collider.gameObject);
        }

        PlaceWall(pos);
    }
    private void PlaceWall(Vector3 pos)
    {
        Vector3[] walls =
        {
            
            new Vector3(4, 0 , 0),
            new Vector3(-4 , 0,0),
            new Vector3(0,0,4),
            new Vector3(0,0,-4)
        };

        for(int i =0; i<walls.Length; i++)
        {
            if (i == 0 && grid[pos + walls[i]] == CellType.None)
            {
                grid[pos + walls[i] - new Vector3(1, 0, 0)] = CellType.Wall;
                GameObject wall = Instantiate(WallPrefab, pos + walls[i] - new Vector3(1, 0 , 0), Quaternion.Euler(0, -90, 0));
                wall.transform.parent = route.transform;
            }
            else if(i == 1 && grid[pos + walls[i]] == CellType.None)
            {
                grid[pos + walls[i] + new Vector3(1, 0, 0)] = CellType.Wall;
                GameObject wall = Instantiate(WallPrefab, pos + walls[i] + new Vector3(1, 0, 0), Quaternion.Euler(0, 90, 0));
                wall.transform.parent = route.transform;
            }
            else if(i == 2 && grid[pos + walls[i]] == CellType.None)
            {
                grid[pos + walls[i] - new Vector3(0, 0, 1)] = CellType.Wall;
                GameObject wall = Instantiate(WallPrefab, pos + walls[i] - new Vector3(0, 0, 1), Quaternion.Euler(0, 180, 0));
                wall.transform.parent = route.transform;
            }
            else if(i == 3 && grid[pos + walls[i]] == CellType.None)
            {
                grid[pos + walls[i] + new Vector3(0, 0, 1)] = CellType.Wall;
                GameObject wall = Instantiate(WallPrefab, pos + walls[i] + new Vector3(0, 0, 1), Quaternion.Euler(0,0,0));
                wall.transform.parent = route.transform;
            }
        }
    }

private void PlaceHallwayAndWalls(Vector3 pos)
{


        Collider[] colliders = Physics.OverlapBox(pos + new Vector3(0, 1, 0), new Vector3(0.4f, 0.4f, 0.4f));
        foreach (var collider in colliders)
        {
            // 오브젝트 제거
            Destroy(collider.gameObject);
        }
        

        if (grid[pos] == CellType.None)
    {
            grid[pos] = CellType.Hallway;   
            GameObject hallway = Instantiate(roadPrefab, pos, Quaternion.identity);
        hallway.GetComponent<Transform>().localScale = new Vector3(1, 1, 1);
        hallway.transform.parent = route.transform;
    }

        
}


   

    
}
