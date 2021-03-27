using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(ObjectPool))]
[RequireComponent(typeof(Sound))]
[RequireComponent(typeof(StaticData))]
public class Game : MonoSingleton<Game>
{
    //全局访问
    [HideInInspector]
    public ObjectPool objectPool;
    [HideInInspector]
    public Sound sound;
    [HideInInspector]
    public StaticData staticData;

    private void Start()
    {
        //不要删除当前物体，当前物体相当于全局控制器
        DontDestroyOnLoad(gameObject);

        objectPool = ObjectPool.Instance;
        sound = Sound.Instance;
        staticData = StaticData.Instance;    

        //初始化StartUpController
        RegisterController(Consts.E_StartUp, typeof(StartUpController));

        //游戏启动
        SendEvent(Consts.E_StartUp);

        //跳转场景
        Instance.LoadLevel(1);
    }

    public void LoadLevel(int level)
    {
        
        SceneAgrs e = new SceneAgrs
        {
            //获取当前场景索引
            sceneIndex = SceneManager.GetActiveScene().buildIndex
        };
        //发送退出场景事件
        SendEvent(Consts.E_ExitScenes, e);

        //加载新场景
        SceneManager.LoadScene(level, LoadSceneMode.Single);
    }

    void SendEvent(string eventName, object data = null)
    {
        MVC.SendEvent(eventName, data);
    }

    void RegisterController(string eventName, Type controllerType)
    {
        MVC.RegisterController(eventName, controllerType);
    }

}
