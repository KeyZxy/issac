using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    #region singleTon
    public static GameManager instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }
    #endregion

    public int stageLevel; // ���� �������� ����
    public int stageSize;  // �������� ũ��
    public int stageMinimunRoom; // �������� �� �ּҰ���
    public GameObject playerObject; // ������ �÷��̾�
    public int maxStage;

    public Transform miniMapPosition;

    [Header("Unity Setup")]
    public StageGenerate stageGenerate;
    public RoomGenerate roomGenerate;
    public GameObject myCamera;
    public GameObject miniMapCamera;



    [Header("reload")]
    [SerializeField] private float curTime;

    private void Start()
    {
        SetStage(1); // 1stage create
        roomGenerate.SetPrefabs(); // room Prefabs Setting
        roomGenerate.SetObjectPooling(); // set room object pool

        // ���� ���� ( �������� ���� )
        Invoke("StageStart", 0.3f);
    }

    void Update()
    {
#if TEST_MODE
        if (Input.GetKeyDown(KeyCode.P))
        {
            SceneManager.LoadScene("02_Game");
        }
#endif
        // �������� �����
        // RŰ ������ -> �������� ���۽÷� ���� �Ұ�. 
        if (Input.GetKey(KeyCode.R) && !UIManager.instance.LodingImage.activeSelf) 
        {
            curTime += Time.deltaTime;

            if (curTime >= 2f) // 2�ʰ� ������������
                SceneManager.LoadScene("02_Game"); // 1���������� ���ư��� ������մϴ�
            
            //UIManager.instance.OnLoading();
            //StageStart();
        }

        if(Input.GetKeyUp(KeyCode.R) && curTime <= 2.4f) 
        {
            curTime = 0;
        }
    }
    public void StageStart()
    {   
        // Create stage/room

        // ���� �÷��̾� ������Ʈ�� ������.
        if (playerObject == null)
        {
            GameObject obj = Instantiate(roomGenerate.objectPrefabs[9]) as GameObject; // �÷��̾ ����
            playerObject = obj; // playerObject �ʱ�ȭ
            
            // SoundManager�� �÷��̾� ���� ���� ������Ʈ �ʱ�ȭ
            SoundManager.instance.playerObject = playerObject.GetComponent<AudioSource>();
        }

        myCamera.transform.SetParent(null); // ī�޶��� ��ġ�� �ʱ�ȭ
        miniMapCamera.transform.SetParent(myCamera.transform); // ī�޶��� ��ġ�� �ʱ�ȭ
        miniMapCamera.GetComponent<MiniMapController>().initCamera();
        // ** �̺κ��� ������ �������� �����ɶ� ���������� ����� �����Ǹ鼭 ī�޶� ���� ������ϴ� **


        // int cnt = 25; // �� ���� ���� �Ѱ�ġ
        // ������� �����߻���
        // while cnt �� Ƚ������ �����! 
        while (true) 
        {
            if (stageGenerate.CreateStage(stageSize, stageMinimunRoom))
            {
                roomGenerate.ClearRoom(); // ���� �����Ǿ��ִ� �� / ������Ʈ / ���� ��� ���� �ʱ�ȭ
                SoundManager.instance.sfxDestoryObjects = new List<AudioSource>(); // soundManager�� sfxDestoryObjects �ʱ�ȭ.
                roomGenerate.CreateRoom(stageLevel, stageSize); // �� ����
                myCamera.transform.position = playerObject.transform.position;

                SoundManager.instance.OnStageBGM();
                SoundManager.instance.SFXInit();
                StartCoroutine(UIManager.instance.StageBanner(stageLevel));
                break;
            }
        }
    }

    public void NextStage()
    {
        SetStage(++stageLevel); // �������� ����
        UIManager.instance.OnLoading();
        StageStart(); // �������� ����
    }

    public void SetStage(int stage)
    {
        stageLevel = stage;
        switch(stageLevel)
        {
            case 1: // 1��������
                stageSize = 5;
                stageMinimunRoom = 8;
                break;
            case 2: // 2��������
                stageSize = 5;
                stageMinimunRoom = 10;
                break;
            case 3: // 3��������
                stageSize = 7;
                stageMinimunRoom = 12;
                break;
            case 4: // 4��������
                stageSize = 7;
                stageMinimunRoom = 14;
                break;
            default:
                stageSize = 5;
                stageMinimunRoom = 8;
                break;
        }
    }
}
