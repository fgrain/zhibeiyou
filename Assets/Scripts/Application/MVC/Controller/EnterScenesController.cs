using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class EnterScenesController : Controller
{
    public override void Excute(object data)
    {
        SceneAgrs e = data as SceneAgrs;
        switch (e.sceneIndex)
        {
            case 0:
                break;
            case 1://广东
                //注册view
                RegisterView(GameObject.FindWithTag(Tag.Player).GetComponent<PlayerMove>());
                RegisterView(GameObject.FindWithTag(Tag.Ground).GetComponent<ClickEvent>());
                break;
            case 2://乒乓球
                break;
            case 3:
                break;
            case 4:
                break;
            case 5:
                break;
        }
    }
}
