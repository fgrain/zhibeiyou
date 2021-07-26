using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
    public List<GameObject> maps;
    public List<GameObject> bookList;
    public List<GameObject> cardList;
    public List<GameObject> cards;
    public List<GameObject> selectMap;
    public GameObject bookBag;
    public GameObject cardBag;
    public GameObject black;
    public GameObject collect;
    public GameObject cardTarget;
    public GameObject scrollView;
    public GameObject collectView;
    public Text scrollText;
    public Text selectText;
    public Text collectText;
    public GameObject end;
    private int mapindex = 0;

    private void Start()
    {
        StartCoroutine(StartBg());
        ShowMap();
    }

    private void ShowMap()
    {
        Debug.Log(Game.state);
        switch (Game.state)
        {
            case 0:
            case 1:
                mapindex = 0;
                UpdateState(-1);
                break;

            case 2:
                mapindex = 1;
                UpdateState(0);
                break;

            case 3:
                mapindex = 1;
                ShowCard(1);
                UpdateState(-1);
                break;

            case 4:
                mapindex = 2;
                UpdateState(1);
                break;

            case 5:
                mapindex = 2;
                ShowCard(2);
                UpdateState(-1);
                break;

            case 6:
                mapindex = 3;
                UpdateState(2);
                break;

            case 7:
                mapindex = 3;
                ShowCard(3);
                UpdateState(-1);
                break;

            case 8:
                mapindex = 4;
                ShowCard(2);
                UpdateState(3);
                break;

            case 9:
                mapindex = 4;
                ShowCard(4);
                UpdateState(-1);
                break;

            case 10:
                mapindex = -1;
                UpdateState(4);
                break;
        }
    }

    private void UpdateState(int i)
    {
        if (mapindex != -1)
            selectMap[mapindex].SetActive(true);
        else
            end.SetActive(true);
        RemoveButton();
        ShowBook();
        if (i != -1)
        {
            CollectCard(i);
            ShowCard(i + 1);
        }
    }

    public void CollectCard(int i)
    {
        cards[i].SetActive(true);
        cardList[i].SetActive(true);
        StartCoroutine(MovetoTarget(cards[i], i));
    }

    private void ShowBook()
    {
        for (int i = 0; i < bookList.Count; i++)
        {
            if (Game.mapUsed.ContainsKey(maps[i].name) && Game.mapUsed[bookList[i].name])
            {
                bookList[i].SetActive(true);
            }
        }
        int bookNums = Game.state % 2 == 0 ? Game.state : Game.state - 1;
        for (int i = 0; i < bookNums; i++)
        {
            bookList[i].SetActive(true);
        }
        if (Game.state > 0)
        {
            if (PlayerPrefs.GetString("keyString", "new") == bookList[Game.state - 1].name)
            {
                bookList[Game.state - 1].SetActive(true);
            }
        }
    }

    private void ShowCard(int index)
    {
        for (int i = 0; i < index; i++)
        {
            cardList[i].SetActive(true);
        }
    }

    private IEnumerator MovetoTarget(GameObject go, int i)
    {
        go.transform.SetAsLastSibling();
        Game.Instance.sound.PlayEffect("Card");
        yield return new WaitForSeconds(1f);
        collectView.SetActive(true);
        switch (i)
        {
            case 0:
                collectText.text = "恭喜你完成了广东-战国的考验，战国奔跑卡牌已经收集到你的卡牌包里啦，快去看看吧！";
                break;

            case 1:
                collectText.text = "恭喜你完成了湖南-秦朝的考验，秦朝射箭卡牌已经收集到你的卡牌包里啦，快去看看吧！";
                break;

            case 2:
                collectText.text = "恭喜你完成了湖北-两汉的考验，两汉蹴鞠卡牌已经收集到你的卡牌包里啦，快去看看吧！";
                break;

            case 3:
                collectText.text = "恭喜你完成了河南-唐朝的考验，唐朝角力牌已经收集到你的卡牌包里啦，快去看看吧！";
                break;

            case 4:
                collectText.text = "恭喜你完成了北京-清朝的考验，清朝冰嬉卡牌已经收集到你的卡牌包里啦，快去看看吧！";
                break;
        }
        Vector3 target = cardTarget.transform.position;
        while (go != null && go.transform.position != target)
        {
            //Debug.Log(go.transform.position);
            go.transform.localScale = Vector3.Lerp(go.transform.localScale, Vector3.zero, 2 * Time.deltaTime);
            go.transform.position = Vector3.MoveTowards(go.transform.position, target, 5 * Time.deltaTime);
            //Debug.Log(target + " " + go.transform.position + " " + (go.transform.position == target));
            if (Mathf.Abs(go.transform.position.x - target.x) < 0.01f)
            {
                Destroy(go);
            }
            yield return 0;
        }
        collectView.SetActive(false);
    }

    public void OpenBag(GameObject go)
    {
        collect.SetActive(false);
        go.SetActive(true);
    }

    public void ExitBag(GameObject go)
    {
        collect.SetActive(true);
        go.SetActive(false);
    }

    public void OpenMap(GameObject go)
    {
        black.SetActive(true);
        switch (Game.state)
        {
            case 0:
            case 1:
                selectText.text = "你选择的是广东省，你可以选择进入广东或战国的游戏剧情......";
                break;

            case 2:
            case 3:
                selectText.text = "你选择的是湖南省，你可以选择进入湖南或秦朝的游戏剧情......";
                break;

            case 4:
            case 5:
                selectText.text = "你选择的是湖北省，你可以选择进入湖北或两汉的游戏剧情......";
                break;

            case 6:
            case 7:
                selectText.text = "你选择的是河南省，你可以选择进入河南或唐朝的游戏剧情......";
                break;

            case 8:
            case 9:
                selectText.text = "你选择的是北京，你可以选择进入北京或清朝的游戏剧情......";
                break;
        }
        collect.SetActive(false);
        transform.GetComponent<SpriteRenderer>().color = new Color(160f / 255f, 160f / 255f, 160f / 255f, 1);
        selectMap[mapindex].SetActive(false);
        go.SetActive(true);
    }

    public void ExitMap(GameObject go)
    {
        collect.SetActive(true);
        black.SetActive(false);
        transform.GetComponent<SpriteRenderer>().color = Color.white;
        selectMap[mapindex].SetActive(true);
        go.SetActive(false);
    }

    public void ReadBook(string name)
    {
        TextAsset textAsset = Resources.Load<TextAsset>("Story/" + name);
        scrollText.text = textAsset.text;
        scrollView.SetActive(true);
    }

    public void ExitBook()
    {
        scrollView.SetActive(false);
    }

    public void LoadScene(int i)
    {
        black.SetActive(false);
        collect.SetActive(false);
        maps[mapindex].SetActive(false);
        Game.Instance.LoadLevel(i);
    }

    public void RemoveButton()
    {
        for (int i = 0; i < maps.Count; i++)
        {
            if (Game.mapUsed.ContainsKey(maps[i].name) && Game.mapUsed[maps[i].name])
            {
                maps[i].GetComponent<Button>().enabled = false;
            }
        }
    }

    private void OnDisable()
    {
        Game.Instance.sound.StopAllSound();
    }

    private IEnumerator StartBg()
    {
        yield return new WaitForEndOfFrame();
        Game.Instance.sound.PlayBG("StartBG");
    }

    public void ExitGame()
    {
        PlayerPrefs.SetInt("state", Game.state);
        Application.Quit();
    }
}