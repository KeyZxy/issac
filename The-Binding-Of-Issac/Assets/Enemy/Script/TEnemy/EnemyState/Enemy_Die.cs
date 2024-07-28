using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Die : FSM<TEnemy>
{
    [SerializeField] TEnemy e_Owner;

    public Enemy_Die(TEnemy _ower)
    {
        e_Owner = _ower;
    }

    public override void Enter()
    {
        //Debug.Log(e_Owner.gameObject.tag + " : DIE ���� ");
        e_Owner.eCurState = TENEMY_STATE.Die;          // ���� ���¸� TENEMY_STATE�� Die 
        
        e_Owner.e_destroyEnemy();
        
    }

    public override void Excute()
    {

    }

    public override void Exit()
    {

    }


}
