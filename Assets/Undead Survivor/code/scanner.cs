using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class scanner : MonoBehaviour
{
    public float scanRange;
    public LayerMask targetLayer;
    public RaycastHit2D[] targets;
    public Transform nearestTarget;

    void FixedUpdate()
    {
        targets = Physics2D.CircleCastAll(transform.position, scanRange, Vector2.zero, 0, targetLayer);
        nearestTarget = GetNearest();
    }
    
    Transform GetNearest()
    {
        Transform result = null;
        float diff = 100;

        foreach (RaycastHit2D target in targets){
            Vector3 myPos = transform.position;
            Vector3 targetPos =  target.transform.position;
            float curdiff = Vector3.Distance(myPos, targetPos);

            if (curdiff < diff){
                diff = curdiff;
                result = target.transform;
            }
        }


        return result;
    }
}
