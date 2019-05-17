using UnityEngine;
using System.Collections;

public class MoveCylinder : MonoBehaviour {

    private Vector3 startingPosition;
    private Quaternion startingRotation;
	// Use this for initialization
	void Start () {
        startingPosition = transform.localPosition;
        startingRotation = transform.rotation;
	}
	

    public void Reset()
    {
        transform.localPosition = startingPosition;
        transform.rotation = startingRotation;
    }
}
