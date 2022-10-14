using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class HeroTest : MonoBehaviour
{
    public float Speed = 5f;
    
    private Rigidbody2D m_rigidbody2D;

    private void Awake()
    {
        m_rigidbody2D = GetComponent<Rigidbody2D>();
        
    }

    private void Update()
    {
        InputHandle();
    }

    private void InputHandle()
    {
        Vector2 dir = Vector2.zero;
        if (Input.GetKey(KeyCode.W))
        {
            dir += Vector2.up;
        }
        if (Input.GetKey(KeyCode.S))
        {
            dir += Vector2.down;
        }
        if (Input.GetKey(KeyCode.A))
        {
            dir += Vector2.left;
        }
        if (Input.GetKey(KeyCode.D))
        {
            dir += Vector2.right;
        }
        
        m_rigidbody2D.velocity = dir * Speed;
    }
}
