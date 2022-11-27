using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RaceController : MonoBehaviour {
    public int lapsInRace;
    public Text LapInfoText;
    public Text CheckpointInfoText;
    public Text RaceOverText;

    private int nextCheckpointNumber;
    private int checkpointCount;
    private int lapCount;
    private float lapStartTime;
    private bool isRaceActive;
    // Laptimes get stored in a list
    private List<float> lapTimes = new List<float>();
    private Checkpoint activeCheckpoint;

    // Use this for initialization
    void Start() {
        isRaceActive = true;
        lapStartTime = Time.time;
        nextCheckpointNumber = 0;
        lapCount = 0;
        checkpointCount = this.transform.childCount;
        // Assign each of the checkpoints its own number in order in Hierarchy
        for (int i = 0; i < checkpointCount; i++) {
            Checkpoint cp = transform.GetChild(i).GetComponent<Checkpoint>();
            cp.checkpointNumber = i;
            cp.isActiveCheckpoint = false;
        }
        StartRace();
    }

    // Update is called once per frame
    void Update() {
        if (isRaceActive) {
            LapInfoText.text = TimeParser(Time.time - lapStartTime);
            CheckpointInfoText.text = ("CHECKPOINT " + (nextCheckpointNumber + 1) + " / " + checkpointCount + "\nLAP " + (lapCount + 1) + " / " + lapsInRace);
        }
        else {
            LapInfoText.text = "";
            CheckpointInfoText.text = "";
            RaceOverText.color = Color.HSVToRGB(Mathf.Abs(Mathf.Sin(Time.time)), 1, 1);
        }
    }

    public void StartRace() {
        // Set the first checkpoint as the active one
        activeCheckpoint = transform.GetChild(nextCheckpointNumber).GetComponent<Checkpoint>();
        activeCheckpoint.isActiveCheckpoint = true;
        lapStartTime = Time.time;
    }

    public void CheckpointPassed() {
        // A checkpoint was passed, so we make it inactive and activate the next one
        activeCheckpoint.isActiveCheckpoint = false;
        nextCheckpointNumber++;
        if (nextCheckpointNumber < checkpointCount) {
            activeCheckpoint = transform.GetChild(nextCheckpointNumber).GetComponent<Checkpoint>();
            activeCheckpoint.isActiveCheckpoint = true;
        }
        // If a lap was finished, we enter the new lap, and the checkpoint-counter is reset
        else {
            // Add the laptime to the list of laptimes
            // Lisätään kierrosaika kierrosaikojen listaan
            lapTimes.Add(Time.time - lapStartTime);
            lapCount++;
            // Reset the lap timer
            lapStartTime = Time.time;
            nextCheckpointNumber = 0;
            // If the finished lap wasn't the last lap
            if (lapCount < lapsInRace) {
                activeCheckpoint = transform.GetChild(nextCheckpointNumber).GetComponent<Checkpoint>();
                activeCheckpoint.isActiveCheckpoint = true;
            }
            // If final lap, end the game and calculate results
            else {
                isRaceActive = false;
                float raceTotalTime = 0.0f;
                float fastestLapTime = lapTimes[0];
                for (int i = 0; i < lapsInRace; i++) {
                    // Compare the laptimes to pick fastest
                    if (lapTimes[i] < fastestLapTime) {
                        fastestLapTime = lapTimes[i];
                    }
                    // Count total time
                    raceTotalTime += lapTimes[i];
                }
                RaceOverText.text = "RACE COMPLETE!\n\nTotal Time:" + TimeParser(raceTotalTime) + "\nBest Lap: " + TimeParser(fastestLapTime);
            }
        }
    }

    private string TimeParser(float time) {
        float minutes = Mathf.Floor((time) / 60);
        float seconds = Mathf.Floor((time) % 60);
        float msecs = Mathf.Floor(((time) * 100) % 100);
        return (minutes.ToString() + ":" + seconds.ToString("00") + ":" + msecs.ToString("00"));
    }
}