using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Goldmetal.UndeadSurvivor
{
    public class Enemy : MonoBehaviour
    {
        public float speed; // Enemy speed
        public float health; // Current health
        public float maxHealth; // Maximum health
        public RuntimeAnimatorController[] animCon; // Array of animator controllers for different enemy types
        public Rigidbody2D target; // Target to follow (player)

        bool isLive; // Is the enemy alive

        Rigidbody2D rigid; // Rigidbody2D component
        Collider2D coll; // Collider2D component
        Animator anim; // Animator component
        SpriteRenderer spriter; // SpriteRenderer component
        WaitForFixedUpdate wait; // WaitForFixedUpdate instance for knockback coroutine

        void Awake()
        {
            // Get components
            rigid = GetComponent<Rigidbody2D>();
            coll = GetComponent<Collider2D>();
            anim = GetComponent<Animator>();
            spriter = GetComponent<SpriteRenderer>();
            wait = new WaitForFixedUpdate();
        }

        void FixedUpdate()
        {
            if (!GameManager.instance.isLive)
                return;

            if (!isLive || anim.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
                return;

            // Calculate direction and move towards target
            Vector2 dirVec = target.position - rigid.position;
            Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
            rigid.MovePosition(rigid.position + nextVec);
            rigid.velocity = Vector2.zero;
        }

        void LateUpdate()
        {
            if (!GameManager.instance.isLive)
                return;

            if (!isLive)
                return;

            // Flip sprite based on target position
            spriter.flipX = target.position.x < rigid.position.x;
        }

        void OnEnable()
        {
            // Initialize enemy state when enabled
            target = GameManager.instance.player.GetComponent<Rigidbody2D>();
            isLive = true;
            coll.enabled = true;
            rigid.simulated = true;
            spriter.sortingOrder = 2;
            anim.SetBool("Dead", false);
            health = maxHealth;
        }

        // Initialize enemy with spawn data
        public void Init(SpawnData data)
        {
            anim.runtimeAnimatorController = animCon[data.spriteType];
            speed = data.speed;
            maxHealth = data.health;
            health = data.health;
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.CompareTag("Bullet") || !isLive)
                return;

            // Reduce health based on bullet damage
            health -= collision.GetComponent<Bullet>().damage;
            StartCoroutine(KnockBack());

            if (health > 0) {
                // Trigger hit animation and sound if still alive
                anim.SetTrigger("Hit");
                AudioManager.instance.PlaySfx(AudioManager.Sfx.Hit);
            }
            else {
                // Handle enemy death
                isLive = false;
                coll.enabled = false;
                rigid.simulated = false;
                spriter.sortingOrder = 1;
                anim.SetBool("Dead", true);
                GameManager.instance.kill++;
                GameManager.instance.GetExp();

                if (GameManager.instance.isLive)
                    AudioManager.instance.PlaySfx(AudioManager.Sfx.Dead);
            }
        }

        IEnumerator KnockBack()
        {
            yield return wait; // Wait for the next fixed frame update
            Vector3 playerPos = GameManager.instance.player.transform.position;
            Vector3 dirVec = transform.position - playerPos;
            rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);
        }

        void Dead()
        {
            // Deactivate the game object when dead
            gameObject.SetActive(false);
        }
    }
}
