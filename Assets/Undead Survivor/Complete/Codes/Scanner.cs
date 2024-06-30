using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Goldmetal.UndeadSurvivor
{
    public class Scanner : MonoBehaviour
    {
        public float scanRange; // The range within which the scanner looks for targets
        public LayerMask targetLayer; // The layer mask to specify which objects are considered targets
        public RaycastHit2D[] targets; // Array to store the results of the scan
        public Transform nearestTarget; // The nearest target found by the scanner

        void FixedUpdate()
        {
            // Perform a circle cast to find all targets within the scan range
            targets = Physics2D.CircleCastAll(transform.position, scanRange, Vector2.zero, 0, targetLayer);
            // Determine the nearest target
            nearestTarget = GetNearest();
        }

        Transform GetNearest()
        {
            Transform result = null; // To store the nearest target's transform
            float diff = 100; // Initial large value for comparison

            // Iterate through all found targets to find the nearest one
            foreach (RaycastHit2D target in targets) {
                Vector3 myPos = transform.position; // Current position of the scanner
                Vector3 targetPos = target.transform.position; // Position of the target
                float curDiff = Vector3.Distance(myPos, targetPos); // Distance between scanner and target

                // If the current target is closer than the previous closest, update the nearest target
                if (curDiff < diff) {
                    diff = curDiff; // Update the smallest distance found
                    result = target.transform; // Update the nearest target
                }
            }

            return result; // Return the nearest target
        }
    }
}
