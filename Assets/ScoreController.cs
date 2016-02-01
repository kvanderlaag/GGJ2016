using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreController : MonoBehaviour {

	public Text scoreTxt;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void UpdateScore() {
		int score = GameObject.Find ("Catcher").GetComponent<CatcherScript> ().score;

		scoreTxt.text = "Score: " + score.ToString ();
	}
}
