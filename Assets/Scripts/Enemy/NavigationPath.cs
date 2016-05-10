﻿using System.Collections.Generic;
using UnityEngine;

public class NavigationPath : MonoBehaviour
{
    public List<Vector2> Path { get; private set; }

    // Use this for initialization
    private void Start()
    {
        Path = new List<Vector2>();
        FillNavPointList();
    }

    private void FillNavPointList()
    {
        Vector3 tempVec;
        for (var i = 0; i < transform.childCount; i++)
        {
            tempVec = transform.GetChild(i).transform.position;
            Path.Add(new Vector2(tempVec.x, tempVec.z));
        }
    }

    // Update is called once per frame
    private void Update()
    {
    }
}