using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MiniMapController : MonoBehaviour
{
    [SerializeField] float miniMapScale;
    [SerializeField] int miniMapCameraSize;

    [SerializeField] Transform miniMapLockTransform;

    [Header("Unity Setup")]
    [SerializeField] RectTransform miniMapUI;
    [SerializeField] Camera miniMapCamera;
    [SerializeField] Transform mainCameraTransfrom;
    [SerializeField] GameObject timeStampObject; // �ð������� ������Ʈ on off
    [SerializeField] Text timeText; // ����ð� �ؽ�Ʈ

    private void Start()
    {
        initCamera();
        StartCoroutine(TimeCalculate());
    }

    public void initCamera()
    {
        miniMapScale = 1;
        miniMapCameraSize = 30;
        AdjustmentMiniMap();
        MiniMapLock(1);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SizeChanger();
            AdjustmentMiniMap();
            timeStampObject.SetActive(!timeStampObject.activeSelf);
        }
    }

    IEnumerator TimeCalculate()
    {
        int m = 0;
        int s = 0;
        while(true)
        {
            timeText.text = m.ToString("00") + " : " + s.ToString("00");
            yield return new WaitForSeconds(1f);
            s++;
            if(s >= 60)
            {
                s = 0;
                m++;
            }
        }
    }
    void SizeChanger()
    {
        // �⺻ ������ �϶� ->  Ŀ��
        if (miniMapScale <= 1)
        {
            miniMapScale = 2.5f;
            miniMapCameraSize = 70;
            MiniMapLock(2);
        }

        // Ŀ�� ������ �϶� -> �۾���
        else if (miniMapScale >= 2)
        {
            miniMapScale = 1;
            miniMapCameraSize = 30;
            MiniMapLock(1);
        }
    }
    void AdjustmentMiniMap()
    {
        miniMapUI.localScale = new Vector3(miniMapScale,miniMapScale,1);
        miniMapCamera.orthographicSize = miniMapCameraSize;
    }

    void MiniMapLock(int mode)
    {
        switch(mode)
        {
            // �۾�������
            case 1:
                miniMapCamera.transform.SetParent(mainCameraTransfrom);
                miniMapCamera.transform.localPosition = Vector3.zero;
                break;

            // Ŀ������
            case 2:
                if (miniMapLockTransform == null && GameManager.instance.miniMapPosition != null)
                {
                    miniMapLockTransform = GameManager.instance.miniMapPosition;
                }
                miniMapCamera.transform.SetParent(miniMapLockTransform);
                miniMapCamera.transform.localPosition = Vector3.zero;
                break;
        }
    }
}
