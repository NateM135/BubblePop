using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;

public class FinalScore : MonoBehaviour
{

    public Text finalScore;

    void Start()
    {
        finalScore.text = "Final Score: " + "" + PlayerEvents.endScore;
        Debug.Log("Final Score: " + "" + PlayerEvents.endScore);
    }

    void Update()
    {
        finalScore.text = "Final Score: " + "" + PlayerEvents.endScore;
    }
}
