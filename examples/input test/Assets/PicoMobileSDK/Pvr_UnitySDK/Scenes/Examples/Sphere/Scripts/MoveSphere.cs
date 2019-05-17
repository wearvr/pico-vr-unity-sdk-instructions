using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class MoveSphere : MonoBehaviour {


    private Vector3 startingPosition;
	// Use this for initialization
	void Start () {
        startingPosition = transform.localPosition;
        SetGazedAt(false);
	
        gameObject.GetComponent<Rigidbody> ().AddForce ( Vector3.right * 120.0f );
	}
	

	void OnTriggerEnter( Collider other )
	{
	}


    public void SetGazedAt(bool gazedAt)
    {
        GetComponent<Renderer>().material.color = gazedAt ? Color.yellow : Color.blue;
    }

    public void Reset()
    {
        transform.localPosition = startingPosition;
        
        SetGazedAt(false);
        gameObject.GetComponent<Rigidbody>().Sleep();
        gameObject.GetComponent<Rigidbody>().AddForce(Vector3.right * 120.0f);
    }

    public void TeleportRandomly()
    {
        Vector3 direction = Random.onUnitSphere;
        direction.y = Mathf.Clamp(direction.y, 0.5f, 1f);
        float distance = 2 * Random.value + 1.5f;
        transform.localPosition = direction * distance;
    }
}
