using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModel : Model
{
    #region 常量
    #endregion

    #region 字段
    int m_State = 0;
    bool m_IsPause = false;
    Dictionary<string, bool> m_pickedObj = new Dictionary<string, bool>();
    Dictionary<string, string> m_objinfo = new Dictionary<string, string>();
    #endregion

    #region 事件
    #endregion

    #region 属性
    public override string Name => Consts.M_GameModel;
    public int State { get => m_State; set => m_State = value; }
    public bool IsPause { get => m_IsPause; set => m_IsPause = value; }
    public Dictionary<string, bool> PickedObj { get => m_pickedObj; set => m_pickedObj = value; }
    public Dictionary<string, string> Objinfo { get => m_objinfo; set => m_objinfo = value; }
    #endregion

    #region 方法
    public void PickedStateChange(string name)
    {
        m_pickedObj[name] = true;
        UIControl.Instance.ShowPicked(name);
    }
    #endregion

    #region Unity回调
    #endregion

    #region 事件回调
    #endregion

    #region 帮助方法
    #endregion



}
