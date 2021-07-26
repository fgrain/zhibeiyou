using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Question : MonoBehaviour
{
    public Text question;
    public string text;
    private string queText;
    public int id;
    public Button submit, next;
    public Toggle A, B, C, D;
    public Queinfo queWarehouse = new Queinfo();
    public Text desAnswer;
    private bool isSubmit = false;

    // Start is called before the first frame update
    private void Start()
    {
        queText = Resources.Load<TextAsset>("Question/" + text).text;
        JsonUtility.FromJsonOverwrite(queText, queWarehouse);
        submit.onClick.AddListener(() => Submit());
        next.onClick.AddListener(() => Next());
        Initque();
        A.onValueChanged.AddListener((bool bo) => SetMyChoice(bo, "A"));
        B.onValueChanged.AddListener((bool bo) => SetMyChoice(bo, "B"));
        C.onValueChanged.AddListener((bool bo) => SetMyChoice(bo, "C"));
        D.onValueChanged.AddListener((bool bo) => SetMyChoice(bo, "D"));
    }

    private void Initque()
    {
        isSubmit = false;
        question.text = queWarehouse.context[id].内容;
        if (queWarehouse.context[id].MyChoice == null)
        {
            A.isOn = false;
            B.isOn = false;
            C.isOn = false;
            D.isOn = false;
        }
        else
        {
            switch (queWarehouse.context[id].MyChoice)
            {
                case "A":
                    A.isOn = true;
                    break;

                case "B":
                    B.isOn = true;
                    break;

                case "C":
                    C.isOn = true;
                    break;

                case "D":
                    D.isOn = true;
                    break;
            }
        }
        A.GetComponentInChildren<Text>().text = "A." + queWarehouse.context[id].A;
        B.GetComponentInChildren<Text>().text = "B." + queWarehouse.context[id].B;
        //C.GetComponentInChildren<Text>().text = "C." + queWarehouse.context[id].C;
        if (queWarehouse.context[id].C == null || queWarehouse.context[id].C == "")
        {
            C.gameObject.SetActive(false);
        }
        else
        {
            C.gameObject.SetActive(true);
            C.GetComponentInChildren<Text>().text = "C." + queWarehouse.context[id].C;
        }
        if (queWarehouse.context[id].D == null || queWarehouse.context[id].D == "")
        {
            D.gameObject.SetActive(false);
        }
        else
        {
            D.gameObject.SetActive(true);
            D.GetComponentInChildren<Text>().text = "D." + queWarehouse.context[id].D;
        }
    }

    public void SetMyChoice(bool bo, string msg)
    {
        if (bo)
        {
            queWarehouse.context[id].MyChoice = msg;
        }
        else
        {
            queWarehouse.context[id].MyChoice = null;
        }
    }

    public void Next()
    {
        if (queWarehouse.context[id].MyChoice == null)
        {
            desAnswer.gameObject.SetActive(true);
            desAnswer.text = "请选择你的答案！";
        }
        else if (!isSubmit)
        {
            desAnswer.gameObject.SetActive(true);
            desAnswer.text = "请确认你的答案！";
        }
        else
        {
            isSubmit = false;
            desAnswer.gameObject.SetActive(false);
            id++;
            Initque();
            if (id == queWarehouse.context.Count - 1)
            {
                next.GetComponentInChildren<Text>().text = "答题结束";
                next.onClick.RemoveAllListeners();
                next.onClick.AddListener(() => GameOver());
            }
        }
    }

    public void Submit()
    {
        desAnswer.gameObject.SetActive(true);
        if (queWarehouse.context[id].MyChoice == queWarehouse.context[id].Answer)
        {
            isSubmit = true;
            Game.Instance.sound.PlayEffect("Correct");
            desAnswer.text = "回答正确！\n" + queWarehouse.context[id].解析;
        }
        else if (queWarehouse.context[id].MyChoice == null)
        {
            desAnswer.text = "请选择你的答案！";
        }
        else
        {
            isSubmit = true;
            Game.Instance.sound.PlayEffect("Error");
            desAnswer.text = "答案错误！正确答案是" + queWarehouse.context[id].Answer + "\n" + queWarehouse.context[id].解析;
        }
    }

    public void GameOver()
    {
        if (!isSubmit)
        {
            if (queWarehouse.context[id].MyChoice == null)
            {
                desAnswer.gameObject.SetActive(true);
                desAnswer.text = "请选择你的答案！";
            }
            else
            {
                desAnswer.gameObject.SetActive(true);
                desAnswer.text = "请确认你的答案！";
            }
        }
        else
        {
            if (Game.state == 10)
            {
                Game.Instance.LoadLevel(16);
            }
            else Game.Instance.LoadLevel(1);
        }
    }
}