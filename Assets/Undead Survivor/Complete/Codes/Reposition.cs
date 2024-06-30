using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Goldmetal.UndeadSurvivor
{
    public class Reposition : MonoBehaviour
    {
        Collider2D coll; // Reference to the Collider2D component

        void Awake()
        {
            // Get the Collider2D component attached to this GameObject
            coll = GetComponent<Collider2D>();
        }

        void OnTriggerExit2D(Collider2D collision)
        {
            // Check if the object that exited the trigger is tagged as "Area"
            if (!collision.CompareTag("Area"))
                return;

            // Get the position of the player and this object
            Vector3 playerPos = GameManager.instance.player.transform.position;
            Vector3 myPos = transform.position;

            // Handle repositioning based on the tag of this object
            switch (transform.tag) {
                case "Ground":
                    // Calculate the difference and direction between the player and this object
                    float diffX = playerPos.x - myPos.x;
                    float diffY = playerPos.y - myPos.y;
                    float dirX = diffX < 0 ? -1 : 1;
                    float dirY = diffY < 0 ? -1 : 1;
                    diffX = Mathf.Abs(diffX);
                    diffY = Mathf.Abs(diffY);

                    // Reposition the ground object based on the largest difference (horizontal or vertical)
                    if (diffX > diffY) {
                        transform.Translate(Vector3.right * dirX * 40);
                    }
                    else if (diffX < diffY) {
                        transform.Translate(Vector3.up * dirY * 40);
                    }
                    break;

                case "Enemy":
                    // Check if the collider is enabled before repositioning
                    if (coll.enabled) {
                        // Calculate the distance to the player and add a random offset
                        Vector3 dist = playerPos - myPos;
                        Vector3 ran = new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), 0);
                        // Reposition the enemy to a new position
                        transform.Translate(ran + dist * 2);
                    }
                    break;
            }
        }
    }
}
