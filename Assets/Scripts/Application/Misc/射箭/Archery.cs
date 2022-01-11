using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class Archery : MonoBehaviour
{
    public List<GameObject> arrows;
    public GameObject target;
    public GameObject bow;

    public Vector2[] bowMove;
    public Vector2[] targetMove;
    public float bowSpeed = 3;
    public float targetSpeed = 5;
    public float arrowSpeed = 8;
    public GameObject endGame;
    public int nums = 30;
    private bool gameOver = false;

    private int arrowIndex = 9;
    private Transform arrow;

    private GameObject backGround;
    private Vector2 initArrowPos;
    private Rigidbody2D arrowBody;

    private void Start()
    {
        Game.Instance.sound.PlayBG("SceneBGM");
        backGround = GameObject.Find("BackGround");
        arrow = bow.transform.Find("箭");
        initArrowPos = arrow.localPosition;
        arrowBody = arrow.GetComponent<Rigidbody2D>();
        StartCoroutine(AutoMove(bowMove, bow, bowSpeed, true));
        StartCoroutine(AutoMove(targetMove, target, targetSpeed, true));
    }

    private IEnumerator AutoMove(Vector2[] moveTarget, GameObject go, float speed, bool cycle)
    {
        while (cycle)
        {
            if (gameOver) break;
            foreach (var target in moveTarget)
            {
                while (!gameOver)
                {
                    go.transform.localPosition = Vector2.MoveTowards(go.transform.localPosition, target, speed * Time.deltaTime);
                    if (Math.Abs(go.transform.localPosition.x - target.x) < 0.01)
                    {
                        break;
                    }
                    yield return null;
                }
            }
        }
    }

    public void Shoot()
    {
        if (arrow.parent == bow.transform && arrow.localPosition.y == initArrowPos.y)
        {
            Game.Instance.sound.PlayEffect("射箭");
            StartCoroutine(IShoot());
        }
    }

    private void InitArrow()
    {
        arrow.parent = bow.transform;
        arrow.localPosition = initArrowPos;
        arrowBody.velocity = Vector2.zero;
        if (arrowIndex == 0)
        {
            arrow.gameObject.SetActive(false);
            gameOver = true;
            ReGame();
        }
        else
        {
            arrowIndex -= 1;
            arrows[arrowIndex].SetActive(false);
        }
    }

    private void ReGame()
    {
        if (Arrow.Instance.ScoreNum < nums)
        {
            Arrow.Instance.InitScore();
            for (int i = 0; i < 9; i++)
            {
                arrows[i].SetActive(true);
            }
            arrowIndex = 9;
            arrow.parent = bow.transform;
            arrow.localPosition = initArrowPos;
            arrowBody.velocity = Vector2.zero;
            arrow.gameObject.SetActive(true);
            gameOver = false;
            StopAllCoroutines();
            StartCoroutine(Pause(1));
        }
        else
        {
            endGame.SetActive(true);
        }
    }

    private IEnumerator Pause(float t)
    {
        yield return new WaitForSeconds(t);
        StartCoroutine(AutoMove(bowMove, bow, bowSpeed, true));
        StartCoroutine(AutoMove(targetMove, target, targetSpeed, true));
    }

    private IEnumerator IShoot()
    {
        arrow.parent = backGround.transform;
        Vector2 aSpeed = arrowBody.velocity;
        aSpeed.y = arrowSpeed;
        arrowBody.velocity = aSpeed;
        while (arrow.localPosition.y < 11)
        {
            Debug.Log(arrow.localPosition.y);
            if (Arrow.Instance.Hit)
            {
                yield return new WaitForSeconds(1);
                InitArrow();
                break;
            }
            yield return null;
        }
        if (arrow.localPosition.y >= 11)
            InitArrow();
    }

    public void ExitGame()
    {
        Game.state--;
        Game.mapUsed["秦朝"] = false;
        Game.Instance.LoadLevel(5);
        PlayerPrefs.SetString("keyString", "大秦");
    }
}