using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadChange : MonoSingleton<RoadChange>
{
    private GameObject RoadNow;
    private GameObject RoadNext;
    private GameObject roadParent;

    private void Start()
    {
        initGame();
    }

    public void initGame()
    {
        if (RoadNow != null) { Destroy(RoadNow); }
        if (RoadNext != null) { Destroy(RoadNext); }
        roadParent = GameObject.Find("RoadParedt");
        RoadNow = Game.Instance.objectPool.Spawn("奔跑背景", roadParent.transform);
        RoadNext = Game.Instance.objectPool.Spawn("奔跑背景", roadParent.transform);
        RoadNext.transform.position += new Vector3(108, 0, 0);
        AddItem(RoadNow);
        AddItem(RoadNext);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Contains(Tag.Road))
        {
            //回收
            Game.Instance.objectPool.UnSpawn(collision.gameObject);
            //创建新跑道
            SpawnNewRoad();
        }
    }

    private void SpawnNewRoad()
    {
        RoadNow = RoadNext;
        RoadNext = Game.Instance.objectPool.Spawn("奔跑背景", roadParent.transform);
        RoadNext.transform.position = RoadNow.transform.position + new Vector3(108, 0, 0);
        AddItem(RoadNext);
    }

    public void AddItem(GameObject go)
    {
        Transform itemChild = go.transform.Find("Item");
        if (itemChild != null)
        {
            var patternManager = PatternManager.Instance;
            if (patternManager != null && patternManager.patterns.Count > 0)
            {
                var pattern = patternManager.patterns[Random.Range(0, patternManager.patterns.Count)];
                Debug.Log(patternManager.patterns.Count);
                if (pattern != null && pattern.patternItems.Count > 0)
                {
                    foreach (var item in pattern.patternItems)
                    {
                        GameObject obj = Game.Instance.objectPool.Spawn(item.prefabName, itemChild);
                        obj.transform.parent = itemChild;
                        obj.transform.localPosition = item.pos;
                    }
                }
            }
        }
    }
}