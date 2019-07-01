using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;

public class Countdown : MonoBehaviour
{
    public int timeLeft = 31; //Seconds Overall
    public Text countdown; //UI Text Object
    public void Start()
    {
        StartCoroutine("LoseTime");
        Time.timeScale = 1; //Just making sure that the timeScale is right
    }
    public void Update()
    {
        countdown.text = ("Time Left: " + "" + timeLeft); //Showing the Score on the Canvas
    }
    //Simple Coroutine
    public IEnumerator LoseTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            timeLeft--;
        }
    }
}