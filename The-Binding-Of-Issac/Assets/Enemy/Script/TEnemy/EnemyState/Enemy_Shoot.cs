using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Shoot : FSM<TEnemy>
{
    [SerializeField] TEnemy e_Owner;


    //������ �ʱ�ȭ
    public Enemy_Shoot(TEnemy _ower)
    {
        e_Owner = _ower;
    }

    public override void Enter()
    {
        //Debug.Log(e_Owner.gameObject.tag + " : Shoot ���� ");
        e_Owner.eCurState = TENEMY_STATE.Shoot;          // ���� ���¸� TENEMY_STATE�� Tracking���� 

        e_Owner.setIsReadyShoot = true;
    }

    public override void Excute()
    {
        e_Owner.e_Shoot();                                          // �Ѿ� ���

        if (e_Owner.e_isDead())                                     // ���Ͱ� ������ 
        {
            e_Owner.ChageFSM(TENEMY_STATE.Die);                     // Die�� ���º�ȭ 
        }

        if (e_Owner.e_SearchingPlayer())                            // �ѽ� �� -> �÷��̾ ���� �ȿ� ������
        {
            e_Owner.ChageFSM(TENEMY_STATE.Tracking);                // tracking
        }
        else if (!e_Owner.e_SearchingPlayer())                      // �ѽ� �� -> �÷��̾ ���� ��
        { 
            e_Owner.ChageFSM(TENEMY_STATE.Prowl);                   // prowl
        }
    }

    public override void Exit()
    {
        e_Owner.ePreState = TENEMY_STATE.Shoot;
    }


}
