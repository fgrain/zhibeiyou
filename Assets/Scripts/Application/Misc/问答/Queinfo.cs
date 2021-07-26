using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Context
{
    public string 内容;

    public string A;

    public string B;

    public string C;

    public string D;

    public string Answer;

    public string MyChoice;

    public string 解析;
}

[Serializable]
public class Queinfo
{
    public List<Context> context;
}