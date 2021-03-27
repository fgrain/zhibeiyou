using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetHigh : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(GetComponent<Renderer>().bounds.size);
        float h = GetComponent<Renderer>().bounds.size.y;
        Vector3 scale = transform.localScale;
        scale.y = (Camera.main.orthographicSize * 2) / h;//屏幕宽/屏幕高=相机宽/相机高,相机宽=图片宽*图片在x轴的缩放系数
        transform.parent.localScale = scale;
        Debug.Log(GetComponent<Renderer>().bounds.size);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
