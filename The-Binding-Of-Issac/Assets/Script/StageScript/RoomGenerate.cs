using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerate : MonoBehaviour
{
    int[] dy = new int[4] { -1, 0, 1, 0 };
    int[] dx = new int[4] { 0, 1, 0, -1 };

    List<GameObject> doors; // ������ �� ������Ʈ��

    public GameObject[,] roomList; // ������ �� ������Ʈ��
    
    public GameObject[,] roomPrefabs; // ������� ����� ������

    public List<GameObject> itemList; // ������ ������ ������Ʈ��

    [Header("Unity Setup")]
    public EnemyGenerate enemyGenerate;
    public RoomPattern pattern; // ������Ʈ ����
    public Transform roomPool; // �� �Ѱ��� ��Ƶ� ������Ʈ
    public GameObject[] rooms; // room prefabs�� ������������ ����� ������Ʈ
    public GameObject[] objectPrefabs; // ������� ������Ʈ �����Ҷ� ����� �����յ�
    public GameObject[] doorPrefabs;   // ������� �� �����Ҷ� ����� �����յ�

    [Header("Pooling")]
    // �� ������Ʈ
    public Transform obstaclePool_Transform;
    private Stack<GameObject> rockPool = new Stack<GameObject>();
    private Stack<GameObject> poopPool = new Stack<GameObject>();
    private Stack<GameObject> firePool = new Stack<GameObject>();
    private Stack<GameObject> spikePool = new Stack<GameObject>();
    public Stack<GameObject> RockPool { get => rockPool; }
    public Stack<GameObject> PoopPool { get => poopPool; }
    public Stack<GameObject> FirePool { get => firePool; }
    public Stack<GameObject> SpikePool { get => spikePool; }

    // ���� 
    public Transform chestPool_Transform;
    Stack<GameObject> normalChestPool = new Stack<GameObject>();
    Stack<GameObject> goldChestPool = new Stack<GameObject>();
    Stack<GameObject> curseChestPool = new Stack<GameObject>();
    public Stack<GameObject> NormalChestPool { get => normalChestPool; }
    public Stack<GameObject> GoldChestPool { get => goldChestPool; }
    public Stack<GameObject> CurseChestPool { get => curseChestPool; }


    // ����/Ȳ�ݹ� ���� ���̺�
    public Transform shopTable_Transform;
    Stack<GameObject> shopTablePool = new Stack<GameObject>();

    public Transform goldTable_Transform;
    Stack<GameObject> goldTablePool = new Stack<GameObject>();

    public void SetObjectPooling()
    {
        rockPool = new Stack<GameObject>();
        poopPool = new Stack<GameObject>();
        firePool = new Stack<GameObject>();
        spikePool = new Stack<GameObject>();
        normalChestPool = new Stack<GameObject>();
        goldChestPool = new Stack<GameObject>();
        goldTablePool = new Stack<GameObject>();
        shopTablePool = new Stack<GameObject>();
        curseChestPool = new Stack<GameObject>();

        for (int i = 0; i < 10; i++)
        {
            // ������Ʈ ����
            GameObject rock = Instantiate(objectPrefabs[0], obstaclePool_Transform.position, Quaternion.identity);
            GameObject poop = Instantiate(objectPrefabs[1], obstaclePool_Transform.position, Quaternion.identity);
            GameObject fire = Instantiate(objectPrefabs[2], obstaclePool_Transform.position, Quaternion.identity); 
            GameObject spike = Instantiate(objectPrefabs[3], obstaclePool_Transform.position, Quaternion.identity);

            GameObject shopTable = Instantiate(objectPrefabs[5], shopTable_Transform.position, Quaternion.identity);
            GameObject goldTable = Instantiate(objectPrefabs[6], goldTable_Transform.position, Quaternion.identity);

            GameObject normalChest = Instantiate(objectPrefabs[7], chestPool_Transform.position, Quaternion.identity);
            GameObject goldChest = Instantiate(objectPrefabs[8], chestPool_Transform.position, Quaternion.identity);
            GameObject curseChest = Instantiate(objectPrefabs[10], chestPool_Transform.position, Quaternion.identity);

            // ������Ʈ Ǯ ����ֱ�
            rockPool.Push(rock);
            poopPool.Push(poop);
            firePool.Push(fire);
            spikePool.Push(spike);

            shopTablePool.Push(shopTable);
            goldTablePool.Push(goldTable);

            normalChestPool.Push(normalChest);
            goldChestPool.Push(goldChest);
            curseChestPool.Push(curseChest);

            // ������Ʈ �Ѱ��� ��Ƶα�.
            rock.transform.SetParent(obstaclePool_Transform);
            poop.transform.SetParent(obstaclePool_Transform);
            fire.transform.SetParent(obstaclePool_Transform);
            spike.transform.SetParent(obstaclePool_Transform);

            shopTable.transform.SetParent(shopTable_Transform);
            goldTable.transform.SetParent(goldTable_Transform);

            normalChest.transform.SetParent(chestPool_Transform);
            goldChest.transform.SetParent(chestPool_Transform);
            curseChest.transform.SetParent(chestPool_Transform);

            // ���� ������ ���� SFXObject�� �־��ֱ� // �� �κ� �۵�����.
            SetSFXObject(rock);
            SetSFXObject(poop);
            SetSFXObject(fire);
            SetSFXObject(spike);

            SetSFXObject(shopTable); // ���� ���� ������Ʈ �Դϴ�.
            SetSFXObject(goldTable); // ���� ���� ������Ʈ�Դϴ�.

            SetSFXObject(normalChest);
            SetSFXObject(goldChest);
            SetSFXObject(curseChest);

            rock.SetActive(false);
            poop.SetActive(false);
            fire.SetActive(false);
            spike.SetActive(false);

            shopTable.SetActive(false);
            goldTable.SetActive(false);

            normalChest.SetActive(false);
            goldChest.SetActive(false);
            curseChest.SetActive(false);
        }
    }

    GameObject GetObstacle(int num)
    {
        switch(num)
        {
            #region ��
            case 0: // �� 
                if(rockPool.Count == 0) // ������ƮǮ�� ������Ʈ�� ������
                {
                    // ������Ʈ �����ؼ� ����
                    GameObject rock = Instantiate(objectPrefabs[0], obstaclePool_Transform.position, Quaternion.identity);
                    rockPool.Push(rock);
                    rock.transform.SetParent(obstaclePool_Transform);
                    SetSFXObject(rock);
                }
                GameObject rockObj = rockPool.Pop();
                rockObj.SetActive(true);
                return rockObj;
            #endregion

            #region ��
            case 1: // ��
                if (poopPool.Count == 0) // ������ƮǮ�� ������Ʈ�� ������
                {
                    // ������Ʈ �����ؼ� ����
                    GameObject poop = Instantiate(objectPrefabs[1], obstaclePool_Transform.position, Quaternion.identity);
                    poopPool.Push(poop);
                    poop.transform.SetParent(obstaclePool_Transform);
                    SetSFXObject(poop);
                }
                GameObject poopObj = poopPool.Pop();
                poopObj.SetActive(true);
                return poopObj;
            #endregion

            #region ��
            case 2: // ��
                if (firePool.Count == 0) // ������ƮǮ�� ������Ʈ�� ������
                {
                    // ������Ʈ �����ؼ� ����
                    GameObject fire = Instantiate(objectPrefabs[2], obstaclePool_Transform.position, Quaternion.identity);
                    firePool.Push(fire);
                    fire.transform.SetParent(obstaclePool_Transform);
                    SetSFXObject(fire);
                }
                GameObject fireObj = firePool.Pop();
                fireObj.SetActive(true);
                return fireObj;
            #endregion

            #region ����
            case 3: // ����
                if (spikePool.Count == 0) // ������ƮǮ�� ������Ʈ�� ������
                {
                    // ������Ʈ �����ؼ� ����
                    GameObject spike = Instantiate(objectPrefabs[3], obstaclePool_Transform.position, Quaternion.identity);
                    spikePool.Push(spike);
                    spike.transform.SetParent(obstaclePool_Transform);
                    SetSFXObject(spike);
                }
                GameObject spikeObj = spikePool.Pop();
                spikeObj.SetActive(true);
                return spikeObj;
            #endregion

            #region ������ ���������̺�
            case 5:
                if (shopTablePool.Count == 0)
                {
                    GameObject shopTable = Instantiate(objectPrefabs[6], shopTable_Transform.position, Quaternion.identity);
                    shopTablePool.Push(shopTable);
                    shopTable.transform.SetParent(shopTable_Transform);
                    SetSFXObject(shopTable); // ���尡���� ������Ʈ�Դϴ�.
                }
                GameObject shopTableObj = shopTablePool.Pop();
                shopTableObj.SetActive(true);
                return shopTableObj;
            #endregion

            #region Ȳ�ݹ� �������̺�
            case 6:
                if(goldTablePool.Count == 0)
                {
                    GameObject goldTable = Instantiate(objectPrefabs[6], goldTable_Transform.position, Quaternion.identity);
                    goldTablePool.Push(goldTable);
                    goldTable.transform.SetParent(goldTable_Transform);
                    SetSFXObject(goldTable); // ���尡���� ������Ʈ�Դϴ�.
                }
                GameObject goldTableObj = goldTablePool.Pop();
                goldTableObj.SetActive(true);
                return goldTableObj;
                #endregion

            #region �Ϲݻ���
            case 7:
                if(normalChestPool.Count == 0)
                {
                    GameObject normalChest = Instantiate(objectPrefabs[7], chestPool_Transform.position, Quaternion.identity);
                    normalChestPool.Push(normalChest);
                    normalChest.transform.SetParent(chestPool_Transform);
                    SetSFXObject(normalChest);
                }
                GameObject normalChestObj = normalChestPool.Pop();
                normalChestObj.SetActive(true);
                return normalChestObj;
            #endregion

            #region Ȳ�ݻ���
            case 8:
                if (goldChestPool.Count == 0)
                {
                    GameObject goldChest = Instantiate(objectPrefabs[8], chestPool_Transform.position, Quaternion.identity);
                    goldChestPool.Push(goldChest);
                    goldChest.transform.SetParent(chestPool_Transform);
                    SetSFXObject(goldChest);
                }
                GameObject goldChestObj = goldChestPool.Pop();
                goldChestObj.SetActive(true);
                return goldChestObj;
            #endregion

            #region ���ֹ����
            case 10:
                if (curseChestPool.Count == 0)
                {
                    GameObject curseChest = Instantiate(objectPrefabs[10], chestPool_Transform.position, Quaternion.identity);
                    curseChestPool.Push(curseChest);
                    curseChest.transform.SetParent(chestPool_Transform);
                    SetSFXObject(curseChest);
                }
                GameObject curseChestObj = curseChestPool.Pop();
                curseChestObj.SetActive(true);
                return curseChestObj;
                #endregion
        }
        return null;
    }

    void AllReturnObject()
    {
        // �� �� �� ����
        for(int i = 0; i < obstaclePool_Transform.childCount; i++)
        {
            GameObject obj = obstaclePool_Transform.GetChild(i).gameObject;
            if (obj.activeSelf)
            {
                obj.GetComponent<Obstacle>().ResetObject();
                obj.GetComponent<Obstacle>().Returnobject(); ;
            }
        }

        // �Ϲݻ���/ Ȳ�ݻ���/ ���ֹ����
        for (int i = 0; i < chestPool_Transform.childCount; i++)
        {
            GameObject obj = chestPool_Transform.GetChild(i).gameObject;
            if (obj.activeSelf)
            {
                obj.GetComponent<Chest>().ResetObject();
                obj.GetComponent<Chest>().Returnobject();
            }
        }


        // ������ ������ ���̺� 5
        for (int i = 0; i < shopTable_Transform.childCount; i++)
        {
            GameObject obj = shopTable_Transform.GetChild(i).gameObject;
            if (obj.activeSelf)
            {
                obj.GetComponent<ShopTable>().ResetObject();
                shopTablePool.Push(obj);
            }
        }

        // Ȳ�ݹ� �������̺� 6
        for (int i = 0; i < goldTable_Transform.childCount; i++)
        {
            GameObject obj = goldTable_Transform.GetChild(i).gameObject;
            if (obj.activeSelf)
            {
                obj.GetComponent<GoldTable>().ResetObject();
                goldTablePool.Push(obj);
            }
        }
    }

    public void SetPrefabs()
    {
        /* 
         * roomPrefabs[ �������� - 1 , ���ȣ - 1 ]
         * (-1 : ����X)  (0 : ���۹�)  (1 : �Ϲݹ�)  (2 : ������)  (3 : ������)  (4 : Ȳ�ݹ�) (5 : ���ֹ�)
         * 
         * doorPrefabs[]
         * (0 : �Ϲݹ�)  (1 : ������)  (2 : ������)  (3 : ����)  (4 : ���ֹ�)
         */
        int cnt = 0;
        roomPrefabs = new GameObject[4, 6];

        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                roomPrefabs[i, j] = rooms[cnt++];
            }
        }
    }
    public void ClearRoom()
    {
        roomList = null; // ������ �� ������Ʈ �迭 �ʱ�ȭ
        doors = new List<GameObject>(); // ������ �� ������Ʈ �迭 �ʱ�ȭ

        AllReturnObject(); // �� ������Ʈ ����

        // �����Ǿ��ִ� ��� ����� ����
        for(int i = 0; i < roomPool.childCount; i++)
        {
            Destroy(roomPool.GetChild(i).gameObject);
        }

        // ������ ������ ������Ʈ List �ʱ�ȭ
        // ������ ����Ʈ�� ���������
        if (itemList == null)
        {
            itemList = new List<GameObject>();
        }

        // ������ ����Ʈ�� �������������.
        else 
        {
            // �����Ǿ��ִ� �������������� ���� ���� �� �ʱ�ȭ
            for(int i = 0; i < itemList.Count; i++)
            {
                Destroy(itemList[i]);
                // ����������� ������Ʈ Ǯ��������Ѽ� �����ְ�,
                // �׿� ��Ƽ��,�нú�,��ű� �������� �����ؾ���.
            }
            itemList = new List<GameObject>();
        }

        // ������Ʈ Ǯ���� �����( ��� ������ ) �ʱ�ȭ
        ItemManager.instance.itemTable.AllReturnDropItem();
        
    }
    public void CreateRoom(int stage, int size)
    {
        roomList = new GameObject[size, size];

        Vector3 roomPos = new Vector3(0, 0, 0);
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                int roomNum = GameManager.instance.stageGenerate.stageArr[i, j]; // �������� �������� �� ��ȣ

                // ����ִ¹�
                if (roomNum == 0)
                {
                    roomPos += new Vector3(15, 0, 0);
                    continue;
                }
                // �׿�

                // �� ������Ʈ ����
                GameObject room = Instantiate(roomPrefabs[stage - 1, roomNum - 1], roomPos, Quaternion.identity) as GameObject; // �� ������Ʈ�� ����.
                SetSFXDestoryObject(room); // �ش� ���� Sound Manager�� SFXObject�� �߰�
                roomList[i, j] = room; 
                roomList[i, j].GetComponent<Room>().isClear = false; // �ش� ���� ��Ŭ���� ���·� ��ȯ

                // (4 ����) (5 Ȳ��) (6 ����) (7 ����) �� ��� �ٷ� Ŭ���� ���·� ��ȯ
                if (roomNum == 4 || roomNum == 5 || roomNum == 6 || roomNum == 1)
                    roomList[i, j].GetComponent<Room>().isClear = true;
                

                // �� ������Ʈ ����
                roomList[i, j].GetComponent<Room>().SetGrid(); //
                CreateObstacle(i, j, roomNum);
                room.transform.SetParent(roomPool);

                roomPos += new Vector3(15, 0, 0);
            }
            roomPos = new Vector3(0, roomPos.y, 0);
            roomPos += new Vector3(0, -10, 0);
        }

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                int roomNum = GameManager.instance.stageGenerate.stageArr[i, j];
                if (roomNum == 0)
                {
                    continue;
                }
                CreateDoor(i, j);
            }
        }
    }
    public void CreateDoor(int y, int x)
    {
        for (int i = 0; i < 4; i++)
        {
            int ny = y + dy[i];
            int nx = x + dx[i];

            // out of range
            if (ny < 0 || nx < 0 || ny >= GameManager.instance.stageSize || nx >= GameManager.instance.stageSize)
                continue;

            // nextRoom  Null 
            if (GameManager.instance.stageGenerate.stageArr[ny, nx] == 0)
                continue;

            // �� ����
            int doorNumder = ChoiceDoor(y,x,ny,nx);

            GameObject door = Instantiate(doorPrefabs[doorNumder]);
            door.transform.SetParent(roomList[y, x].GetComponent<Room>().doorPosition[i]);
            door.transform.localPosition = new Vector3(0, 0, 0);
            door.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));

            door.GetComponent<Door>().doorDir = i; // door dir

            door.GetComponent<Door>().roomInfo = roomList[y, x]; // door in Room Info
            door.GetComponent<Door>().movePosition = roomList[ny, nx].GetComponent<Room>().movePosition[i]; // door MovePosition 

            int roomNum = GameManager.instance.stageGenerate.stageArr[y, x];
            if (roomNum == 4 || roomNum == 5 || roomNum == 6)
                door.GetComponent<Door>().UsingKey();

            doors.Add(door);

            // ���� ���� ������.
            if (doorNumder == 4)
                door.GetComponent<Door>().doorDamage = 1;
            else
                door.GetComponent<Door>().doorDamage = 0;
        }
    }
    int ChoiceDoor(int y, int x, int ny, int nx)
    {
        int doorNum;
        int roomNum = GameManager.instance.stageGenerate.stageArr[y, x];
        int nextRoomNum = GameManager.instance.stageGenerate.stageArr[ny, nx];
        // ������� ����,����,Ȳ��,���� �϶�
        if (3 <= roomNum && roomNum <= 6)
        {
            doorNum = roomNum - 2;
        }
        // ������� ����, �Ϲ� �϶�
        else
        {
            // �������� ����,����,Ȳ��,���� �϶�
            if (3 <= nextRoomNum && nextRoomNum <= 6)
            {
                doorNum = nextRoomNum - 2;
            }
            else
            {
                doorNum = 0;
            }
        }

        return doorNum;
    }

    void CreateObstacle(int y, int x, int roomNumber)
    {
        int idx = 0;
        int[,] rdPattern = pattern.GetPattern(roomNumber);
        for (int i = 0; i < rdPattern.GetLength(0); i++)
        {
            for(int j = 0; j < rdPattern.GetLength(1); j++)
            {
                int pNum = rdPattern[i, j]; ;
                if (pNum == 0) // ��ĭ
                {
                    idx++;
                    continue;
                }

                else if (pNum == 10) // �÷��̾� ������Ʈ�϶�
                {
                    Transform pos = roomList[y, x].GetComponent<Room>().roomObjects[idx].transform;
                    GameManager.instance.playerObject.transform.position = pos.position;
                    GameManager.instance.miniMapPosition = pos;
                }

                else if (pNum == 5) // ���� ������Ʈ�϶�
                {
                    // ������ �Ϲݸ��͸� ��ȯ����.
                    GameObject enemy = enemyGenerate.GetEnemy();
                    enemy.transform.SetParent(roomList[y, x].GetComponent<Room>().roomObjects[idx]);
                    enemy.transform.localPosition = new Vector3(0, 0, 0);
                    enemy.GetComponent<TEnemy>().roomInfo = roomList[y, x];
                    roomList[y, x].GetComponent<Room>().enemis.Add(enemy); // �ش� ���� ���͸���Ʈ�� �߰�

                    // sfx ���� ������ ���� ������Ʈ ����
                    SetSFXDestoryObject(enemy);
                }

                else // �� �� �� ���� �����ۻ��� 3�� , Ȳ�ݹ�/������ ���� ���̺� , ������ ������ ���̺�
                {
                    GameObject obstacle = GetObstacle(pNum-1);
                    Vector3 pos = roomList[y, x].GetComponent<Room>().roomObjects[idx].position;
                    obstacle.transform.position = pos;

                    // Ȳ�ݹ� ���̺�
                    if(pNum == 7)
                    {
                        obstacle.GetComponent<GoldTable>().SetRoomInfo(roomList[y, x]);
                    }

                    // ������ ���̺�
                    else if(pNum == 6)
                    {
                        obstacle.GetComponent<ShopTable>().SetRoomInfo(roomList[y, x]);
                    }

                    // ���ֹ� ����
                    else if (pNum == 11)
                    {
                        obstacle.GetComponent<CurseChest>().SetRoomInfo(roomList[y, x]);
                    }
                }
                idx++;
            }
        }    
    }

    void SetSFXObject(GameObject obj)
    {
        // sfx ���� ������ ���� ������Ʈ ����
        if (obj.GetComponent<AudioSource>() != null)
        {
            SoundManager.instance.sfxObjects.Add(obj.GetComponent<AudioSource>());
        }
    }

    public void SetSFXDestoryObject(GameObject obj)
    {
        // sfx ���� ������ ���� ������Ʈ ����
        if (obj.GetComponent<AudioSource>() != null)
            SoundManager.instance.sfxDestoryObjects.Add(obj.GetComponent<AudioSource>());
    }

    private void Update()
    {
        if(doors != null)
        {
            for(int i = 0; i < doors.Count; i++)
            {
                doors[i].GetComponent<Door>().CheckedClear();
            }
        }
    }
}
