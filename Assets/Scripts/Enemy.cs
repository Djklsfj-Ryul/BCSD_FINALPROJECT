using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Node
{
    public Node(bool _isWall, int _y, int _x)
    {
        isWall = _isWall;
        x = _x;
        y = _y;
    }

    public bool isWall;
    public Node ParentNode;

    // G : 시작으로부터 이동했던 거리, H : |가로|+|세로| 장애물 무시하여 목표까지의 거리, F : G + H
    public int x, y, G, H;
    public int F { get { return G + H; } }
}

public class Enemy : MonoBehaviour
{
    private static Respawn_Enemy res;
    public Vector3 bottomLeft, topRight;
    public List<Node> FinalNodeList;
    public int HOW = 0;

    public static bool em = false;

    bool Pick_Up = false;
    bool Trigger = true;
    //public static bool plmove = true;
    int Rand_Pos;
    int sizeX, sizeY;
    int x, y;
    public static int Pos_x, Pos_z;
    int Rand_Pos_x, Rand_Pos_z;
    public static bool enemymove = true;

    Node[,] NodeArray;
    Node StartNode, TargetNode, CurNode;
    List<Node> OpenList, ClosedList;

    public void Start()
    {
        res = FindObjectOfType<Respawn_Enemy>();
    }
    public void Update()
    {
        if(!Full_System.trap)
        {
            Debug.Log(Full_System.trap);
            Enemy_Move();
            em = true;
            Full_System.trap = true;
        }
    }
    public void Enemy_Move()
    {
        Rand_Pos = Random.Range(0, Respawn_Enemy.Count_num - 2);
        Debug.Log((int)res.Rand_Pos[Rand_Pos].x +","+ (int)res.Rand_Pos[Rand_Pos].z);
        if (Full_System.Stamina_Enemy != 0)
        {
            PathFinding((int)res.Rand_Pos[Rand_Pos].x, (int)res.Rand_Pos[Rand_Pos].z);
            Reset((int)res.Rand_Pos[Rand_Pos].x, (int)res.Rand_Pos[Rand_Pos].z);
            while (true)
            {
                if (!Checking())
                {
                    Pick_Up = true;
                    if(!PathFinding(x, y))
                    {
                        Pos_x = (int)Mathf.Ceil( res.Instant_Enemy.gameObject.transform.position.x);
                        Pos_z = (int)Mathf.Ceil(res.Instant_Enemy.gameObject.transform.position.z);
                        continue;
                    }
                    break;
                }
            }
            Pick_Up = false;
            HOW = 0;
        }
        //ShowMap();
    }
    public bool PathFinding(int xx, int zz)
    {
        // NodeArray의 크기 정해주고, isWall, x, y 대입
        sizeX = Respawn_Enemy.MAP.GetLength(1);
        sizeY = Respawn_Enemy.MAP.GetLength(0);
        bottomLeft = new Vector3(0, 0, sizeY);
        topRight = new Vector3(sizeX, 0, 0);
        NodeArray = new Node[sizeY, sizeX];

        // 시작과 끝 노드, 열린리스트와 닫힌리스트, 마지막리스트 초기화
        Pos_x = (int)res.Instant_Enemy.gameObject.transform.position.x;
        Pos_z = (int)res.Instant_Enemy.gameObject.transform.position.z;

        //Debug.Log(Pos_z + "," + Pos_x);

        Respawn_Enemy.MAP[Pos_z, Pos_x] = 0;

        if(!Pick_Up)
            HOW = Distinguish(zz, xx, Rand_Pos);
        for (int j = 0; j < sizeY; j++)
        {
            for (int i = 0; i < sizeX; i++)
            {
                bool isWall = false;

                if (Respawn_Enemy.MAP[j, i] == 7 || Respawn_Enemy.MAP[j, i] == 8)
                {
                    isWall = true;
                }
                NodeArray[j, i] = new Node(isWall, j + (int)bottomLeft.y, i + (int)bottomLeft.x);
            }
        }
        //Debug.Log(Pos_z + "," + Pos_x);
        //Debug.Log(Rand_Pos_z + "," + Rand_Pos_x);
        StartNode = NodeArray[Pos_z, Pos_x];
        TargetNode = NodeArray[zz, xx];

        OpenList = new List<Node>() { StartNode };
        ClosedList = new List<Node>();
        FinalNodeList = new List<Node>();


        while (OpenList.Count > 0)
        {
            // 열린리스트 중 가장 F가 작고 F가 같다면 H가 작은 걸 현재노드로 하고 열린리스트에서 닫힌리스트로 옮기기
            CurNode = OpenList[0];
            for (int i = 1; i < OpenList.Count; i++)
                if (OpenList[i].F <= CurNode.F && OpenList[i].H < CurNode.H)
                {
                    CurNode = OpenList[i];
                }
            OpenList.Remove(CurNode);
            ClosedList.Add(CurNode);

            // 마지막
            if (CurNode == TargetNode)
            {
                Node TargetCurNode = TargetNode;
                while (TargetCurNode != StartNode)
                {
                    FinalNodeList.Add(TargetCurNode);
                    TargetCurNode = TargetCurNode.ParentNode;
                }
                FinalNodeList.Add(StartNode);
                FinalNodeList.Reverse();

                if (!Pick_Up && Full_System.Stamina_Enemy >= 1)
                {
                    if (Full_System.Stamina_Enemy < FinalNodeList.Count * 1)
                    {
                        //Debug.Log(FinalNodeList[0].y + "," + FinalNodeList[0].x + "에서" + FinalNodeList[FinalNodeList.Count - 1].y + "," + FinalNodeList[FinalNodeList.Count - 1].x + "로의 이동은 불가합니다.");
                    }
                    else
                    {
                        res.Instant_Enemy.gameObject.transform.position = new Vector3(FinalNodeList[FinalNodeList.Count - 1].x, 21, FinalNodeList[FinalNodeList.Count - 1].y);
                    }
                }
                else if (Pick_Up && Full_System.Stamina_Enemy >= 1)
                {
                    if (Full_System.Stamina_Enemy < FinalNodeList.Count * 2)
                    {
                        //Debug.Log(FinalNodeList[0].y + "," + FinalNodeList[0].x + "에서" + FinalNodeList[FinalNodeList.Count - 1].y + "," + FinalNodeList[FinalNodeList.Count - 1].x + "로의 이동은 불가합니다.");
                        Respawn_Enemy.MAP[FinalNodeList[0].y, FinalNodeList[0].x] = 9;
                        return false;
                    }
                    else
                    {
                        if (HOW == 3)
                        {
                            if (FinalNodeList.Count < 4)
                            {
                                res.Instant_Enemy.gameObject.transform.position = new Vector3(FinalNodeList[0].x, 21, FinalNodeList[0].y);
                                Pointing(FinalNodeList[0].x, FinalNodeList[0].y, 0);
                            }
                            else
                            {
                                int i = 4;
                                while (true)
                                {
                                    if (Respawn_Enemy.MAP[FinalNodeList[FinalNodeList.Count - i].y, FinalNodeList[FinalNodeList.Count - i].x] != 0)
                                        i++;
                                    else
                                    {
                                        Pointing(FinalNodeList[FinalNodeList.Count - i].x, FinalNodeList[FinalNodeList.Count - i].y, 0);
                                        Pointing(FinalNodeList[FinalNodeList.Count - 1].x, FinalNodeList[FinalNodeList.Count - 1].y, HOW);
                                        res.Instant_Enemy.gameObject.transform.position = new Vector3(FinalNodeList[FinalNodeList.Count - i].x, 21, FinalNodeList[FinalNodeList.Count - i].y);
                                        break;
                                    }
                                }
                            }
                            res.Instant_Big.gameObject.transform.position = new Vector3(FinalNodeList[FinalNodeList.Count - 1].x, 21, FinalNodeList[FinalNodeList.Count - 1].y);
                        }
                        else if (HOW == 2)
                        {
                            for (int j = 0; j < Respawn_Enemy.Count_Medium; j++)
                            {
                                if (Mathf.Ceil(res.manage[0, j].transform.position.x) == Mathf.Ceil(res.Rand_Pos[Rand_Pos].x) && Mathf.Ceil(res.manage[0, j].transform.position.z) == Mathf.Ceil(res.Rand_Pos[Rand_Pos].z))
                                {
                                    if (FinalNodeList.Count < 3)
                                    {
                                        res.Instant_Enemy.gameObject.transform.position = new Vector3(FinalNodeList[0].x, 21, FinalNodeList[0].y);
                                        Pointing(FinalNodeList[0].x, FinalNodeList[0].y, 0);
                                    }
                                    else
                                    {
                                        int i = 3;
                                        while (true)
                                        {
                                            if (Respawn_Enemy.MAP[FinalNodeList[FinalNodeList.Count - i].y, FinalNodeList[FinalNodeList.Count - i].x] != 0)
                                                i++;
                                            else
                                            {
                                                Pointing(FinalNodeList[FinalNodeList.Count - i].x, FinalNodeList[FinalNodeList.Count - i].y, 0);
                                                Pointing(FinalNodeList[FinalNodeList.Count - 1].x, FinalNodeList[FinalNodeList.Count - 1].y, HOW);
                                                res.Instant_Enemy.gameObject.transform.position = new Vector3(FinalNodeList[FinalNodeList.Count - i].x, 21, FinalNodeList[FinalNodeList.Count - i].y);
                                                break;
                                            }
                                        }
                                    }
                                    res.manage[0,j].gameObject.transform.position = new Vector3(FinalNodeList[FinalNodeList.Count - 1].x, 21, FinalNodeList[FinalNodeList.Count - 1].y);
                                }
                            }
                        }
                        else if (HOW == 1)
                        {
                            for (int j = 0; j < Respawn_Enemy.Count_Small; j++)
                            {
                                if (Mathf.Ceil(res.manage[1, j].transform.position.x) == Mathf.Ceil(res.Rand_Pos[Rand_Pos].x) && Mathf.Ceil(res.manage[1, j].transform.position.z) == Mathf.Ceil(res.Rand_Pos[Rand_Pos].z))
                                {
                                    if (FinalNodeList.Count < 2)
                                    {
                                        res.Instant_Enemy.gameObject.transform.position = new Vector3(FinalNodeList[0].x, 21, FinalNodeList[0].y);
                                        Pointing(FinalNodeList[0].x, FinalNodeList[0].y, 0);
                                    }
                                    else
                                    {
                                        int i = 2;
                                        while (true)
                                        {
                                            if (Respawn_Enemy.MAP[FinalNodeList[FinalNodeList.Count - i].y, FinalNodeList[FinalNodeList.Count - i].x] != 0)
                                                i++;
                                            else
                                            {
                                                Pointing(FinalNodeList[FinalNodeList.Count - i].x, FinalNodeList[FinalNodeList.Count - i].y, 0);
                                                Pointing(FinalNodeList[FinalNodeList.Count - 1].x, FinalNodeList[FinalNodeList.Count - 1].y, HOW);
                                                res.Instant_Enemy.gameObject.transform.position = new Vector3(FinalNodeList[FinalNodeList.Count - i].x, 21, FinalNodeList[FinalNodeList.Count - i].y);
                                                break;
                                            }
                                        }
                                    }
                                    res.manage[1,j].gameObject.transform.position = new Vector3(FinalNodeList[FinalNodeList.Count - 1].x, 21, FinalNodeList[FinalNodeList.Count - 1].y);
                                }
                            }
                        }
                    }
                }
                if (!Pick_Up)
                {
                    Rand_Pos_x = xx;
                    Rand_Pos_z = zz;
                    Full_System.Stamina_Enemy -= FinalNodeList.Count;
                    res.Rand_Pos[Respawn_Enemy.Count_num - 1].z = TargetNode.y;
                    res.Rand_Pos[Respawn_Enemy.Count_num - 1].x = TargetNode.x;
                    Respawn_Enemy.MAP[(int)res.Rand_Pos[Respawn_Enemy.Count_num - 1].z, (int)res.Rand_Pos[Respawn_Enemy.Count_num - 1].x] = 9;
                }
                else if(HOW == 3)
                {
                    Full_System.Stamina_Enemy -= (FinalNodeList.Count * 2);
                }
                else if (HOW == 2)
                {
                    Full_System.Stamina_Enemy -= (FinalNodeList.Count * 2);
                }
                else if (HOW == 1)
                {
                    Full_System.Stamina_Enemy -= (FinalNodeList.Count * 2);
                }
                return true;
            }

            // ↑ → ↓ ←
            OpenListAdd(CurNode.x, CurNode.y + 1);
            OpenListAdd(CurNode.x + 1, CurNode.y);
            OpenListAdd(CurNode.x, CurNode.y - 1);
            OpenListAdd(CurNode.x - 1, CurNode.y);
        }
        return true;
    }
    public void Reset(int x, int y)
    {
        if (HOW == 3)
        {
            for (int dy = -3; dy < 4; dy++)
            {
                for (int dx = -3; dx < 4; dx++)
                {
                    if (dx == 3 || dx == -3 || dy == 3 || dy == -3)
                    {
                        if (Respawn_Enemy.MAP[y + dy, x + dx] == 5)
                        {
                            Respawn_Enemy.MAP[y + dy, x + dx] = 0;
                        }
                    }
                    else
                    {
                        Respawn_Enemy.MAP[y + dy, x + dx] = 0;
                    }
                }
            }
        }
        if (HOW == 2)
        {
            for (int dy = -2; dy < 3; dy++)
            {
                for (int dx = -2; dx < 3; dx++)
                {
                    if (dx == 2 || dx == -2 || dy == 2 || dy == -2)
                    {
                        if (Respawn_Enemy.MAP[y + dy, x + dx] == 5)
                        {
                            Respawn_Enemy.MAP[y + dy, x + dx] = 0;
                        }
                    }
                    else
                    {
                        Respawn_Enemy.MAP[y + dy, x + dx] = 0;
                    }
                }
            }
        }
        if (HOW == 1)
        {
            for (int dy = -1; dy < 2; dy++)
            {
                for (int dx = -1; dx < 2; dx++)
                {
                    if (dx == 1 || dx == -1 || dy == 1 || dy == -1)
                    {
                        if (Respawn_Enemy.MAP[y + dy, x + dx] == 5)
                        {
                            Respawn_Enemy.MAP[y + dy, x + dx] = 0;
                        }
                    }
                    else
                    {
                        Respawn_Enemy.MAP[y + dy, x + dx] = 0;
                    }
                }
            }
        }
    }
    public bool Checking()
    {
        x = Random.Range(3, (int)Respawn_Enemy.MAP.GetLength(1) - 3);
        y = Random.Range(3, (int)Respawn_Enemy.MAP.GetLength(0) - 3);
        if(HOW == 3)
        {
            for (int i = -2; i < 3; i++)
            {
                for (int j = -2; j < 3; j++)
                {
                    if (y + j >= (int)Respawn_Enemy.MAP.GetLength(0) || y + j < 0 || x + i < 0 || x + i >= (int)Respawn_Enemy.MAP.GetLength(1))
                        return true;
                    if (Respawn_Enemy.MAP[y + j, x + i] != 0)
                        return true;
                }
            }
            return false;
        } 
        else if(HOW == 2)
        {
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    if (y + j >= (int)Respawn_Enemy.MAP.GetLength(0) || y + j < 0 || x + i < 0 || x + i >= (int)Respawn_Enemy.MAP.GetLength(1))
                        return true;
                    if (Respawn_Enemy.MAP[y + j, x + i] != 0)
                        return true;
                }
            }
            return false;
        }
        else if (HOW == 1)
        {
            if (Respawn_Enemy.MAP[y, x] != 0)
                return true;
            return false;
        }
        return false;
    }
    //checkx,y : 현재 내 x,y좌표 +1 -1
    void OpenListAdd(int checkX, int checkY)
    {
        try
        {
            //Debug.Log(checkY +","+ checkX);
            // 상하좌우 범위를 벗어나지 않고, 벽이 아니면서, 닫힌리스트에 없다면
            if (checkX >= bottomLeft.x && checkX < topRight.x + 1 && checkY < bottomLeft.z + 1 && checkY >= topRight.z && !NodeArray[checkY, checkX].isWall && !ClosedList.Contains(NodeArray[checkY, checkX]))
            {
                if (Respawn_Enemy.MAP[checkY, checkX] == 5 && Trigger)
                {
                    TargetNode = NodeArray[checkY, checkX];
                    //Debug.Log("Enemy 이동거리 변경 : " + checkY + "," + checkX);
                    Trigger = false;
                }
                // 이웃노드에 넣고, 직선은 10, 대각선은 14비용(사용 안함)
                //Debug.Log(checkY + "," + checkX);
                Node NeighborNode = NodeArray[checkY, checkX];
                int MoveCost = CurNode.G + 10;

                // 이동비용이 이웃노드G보다 작거나 또는 열린리스트에 이웃노드가 없다면 G, H, ParentNode를 설정 후 열린리스트에 추가
                if (MoveCost < NeighborNode.G || !OpenList.Contains(NeighborNode))
                {
                    NeighborNode.G = MoveCost;
                    NeighborNode.H = (Mathf.Abs(NeighborNode.x - TargetNode.x) + Mathf.Abs(NeighborNode.y - TargetNode.y)) * 10;
                    NeighborNode.ParentNode = CurNode;
                    OpenList.Add(NeighborNode);
                }
            }
        }
        catch
        {
            return;
        }
    }
    int Distinguish(int Rand_Pos_z, int Rand_Pos_x, int Random)
    {
        for (int i = 0; i < Respawn_Enemy.Count_num - 1; i++)
        {
            if (Rand_Pos < Respawn_Enemy.Count_Big)
            {
                for (int dy = -3; dy < 4; dy++)
                {
                    for (int dx = -3; dx < 4; dx++)
                    {
                        if (dx == 3 || dx == -3 || dy == 3 || dy == -3)
                            if (Respawn_Enemy.MAP[Rand_Pos_z + dy, Rand_Pos_x + dx] == 0 && !((dx == -3 && dy == -3) || (dx == 3 && dy == -3) || (dx == -3 && dy == 3) || (dx == 3 && dy == 3)))
                                Respawn_Enemy.MAP[Rand_Pos_z + dy, Rand_Pos_x + dx] = 5;
                    }
                }
                return 3;
            }
            else if (Rand_Pos < Respawn_Enemy.Count_Big + Respawn_Enemy.Count_Medium)
            {
                for (int dy = -2; dy < 3; dy++)
                {
                    for (int dx = -2; dx < 3; dx++)
                    {
                        if (dx == 2 || dx == -2 || dy == 2 || dy == -2)
                            if (Respawn_Enemy.MAP[Rand_Pos_z + dy, Rand_Pos_x + dx] == 0 && !((dx == -2 && dy == -2) || (dx == 2 && dy == -2) || (dx == -2 && dy == 2) || (dx == 2 && dy == 2)))
                                Respawn_Enemy.MAP[Rand_Pos_z + dy, Rand_Pos_x + dx] = 5;
                    }
                }
                return 2; 
            }
            else if (Rand_Pos < Respawn_Enemy.Count_Big + Respawn_Enemy.Count_Medium + Respawn_Enemy.Count_Small)
            {
                for (int dy = -1; dy < 2; dy++)
                {
                    for (int dx = -1; dx < 2; dx++)
                    {
                        if (dx == 1 || dx == -1 || dy == 1 || dy == -1)
                            if (Respawn_Enemy.MAP[Rand_Pos_z + dy, Rand_Pos_x + dx] == 0 && !((dx == -1 && dy == -1) || (dx == 1 && dy == -1) || (dx == -1 && dy == 1) || (dx == 1 && dy == 1)))
                                Respawn_Enemy.MAP[Rand_Pos_z + dy, Rand_Pos_x + dx] = 5;
                    }
                }
                return 1;
            }
        }
        return 0;
    }
    public void Pointing(int x,int y,int HOW)
    {
        if(HOW==3)
        {
            for (int dy = -2; dy < 3; dy++)
            {
                for (int dx = -2; dx < 3; dx++)
                {
                    if (dy == 0 && dx == 0)
                        Respawn_Enemy.MAP[y + dy, x + dx] = 8;
                    else
                        Respawn_Enemy.MAP[y + dy, x + dx] = 7;
                }
            }
        }
        else if (HOW == 2)
        {
            for (int dy = -1; dy < 2; dy++)
            {
                for (int dx = -1; dx < 2; dx++)
                {
                    if (dy == 0 && dx == 0)
                        Respawn_Enemy.MAP[y + dy, x + dx] = 8;
                    else
                        Respawn_Enemy.MAP[y + dy, x + dx] = 7;
                }
            }
        }
        else if(HOW == 1)
        {
            Respawn_Enemy.MAP[y, x] = 8;
        }
        else
        {
            Respawn_Enemy.MAP[y, x] = 9;
        }
    }
    public void ShowMap()
    {
        string a1 = "";
        for (int i = 0; i < 20; i++)
        {
            for (int j = 0; j < 20; j++)
            {
                a1 = a1 + Respawn_Enemy.MAP[j, i];
            }
           //Debug.Log(a1);
            a1 = "";
        }
    }
}