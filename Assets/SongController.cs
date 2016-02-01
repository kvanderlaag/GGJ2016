using UnityEngine;
using System.Collections;

public class SongController : MonoBehaviour {
	GameObject[] audioTracks;
	public GameObject TrackPrefab;
	static GameObject[] tracks;
	public GameObject[] allTracks = new GameObject[18];
	PulseControllerScript pulseController;



	public ParticleSystem trackPS;

	public int delay;

	public float tempo = 120f;
	public float updateTime;
	// Use this for initialization

	public void Init () {
		updateTime = 60 / tempo / 8f;
		Time.fixedDeltaTime = updateTime;
		float startTime = Time.fixedTime;
		float y = 10f / (2 * Mathf.Tan(Mathf.PI / 18f));
		y = 26.35f;
		GameObject drumTrack = Instantiate (TrackPrefab, new Vector3 (0f, 0f, 0f), Quaternion.identity) as GameObject;
		drumTrack.transform.parent = transform;
		drumTrack.GetComponent<TrackController> ().SetTrackType (TrackController.TrackType.Drum);

		GameObject drum2Track = Instantiate (TrackPrefab, new Vector3 (0f, 0f, 0f), Quaternion.identity) as GameObject;
		drum2Track.transform.parent = transform;
		drum2Track.transform.RotateAround (new Vector3 (0f, y, 0f), Vector3.forward, 20);
		drum2Track.GetComponent<TrackController> ().SetTrackType (TrackController.TrackType.Drum2);

		GameObject bassTrack = Instantiate (TrackPrefab, new Vector3 (0f, 0f, 0f), Quaternion.identity) as GameObject;
		bassTrack.transform.parent = transform;
		bassTrack.transform.RotateAround (new Vector3 (0f, y, 0f), Vector3.forward, -20);
		bassTrack.GetComponent<TrackController> ().SetTrackType (TrackController.TrackType.Bass);

		GameObject synthTrack = Instantiate (TrackPrefab, new Vector3 (0f, 0f, 0f), Quaternion.identity) as GameObject;
		synthTrack.transform.parent = transform;
		synthTrack.transform.RotateAround (new Vector3 (0f, y, 0f), Vector3.forward, 40);
		synthTrack.GetComponent<TrackController> ().SetTrackType (TrackController.TrackType.Synth);

		GameObject padTrack = Instantiate (TrackPrefab, new Vector3 (0f, 0f, 0f), Quaternion.identity) as GameObject;
		padTrack.transform.parent = transform;
		padTrack.transform.RotateAround (new Vector3 (0f, y, 0f), Vector3.forward, -40);
		padTrack.GetComponent<TrackController> ().SetTrackType (TrackController.TrackType.Pad);

		float endTime = Time.fixedTime;

		delay = (int) ((endTime - startTime) / updateTime);

		pulseController = GameObject.Find ("GameController").GetComponent<PulseControllerScript> ();
		pulseController.enabled = true;
		audioTracks = GameObject.FindGameObjectsWithTag("AudioTrack");
		foreach (GameObject audioTrack in audioTracks) {
			audioTrack.GetComponent<AudioSource> ().enabled = true;
		}
		//Debug.Log (tracks.Length);

	}
	
	// Update is called once per frame
	void FixedUpdate () {
		
	}


	public void SpawnNext() {

	}

}



