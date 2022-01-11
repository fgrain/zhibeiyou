using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollContent : MonoSingleton<ScrollContent>
{
    public string ResourceDir = "";
    public string readText;
    private TextAsset story;
    private Text text;
    private GameObject _content; //添加Text组件的Content
    private float _scrollSpeed = 0.5f;    //滚动速度
    private float heightChange = 0;     //滚动的高度（根据当前文字数量和默认文本框的大小决定）
    private float posY = 0; //动态修改的Content_PosY

    private void Start()
    {
        string path = ResourceDir + "/" + readText;
        story = Resources.Load<TextAsset>(path);
        _content = transform.Find("Content").gameObject;
        text = _content.GetComponent<Text>();
        text.text = story.text;
        StartCoroutine(SetContent(_content, _scrollSpeed));
    }

    //调用此协程即可
    public IEnumerator SetContent(GameObject content, float scrollSpeed)
    {
        yield return new WaitForEndOfFrame();    //等待 否则获取不到当前Height
        _content = content;
        _scrollSpeed = scrollSpeed;
        heightChange = content.GetComponent<RectTransform>().sizeDelta.y - content.transform.parent.parent.GetComponent<RectTransform>().sizeDelta.y;
        posY = 0;
        yield return new WaitForSeconds(3);
        yield return ScorllShow();
        yield return new WaitForSeconds(3);
        UIControl.Instance.isPause = false;
        gameObject.SetActive(false);
    }

    private IEnumerator ScorllShow()
    {
        while (posY < heightChange)
        {
            posY += _scrollSpeed;
            _content.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, posY);
            yield return null;
        }
    }
}