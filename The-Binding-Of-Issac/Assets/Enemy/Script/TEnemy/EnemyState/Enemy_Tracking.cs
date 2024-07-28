using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Tracking : FSM<TEnemy>
{
    [SerializeField] TEnemy e_Owner;                          // ���� ����
    bool isIsInvoke;

    public Enemy_Tracking(TEnemy _ower)                       // ������ �ʱ�ȭ
    {
        e_Owner = _ower;
    }

    public override void Enter()                            // �ش� ���¸� ������ �� "1ȸ" ȣ��
    {
        //Debug.Log(e_Owner.gameObject.tag + " : Tracking ���� ");
        e_Owner.eCurState = TENEMY_STATE.Tracking;          // ���� ���¸� TENEMY_STATE�� Tracking���� 

        isIsInvoke = true;
    }

    public override void Excute()                                   // �ش� ���¸� ������Ʈ �� �� "�� ������" ȣ��
    {
        e_Owner.e_findPlayer();                                     // player Ž��
        e_Owner.e_Tracking(e_Owner.getMoveSpeed);                   // tracking
        e_Owner.e_Lookplayer();                                     // �÷��̾� look
       
        if (e_Owner.playerPosi == null)
            return;

        if (e_Owner.e_isDead())                                     // ���Ͱ� ������ 
        {
            e_Owner.ChageFSM(TENEMY_STATE.Die);                     // Die�� ���º�ȭ 
        }

        // ���� ���� x
        if (!e_Owner.getisDetective) 
        {
            if (!e_Owner.getisJump)                            // ������ �ϴ� �ָ�? 
            {
                return;
            }

            if (isIsInvoke)
            {
                e_Owner.invokeJump();                          // ���� �ð��� ������ �Ѿ
                isIsInvoke = false;
            }
        }

        // ���� ���� �ϴ¾ָ�?
        // -> �� ����� �ƴ����� ���� �޶���
        else                             
        {
            
            if (e_Owner.e_SearchingPlayer())                         // sight ���� �ȿ� ������
            {
                if (!e_Owner.getisShoot)                              
                {
                    return;
                }

                if (isIsInvoke)                                     // �� ��� �ָ�?
                {
                    e_Owner.invokeShoot();                          // ���� �ð��� �� ���� �Ѿ
                    isIsInvoke = false;
                }

                return;
            }

            // �����ȿ� ������
            e_Owner.ChageFSM(TENEMY_STATE.Prowl);               // prowl�� ���� ��ȭ
            
        }

    }

    public override void Exit()                              // �ش� ���¸� ������ �� "1ȸ" ȣ��
    {
        e_Owner.ePreState = TENEMY_STATE.Tracking;              // �� ���¸� TENEMY_STATE�� Tracking
    }





}
