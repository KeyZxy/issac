using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LarryJrHead : MonoBehaviour
{
    SnakeManager parent;
    bool canBombDamage;
    void Start()
    {
        parent = transform.parent.GetComponent<SnakeManager>();
        canBombDamage = true;
    }

    /// <summary>
    /// ����, player �浹
    /// 
    /// 1. �θ� rigidbody�� ����
    /// 2. �ڽ��� �浹�ص� ���� �浹ó���� �ذ� ����
    /// 
    /// </summary>

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Larry �� + �Ӹ� , ���� �浹
        if (collision.gameObject.CompareTag("Tears") || collision.gameObject.CompareTag("Knife"))
        {
            parent.getDamageLarry();
        }
        if (collision.gameObject.CompareTag("AttackBomb")) 
        {
            if (canBombDamage) 
            {
                // �浹�� ������Ʈ (��ź)�� �������� ������
                float damage;
                damage = collision.gameObject.GetComponent<PutBomb>().retunbossBombDamage();
                parent.getBombDamage(damage);

                // ��ø�������� �� �ް�
                canBombDamage = false;
                StartCoroutine("chageCanBombDamage");

            }
        }
    }

    IEnumerator chageCanBombDamage() 
    {
        yield return new WaitForSeconds(1f);
        canBombDamage = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //�÷��̾�� �浹 ���� �� (�ڽ�����)
        if (collision.gameObject.CompareTag("Player"))
        {
            parent.hitDamagePlayer();
        }
    }


}
