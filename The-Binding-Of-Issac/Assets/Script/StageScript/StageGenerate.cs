using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageGenerate : MonoBehaviour
{
    int[] dy = new int[4] { -1, 0, 1, 0 };
    int[] dx = new int[4] { 0, 1, 0, -1 };

    // 1: ���۹�  2:�Ϲݹ�  3:������  4:������  5:Ȳ�ݹ�  6:���ֹ�
    public int[,] stageArr;
    public bool CreateStage(int size, int min)
    {
        stageArr = new int[size, size]; // �������� ������ 2���� �迭�� ����

        if (CreateStructure(size, min)) // ���� ����
        {
            // ���� ������ ���������� �÷��̿� �ʿ��� ����� ����.
            if (SelectRoom(size))
            {
                return true;
            }
        }
        return false;
    }

    private bool SelectRoom(int size)
    {
        int roomNum = 3;

        List<KeyValuePair<int, int>> temp = new List<KeyValuePair<int, int>>();

        // ��� ���� Ž��
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                // ����ִ¹� �Ǵ� ���۹��� ����
                if (stageArr[i, j] == 0 || stageArr[i, j] == 1)
                    continue;

                // ����ִ¹�� ���۹��� ������ ����������� �����ѹ��� ����
                int adj = CheckAbjCount(i, j, size);
                // ������ ���� ������1���϶�
                if (adj == 1)
                    temp.Add(new KeyValuePair<int, int>(i, j)); 
            }
        }

        // ������ ���� ������ 1���� ���� 4�� �̻��϶�
        if (temp.Count >= 4)
        {
            // Ȳ�ݹ�,������,������,���ֹ��� ����
            for (int i = 0; i < 4; i++)
            {
                // ���ֹ� ����
                if (i == 3)
                {
                    // ���� 50%Ȯ���� ���� �Ǵ� ����X
                    int r = Random.Range(0, 2);
                    if (r == 0)
                        continue;
                }

                // ������ ���� ������1���� ����� ������ �����
                // ������ ������ Ȳ�ݹ� ���ֹ� ������� ����.
                int rd = Random.Range(0, temp.Count);
                stageArr[temp[rd].Key, temp[rd].Value] = roomNum;
                roomNum++;
                temp.RemoveAt(rd);
            }
            return true;
        }

        // ������ ���� ������ 1���� ���� 3�� ���ϸ� ���� ���� ����.
        return false;
    }

    bool CreateStructure(int size, int min)
    {
        int roomCount = 1; // ����  ������ �� ���� 
        int midY = size / 2; // �߾� ��ġ ( ���۹� )
        int midX = size / 2; // �߾� ��ġ ( ���۹� )

        stageArr[midY, midX] = 1; // ���������� �߾��� ���۹����� ����
        Queue<KeyValuePair<int, int>> q = new Queue<KeyValuePair<int, int>>(); // �ʺ� Ž������ ������ �����ϱ����� Queue
        q.Enqueue(new KeyValuePair<int, int>(midY, midX)); // ���۹���� Ž���� �����ϱ� ���� ���۹� ��ġ push
        while (q.Count != 0)
        {
            KeyValuePair<int, int> qFront = q.Dequeue();

            int y = qFront.Key; // ������ġ y
            int x = qFront.Value; // ������ġ x

            for (int i = 0; i < 4; i++) // �����¿�
            {
                int ny = y + dy[i]; // ������ġ y
                int nx = x + dx[i]; // ������ġ x

                if (ny < 0 || nx < 0 || ny >= size || nx >= size) // ���� �������
                    continue;

                if (stageArr[ny, nx] == 0) // ���� �������� ���� ���϶�
                {
                    int adjCnt = CheckAbjCount(ny, nx, size); // ny nx ���̶� �������ִ� ���� ����
                    if (adjCnt >= 2) // �������ִ� ���� ������ 2�� �̻��϶�
                        continue;  // pass

                    // �������ִ� ���� ������ 1�� �����϶�
                    int rd = (Random.Range(0, 3));
                    if (rd == 0) 
                        continue;

                    stageArr[ny, nx] = 2; // ny nx ���� �Ϲݹ����� ����
                    roomCount++; // ������ �� ���� ++
                    q.Enqueue(new KeyValuePair<int, int>(ny, nx));  // ���� Ž���� ���� push
                }
            }
        }

        // ���� ������ �Ϸ��Ͽ�����
        // ������ ���� ������ �ּҹ氳���� �Ѿ����.
        if (roomCount >= min)
            return true;
        return false;
    }
    
    private int CheckAbjCount(int y, int x, int size)
    {
        int ret = 0;

        for (int i = 0; i < 4; i++) // �����¿� Ž��
        {
            int ny = y + dy[i]; 
            int nx = x + dx[i];

            if (ny < 0 || nx < 0 || ny >= size || nx >= size) // ���� ������� x
                continue;

            if (stageArr[ny, nx] == 0) // ����ִ¹��϶� 
                continue;

            // �������� ���� ���� �ְ�, ���� �����Ҷ� ++
            ret++;
        }

        return ret; // �������մ� �� ���� ����.
    }
}
