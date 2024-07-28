using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextStageDoor : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            if(GameManager.instance.stageLevel >= GameManager.instance.maxStage) // ���� ���������� 4�϶� ( ������ ���������϶� )
            {
                SceneManager.LoadScene("03_Outro"); // �ƿ�Ʈ�� ���.
            }
            else
            {
                GameManager.instance.NextStage();
            }
        }
    }
}
