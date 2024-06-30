using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Goldmetal.UndeadSurvivor
{
    public class Follow : MonoBehaviour
    {
        RectTransform rect; // Reference to the RectTransform component

        void Awake()
        {
            // Get the RectTransform component attached to this GameObject
            rect = GetComponent<RectTransform>();
        }

        void FixedUpdate()
        {
            // Update the RectTransform's position to follow the player's position
            // Convert the player's world position to screen position using the main camera
            rect.position = Camera.main.WorldToScreenPoint(GameManager.instance.player.transform.position);
        }
    }
}
