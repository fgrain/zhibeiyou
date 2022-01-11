using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIControl : MonoSingleton<UIControl>, IPointerDownHandler
{
    private Image I_Bag;
    private GameObject Button;
    public bool isPause = false;
    public Image[] picked;
    private GameObject P_Anima;
    private GameModel gm;
    private GameObject startGame;
    private GameObject scorllView;
    private GameObject But_Bag;
    private GameObject question;

    protected override void Awake()
    {
        base.Awake();
        P_Anima = GameObject.Find("P_Anima");
    }

    private void Start()
    {
        WhetherGame();
        scorllView = transform.Find("ScrollView").gameObject;
        Button = transform.Find("Button").gameObject;
        But_Bag = Button.transform.Find("But_Bag").gameObject;
        I_Bag = transform.Find("Bag").gameObject.GetComponent<Image>();
        startGame = I_Bag.transform.Find("startGame").gameObject;
        if (transform.Find("question") != null)
            question = transform.Find("question").gameObject;
        Game.Instance.sound.PlayBG("SceneBGM");
        gm = MVC.GetModel<GameModel>();
    }

    public void OpenButton(Image i)
    {
        i.gameObject.SetActive(true);
        Button.SetActive(false);
        isPause = true;
        Game.Instance.sound.StopPlayEffect();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        print("OnPointerClick");
        if (eventData.pointerCurrentRaycast.gameObject.tag == Tag.Bag)
        {
            ReturnGame(I_Bag);
        }
    }

    public void ReturnGame(Image i)
    {
        i.gameObject.SetActive(false);
        Button.SetActive(true);
        isPause = false;
    }

    public void ShowPicked(string name)
    {
        Image image = null;
        foreach (var p in picked)
        {
            //Debug.Log(p.name);
            if (p.name == name)
            {
                image = p;
                break;
            }
        }
        if (image != null)
        {
            //Debug.Log(image.transform.parent.GetComponent<Image>().name);
            //星星亮起
            image.transform.parent.GetComponent<Image>().color = new Color(255, 255, 255, 1);
            image.gameObject.SetActive(true);
        }
    }

    public void PickedAnima(GameObject img, bool remove)
    {
        StartCoroutine(MoveOnPath(img, remove));
    }

    private IEnumerator MoveForTarget(GameObject go, bool b)
    {
        Vector3 target = Camera.main.ScreenToWorldPoint(But_Bag.transform.position);
        while (go != null && go.transform.position != target)
        {
            if (b)
                go.transform.localScale = Vector3.Lerp(go.transform.localScale, new Vector3(0.1f, 0.1f, 0.1f), 2 * Time.deltaTime);
            go.transform.position = Vector3.MoveTowards(go.transform.position, target, 8 * Time.deltaTime);
            yield return 0;
            target = Camera.main.ScreenToWorldPoint(But_Bag.transform.position);
        }
    }

    private IEnumerator MoveOnPath(GameObject go, bool remove)
    {
        Game.Instance.sound.PlayEffect("Tirgger");
        Vector2 t1pos = Camera.main.ScreenToWorldPoint(P_Anima.transform.position);
        go.transform.position = t1pos;
        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(MoveForTarget(go, true));
        if (remove)
        {
            FollowPlayer.Instance.ChangeState();
        }
        if (gm.PickedObj.Count == 6)
        {
            isPause = true;
            Game.Instance.sound.StopPlayEffect();
            yield return new WaitForSeconds(1);
            scorllView.SetActive(true);
            startGame.SetActive(true);
        }
        Destroy(go);
    }

    public void ExitGame()
    {
        Game.Instance.LoadLevel(1);
        Game.state--;
        Game.mapUsed[SceneManager.GetActiveScene().name] = false;
        Game.Instance.sound.StopAllSound();
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        Game.Instance.sound.StopAllSound();
    }

    public void StartQue()
    {
        question.SetActive(true);
    }

    public void WhetherGame()
    {
        string flag = PlayerPrefs.GetString("keyString", "new");
        if (flag == SceneManager.GetActiveScene().name)
        {
            StartCoroutine(DesPicked());
        }
    }

    public IEnumerator DesPicked()
    {
        yield return new WaitForEndOfFrame();
        GameObject go = GameObject.Find("场景");
        foreach (Transform child in go.transform)
        {
            if (child.CompareTag(Tag.Picked))
            {
                Destroy(child.gameObject);
            }
        }
        for (int i = 0; i < picked.Length; i++)
        {
            picked[i].transform.parent.GetComponent<Image>().color = new Color(255, 255, 255, 1);
            picked[i].gameObject.SetActive(true);
        }
        startGame.SetActive(true);
    }
}