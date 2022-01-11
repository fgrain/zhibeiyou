using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class SpawnManager : Editor
{
    [MenuItem("Tools/SpawnPattern")]
    private static void PatternSystem()
    {
        Debug.Log("spawnPattern");
        GameObject spawnMannager = GameObject.Find("PatternManager");
        if (spawnMannager != null)
        {
            PatternManager patternManager = spawnMannager.GetComponent<PatternManager>();
            Transform item = Selection.gameObjects[0].transform.Find("Item");
            if (item != null)
            {
                Pattern pattern = new Pattern();
                foreach (Transform child in item)
                {
                    GameObject prefab = PrefabUtility.GetCorrespondingObjectFromSource(child.gameObject);
                    if (prefab != null)
                    {
                        PatternItem patternItem = new PatternItem
                        {
                            prefabName = prefab.name,
                            pos = child.localPosition
                        };
                        pattern.patternItems.Add(patternItem);
                    }
                }
                Debug.Log("addPattern");
                patternManager.patterns.Add(pattern);
            }
        }
    }

    [MenuItem("Tools/ChangeToTrigger")]
    public static void ChangeToTrigger()
    {
        var obj = Selection.gameObjects[0];

        foreach (Transform child in obj.transform)
        {
            if (child != null)
            {
                Collider2D[] colliders = child.GetComponents<Collider2D>();
                foreach (var c in colliders)
                {
                    if (c.CompareTag(Tag.Ground))
                    {
                        c.isTrigger = false;
                    }
                    else c.isTrigger = true;
                }
            }
        }
    }

    [MenuItem("Tools/DelCollider")]
    public static void DelCollider()
    {
        var obj = Selection.gameObjects[0];

        foreach (Transform child in obj.transform)
        {
            foreach (var component in child.gameObject.GetComponents<Component>())
            {
                //if (component.GetType().Name == "MouseTexture" || component.GetType().Name == "PolygonCollider2D")
                //{
                //    DestroyImmediate(component);
                //}
                if (component.GetType().Name == "MouseTexture")
                {
                    DestroyImmediate(component);
                }
            }
        }
    }

    [MenuItem("Tools/AddCollider")]
    public static void AddCollider()
    {
        var obj = Selection.gameObjects[0];

        foreach (Transform child in obj.transform)
        {
            if (child.CompareTag(Tag.Picked) || child.CompareTag(Tag.Select))
            {
                var comp = child.gameObject.GetComponent<PolygonCollider2D>();
                if (comp == null)
                    child.gameObject.AddComponent<PolygonCollider2D>();
                var comM = child.gameObject.GetComponent<MouseTexture>();
                if (comM == null)
                    child.gameObject.AddComponent<MouseTexture>();
            }
        }
    }
}