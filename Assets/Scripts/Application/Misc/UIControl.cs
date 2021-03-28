using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIControl : MonoSingleton<UIControl>,IPointerDownHandler
{
    public Image I_Bag;
    public GameObject Button;
    public bool isPause = false;
    public Image[] picked;
    GameObject P_Anima;

    protected override void Awake()
    {
        base.Awake();
        P_Anima = GameObject.Find("P_Anima");
    }
    public void OpenBag()
    {
        I_Bag.gameObject.SetActive(true);
        Button.SetActive(false);
        isPause = true;
        Time.timeScale = 0;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        print("OnPointerClick");
        Debug.Log(eventData.pointerCurrentRaycast);
        if (eventData.pointerCurrentRaycast.gameObject.tag == Tag.Bag)
        {
            I_Bag.gameObject.SetActive(false);
            Button.SetActive(true);
            isPause = false;
            Time.timeScale = 1;
        }       
    }

    public void ShowPicked(string name)
    {
        Image image = null;
        foreach (var p in picked)
        {
            Debug.Log(p.name);
            if (p.name == name)
            {
                image = p;
                break;
            }
        }
        if (image != null)
        {
            Debug.Log(image.transform.parent.GetComponent<Image>().name);
            image.transform.parent.GetComponent<Image>().color = new Color(255, 255, 255, 1);
            image.gameObject.SetActive(true);
        }           
    }

    public void PickedAnima(GameObject img,bool remove)
    {
        
        StartCoroutine(MoveOnPath(img,remove));
    }

    IEnumerator MoveForTarget(GameObject go, bool b)
    {
        Vector3 target = Camera.main.ScreenToWorldPoint(Button.transform.position);
        while (go.transform.position!= target) 
        {
            if (b)
                go.transform.localScale = Vector3.Lerp(go.transform.localScale, new Vector3(0.1f,0.1f ,0.1f), 0.5f * Time.deltaTime);
            go.transform.position = Vector3.MoveTowards(go.transform.position, target, 5 * Time.deltaTime);
            yield return 0;
        }
    }

    IEnumerator MoveOnPath(GameObject go,bool remove)
    {
        go.GetComponent<SpriteRenderer>().sortingLayerName = "Picked";
        //yield return StartCoroutine(MoveForTarget(go, path[0],false));
        Vector2 t1pos = Camera.main.ScreenToWorldPoint(P_Anima.transform.position);
        go.transform.position = t1pos;
        Debug.Log("over1");
        yield return new WaitForSeconds(0.5f);
        //Debug.Log("wait");
        yield return StartCoroutine(MoveForTarget(go,true));
        Debug.Log("over2");
        if (remove)
        {
            FollowPlayer.Instance.ChangeState();
        }        
        Destroy(go);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
