using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardControl : MonoBehaviour, IPointerDownHandler
{
    public Button card1, card2, card3, card4, card5;
    public GameObject[] cards;

    private Vector2 initPos;
    private bool isUsed = false;
    private GameObject curGo;

    // Start is called before the first frame update
    private void Start()
    {
        card1.onClick.AddListener(() => CardMove(card1.gameObject));
        card2.onClick.AddListener(() => CardMove(card2.gameObject));
        card3.onClick.AddListener(() => CardMove(card3.gameObject));
        card4.onClick.AddListener(() => CardMove(card4.gameObject));
        card5.onClick.AddListener(() => CardMove(card5.gameObject));
    }

    public void CardMove(GameObject go)
    {
        if (!isUsed)
        {
            foreach (var c in cards)
            {
                if (c.name != go.name)
                {
                    StartCoroutine(CardHide(c.GetComponent<Image>()));
                }
            }
            initPos = go.GetComponent<RectTransform>().localPosition;
            curGo = go.GetComponent<RectTransform>().gameObject;
            go.transform.SetAsLastSibling();
            isUsed = true;
            StartCoroutine(MoveTarget(go.GetComponent<RectTransform>(), new Vector2(0, 0), new Vector2(600f, 900f), true));
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(MoveTarget(curGo.GetComponent<RectTransform>(), initPos, new Vector2(300f, 450f), false));
            foreach (var c in cards)
            {
                if (c.name != curGo.name)
                {
                    StartCoroutine(CardDisplay(c.GetComponent<Image>()));
                }
            }
        }
    }

    private IEnumerator MoveTarget(RectTransform rt, Vector2 target, Vector2 size, bool b)
    {
        while (true)
        {
            rt.localPosition = Vector2.MoveTowards(rt.localPosition, target, 3);
            rt.sizeDelta = Vector2.MoveTowards(rt.sizeDelta, size, 2.3f);
            if (Mathf.Abs(rt.localPosition.x - target.x) < 1 && Mathf.Abs(rt.sizeDelta.x - size.x) < 1)
            {
                Debug.Log("到达");
                isUsed = b;
                break;
            }
            yield return null;
        }
    }

    private IEnumerator CardHide(Image sr)
    {
        while (sr.color.a > 0)
        {
            sr.color = Color.Lerp(sr.color, Color.clear, 5 * Time.deltaTime);
            if (Mathf.Abs(sr.color.a - 1) < 0.1)
            {
                sr.color = Color.clear;
                break;
            }
            yield return null;
        }
    }

    private IEnumerator CardDisplay(Image sr)
    {
        while (sr.color.a < 1)
        {
            sr.color = Color.Lerp(sr.color, Color.white, 5 * Time.deltaTime);
            if (Mathf.Abs(sr.color.a - 1) < 0.1)
            {
                sr.color = Color.white;
                break;
            }
            yield return null;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        RaycastResult rr = eventData.pointerCurrentRaycast;
        Debug.Log(rr.gameObject.name);
        if (isUsed)
        {
            if (rr.gameObject.name != curGo.name)
            {
                StopAllCoroutines();
                StartCoroutine(MoveTarget(curGo.GetComponent<RectTransform>(), initPos, new Vector2(300f, 450f), false));
                foreach (var c in cards)
                {
                    if (c.name != curGo.name)
                    {
                        StartCoroutine(CardDisplay(c.GetComponent<Image>()));
                    }
                }
            }
        }
    }
}