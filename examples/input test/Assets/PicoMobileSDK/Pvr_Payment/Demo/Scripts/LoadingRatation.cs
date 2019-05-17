using UnityEngine;
using System.Collections;

public class LoadingRatation : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        this.gameObject.transform.Rotate(new Vector3(0, 0, -4));
	}
}
