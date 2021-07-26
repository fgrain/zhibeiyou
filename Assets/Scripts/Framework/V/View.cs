using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class View : MonoBehaviour
{
    public abstract string Name { get; }

    [HideInInspector]//不让事件列表在inspector面板显示
    public List<string> AttentionEventList = new List<string>();

    public virtual void RigisterAttentionEvent()
    {

    }

    //处理事件
    public abstract void HandleEvent(string enentName, object data);
    //发送事件
    protected void SendEvent(string eventName, object data = null)
    {
        MVC.SendEvent(eventName, data);
    }

    //获取Model
    protected T GetModel<T>()
        where T:Model
    {
        return MVC.GetModel<T>();
    }
}
