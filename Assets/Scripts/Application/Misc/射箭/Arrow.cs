using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Arrow : MonoSingleton<Arrow>
{
    private bool hit = false;
    private int scoreNum = 0;
    public Text scoreText;
    public GameObject AddAnim;
    private Text addText;
    private Animator ator;
    public bool Hit { get => hit; }
    public int ScoreNum { get => scoreNum; }

    private void Start()
    {
        addText = AddAnim.GetComponent<Text>();
        ator = AddAnim.GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == Tag.Goal)
        {
            Game.Instance.sound.PlayEffect("中靶");
            transform.parent = collision.transform;
            gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            float targetx = Mathf.Abs(0.7f - transform.localPosition.x) * 10;
            int score = 10 - (int)targetx;
            scoreNum += score;
            StartCoroutine(AddScore(score));
            scoreText.text = scoreNum.ToString();
            hit = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.tag == Tag.Goal)
        {
            hit = false;
        }
    }

    public void InitScore()
    {
        scoreNum = 0;
        scoreText.text = scoreNum.ToString();
    }

    private IEnumerator AddScore(int score)
    {
        AddAnim.SetActive(true);
        addText.text = "+" + score.ToString();
        yield return new WaitForSeconds(1);
        AddAnim.SetActive(false);
    }
}