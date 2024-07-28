using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Prowl : FSM<TEnemy>
{
    [SerializeField] TEnemy e_Owner;                          // ���� ����

    public Enemy_Prowl(TEnemy _ower)                        // ������ �ʱ�ȭ
    {
        e_Owner = _ower;
    }

    public override void Enter()                                // �ش� ���¸� ������ �� "1ȸ" ȣ��
    {
        //Debug.Log(e_Owner.gameObject.tag + " : Prowl ���� ");
        e_Owner.eCurState = TENEMY_STATE.Prowl;                 // ���� ���¸� TENEMY_STATE�� Prowl  

        e_Owner.e_moveInialize();                               // �������� �����̱� ���� �ʱⰪ ����
        e_Owner.startRaomPosiCoru();                            // ���� ��ġ ���� �ڷ�ƾ ����
    }

    public override void Excute()                               // �ش� ���¸� ������Ʈ �� �� "�� ������" ȣ��
    {
        e_Owner.e_Prwol();                                          // prowl 

        if (e_Owner.e_isDead())                                     // ���Ͱ� ������ 
        {
            e_Owner.ChageFSM(TENEMY_STATE.Die);                     // Die�� ���º�ȭ 
        }

        if (!e_Owner.getisDetective) 
        {
            return;
        }

                                                                    //���� ���� �ϴ� �ָ�?
        if (e_Owner.e_SearchingPlayer())                            // sight ���� �ȿ� ������
        {
            if (e_Owner.getIsTracking)                              // tracking �ϴ� �ָ�
            {
                e_Owner.ChageFSM(TENEMY_STATE.Tracking);            // tracking���� ��ȭ
            }
        }
    }

    public override void Exit()                              // �ش� ���¸� ������ �� "1ȸ" ȣ��
    {        
        e_Owner.ePreState = TENEMY_STATE.Prowl;              // �� ���¸� TENEMY_STATE�� Prowl
    }
}
