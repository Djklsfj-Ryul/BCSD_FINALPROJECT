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

    bool Trigger = true;
    int Rand_Pos;
    int sizeX, sizeY;
    int Pos_x, Pos_z;
    int Rand_Pos_x, Rand_Pos_z;

    Node[,] NodeArray;
    Node StartNode, TargetNode, CurNode;
    List<Node> OpenList, ClosedList;

    public void Start()
    {
        res = FindObjectOfType<Respawn_Enemy>();
        PathFinding();
    }

    public void PathFinding()
    {
        // NodeArray의 크기 정해주고, isWall, x, y 대입
        sizeX = res.MAP.GetLength(1);
        sizeY = res.MAP.GetLength(0);
        bottomLeft = new Vector3(0, 0, sizeY);
        topRight   = new Vector3(sizeX, 0, 0);
        NodeArray  = new Node[sizeY, sizeX];

        // 시작과 끝 노드, 열린리스트와 닫힌리스트, 마지막리스트 초기화
        Pos_x = (int)res.Rand_Pos[res.Count_num - 1].x;
        Pos_z = (int)res.Rand_Pos[res.Count_num - 1].z;
        Rand_Pos = Random.Range(0, res.Count_num - 2);
        Rand_Pos_x = (int)res.Rand_Pos[Rand_Pos].x;
        Rand_Pos_z = (int)res.Rand_Pos[Rand_Pos].z;

        for (int j = 0; j < sizeY; j++)
        {
            for (int i = 0; i < sizeX; i++)
            {
                bool isWall = false;

                Distinguish(Rand_Pos_z, Rand_Pos_x, Rand_Pos);

                if (res.MAP[j,i] == 7 || res.MAP[j, i] == 8)
                {
                    isWall = true;
                }
                NodeArray[j, i] = new Node(isWall, j + (int)bottomLeft.y, i + (int)bottomLeft.x);
            }
        }
        Debug.Log(Pos_z + "," + Pos_x);
        Debug.Log(Rand_Pos_z + "," + Rand_Pos_x);
        StartNode = NodeArray[Pos_z, Pos_x];
        if(Trigger)
            TargetNode = NodeArray[Rand_Pos_z, Rand_Pos_x];

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

                for (int i = 0; i < FinalNodeList.Count; i++)
                    Debug.Log(i + "번째는 " + FinalNodeList[i].y + ", " + FinalNodeList[i].x);
                return;
            }

            // ↑ → ↓ ←
            OpenListAdd(CurNode.x, CurNode.y + 1);
            OpenListAdd(CurNode.x + 1, CurNode.y);
            OpenListAdd(CurNode.x, CurNode.y - 1);
            OpenListAdd(CurNode.x - 1, CurNode.y);
        }
    }
    //checkx,y : 현재 내 x,y좌표 +1 -1
    void OpenListAdd(int checkX, int checkY)
    {
        // 상하좌우 범위를 벗어나지 않고, 벽이 아니면서, 닫힌리스트에 없다면
        if (checkX >= bottomLeft.x && checkX < topRight.x + 1 && checkY < bottomLeft.z + 1 && checkY >= topRight.z && !NodeArray[checkY, checkX].isWall && !ClosedList.Contains(NodeArray[checkY, checkX]))
        {
            if(res.MAP[checkY,checkX] == 5)
            {
                TargetNode = NodeArray[checkY, checkX];
                Trigger = false;
            }
            // 이웃노드에 넣고, 직선은 10, 대각선은 14비용(사용 안함)
            Debug.Log(checkY + "," + checkX);
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
    void OnDrawGizmos()
    {
        if (FinalNodeList.Count != 0)
            for (int i = 0; i < FinalNodeList.Count - 1; i++)
            {
                Gizmos.DrawLine(new Vector3(FinalNodeList[i].x, 25, FinalNodeList[i].y), new Vector3(FinalNodeList[i + 1].x, 25, FinalNodeList[i + 1].y));
            }
    }
    void Distinguish(int Rand_Pos_z, int Rand_Pos_x, int Random)
    {
        for (int i = 0; i < res.Count_num - 1; i++)
        {
            if (i < Respawn_Enemy.Count_Big)
            {
                for (int dy = -2; dy < 3; dy++)
                {
                    for (int dx = -2; dx < 3; dx++)
                    {
                        res.MAP[Rand_Pos_z + dy, Rand_Pos_x + dx] = 5;
                    }
                }
                break;
            }
            else if (i < Respawn_Enemy.Count_Big + Respawn_Enemy.Count_Medium)
            {
                for (int dy = -1; dy < 2; dy++)
                {
                    for (int dx = -1; dx < 2; dx++)
                    {
                        res.MAP[Rand_Pos_z + dy, Rand_Pos_x + dx] = 5;
                    }
                }
                break;
            }
            else if (i < Respawn_Enemy.Count_Big + Respawn_Enemy.Count_Medium + Respawn_Enemy.Count_Small)
            {
                res.MAP[Rand_Pos_z, Rand_Pos_x] = 5;
                break;
            }
        }
    }
}
