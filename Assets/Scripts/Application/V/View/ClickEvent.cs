using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ClickEvent : View, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    private Texture2D mouse1;
    private Texture2D mouse2;
    public string ResourceDir = "";
    public string readText;
    private TextAsset textAsset;
    public Image I_Description;
    public override string Name => Consts.V_ClickEvent;
    private GameModel gm;

    private void Awake()
    {
        gm = GetModel<GameModel>();
    }

    private void Start()
    {
        Game.state++;
        Game.mapUsed[SceneManager.GetActiveScene().name] = true;
        mouse1 = Resources.Load<Texture2D>("UI/鼠标1");
        mouse2 = Resources.Load<Texture2D>("UI/鼠标2");
        ReadFile(readText);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        RaycastResult rr = eventData.pointerCurrentRaycast;
        Debug.Log(rr.gameObject.tag);
        if (gm.Objinfo.ContainsKey(rr.gameObject.name))
        {
            Game.Instance.sound.PlayEffect("Tirgger");
            if (rr.gameObject.tag == Tag.Picked)
            {
                rr.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Picked";
            }
            StopCoroutine("ShowDescription");
            string s = gm.Objinfo[rr.gameObject.name];
            Text des = I_Description.GetComponentInChildren<Text>();
            des.text = s;
            StartCoroutine("ShowDescription");
        }
    }

    public IEnumerator ShowDescription()
    {
        I_Description.gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);
        I_Description.gameObject.SetActive(false);
    }

    public void ReadFile(string TextName)
    {
        string path = ResourceDir + "/" + TextName;
        textAsset = Resources.Load<TextAsset>(path);
        string t = textAsset.text;
        string[] linetxt = t.Split('\n');
        foreach (var line in linetxt)
        {
            string[] s = line.Split('：');
            gm.Objinfo[s[0]] = s[1];
            //Debug.Log(s[0] + s[1]);
        }
    }

    public override void HandleEvent(string enentName, object data)
    {
        throw new System.NotImplementedException();
    }

    private void OnDisable()
    {
        gm.Objinfo.Clear();
        gm.PickedObj.Clear();
        StopAllCoroutines();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Cursor.SetCursor(mouse1, Vector2.zero, CursorMode.Auto);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Cursor.SetCursor(mouse2, Vector2.zero, CursorMode.Auto);
    }
}