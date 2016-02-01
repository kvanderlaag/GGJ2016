using UnityEngine;
using System.Collections;

public class PulseControllerScript : MonoBehaviour {

	SongController songController;

	public float tempo;
	public float maxScale = 2.4f;
	public float minScale = 0.5f;
	public float timePerBeat;
	public float scale;
	float scalePerBeat;
	public Vector3 vScale;
	int thirtyseconds;

	// Use this for initialization
	void Start () {
		songController = GameObject.Find ("Tracks").GetComponent<SongController> ();
		tempo = songController.tempo;
		thirtyseconds = 0;
		timePerBeat = 60 / tempo;
		scale = maxScale;
		scalePerBeat = (maxScale - minScale);
		vScale = new Vector3 (maxScale, maxScale, maxScale);

			
	}

	void FixedUpdate() {
			if (thirtyseconds == 8) {
				scale = maxScale;
				thirtyseconds = 0;
			}
			thirtyseconds++;
			
	}

	// FixedUpdate = 0.01s
	void Update () {
		scale -= scalePerBeat * (Time.deltaTime / timePerBeat);

		vScale.x = scale;
		vScale.y = scale;
		vScale.z = scale;
	}
}
