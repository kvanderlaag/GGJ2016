using UnityEngine;
using System.Collections;

public class CatcherScript : MonoBehaviour {

	public PulseControllerScript PulseController;
	public GameController gameController;

	public int maxEnergy;
	public int energyPerNote;
	int energy;
	public int score;

	public GameObject ParticlePrefab;
	public AudioSource streakSource;

	GameObject currentTrack;

	MeshRenderer[] streakBar = new MeshRenderer[7];
	MeshRenderer[] energyBar = new MeshRenderer[8];
	MeshRenderer[] multiplierBar = new MeshRenderer[4];

	Transform[] buttonTransforms = new Transform[3];
	Light[] buttonLights = new Light[3];

	GameObject[] NotesOver = new GameObject[3];

	public int maxStreak;
	public int disableBars = 12;

	int streak = 0;
	int streakDivisions = 0;
	int multiplier = 1;
	bool onStreak = false;

	bool[] active = { false, false, false };
	float[] timeSinceHit = { 0f, 0f, 0f };
	public float pressDelay;
	public float maxScale = 1.5f;
	public float minScale = 1f;

	public float minLight = 1f;
	public float maxLight = 10f;

	// Use this for initialization
	void Start () {

		gameController = GameObject.Find ("GameController").GetComponent<GameController> ();

		energy = (int)(maxEnergy * 0.75f);
		streakSource = GetComponent<AudioSource> ();
		PulseController = GameObject.Find("GameController").GetComponent<PulseControllerScript>();
		buttonTransforms [0] = GameObject.Find ("XIndicator").GetComponent<Transform> ();
		buttonLights [0] = GameObject.Find ("XIndicator").GetComponent<Light> ();
		buttonTransforms [1] = GameObject.Find ("YIndicator").GetComponent<Transform> ();
		buttonLights [1] = GameObject.Find ("YIndicator").GetComponent<Light> ();
		buttonTransforms [2] = GameObject.Find ("BIndicator").GetComponent<Transform> ();
		buttonLights [2] = GameObject.Find ("BIndicator").GetComponent<Light> ();

		streakBar [0] = GameObject.Find ("Dot1").GetComponent<MeshRenderer>();
		streakBar [1] = GameObject.Find ("Dot2").GetComponent<MeshRenderer>();
		streakBar [2] = GameObject.Find ("Dot3").GetComponent<MeshRenderer>();
		streakBar [3] = GameObject.Find ("Dot4").GetComponent<MeshRenderer>();
		streakBar [4] = GameObject.Find ("Dot5").GetComponent<MeshRenderer>();
		streakBar [5] = GameObject.Find ("Dot6").GetComponent<MeshRenderer>();
		streakBar [6] = GameObject.Find ("Dot7").GetComponent<MeshRenderer>();

		energyBar [0] = GameObject.Find ("Bar1").GetComponent<MeshRenderer> ();
		energyBar [1] = GameObject.Find ("Bar2").GetComponent<MeshRenderer> ();
		energyBar [2] = GameObject.Find ("Bar3").GetComponent<MeshRenderer> ();
		energyBar [3] = GameObject.Find ("Bar4").GetComponent<MeshRenderer> ();
		energyBar [4] = GameObject.Find ("Bar5").GetComponent<MeshRenderer> ();
		energyBar [5] = GameObject.Find ("Bar6").GetComponent<MeshRenderer> ();
		energyBar [6] = GameObject.Find ("Bar7").GetComponent<MeshRenderer> ();
		energyBar [7] = GameObject.Find ("Bar8").GetComponent<MeshRenderer> ();

		multiplierBar[0] = GameObject.Find ("Mult1").GetComponent<MeshRenderer> ();
		multiplierBar[1] = GameObject.Find ("Mult2").GetComponent<MeshRenderer> ();
		multiplierBar[2] = GameObject.Find ("Mult3").GetComponent<MeshRenderer> ();
		multiplierBar[3] = GameObject.Find ("Mult4").GetComponent<MeshRenderer> ();
	}

	void FixedUpdate() {
		if (!gameController.started)
			return;
		if (onStreak) {
			streakDivisions++;
			if (streakDivisions / 16 > streak) {
				streak++;
				/*
				if (streak == maxStreak) {
					if (currentTrack.GetComponent<TrackController>().divisionsDisabled == 0) {
						multiplier = Mathf.Min (multiplier + 1, 4);
						streakSource.Play ();
						currentTrack.GetComponent<TrackController> ().DisableTrack (disableBars);
					}
					SetStreak (0, false);
				} else {*/
				SetStreak (streak, false);
				//}
			}
		}

		for (int i = 0; i < 8; i++) {
			if (energy > maxEnergy / 8 * i)
				energyBar [i].enabled = true;
			else {
				energyBar [i].enabled = false;
			}
		}

		for (int i = 0; i < 4; i++) {
			if (multiplier > i)
				multiplierBar [i].enabled = true;
			else {
				multiplierBar [i].enabled = false;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (!gameController.started) {
			return;
		}
		pressDelay = PulseController.timePerBeat / 4;
		
		if (Input.GetButtonDown ("Fire1") && !active [0]) {
			//Debug.Log ("X");
			active [0] = true;
			timeSinceHit[0] = pressDelay;
			buttonTransforms[0].localScale = new Vector3 (maxScale, maxScale, maxScale);
			buttonLights [0].intensity = maxLight;
			if (NotesOver [0]) {
				currentTrack = NotesOver [0].transform.parent.gameObject;
				NotesOver[0].transform.parent.GetComponent<TrackController>().PlayNote(TrackController.Notes.X);
				Destroy (NotesOver [0]);
				GameObject ps = Instantiate (ParticlePrefab, buttonTransforms [0].position, Quaternion.identity) as GameObject;
				ps.transform.parent = NotesOver [0].transform.parent;
				ps.GetComponent<CaptureParticleScript> ().Init (Color.blue);
				energy = Mathf.Min(energy + (energyPerNote * multiplier), maxEnergy);
				score += energyPerNote * multiplier;
				if (!onStreak) {
					onStreak = true;
				}
				if (streak == maxStreak) {
					streakSource.Play ();
					multiplier = Mathf.Min (multiplier + 1, 4);
					score += 100;
					NotesOver[0].transform.parent.GetComponent<TrackController>().DisableTrack (4);
					SetStreak (0, false);
				}
				//Debug.Log ("Caught X");
			} else {
				SetStreak (0);
				multiplier = 1;
				energy = Mathf.Max (0, energy - (energyPerNote / 2));
				GameObject.Find ("MissSource").GetComponent<AudioSource> ().Play ();
			}
		}
		if (Input.GetButtonDown ("Fire2") && !active [1]) {
			if (!gameController.started)
				return;
			//Debug.Log ("Y");
			active [1] = true;
			timeSinceHit [1] = pressDelay;
			buttonTransforms[1].localScale = new Vector3 (maxScale, maxScale, maxScale);
			buttonLights [1].intensity = maxLight;
			if (NotesOver [1]) {
				currentTrack = NotesOver [1].transform.parent.gameObject;
				NotesOver [1].transform.parent.GetComponent<TrackController> ().PlayNote (TrackController.Notes.Y);
				GameObject ps = Instantiate (ParticlePrefab, buttonTransforms [1].position, Quaternion.identity) as GameObject;
				ps.transform.parent = NotesOver [1].transform.parent;
				ps.GetComponent<CaptureParticleScript> ().Init (Color.yellow);
				energy = Mathf.Min (energy + (energyPerNote * multiplier), maxEnergy);
				score += energyPerNote * multiplier;

				if (!onStreak) {
					onStreak = true;
				}
				if (streak == maxStreak) {
					streakSource.Play ();
					multiplier = Mathf.Min (multiplier + 1, 4);
					score += 100;
					NotesOver [1].transform.parent.GetComponent<TrackController> ().DisableTrack (4);
					SetStreak (0, false);
				}
				Destroy (NotesOver [1]);
				//Debug.Log ("Caught Y");
			} else {
				SetStreak (0);
				multiplier = 1;
				energy = Mathf.Max (0, energy - (energyPerNote / 2));
				GameObject.Find ("MissSource").GetComponent<AudioSource> ().Play ();
			}
		}
		if (Input.GetButtonDown ("Fire3") && !active [2]) {
			//Debug.Log ("B");
			active [2] = true;
			timeSinceHit [2] = pressDelay;
			buttonTransforms[2].localScale = new Vector3 (maxScale, maxScale, maxScale);
			buttonLights [2].intensity = maxLight;
			if (NotesOver [2]) {
				currentTrack = NotesOver [2].transform.parent.gameObject;
				NotesOver[2].transform.parent.GetComponent<TrackController>().PlayNote(TrackController.Notes.B);
				GameObject ps = Instantiate (ParticlePrefab, buttonTransforms [2].position, Quaternion.identity) as GameObject;
				ps.transform.parent = NotesOver [2].transform.parent;
				ps.GetComponent<CaptureParticleScript>().Init (Color.red);
				energy = Mathf.Min(energy + (energyPerNote * multiplier), maxEnergy);
				score += energyPerNote * multiplier;
				if (!onStreak) {
					onStreak = true;
				}

				if (streak == maxStreak) {
					streakSource.Play ();
					multiplier = Mathf.Min (multiplier + 1, 4);
					score += 100;
					NotesOver[2].transform.parent.GetComponent<TrackController>().DisableTrack (4);
					SetStreak (0, false);
				}

				Destroy (NotesOver [2]);
				//Debug.Log ("Caught B");
			} else {
				SetStreak (0);
				multiplier = 1;
				energy = Mathf.Max (0, energy - (energyPerNote / 2));
				GameObject.Find ("MissSource").GetComponent<AudioSource> ().Play ();
			}
		}


		for (int i = 0; i < 3; i++) {
			if (!active [i])
				continue;
			timeSinceHit [i] -= Time.deltaTime;
			//Debug.Log ("Time Since Hit " + i + " : " + timeSinceHit);

			if (timeSinceHit [i] <= 0) {
				active [i] = false;
				timeSinceHit [i] = 0;
				buttonTransforms [i].localScale = new Vector3(minScale, minScale, minScale);
			} else {
				float scaleval = (maxScale - minScale) / pressDelay * Time.deltaTime;
				float lightval = (maxLight - minLight) / pressDelay * Time.deltaTime;
				buttonTransforms [i].localScale -= new Vector3 (scaleval, scaleval, scaleval);
				buttonLights [i].intensity -= lightval;
			}
		}

	}

	public void CollisionEnter(GameObject other) {
		if (!gameController.started)
			return;
		if (other.name == "XNote(Clone)") {
			NotesOver [0] = other;
			//currentTrack = other.transform.parent.gameObject;

		} else if (other.name == "YNote(Clone)") {
			NotesOver [1] = other;
			//currentTrack = other.transform.parent.gameObject;

		} else if (other.name == "BNote(Clone)") {
			NotesOver [2] = other;
			//currentTrack = other.transform.parent.gameObject;

		}
	}

	public void CollisionExit(GameObject other) {
		if (!gameController.started)
			return;
		if (other.name == "XNote(Clone)") {
			if (NotesOver [0]) {
				NotesOver [0].transform.parent.GetComponent<TrackController> ().MissNote ();
				NotesOver [0] = null;
				multiplier = 1;
				energy = Mathf.Max(energy - (energyPerNote / 2), 0);
				if (energy == 0) {
					gameController.Lose ();
				}
			}
			SetStreak (0);

		} else if (other.name == "YNote(Clone)") {
			if (NotesOver [1]) {
				NotesOver [1].transform.parent.GetComponent<TrackController> ().MissNote ();
				NotesOver [1] = null;
				multiplier = 1;
				energy = Mathf.Max(energy - (energyPerNote / 2), 0);
				if (energy == 0) {
					gameController.Lose ();
				}
			}
			SetStreak (0);


		} else if (other.name == "BNote(Clone)") {
			if (NotesOver [2]) {
				NotesOver [2].transform.parent.GetComponent<TrackController> ().MissNote ();
				NotesOver [2] = null;
				multiplier = 1;
				SetStreak (0);
				energy = Mathf.Max(energy - (energyPerNote / 2), 0);
				if (energy == 0) {
					gameController.Lose ();
				}
			}
			SetStreak (0);

		}
	}

	public void SetStreak(int streakval, bool reset = true) {
		if (streakval == 0) {
			onStreak = false;
			streakDivisions = 0;
		}

		streak = streakval;
		for (int i = 0; i < 7; i++) {
			if (streakval - 1 >= i) {
				streakBar [i].enabled = true;
			} else {
				streakBar [i].enabled = false;
			}
		}
	}
}
