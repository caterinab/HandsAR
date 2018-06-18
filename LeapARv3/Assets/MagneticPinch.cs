using UnityEngine;
using System.Collections;
using Leap.Unity;

public class MagneticPinch : MonoBehaviour
{

    public float forceSpringConstant;
    public float magnetDistance;
    public GameObject anchorPoint;
    // In the unity component pannel, link the hand you are using here
    public GameObject leapHand;

    protected bool pinching;
    protected bool startPinch;
    protected bool endPinch;
    protected Collider objectGrabbed;

    // Do not forget to add the Leap Motion Orion PinchDetector script to your hand
    protected PinchDetector pinchScript;

    protected Vector3 pinch_position;
    protected Vector3 new_position;

    void Start()
    {
        pinching = false;
        objectGrabbed = null;
        pinchScript = leapHand.GetComponent<PinchDetector>();
    }

    void Update()
    {
        // Gets the state of your pinch
        startPinch = pinchScript.DidStartPinch;
        endPinch = pinchScript.DidEndPinch;
        pinching = pinchScript.IsPinching;

        // Does something once the state of the pinch changes
        if (startPinch)
        {
            Debug.Log("Start pinch");
            pinch_position = pinchScript.Position;
            OnStartPinch(pinch_position);
        }
        else if (endPinch)
        {
            Debug.Log("End pinch");
            OnRelease();
        }

        if (pinching && objectGrabbed != null)
        {
            //pinch_position = pinchScript.Position;
            //new_position = pinch_position - objectGrabbed.transform.position;
            // Any object that should be grabbed must include a Rigidbody
            //Rigidbody objectRB = objectGrabbed.GetComponent<Rigidbody>();
            //if (objectRB.isKinematic) objectRB.isKinematic = false;
            //if (!objectRB.useGravity) objectRB.useGravity = true;
            // Accelerates grabbed object towards the pinch
            //objectRB.AddForce(forceSpringConstant * new_position);
            objectGrabbed.transform.position = pinchScript.Position;
            objectGrabbed.transform.rotation = pinchScript.Rotation;
        }
    }

    /** Finds an object to grab and grabs it. */
    void OnStartPinch(Vector3 pinch_position)
    {
        // Checks if there is  objects around and grabs the closest one
        Collider[] close_things = Physics.OverlapSphere(pinch_position, magnetDistance);

        if (close_things.Length > 0)
        {
            objectGrabbed = close_things[0];
            Debug.Log("Object grabbed = " + objectGrabbed);
        }

        /*
        Debug.Log("Close things: " + close_things.Length);
        for (int j = 0; j < close_things.Length; ++j)
        {
            float distance = Vector3.Distance(pinch_position, close_things[j].transform.position);
            if (close_things[j].GetComponent<Rigidbody>() != null && distance < magnetDistance && !close_things[j].transform.IsChildOf(transform))
            {
                objectGrabbed = close_things[j];

                Debug.Log("Object grabbed = " + objectGrabbed);
            }
        }*/
    }

    void OnRelease()
    {
        //Rigidbody objectRB = objectGrabbed.GetComponent<Rigidbody>();
        //if (!objectRB.isKinematic) objectRB.isKinematic = true;
        //if (objectRB.useGravity) objectRB.useGravity = false;
        objectGrabbed = null;
    }


}