using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FSM<T>
{
    // ���°� ��� ���� �θ� ( �ش� ���´� Begine , Run, Exit ���¸� �� ��������)

    /// <summary>
    /// �ش� ���¸� ������ �� "1ȸ" ȣ��
    /// </summary>
    abstract public void Enter();

    /// <summary>
    /// �ش� ���¸� ������Ʈ �� �� "�� ������" ȣ��
    /// </summary>
    abstract public void Excute();

    /// <summary>
    ///  �ش� ���¸� ������ �� "1ȸ" ȣ��
    /// </summary>
    abstract public void Exit();
}
