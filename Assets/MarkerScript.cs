using UnityEngine;
using System.Collections;

public class MarkerScript : MonoBehaviour
{

    CatcherScript catcherScript;

    public Material activeMat, markerMat;

    SongController songController;
    AudioSource trackSource;

    int samplesSinceStart;

    int markerMeasure = 0;

    public float tempo;
    public float timePerBeat;

    float movePerSecond = 0;
    float initPos;

    // Use this for initialization
    void Start()
    {
        catcherScript = GameObject.Find("Catcher").GetComponent<CatcherScript>();
        trackSource = transform.parent.GetComponent<TrackController>().source;
        songController = GameObject.Find("Tracks").GetComponent<SongController>();
        initPos = transform.position.z;
        samplesSinceStart = trackSource.timeSamples;

        tempo = songController.tempo;
        timePerBeat = 60 / tempo;

    }

    void Update()
    {
        if (catcherScript.currentMeasure == markerMeasure)
        {
            GetComponent<Renderer>().material = activeMat;
        }
        else
        {
            GetComponent<Renderer>().material = markerMat;
        }
        int newSamples = trackSource.timeSamples;
        int sampleMove = newSamples - samplesSinceStart;
        samplesSinceStart = newSamples;

        // total distance = 96;
        float timeElapsed = sampleMove / 44100f;
        // 96 / 16 = distance per beat
        // 0.5 = time per beat
        float dist = (timeElapsed / timePerBeat) * (initPos / 16f);

        transform.Translate(Vector3.back * dist);

        if (transform.position.z < -10)
            Destroy(gameObject);
    }

    public void setMeasure(int measure)
    {
        markerMeasure = measure;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //divisionsSinceStart++;
        //transform.Translate (new Vector3(0f, 0f, -(96f / 16f / 8f)));
        //float z = 96 - (divisionsSinceStart * (96f / 16f / 8f));
        //transform.position = new Vector3 (transform.position.x, transform.position.y, z);
        //if (divisionsSinceStart == 128)
        //	Debug.Log (gameObject.name + ": " + transform.position.z);
        /*movePerSecond = (96f / 16f) / pulseController.timePerBeat;
        transform.Translate(new Vector3(0, 0, -movePerSecond * Time.deltaTime));
        transform.localScale = pulseController.vScale;
        if (transform.position.z < -10)
            Destroy (gameObject);
            */

    }
}
