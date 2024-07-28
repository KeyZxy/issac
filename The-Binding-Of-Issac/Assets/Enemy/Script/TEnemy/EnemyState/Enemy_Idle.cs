using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Idle : FSM<TEnemy>
{
    /// <summary>
    /// 1. Enemy�� idle ���� ����
    /// 2. "Enemy_Idle" ��ũ��Ʈ�� ���� ��ų ��ũ��Ʈ ���� (TENemy�� ����)
    /// 
    /// * Enemy�� ��� �޴� Attackly �ö��̿��� Idle�� �����ߴٸ�?
    ///     - m_Owner�� Attackly
    ///     - ��� ���� Attackly�� Enemy Ÿ����!! (Enemy�� ��� �޾ұ� ����)
    /// </summary>
    [SerializeField] TEnemy e_Owner;                        // ���� ���� 

    /// <summary>
    /// 1. ������ 
    /// 2. ���� ��ũ��Ʈ���� this�� �Ѱ��� 
    /// 3. Enemy �ڽ� ��ũ��Ʈ���� init�� �ϸ� this �� �ڽ� ������Ʈ�ϱ� �װ� �ѱ���� ?!
    /// </summary>

    public Enemy_Idle(TEnemy _ower)                          // ������ �ʱ�ȭ
    {
        e_Owner = _ower;  
    }
                                    
    public override void Enter()                            // �ش� ���¸� ������ �� "1ȸ" ȣ��
    {
        //Debug.Log(e_Owner.gameObject.tag + " : idle ���� ");
        e_Owner.eCurState = TENEMY_STATE.Idle;              // ���� ���¸� TENEMY_STATE�� idle��
    }


    public override void Excute()                               // �ش� ���¸� ������Ʈ �� �� "�� ������" ȣ��
    {
        if (e_Owner.playerInRoom)                               // �÷��̾ �� �ȿ� ������ ���º�ȯ
        {
            if (e_Owner.getIsTracking)                          // tracking�� �ϴ�
            {
                e_Owner.ChageFSM(TENEMY_STATE.Tracking);        // tracking ���� ���� ��ȯ
            }
            //if (!e_Owner.getIsTracking && e_Owner.getisProwl)   // prowl �� �ϸ�?
            else
            {
                e_Owner.ChageFSM(TENEMY_STATE.Prowl);           // prowl ���� ���� ��ȯ
            }
            
        }

    }

    public override void Exit()                             // �ش� ���¸� ������ �� "1ȸ" ȣ��
    {
        e_Owner.ePreState = TENEMY_STATE.Idle;              // �� ���¸� TENEMY_STATE�� idle��
    }

    
}


