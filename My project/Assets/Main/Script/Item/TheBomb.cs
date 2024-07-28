using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheBomb : MonoBehaviour
{
    // ը����ը�����µĺۼ��ľ���
    public Sprite traces;
    // ը������ʱ��
    float detonatingTime = 1.5f;
    // ը����ը��Ӱ��뾶
    float radius = 0.5f;

    // ��ʼ������
    void Start()
    {
        // ����ը������Э��
        StartCoroutine(Detonating(detonatingTime));
    }

    // ը��������Э��
    IEnumerator Detonating(float detonatingTime)
    {
        // ��ȡը���ľ�����Ⱦ�����
        SpriteRenderer SR = GetComponent<SpriteRenderer>();

        // ��˸��ʱ��������
        float[] flashCD = new float[] { 0f, 0.4f, 0.3f, 0.2f };
        // ������˸�ĳ���ʱ��
        float flashTime = 0.05f;
        // ��ǰ��˸ʱ����������
        int flashCDIndex = 0;

        // ������ʱ��
        float time = 0;
        // ��һ����˸��ʱ���
        float timeGate = flashCD[0];
        // ��ը������ʱ���ڷ���ִ��
        while (time < detonatingTime)
        {
            // �����������˸��ʱ����ҵ�ǰ��ɫΪ��ɫ
            if (time >= timeGate && SR.color == Color.white)
            {
                // ����ɫ����Ϊ��ɫ
                SR.color = Color.red;
                // ������һ����˸��ʱ���
                timeGate += flashTime;
                // ������˸�������
                if (flashCDIndex < flashCD.Length - 1)
                {
                    flashCDIndex++;
                }
            }
            // �����������˸��ʱ����ҵ�ǰ��ɫΪ��ɫ
            else if (time >= timeGate && SR.color == Color.red)
            {
                // ����ɫ����Ϊ��ɫ
                SR.color = Color.white;
                // ������һ����˸��ʱ���
                timeGate = timeGate + flashCD[flashCDIndex] - flashTime;
            }

            // ���¾�����ʱ��
            time += Time.deltaTime;
            // �ȴ���һ֡
            yield return 0;
        }
        // ���ս���ɫ����Ϊ��ɫ
        SR.color = Color.white;
        // ���ñ�ը����
        Explosion();
    }

    // ��ը����
    void Explosion()
    {
        // ��ȡ��ը��Χ�ڵ�������ײ��
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius);
        // ����������ײ��
        foreach (var item in colliders)
        {
            // ���㱬ը���ķ���
            Vector2 force = (item.transform.position - transform.position).normalized;
            // �����ײ������ʵ���� IDestructible �ӿ�
            if (item.GetComponent<IDestructible>() != null)
            {
                // ���ٸö���
                item.GetComponent<IDestructible>().DestorySelf();
            }
            // �����ײ�����������
            else if (item.GetComponent<Player>())
            {
                // �������
                item.GetComponent<Player>().BeAttacked(2, force, 1.5f);
            }
            // �����ײ������ʵ���� IAttackable �ӿ�
            else if (item.GetComponent<IAttackable>() != null)
            {
                // �����ö���
                item.GetComponent<IAttackable>().BeAttacked(10, force, 1.5f);
            }
            // �����ײ�������и������
            else if (item.GetComponent<Rigidbody2D>())
            {
                // ʩ�ӱ�ը��
                item.GetComponent<Rigidbody2D>().AddForce(force * 10);
            }
        }
        // ����ը������������Լ��
        GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        // ����ը������ײ��
        GetComponent<Collider2D>().enabled = false;
        // ���ű�ը����
        //GetComponent<Animator>().Play("Explosion");
        // ���ɱ�ը�ۼ�
        //GameManager.Instance.level.manager.GenerateTraceInCurrentRoom(traces, transform.position);
        // �ӳ� 0.5 �������ը������
        Invoke("Destroy", 0.5f);
    }

    // ����ը������ķ���
    void Destroy()
    {
        Destroy(gameObject);
    }
}
