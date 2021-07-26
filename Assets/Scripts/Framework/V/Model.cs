using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract class Model
{
    public abstract string Name { get; }

    protected void SendEvent(string eventName,object data = null)
    {
        MVC.SendEvent(eventName, data);
    }
}
