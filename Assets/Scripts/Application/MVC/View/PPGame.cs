using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PPGame : View
{
    private static PPGame instance;
    private Rigidbody2D body;
    public Text playerScoreText;
    private float playerScore = 0;
    public Text AIScoreText;
    private int AIScore = 0;
    private Vector3 initPosition;
    public bool gameOver = false;
    private RectTransform rectTransform;
    public Image I_Win;
    public Image I_Lose;
    public Image I_Level;

    public static PPGame Instance { get => instance; }
    public RectTransform RectTransform { get => rectTransform;}
    public float SpeedVertical { get => speedVertical; }

    public override string Name => Consts.V_PPGame;

    [SerializeField, Range(0, 15)] private float speedHorizontal = 6;
    [SerializeField, Range(0, 10)] private float speedVertical = 4;
    private void Awake()
    {
        rectTransform = transform.GetComponent<RectTransform>();
        initPosition = rectTransform.anchoredPosition;
        //Debug.Log(initPosition);
        instance = this;
        body = gameObject.GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        StartCoroutine(UpdataLevel(1));
    }

    private void Update()
    {        
        if (Win())
        {
            body.velocity = Vector2.zero;
            I_Win.gameObject.SetActive(true);
        }
        if (Lose())
        {
            body.velocity = Vector2.zero;
            I_Lose.gameObject.SetActive(true);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!gameOver)
        {
            Vector2 v = body.velocity;
            Collider2D c = collision.collider;
            if (c.tag == "Border")
            {
                speedVertical *= -1;
                v.y = speedVertical;
                body.velocity = v;
            }
            else if (c.tag == "LeftBorder")
            {
                playerScore -=5 ;
                if (playerScore < 0) playerScore = 0;
                playerScoreText.text = playerScore.ToString();
                StartCoroutine(WaitforInit());
            }
            else if (c.tag == "RightBorder")
            {
                AIScore -= 5;
                if (AIScore < 0) AIScore = 0;
                AIScoreText.text = AIScore.ToString();
                StartCoroutine(WaitforInit());
            }
            else if (c.tag == "Beat")
            {              
                if (transform.localPosition.x > 0)
                {
                    AIScore++;
                    AIScoreText.text = AIScore.ToString();
                }
                else
                {
                    playerScore++;
                    playerScoreText.text = playerScore.ToString();
                    UpdateLevel();
                }                
            }
        }
        
    }

    void InitBall()
    {
        gameOver = false;
        transform.localPosition = initPosition;       
        int r = Random.Range(0, 3);
        switch (r)
        {
            case 0:
                speedHorizontal *= -1;
                break;
            case 1:
                speedVertical *= -1;
                break;
            case 2:
                speedHorizontal *= -1;
                speedVertical *= -1;
                break;
        }
        ChangeSpeed();
    }

    void ChangeSpeed()
    {
        Vector2 v = body.velocity;
        v.x = speedHorizontal;
        v.y = SpeedVertical;
        body.velocity = v;
    }

    IEnumerator WaitforInit()
    {
        gameOver = true;
        body.velocity = Vector2.zero;
        yield return new WaitForSeconds(1);
        InitBall();
    }

    public bool Win()
    {
        if (playerScore == 20 || playerScore - AIScore > 10)
        {
            return true;
        }
        else return false;
    }

    public bool Lose()
    {
        if (AIScore - playerScore >= 10) return true;
        else return false;
    }

    void UpdateLevel()
    {
        switch (playerScore / 5)
        {
            case 0:
                speedHorizontal = 6;
                speedVertical = 4;
                StartCoroutine(UpdataLevel(1));
                break;
            case 1:
                speedHorizontal= 7;
                speedVertical = 5;
                StartCoroutine(UpdataLevel(2));
                break;
            case 2:
                speedHorizontal = 8;
                speedVertical = 6;
                StartCoroutine(UpdataLevel(3));
                break;
            case 3:
                speedHorizontal = 9;
                speedVertical = 7;
                StartCoroutine(UpdataLevel(4));
                break;
        }
    }

    IEnumerator UpdataLevel(int level)
    {
        Text t = I_Level.GetComponentInChildren<Text>();
        t.text = "阶 段 " + level.ToString();
        I_Level.gameObject.SetActive(true);
        yield return StartCoroutine(WaitforInit());
        I_Level.gameObject.SetActive(false);
    }

    public override void HandleEvent(string enentName, object data)
    {
        
    }

    private void OnEnable()
    {
        StopAllCoroutines();
    }

    public void ReGame()
    {
        playerScore = 0;
        AIScore = 0;
        rectTransform.position = Vector3.zero;
        AIScoreText.text = AIScore.ToString();
        playerScoreText.text = playerScore.ToString();
        I_Lose.gameObject.SetActive(false);
        UpdateLevel();
    } 
}
