using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public float damage;
    public int per;

    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Init(float damage, int per, Vector3 dir)
    {
        this.damage = damage;
        this.per += per;

        Debug.Log($"Bullet initialized: Damage = {damage}, Per = {per}, Direction = {dir}");



        if (per > -1){
            rb.velocity = dir * 15f;
        }
    }

    void OnTriggerEnter2D (Collider2D collision)
    {
        if (collision.CompareTag("enemy") || per == -1)
         return;



        if (per == -1) {
            rb.velocity =  Vector2.zero;
            gameObject.SetActive(false);
            Debug.Log($"Bullet deactivated: {gameObject.name}");

        }


    }
}
