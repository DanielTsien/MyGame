using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class TriggerTest : MonoBehaviour
{
    public CircleCollider2D circleCollider2D;
    private void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log(col.name);
    }

    private void Update()
    {
        if (circleCollider2D.IsTouchingLayers())
        {
            Debug.Log(2222222222222222);
        }
    }
}
