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

    protected override void Awake()
    {
        base.Awake();
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
}
