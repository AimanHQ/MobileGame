using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Goldmetal.UndeadSurvivor
{
    public class Player : MonoBehaviour
    {
        // Public variables
        public Vector2 inputVec; // Stores input direction
        public float speed; // Player movement speed
        public Scanner scanner; // Reference to the Scanner component
        public Hand[] hands; // Array of Hand components attached to the player
        public RuntimeAnimatorController[] animCon; // Array of animator controllers for different player animations

        // Private variables
        Rigidbody2D rigid; // Reference to the Rigidbody2D component
        SpriteRenderer spriter; // Reference to the SpriteRenderer component
        Animator anim; // Reference to the Animator component

        void Awake()
        {
            // Get the required components
            rigid = GetComponent<Rigidbody2D>();
            spriter = GetComponent<SpriteRenderer>();
            anim = GetComponent<Animator>();
            scanner = GetComponent<Scanner>();
            hands = GetComponentsInChildren<Hand>(true); // Get all Hand components in children, including inactive ones
        }

        void OnEnable()
        {
            // Modify speed based on a Character property and set the animator controller
            speed *= Character.Speed;
            anim.runtimeAnimatorController = animCon[GameManager.instance.playerId];
        }

        void Update()
        {
            // If the game is not live, return
            if (!GameManager.instance.isLive)
                return;

            // Handle input (commented out as it uses old input system)
            // inputVec.x = Input.GetAxisRaw("Horizontal");
            // inputVec.y = Input.GetAxisRaw("Vertical");
        }

        void FixedUpdate()
        {
            // If the game is not live, return
            if (!GameManager.instance.isLive)
                return;

            // Calculate the next position and move the player
            Vector2 nextVec = inputVec.normalized * speed * Time.fixedDeltaTime;
            rigid.MovePosition(rigid.position + nextVec);
        }

        void LateUpdate()
        {
            // If the game is not live, return
            if (!GameManager.instance.isLive)
                return;

            // Update the animator's speed parameter
            anim.SetFloat("Speed", inputVec.magnitude);

            // Flip the sprite based on the input direction
            if (inputVec.x != 0) {
                spriter.flipX = inputVec.x < 0;
            }
        }

        void OnCollisionStay2D(Collision2D collision)
        {
            // If the game is not live, return
            if (!GameManager.instance.isLive)
                return;

            // Decrease health over time when colliding
            GameManager.instance.health -= Time.deltaTime * 10;

            // If health drops below 0, handle player death
            if (GameManager.instance.health < 0) {
                // Disable all child objects except the first two
                for (int index = 2; index < transform.childCount; index++) {
                    transform.GetChild(index).gameObject.SetActive(false);
                }

                // Trigger death animation and end the game
                anim.SetTrigger("Dead");
                GameManager.instance.GameOver();
            }
        }

        void OnMove(InputValue value)
        {
            // Update input vector with the new input value
            inputVec = value.Get<Vector2>();
        }
    }
}
