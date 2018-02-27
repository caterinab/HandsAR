using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSpinnerBehaviour : MonoBehaviour {

    private Vector3 originalPosition;
    float xAngle;
    private Rigidbody rb;

	// Use this for initialization
	void Start () {
        originalPosition = transform.localPosition;
        xAngle = transform.eulerAngles.x;
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        transform.localPosition = originalPosition;
        transform.eulerAngles = new Vector3(xAngle, 0, transform.eulerAngles.z);
	}

    void OnCollisionExit(Collision col) {
        rb.AddForce(col.transform.position);
    }
}
