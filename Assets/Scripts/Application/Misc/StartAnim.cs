using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StartAnim : MonoBehaviour, IPointerDownHandler
{
    public GameObject anim;
    public GameObject keepGame;

    private void Start()
    {
        StartCoroutine(KeepGame());
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Game.state = 0;
        anim.SetActive(true);
    }

    public void GameStart()
    {
        Game.Instance.LoadLevel(1);
    }

    private IEnumerator KeepGame()
    {
        yield return new WaitForEndOfFrame();
        if (Game.state > 0)
        {
            keepGame.SetActive(true);
        }
    }
}