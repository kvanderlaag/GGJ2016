using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameController : MonoBehaviour {

	public bool started = false;
	public SongController songController;
	bool win = false;


	// Use this for initialization
	void Start () {
		songController = GameObject.Find ("Tracks").GetComponent<SongController> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("Submit")) {
			songController.enabled = true;
			songController.Init ();
			GetComponentInChildren<Canvas> ().enabled = false;
			started = true;
		}	
		if (Input.GetButtonDown ("Quit")) {
			Debug.Log ("Quit");
			Application.Quit();
		}
	}

	public void Lose() {
		GetComponent<AudioSource> ().Play ();
		songController.enabled = false;
		started = false;
		GameObject[] tracks = GameObject.FindGameObjectsWithTag ("Track");
		foreach (GameObject t in tracks) {
			Destroy (t);
		}
		StartCoroutine (Reload());


	}

	public void Win() {
		Canvas c = GameObject.Find ("EndCanvas").GetComponent<Canvas> ();
		c.gameObject.GetComponent<ScoreController> ().UpdateScore ();
		c.enabled = true;
		songController.enabled = false;
		started = false;
		win = true;
		StartCoroutine (WinReload());


	}

	IEnumerator Reload() {
		yield return new WaitForSeconds (5);
		SceneManager.LoadScene("Tunnel");

	}

	IEnumerator WinReload() {
		GameObject.Find ("TrackParticles").GetComponent<ParticleSystem> ().startSpeed = GameObject.Find ("TrackParticles").GetComponent<ParticleSystem> ().startSpeed * 3;
		yield return new WaitForSeconds (10);
		SceneManager.LoadScene("Tunnel");
	}
}
