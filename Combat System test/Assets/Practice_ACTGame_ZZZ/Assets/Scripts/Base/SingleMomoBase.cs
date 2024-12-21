using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����ģʽ������
/// </summary>
/// <typeparam name="T"></typeparam>
public class SingleMomoBase<T> : MonoBehaviour where T : SingleMomoBase<T>
{
    public static T INSTANCE;

    protected virtual void Awake()
    {
        if (INSTANCE != null) 
        {
            Debug.LogError(this + "�����ϵ���ģʽ");
        }
        INSTANCE = (T)this;
    }

    protected virtual void OnDestroy()
    {
        Destroy();         
    }

    /// <summary>
    /// ������൥��
    /// </summary>
    public void Destroy()
    {
        INSTANCE = null;
    }
}
