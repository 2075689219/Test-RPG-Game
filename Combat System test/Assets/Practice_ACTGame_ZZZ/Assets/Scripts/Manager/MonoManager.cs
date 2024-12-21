using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoManager : SingleMomoBase<MonoManager>
{
    //update���񼯺�
    public Action updatAction;
    //fixedUpdate���񼯺�
    public Action fixedUpdatAction;
    //lateUpdate���񼯺�
    public Action lateUpdatAction;

    /// <summary>
    /// ���update����
    /// </summary>
    /// <param name="task">����</param>
    public void AddUpdateAction(Action task)
    {
        updatAction += task;
    }

    /// <summary>
    /// �Ƴ�update����
    /// </summary>
    /// <param name="task">����</param>
    public void RemoveUpdateAction(Action task)
    {
        updatAction -= task;
    }

    /// <summary>
    /// ���fixedUpdate����
    /// </summary>
    /// <param name="task">����</param>
    public void AddFixedUpdateAction(Action task)
    {
        fixedUpdatAction += task;
    }

    /// <summary>
    /// �Ƴ�fixedUpdate����
    /// </summary>
    /// <param name="task">����</param>
    public void RemoveFixedUpdateAction(Action task)
    {
        fixedUpdatAction -= task;
    }

    /// <summary>
    /// ���lateUpdate����
    /// </summary>
    /// <param name="task">����</param>
    public void AddLateUpdateAction(Action task)
    {
        lateUpdatAction += task;
    }

    /// <summary>
    /// �Ƴ�lateUpdate����
    /// </summary>
    /// <param name="task">����</param>
    public void RemoveLateUpdateAction(Action task)
    {
        lateUpdatAction -= task;
    }

    void Update()
    {
        updatAction?.Invoke();
    }

    void FixedUpdate()
    {
        fixedUpdatAction?.Invoke();
    }
     void LateUpdate()
    {
        lateUpdatAction?.Invoke();
    }
}
