using UnityEngine;
using System.Collections;

public class TracksTestScript : MonoBehaviour {

	public int numTracks = 1;
	public int startTrack = 1;
	CatcherScript catcherScript;
	int track;
	bool rotateLeft, rotateRight;
	float rotateDegrees;

	float timePerBeat;
	float rotateTime;
	float rotatePerSec;

	float timeSinceRotate;
	float rotateDelay = 0.3f;

	// Use this for initialization
	void Start () {
		catcherScript = GameObject.Find ("Catcher").GetComponent<CatcherScript> ();
		track = startTrack;
		timePerBeat = 60f / 120f;
		rotateTime = timePerBeat / 4;
		rotatePerSec = 20f / rotateTime;
		rotateLeft = rotateRight = false;
		rotateDegrees = 0;
	}

	// Update is called once per frame
	void Update () {
		if (timeSinceRotate == 0f) {
			if ((Input.GetAxis ("Horizontal") < 0) && (!rotateRight && !rotateLeft) && (track > 1)) {
				rotateLeft = true;
				rotateRight = false;
				rotateDegrees = 0;
				catcherScript.SetStreak (0, true);
				track--;
			} else if (Input.GetAxis ("Horizontal") > 0 && (!rotateLeft && !rotateRight) && track < numTracks) {
				rotateRight = true;
				rotateLeft = false;
				rotateDegrees = 0;
				catcherScript.SetStreak (0, true);
				track++;
			}
		} else {
			timeSinceRotate = Mathf.Max (timeSinceRotate - Time.deltaTime, 0f);
		}

		float y = 10f / (2 * Mathf.Tan(Mathf.PI / 18f));
		y = 26.35f;
		Vector3 rotatePoint = new Vector3 (0f, y, 0f);
			

		if (rotateLeft) {
			if (20 - rotateDegrees > rotatePerSec * Time.deltaTime) {
				transform.RotateAround (rotatePoint, Vector3.forward, rotatePerSec * Time.deltaTime);
				//transform.Translate ();
				rotateDegrees += rotatePerSec * Time.deltaTime;
			} else {
				transform.RotateAround (rotatePoint, Vector3.forward, 20 - rotateDegrees);
				rotateDegrees = 0;
				rotateLeft = false;
				timeSinceRotate = rotateDelay;


			}
		} else if (rotateRight) {
			if (20 - rotateDegrees > rotatePerSec * Time.deltaTime) {
				transform.RotateAround (rotatePoint, Vector3.forward, -rotatePerSec * Time.deltaTime);

				rotateDegrees += rotatePerSec * Time.deltaTime;
			} else {
				transform.RotateAround (rotatePoint, Vector3.forward, -(20 - rotateDegrees));

				rotateDegrees = 0;
				rotateRight = false;
				timeSinceRotate = rotateDelay;
			}
		}
	}
}
