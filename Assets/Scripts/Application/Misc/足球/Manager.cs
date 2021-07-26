using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoSingleton<Manager>
{
    public GameObject[] gameIndex;
    public int index = 0;
    public GameObject ruler;

    private void Start()
    {
        Game.Instance.sound.PlayBG("SceneBGM");
        StartCoroutine(ShowRuler());
    }

    private IEnumerator ShowRuler()
    {
        ruler.SetActive(true);
        yield return new WaitForSeconds(1);
        ruler.SetActive(false);
    }

    public void RulerText()
    {
        StartCoroutine(ShowRuler());
    }

    public void ExitGame()
    {
        Game.state--;
        Game.mapUsed["两汉"] = false;
        Game.Instance.LoadLevel(7);
        PlayerPrefs.SetString("keyString", "两汉");
    }
}