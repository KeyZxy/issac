using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadMachine<T>
{
    /// <summary>
    /// 1. FSM�� ��� ���� �Լ����� �����ϱ� ���ؼ�
    /// 2. ���� ��ȯ�� �ϴ� �޼��带 ��Ƶ� ����!
    /// 3. ������ Enemy���� �ص� �Ǵµ� ���� �� �ϱ� ���ؼ� �Ϻη� ���� ��������?
    /// </summary>

    // T Ÿ�� ���� (T�� Enemy Ÿ�� �̸� , Enemy Ÿ���� ���� ����)
    private T Owner;

    // ���� 
    // ���� ���ư��� �ִ� Enemy ���� (Idle , Tracking , Prowl , Die ��ũ��Ʈ(����)) 
    private FSM<TEnemy> currState = null; // ���� ����
    private FSM<TEnemy> prestate = null; // ���� ����

    // FSM�� Enter���� , ó�� ���� ����
    public void H_Enter() // Enemy_HeadMachine���� ���ư��� �޼���
    {
        if (currState != null) // ���ư��� ���°� ������
        {
            currState.Enter();  // �ش� ���� (Enter)����
                                // �ش� ��ũ��Ʈ (Ŭ����)���� Enter ����
                                // FSM�� ��ӹް�, �߻�ȭ Ŭ������ �����س��� �� �޼���
        }
    }

    // FSM�� Excute ���� , ���� ������Ʈ
    public void H_Excute()
    {
        CheckState();
    }

    // �����ϱ��� ���� �˻�
    public void CheckState()
    {
        if (currState != null) // ���ư��� ���°� ������
        {
            currState.Excute(); // �ش� ���� (Excute)����
                                // �ش� ��ũ��Ʈ (Ŭ����)���� Excute ����
                                // FSM�� ��ӹް�, �߻�ȭ Ŭ������ �����س��� �� �޼���
        }
    }

    // FSM�� Exit ����, ���� ����
    public void H_Exit()
    {
        currState.Exit(); // �ش� ���� (Exit)����
                          // �ش� ��ũ��Ʈ (Ŭ����)���� Exit ����
                          // FSM�� ��ӹް�, �߻�ȭ Ŭ������ �����س��� �� �޼���
        currState = null;
        prestate = null;
    }

    // FSM �ȿ��� state �ٲٱ�
    public void Chage(FSM<TEnemy> _eState)
    {
        //_eState : enemy�� ���¸� ��Ÿ���� ��ũ��Ʈ�� �Ű�������
        //(Idle , Tracking , Prowl , Die ��ũ��Ʈ(����)) 

        // ���� �����̸� return (ex) idle�ε� idle�� �ٲٷ���?
        if (_eState == currState)
            return;

        prestate = currState;
        // ���� ���°� �ִٸ� ���� ��
        if (currState != null)
            currState.Exit(); //���� ���� ��ũ��Ʈ�� exit �޼��� ���� (���� ����������)

        currState = _eState; //���� ���¸� ���� ���·� ����
        // ���� ����� ���¸� ����
        if (currState != null)
            currState.Enter(); // ���� ���� ������ Enter ���� (�� ���� ����)
    }

    // ���°� ���� (������, ���°� ������ ���� �ϰ� , Idle,Attack ��ũ��Ʈ�� ���� �ؾ��� ��?)
    public void Setstate(FSM<TEnemy> _state, T _owner)
    {
        Owner = _owner;
        currState = _state;

        if (currState != _state && currState != null)
            prestate = currState;


    }
}
