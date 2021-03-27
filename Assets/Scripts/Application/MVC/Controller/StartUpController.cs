using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartUpController : Controller
{
    public override void Excute(object data)
    {
        //注册Controller
        RegisterController(Consts.E_EnterScenes, typeof(EnterScenesController));
        //注册Model
        RegisterModel(new GameModel());
        //初始化
     
    }
}
