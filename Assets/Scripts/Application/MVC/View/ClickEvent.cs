using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickEvent : View,IPointerDownHandler
{
    public string ResourceDir = "";
    List<string> pObjName = new List<string> { "1954素材", "运动服素材" , "五环素材", "相框素材", "红双喜素材", "奖杯素材" };
    public Image I_Description;
    public override string Name => Consts.V_ClickEvent;
    GameModel gm;
    private void Awake()
    {       
        gm = GetModel<GameModel>();       
    }
    private void Start()
    {
        ReadFile("广东.txt");
        //InitPickedObj();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //Debug.Log(eventData.pointerCurrentRaycast);
        RaycastResult rr = eventData.pointerCurrentRaycast;
        if (gm.Objinfo.ContainsKey(rr.gameObject.name))
        {
            StopCoroutine("ShowDescription");
            string s = gm.Objinfo[rr.gameObject.name];
            Text des = I_Description.GetComponentInChildren<Text>();
            des.text = s;
            StartCoroutine("ShowDescription");
        }
    }

    IEnumerator ShowDescription()
    {
        I_Description.gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);
        I_Description.gameObject.SetActive(false);
    }

    public void ReadFile(string TextName)
    {
        string path = Application.dataPath + "/" +ResourceDir + "/" + TextName;
        using (StreamReader sr = new StreamReader(path))
        {
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                string[] s = line.Split('：');
                gm.Objinfo[s[0]] = s[1];
            }
            //foreach (var s in gm.Objinfo)
            //{
            //    Debug.Log("key:" + s.Key + "|Value:" + s.Value);
            //}
        }
    }

    public void InitPickedObj()
    {
        foreach (var s in pObjName)
        {
            gm.PickedObj[s] = false;
        }
    }

    public override void HandleEvent(string enentName, object data)
    {
        throw new System.NotImplementedException();
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
