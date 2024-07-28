using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_Jump : FSM<TEnemy>
{
    [SerializeField] TEnemy e_Owner;                          // ���� ����
    float currTIme;
    float stateTime;

    public Enemy_Jump(TEnemy _ower)                       // ������ �ʱ�ȭ
    {
        e_Owner = _ower;
    }

    public override void Enter()
    {
        //Debug.Log("jump ���� ����");
        e_Owner.eCurState = TENEMY_STATE.Jump;              // ���� ���¸� TENEMY_STATE�� Tracking���� 
        e_Owner.e_jumpSet();                                // ���� �� ����

        stateTime = Random.Range(0.1f, 1.2f);
        currTIme = e_Owner.getattaackSpeed + stateTime;   // ���ݼӵ� ��������

    }

    public override void Excute()
    {
        e_Owner.e_Tracking(e_Owner.JumpSpeed);

        currTIme -= Time.deltaTime;
        if (currTIme <= 0) 
        {
            e_Owner.e_doneJump();
        }

        if (e_Owner.e_isDead())                                     // ���Ͱ� ������ 
        {
            e_Owner.ChageFSM(TENEMY_STATE.Die);                     // Die�� ���º�ȭ 
        }
    }

    public override void Exit()
    {
        e_Owner.ePreState = TENEMY_STATE.Jump;
    }

}
