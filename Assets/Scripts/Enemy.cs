using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Node
{
    public Node(bool _isWall, int _x, int _y) { isWall = _isWall; x = _x; y = _y; }

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

    int Rand_Pos;
    int sizeX, sizeY;
    int Pos_x, Pos_z;
    int Rand_Pos_x, Rand_Pos_z;

    int debug = 1;

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
        NodeArray  = new Node[sizeX, sizeY];
        Debug.Log(sizeY + "," + sizeX);

        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {
                bool isWall = false;
                if(res.MAP[j,i] != 0)      isWall = true;
                NodeArray[i, j] = new Node(isWall, i + (int)bottomLeft.x, j + (int)bottomLeft.y);
            }
        }

        // 시작과 끝 노드, 열린리스트와 닫힌리스트, 마지막리스트 초기화
        Pos_x = (int)res.Rand_Pos[res.Count_num - 1].x;
        Pos_z = (int)res.Rand_Pos[res.Count_num - 1].z;
        Rand_Pos   = Random.Range(0, res.Count_num - 2);
        Rand_Pos_x = (int)res.Rand_Pos[Rand_Pos].x;
        Rand_Pos_z = (int)res.Rand_Pos[Rand_Pos].z;

        Debug.Log(Pos_z + "," + Pos_x);
        Debug.Log(Rand_Pos_z + "," + Rand_Pos_x);
        StartNode = NodeArray[Pos_z, Pos_x];
        TargetNode = NodeArray[Rand_Pos_z, Rand_Pos_x];

        OpenList = new List<Node>() { StartNode };
        ClosedList = new List<Node>();
        FinalNodeList = new List<Node>();

        while (OpenList.Count > 0)
        {
            // 열린리스트 중 가장 F가 작고 F가 같다면 H가 작은 걸 현재노드로 하고 열린리스트에서 닫힌리스트로 옮기기
            CurNode = OpenList[0];
            for (int i = 1; i < OpenList.Count; i++)
                if (OpenList[i].F <= CurNode.F && OpenList[i].H < CurNode.H) CurNode = OpenList[i];

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

                for (int i = 0; i < FinalNodeList.Count; i++) print(i + "번째는 " + FinalNodeList[i].x + ", " + FinalNodeList[i].y);
                return;
            }

            // ↑ → ↓ ←
            OpenListAdd(CurNode.x, CurNode.y + 1);
            OpenListAdd(CurNode.x + 1, CurNode.y);
            OpenListAdd(CurNode.x, CurNode.y - 1);
            OpenListAdd(CurNode.x - 1, CurNode.y);
        }
    }
    void OpenListAdd(int checkX, int checkY)
    {
        // 상하좌우 범위를 벗어나지 않고, 벽이 아니면서, 닫힌리스트에 없다면
        if (checkX >= bottomLeft.x && checkX < topRight.x + 1 && checkY >= bottomLeft.z && checkY < topRight.z + 1 && !NodeArray[checkX - (int)bottomLeft.x, checkY - (int)bottomLeft.z].isWall && !ClosedList.Contains(NodeArray[checkX - (int)bottomLeft.x, checkY - (int)bottomLeft.z]))
        {
            // 이웃노드에 넣고, 직선은 10, 대각선은 14비용
            Node NeighborNode = NodeArray[checkX - (int)bottomLeft.x, checkY - (int)bottomLeft.z];
            int MoveCost = CurNode.G + (CurNode.x - checkX == 0 || CurNode.y - checkY == 0 ? 10 : 14);


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
                Gizmos.DrawLine(new Vector3(FinalNodeList[i].x, 25, FinalNodeList[i].y), new Vector3(FinalNodeList[i + 1].x, 25, FinalNodeList[i + 1].y));
    }
}
