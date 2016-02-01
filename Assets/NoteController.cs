using UnityEngine;
using System.Collections;

public class NoteController : MonoBehaviour {

	PulseControllerScript pulseController;
	SongController songController;

	int divisionsSinceStart;
	public int compensation;

	float movePerSecond = 0;
	float initPos;

	// Use this for initialization
	void Start () {
		pulseController = GameObject.Find("GameController").GetComponent<PulseControllerScript>();
		songController = GameObject.Find ("Tracks").GetComponent<SongController> ();
		initPos = transform.position.z;
		divisionsSinceStart = compensation;
	}

	void Update() {
		//transform.Translate (new Vector3 (0f, 0f, -(Time.deltaTime / pulseController.timePerBeat) *(96f / 16f)));
		transform.localScale = pulseController.vScale;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		divisionsSinceStart++;
		//transform.Translate (new Vector3(0f, 0f, -(96f / 16f / 8f)));
		float z = 96 - (divisionsSinceStart * (96f / 16f / 8f));
		transform.position = new Vector3 (transform.position.x, transform.position.y, z);
		if (divisionsSinceStart == 128)
			Debug.Log (gameObject.name + ": " + transform.position.z);
		/*movePerSecond = (96f / 16f) / pulseController.timePerBeat;
		transform.Translate(new Vector3(0, 0, -movePerSecond * Time.deltaTime));
		transform.localScale = pulseController.vScale;
		if (transform.position.z < -10)
			Destroy (gameObject);
			*/

	}
}
