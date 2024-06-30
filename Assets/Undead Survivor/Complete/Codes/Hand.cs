using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Goldmetal.UndeadSurvivor
{
    // The Hand class manages the behavior and properties of the player's hand.
    public class Hand : MonoBehaviour
    {
        // Public fields for hand attributes.
        public bool isLeft; // Indicates if this hand is the left hand.
        public SpriteRenderer spriter; // The sprite renderer for the hand.

        // Private fields for internal use.
        SpriteRenderer player; // The player's sprite renderer.

        // Positions and rotations for the hand based on player facing direction.
        Vector3 rightPos = new Vector3(0.35f, -0.15f, 0);
        Vector3 rightPosReverse = new Vector3(-0.15f, -0.15f, 0);
        Quaternion leftRot = Quaternion.Euler(0, 0, -35);
        Quaternion leftRotReverse = Quaternion.Euler(0, 0, -135);

        // Awake is called when the script instance is being loaded.
        void Awake()
        {
            // Initialize the player reference by getting the second parent sprite renderer.
            player = GetComponentsInParent<SpriteRenderer>()[1];
        }

        // LateUpdate is called once per frame, after all Update functions have been called.
        void LateUpdate()
        {
            // Check if the player sprite is flipped.
            bool isReverse = player.flipX;

            if (isLeft)
            {
                // Adjust the left hand's rotation, sprite flipping, and sorting order.
                transform.localRotation = isReverse ? leftRotReverse : leftRot;
                spriter.flipY = isReverse;
                spriter.sortingOrder = isReverse ? 4 : 6;
            }
            else
            {
                // Adjust the right hand's position, sprite flipping, and sorting order.
                transform.localPosition = isReverse ? rightPosReverse : rightPos;
                spriter.flipX = isReverse;
                spriter.sortingOrder = isReverse ? 6 : 4;
            }
        }
    }
}
