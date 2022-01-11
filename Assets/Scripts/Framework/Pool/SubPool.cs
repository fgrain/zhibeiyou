using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubPool
{
    //物体集合
    private List<GameObject> m_object = new List<GameObject>();

    //预制体
    private GameObject m_prefab;

    //名字
    public string Name { get { return m_prefab.name; } }

    //父物体位置
    private Transform m_parent;

    //初始化子池子
    public SubPool(GameObject prefab, Transform parent)
    {
        m_parent = parent;
        m_prefab = prefab;
    }

    //取出物体
    public GameObject Spawn()
    {
        GameObject go = null;
        foreach (var obj in m_object)
        {
            if (obj != null)
            {
                //集合里找出未显示的物体
                if (!obj.activeSelf)
                {
                    go = obj;
                    break;
                }
            }
            else
            {
                go = obj;
            }
        }
        //若集合里没有合适的物体，创建一个新物体加入集合
        if (!go)
        {
            m_object.Remove(go);
            go = GameObject.Instantiate(m_prefab);
            go.transform.parent = m_parent;
            m_object.Add(go);
        }
        go.SetActive(true);
        //取出物体后通知物体的OnSpawn方法
        go.SendMessage("OnSpawn", SendMessageOptions.DontRequireReceiver);
        return go;
    }

    //回收物体
    public void UnSpawn(GameObject go)
    {
        //若该物体在集合里，通知物体身上的OnUnSpawn方法
        if (Contain(go))
        {
            go.SendMessage("OnUnSpawn", SendMessageOptions.DontRequireReceiver);
            go.SetActive(false);
        }
    }

    //回收所有物体
    public void UnSpawnAll()
    {
        foreach (var obj in m_object)
        {
            if (obj.activeSelf)
            {
                UnSpawn(obj);
            }
        }
    }

    public bool Contain(GameObject go)
    {
        return m_object.Contains(go);
    }
}