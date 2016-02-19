using UnityEngine;
using System.Collections;

public class IndicatorCollisionScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other) {
		//Debug.Log (other.gameObject.name + " : " + other.gameObject.transform.position.z);
		transform.parent.GetComponent<CatcherScript> ().CollisionEnter (other.gameObject);
	}

	void OnTriggerStay(Collider other) {
		transform.parent.GetComponent<CatcherScript> ().CollisionEnter (other.gameObject);
	}

	void OnTriggerExit(Collider other) {
		transform.parent.GetComponent<CatcherScript> ().CollisionExit (other.gameObject);
	}
}
