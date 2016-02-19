using UnityEngine;
using System.Collections;

public class TrackController : MonoBehaviour {

	public TextAsset drumSequenceTxt, drum2SequenceTxt, bassSequenceTxt, synthSequenceTxt, padSequenceTxt;
	TextAsset sequenceTxt;
	bool init = false;

	int divisionsSinceStart = 0;

    int currentMeasure = 0;

	public enum TrackType {
		Drum,
		Drum2,
		Bass,
		Synth,
		Pad
	}

	public enum Notes {
		none,
		X,
		Y,
		B,
		end
	}

	Notes[] sequence = new Notes[12800];

	public AudioSource source;
	public AudioSource missSource;

	public TrackType trackType;

	public int divisionsDisabled = 0;

	public Transform XSpawn, YSpawn, BSpawn, MarkerSpawn;
	public GameObject XNote, YNote, BNote;
	public GameObject trackPlane;

    public GameObject barMarker;
    public GameObject beatMarker;

	public Material drumMat, bassMat, synthMat, padMat;
	public AudioClip drumClip, drum2Clip, bassClip, synthClip, padClip;

	int noteIndex = 0;

	int divisions = 0;
	int noteType = 0;

	// Use this for initialization
	void Init () {
		string sequenceStr = sequenceTxt.ToString();
		int index = 0;
		foreach (char c in sequenceStr) {
			switch (c) {
			case 'X':
				sequence [index++] = Notes.X;
				break;
			case 'Y':
				sequence [index++] = Notes.Y;
				break;
			case 'B':
				sequence [index++] = Notes.B;
				break;
			case '-':
				sequence [index++] = Notes.none;
				break;
			case '\\':
				sequence [index++] = Notes.end;
				break;
			}
		}

		Transform[] transforms = GetComponentsInChildren<Transform> ();
		foreach (Transform t in transforms) {
			if (t.gameObject.name == "XSpawn") {
				XSpawn = t;
			} else if (t.gameObject.name == "YSpawn") {
				YSpawn = t;
			} else if (t.gameObject.name == "BSpawn") {
				BSpawn = t;
			}
            else if (t.gameObject.name == "MarkerSpawn")
            {
                MarkerSpawn = t;
            }
		}
		init = true;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void FixedUpdate() {
		if (init) {
			divisionsSinceStart++;
			if (divisionsDisabled > 0) {
				divisionsDisabled--;
				noteIndex++;
				if (divisionsDisabled == 0) {
					trackPlane.GetComponent<MeshRenderer> ().enabled = true;
				}
			} else {
                if (divisionsSinceStart % 32 == 0)
                {
                    currentMeasure++;
                    GameObject newBarMarker = Instantiate(barMarker, MarkerSpawn.transform.position, MarkerSpawn.transform.rotation) as GameObject;
                    newBarMarker.transform.parent = transform;
                    newBarMarker.GetComponent<MarkerScript>().setMeasure(currentMeasure);
                }
                else if (divisionsSinceStart % 8 == 0)
                {
                    GameObject newBeatMarker = Instantiate(beatMarker, MarkerSpawn.transform.position, MarkerSpawn.transform.rotation) as GameObject;
                    newBeatMarker.transform.parent = transform;
                    newBeatMarker.GetComponent<MarkerScript>().setMeasure(currentMeasure);
                }
				switch (sequence [noteIndex++]) {
				case Notes.X:
					GameObject xNote = Instantiate (XNote, XSpawn.position, Quaternion.identity) as GameObject;
					xNote.transform.parent = transform;
					break;
				case Notes.Y:
					GameObject yNote = Instantiate (YNote, YSpawn.position, Quaternion.identity) as GameObject;
					yNote.transform.parent = transform;
					break;
				case Notes.B:
					GameObject bNote = Instantiate (BNote, BSpawn.position, Quaternion.identity) as GameObject;
					bNote.transform.parent = transform;
					break;
				}
			}
			if (sequence [noteIndex] == Notes.end) {
				GameObject.Find ("GameController").GetComponent<GameController> ().Win ();
			}

		}
	}

	public void SetTrackType(TrackType t) {
		trackType = t;
		switch (t) {
		case TrackType.Drum:
			trackPlane.GetComponent<MeshRenderer> ().material = drumMat;
			sequenceTxt = drumSequenceTxt;
			source.clip = drumClip;
			source.volume = 1.0f;
			break;
		case TrackType.Drum2:
			trackPlane.GetComponent<MeshRenderer> ().material = drumMat;
			sequenceTxt = drum2SequenceTxt;
			source.clip = drum2Clip;
			source.volume = 1.0f;
			break;
		case TrackType.Bass:
			trackPlane.GetComponent<MeshRenderer> ().material = bassMat;
			sequenceTxt = bassSequenceTxt;
			source.clip = bassClip;
			source.volume = 0.7f;
			break;
		case TrackType.Synth:
			trackPlane.GetComponent<MeshRenderer> ().material = synthMat;
			sequenceTxt = synthSequenceTxt;
			source.clip = synthClip;
			source.volume = 0.5f;
			break;
		case TrackType.Pad:
			trackPlane.GetComponent<MeshRenderer> ().material = padMat;
			sequenceTxt = padSequenceTxt;
			source.clip = padClip;
			source.volume = 0.8f;
			break;
		}
		Init ();
	}

	public void PlayNote(Notes n) {
		switch (n) {
		case Notes.X:
		case Notes.Y:
		case Notes.B:
			if (source.mute)
				source.mute = false;
			break;
		}
	}

	public void MissNote() {
		missSource.Play ();
		source.mute = true;
	}

	public void DisableTrack(int bars) {
		source.mute = false;
		trackPlane.GetComponent<MeshRenderer> ().enabled = false;
		Transform[] transforms = GetComponentsInChildren<Transform> ();
		foreach (Transform t in transforms) {
			if (t.gameObject.name == "XNote(Clone)" || t.gameObject.name == "YNote(Clone)" || t.gameObject.name == "BNote(Clone)" ||
                t.gameObject.name == "BarMarker(Clone)" || t.gameObject.name == "BeatMarker(Clone)")
				Destroy (t.gameObject);
		}
		divisionsDisabled = 32 * bars + (32 - divisionsSinceStart % 32);
	}
}