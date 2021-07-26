using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerRun : View
{
    public override string Name => Consts.V_PlayerRun;

    public override void HandleEvent(string enentName, object data)
    {
        throw new System.NotImplementedException();
    }

    public float speed = 10;
    private Vector2 velocity;
    private Rigidbody2D body;
    private bool desiredJump = false;
    public float jumpHeight = 5;
    private bool onGround = true;
    private Vector2 startPos;
    public Text distance;
    public GameObject gameEnd;
    public GameObject ruler;
    public float targetdis = 400;
    private bool GameOver = false;
    private Animator ator;

    private void Awake()
    {
        body = transform.GetComponent<Rigidbody2D>();
        ator = gameObject.GetComponent<Animator>();
        startPos = transform.position;
    }

    // Start is called before the first frame update
    private void Start()
    {
        Game.Instance.sound.PlayBG("SceneBGM");
        StartCoroutine(ShowRuler());
        StartCoroutine(StartGame());
    }

    private void Update()
    {
        if (onGround && !GameOver)
        {
            desiredJump |= Input.GetButtonDown("Jump");
        }
    }

    private void FixedUpdate()
    {
        if (desiredJump && onGround && !GameOver)
        {
            ator.Play("RunJump");
            Game.Instance.sound.PlayEffect("Jump");
            desiredJump = false;
            onGround = false;
            velocity.y = Mathf.Sqrt(-2f * Physics.gravity.y * jumpHeight);
            body.velocity = velocity;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == Tag.Ground)
        {
            onGround = true;
            ator.Play("PlayerRun");
            velocity.y = 0;
        }
        if (collision.collider.tag == Tag.Stone)
        {
            StartCoroutine(nameof(GameFiled));
        }
    }

    private IEnumerator UpdataDistanceUI()
    {
        while (!GameOver)
        {
            float x = (transform.position.x - startPos.x) / 2;
            distance.text = x.ToString("f0") + "m";
            if (x > targetdis)
            {
                GameOver = true;
                gameEnd.SetActive(true);
                body.velocity = Vector3.zero;
                ator.speed = 0;
                Game.Instance.sound.StopAllSound();
                break;
            }
            yield return null;
        }
    }

    private IEnumerator StartGame()
    {
        ator.speed = 0;
        yield return new WaitForSeconds(1);
        InitGame();
    }

    private void InitGame()
    {
        body.WakeUp();
        GameOver = false;
        velocity.x = speed;
        body.velocity = velocity;
        transform.position = startPos;
        ator.speed = 1;
        StartCoroutine(nameof(UpdataDistanceUI));
    }

    private IEnumerator GameFiled()
    {
        body.velocity = Vector2.zero;
        body.Sleep();
        GameOver = true;
        velocity.y = 0;
        ator.speed = 0;
        StopCoroutine(nameof(UpdataDistanceUI));
        yield return new WaitForSeconds(1);

        transform.position = startPos;
        FollowPlayer.Instance.InitPos();
        RoadChange.Instance.initGame();
        StartCoroutine(StartGame());
    }

    private IEnumerator ShowRuler()
    {
        ruler.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        ruler.SetActive(false);
    }

    public void RulerText()
    {
        StartCoroutine(ShowRuler());
    }

    public void ExitGame()
    {
        Game.state--;
        Game.mapUsed["战国"] = false;
        Game.Instance.LoadLevel(3);
        PlayerPrefs.SetString("keyString", "战国");
    }
}