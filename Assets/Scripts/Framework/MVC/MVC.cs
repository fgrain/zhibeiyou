using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class MVC
{
    //持有资源
    public static Dictionary<string, View> Views = new Dictionary<string, View>(); //名字 -- View
    public static Dictionary<string, Model> Models = new Dictionary<string, Model>(); //名字 -- Model
    public static Dictionary<string, Type> CommandMap = new Dictionary<string, Type>(); //事件名字 -- 类型

    //注册
    public static void RegisterView(View view)
    {
        //防止来回跳转场景导致view重复注册
        if (Views.ContainsKey(view.Name))
        {
            Views.Remove(view.Name);
        }
        view.RigisterAttentionEvent();
        Views[view.Name] = view;
    }
    public static void RegisterModel(Model model)
    {
        Models[model.Name] = model;
    }
    public static void RegisterController(string eventName,Type controllerType)
    {
        CommandMap[eventName] = controllerType;
    }

    //获取模型
    public static T GetModel<T>()
        where T:Model
    {
        foreach(var m in Models.Values)
        {
            if(m is T)
            {
                return m as T;
            }
        }
        return null;
    }

    //获取视图
    public static T GetView<T>()
        where T : View
    {
        foreach (var v in Models.Values)
        {
            if (v is T)
            {
                return v as T;
            }
        }
        return null;
    }

    //发送事件
    public static void SendEvent(string eventName,object data = null)
    {
        //controller执行
        if (CommandMap.ContainsKey(eventName))
        {
            Type t = CommandMap[eventName];
            //通过反射动态创建实例
            Controller c = Activator.CreateInstance(t) as Controller;
            c.Excute(data);
        }

        //view处理
        foreach (var view in Views.Values)
        {
            if (view.AttentionEventList.Contains(eventName))
            {
                //一个view对应多个事件，多个view也可以处理同一个事件
                view.HandleEvent(eventName, data);
            }
        }
    }
}