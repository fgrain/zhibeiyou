using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ruler : MonoBehaviour
{
    private GameObject ruler;

    private void Start()
    {
        ruler = GameObject.Find("Ruler");
        StartCoroutine(ShowRuler());
    }

    private IEnumerator ShowRuler()
    {
        ruler.SetActive(true);
        yield return new WaitForSeconds(3);
        ruler.SetActive(false);
    }

    public void RulerText()
    {
        StartCoroutine(ShowRuler());
    }
}