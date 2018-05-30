using Leap.Unity.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraspDetector : MonoBehaviour
{
    private InteractionBehaviour _intObj;

    void Start()
    {
        _intObj = GetComponent<InteractionBehaviour>();
        _intObj.manager.OnPostPhysicalUpdate += applyXAxisWallConstraint;
    }

    private void applyXAxisWallConstraint()
    {
        // This constraint forces the interaction object to have a positive X coordinate.
        Vector3 objPos = _intObj.rigidbody.position;
        if (objPos.x < 0F)
        {
            objPos.x = 0F;
            _intObj.rigidbody.position = objPos;

            // Zero out any negative-X velocity when the constraint is applied.
            Vector3 objVel = _intObj.rigidbody.velocity;
            if (objVel.x < 0F)
            {
                objVel.x = 0F;
                _intObj.rigidbody.velocity = objVel;
            }
        }
    }
}
