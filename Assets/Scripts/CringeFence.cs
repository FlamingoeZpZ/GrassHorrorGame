using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class CringeFence : MonoBehaviour
{
    [SerializeField] private Collider prefab;

    private Collider previouis;
    
    [SerializeField, Min(1)] private int dimX;
    [SerializeField, Min(1)] private int dimY;

    [SerializeField] private Vector2 grouping;
    private Vector2 oldGroups;
    
    private int curDimX;
    private int curDimY;

    private Transform tr;

    private void Start()
    {
        if(Application.isPlaying) Destroy(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (curDimX == dimX && curDimY == dimY && previouis == prefab && oldGroups == grouping) return;
        oldGroups = grouping;
        //Clear all
        if(tr) DestroyImmediate(tr.gameObject);

        tr = new GameObject().transform;
        tr.parent = transform;
        tr.name = "Root";

        curDimX = dimX;
        curDimY = dimY;
        previouis = prefab;
        Collider t = Instantiate(prefab);
        Vector3 bounds = t.bounds.size;
        print(bounds);
        for (int x = 0; x < curDimX; ++x)
        {
            for (int y = 0; y < curDimY; ++y)
            {
                Instantiate(prefab, transform.position + new Vector3(x * (bounds.x + oldGroups.x), 0, y * (bounds.y + oldGroups.y)), transform.rotation, tr);
            }
        }
        DestroyImmediate(t.gameObject);
        tr.position -= new Vector3((bounds.x + oldGroups.x), 0, (bounds.y + oldGroups.y)) / 2;
    }
}
