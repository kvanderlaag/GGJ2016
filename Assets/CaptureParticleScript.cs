using UnityEngine;
using System.Collections;

public class CaptureParticleScript : MonoBehaviour {

	ParticleSystem ps;

	// Use this for initialization
	public void Init (Color c) {
		ps = GetComponent<ParticleSystem> ();
		ps.startColor = c;
		ps.Play ();
	}
	
	// Update is called once per frame
	void Update () {
		if (ps) {
			if (!ps.IsAlive ()) {
				Destroy (gameObject);
			}
		}
	}
}
