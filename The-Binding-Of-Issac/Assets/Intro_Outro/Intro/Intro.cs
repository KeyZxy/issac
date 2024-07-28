using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Intro : MonoBehaviour
{
    void Update()
    {
        // ���� ���� 01_Intro �϶�
        if(SceneManager.GetActiveScene().name == "01_Intro")
        {
            // EnterŰ ��������
            if(Input.GetKeyDown(KeyCode.Return))
            {
                SceneManager.LoadScene("02_Game");
            }
        }
    }
}
