using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoSingleton<ObjectPool>
{
    public string ResourceDir = "";
    public Dictionary<string, SubPool> m_pools = new Dictionary<string, SubPool>();

    //取出物体
    public GameObject Spawn(string name, Transform parent)
    {
        SubPool pool = null;
        if (!m_pools.ContainsKey(name))
        {
            CreatPool(name, parent);
        }
        pool = m_pools[name];
        return pool.Spawn();
    }

    //回收物体
    public void UnSpawn(GameObject go)
    {
        SubPool pool = null;
        //找到游戏物体属于哪个池子
        foreach (var p in m_pools.Values)
        {
            if (p.Contain(go))
            {
                pool = p;
                break;
            }
        }
        pool.UnSpawn(go);
    }

    //回收全部物体
    public void UnSpawnAll()
    {
        foreach (var p in m_pools.Values)
        {
            p.UnSpawnAll();
        }
    }

    //创建新池子
    private void CreatPool(string name, Transform parent)
    {
        //游戏物体所在资源目录
        string path = ResourceDir + "/" + name;
        //以加载游戏对象(预制体)的相对路径,来查找和加载相应资源的一种方式.
        GameObject go = Resources.Load<GameObject>(path);
        SubPool pool = new SubPool(go, parent);
        m_pools.Add(pool.Name, pool);
    }
}