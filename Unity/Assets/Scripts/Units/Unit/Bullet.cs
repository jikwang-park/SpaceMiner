using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private Transform startPos;

    private Rigidbody rb;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void fire(Transform targetPos, float speed)
    {
        var bullet = Instantiate(gameObject);
        var dir = (targetPos.position - startPos.position).normalized;
        bullet.transform.position = dir * speed;
    }

   
}
