using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternManager : MonoSingleton<PatternManager>
{
    public List<Pattern> patterns = new List<Pattern>();
}

[Serializable]
public class PatternItem
{
    public string prefabName;
    public Vector3 pos;
}

[Serializable]
public class Pattern
{
    public List<PatternItem> patternItems = new List<PatternItem>();
}