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

    // G : �������κ��� �̵��ߴ� �Ÿ�, H : |����|+|����| ��ֹ� �����Ͽ� ��ǥ������ �Ÿ�, F : G + H
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
        
        // NodeArray�� ũ�� �����ְ�, isWall, x, y ����
        sizeX = res.MAP.GetLength(1);
        sizeY = res.MAP.GetLength(0);
        bottomLeft = new Vector3(0, 0, sizeY);
        topRight   = new Vector3(sizeX, 0, 0);
        NodeArray  = new Node[sizeY, sizeX];

        for (int j = 0; j < sizeY; j++)
        {
            for (int i = 0; i < sizeX; i++)
            {
                bool isWall = false;
                if (res.MAP[j, i] != 0 && res.MAP[j,i] != 7 && res.MAP[j, i] != 8 && res.MAP[j, i] != 9)
                {
                    //Debug.Log("��ǥ" + j+","+i+"�� ���Դϴ�.");
                    isWall = true;
                }
                NodeArray[j, i] = new Node(isWall, j + (int)bottomLeft.y, i + (int)bottomLeft.x);
                //Debug.Log(NodeArray[j, i].isWall + "," + NodeArray[j, i].y + "," + NodeArray[j, i].x);
            }
        }

        // ���۰� �� ���, ��������Ʈ�� ��������Ʈ, ����������Ʈ �ʱ�ȭ
        Pos_x = (int)res.Rand_Pos[res.Count_num - 1].x;
        Pos_z = (int)res.Rand_Pos[res.Count_num - 1].z;
        Rand_Pos = Random.Range(0, res.Count_num - 2);
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
            // ��������Ʈ �� ���� F�� �۰� F�� ���ٸ� H�� ���� �� ������� �ϰ� ��������Ʈ���� ��������Ʈ�� �ű��
            CurNode = OpenList[0];
            for (int i = 1; i < OpenList.Count; i++)
                if (OpenList[i].F <= CurNode.F && OpenList[i].H < CurNode.H)
                {
                    CurNode = OpenList[i];
                }
            OpenList.Remove(CurNode);
            ClosedList.Add(CurNode);

            // ������
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
                    Debug.Log(i + "��°�� " + FinalNodeList[i].y + ", " + FinalNodeList[i].x);
                return;
            }

            // �� �� �� ��
            OpenListAdd(CurNode.x, CurNode.y + 1);
            OpenListAdd(CurNode.x + 1, CurNode.y);
            OpenListAdd(CurNode.x, CurNode.y - 1);
            OpenListAdd(CurNode.x - 1, CurNode.y);
        }
    }
    //checkx,y : ���� �� x,y��ǥ +1 -1
    void OpenListAdd(int checkX, int checkY)
    {
        // �����¿� ������ ����� �ʰ�, ���� �ƴϸ鼭, ��������Ʈ�� ���ٸ�
        if (checkX >= bottomLeft.x && checkX < topRight.x + 1 && checkY < bottomLeft.z + 1 && checkY >= topRight.z && !NodeArray[checkY, checkX].isWall && !ClosedList.Contains(NodeArray[checkY, checkX]))
        {
            // �̿���忡 �ְ�, ������ 10, �밢���� 14���
            Debug.Log(checkY + "," + checkX);
            Node NeighborNode = NodeArray[checkY, checkX];
            int MoveCost = CurNode.G + 10;

            // �̵������ �̿����G���� �۰ų� �Ǵ� ��������Ʈ�� �̿���尡 ���ٸ� G, H, ParentNode�� ���� �� ��������Ʈ�� �߰�
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
}
