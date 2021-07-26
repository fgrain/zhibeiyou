using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(ObjectPool))]
[RequireComponent(typeof(Sound))]
public class Game : MonoSingleton<Game>
{
    //全局访问
    [HideInInspector]
    public ObjectPool objectPool;

    [HideInInspector]
    public Sound sound;

    [HideInInspector]
    public static int state = 0;

    [HideInInspector]
    public static Dictionary<string, bool> mapUsed = new Dictionary<string, bool>();

    private GameObject sceneLoad;
    private Animator sceneAnim;

    private void Start()
    {
        //不要删除当前物体，当前物体相当于全局控制器
        DontDestroyOnLoad(gameObject);

        sceneLoad = GameObject.Find("SceneLoad");
        sceneAnim = sceneLoad.GetComponent<Animator>();
        sceneLoad.transform.parent.gameObject.SetActive(false);

        objectPool = ObjectPool.Instance;
        sound = Sound.Instance;

        //初始化StartUpController
        RegisterController(Consts.E_StartUp, typeof(StartUpController));

        //游戏启动
        SendEvent(Consts.E_StartUp);

        //加载游戏
        ReLoadGame();
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
        //SceneManager.LoadScene(level, LoadSceneMode.Single);
        StartCoroutine(SceneLoad(level));
    }

    public IEnumerator SceneLoad(int level)
    {
        sceneLoad.transform.parent.gameObject.SetActive(true);
        sceneAnim.Play("SceneOut");
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(level, LoadSceneMode.Single);
        sceneAnim.Play("SceneIn");
        yield return new WaitForSeconds(1);
        sceneLoad.transform.parent.gameObject.SetActive(false);
    }

    private void SendEvent(string eventName, object data = null)
    {
        MVC.SendEvent(eventName, data);
    }

    private void RegisterController(string eventName, Type controllerType)
    {
        MVC.RegisterController(eventName, controllerType);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ReLoadGame()
    {
        state = PlayerPrefs.GetInt("state", 0);
        //state = 10;
        if (state == 0)
        {
            PlayerPrefs.SetString("keyString", "new");
        }
    }
}