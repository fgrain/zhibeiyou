using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract class Controller
{
    public abstract void Excute(object data);

    //获取Model
    protected T GetModel<T>()
        where T : Model
    {
        return MVC.GetModel<T>();
    }

    //获取View
    protected T GetView<T>()
    where T : View
    {
        return MVC.GetView<T>();
    }

    //注册
    protected void RegisterView(View view)
    {
        MVC.RegisterView(view);
    }
    protected void RegisterModel(Model model)
    {
        MVC.RegisterModel(model);
    }
    protected static void RegisterController(string eventName, Type controllerType)
    {
        MVC.RegisterController(eventName, controllerType);
    }
}
