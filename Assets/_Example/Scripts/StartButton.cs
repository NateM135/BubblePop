using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class StartButton : MonoBehaviour
{
    public AudioClip clank1;
    public bool abletobescored;

    private int Score;
    public Text ScoreText;
    public GameObject myObject;


    public void StartPressed()
    {
        AudioSource audio = gameObject.AddComponent<AudioSource>();
        audio.PlayOneShot(clank1, 0.7F);
        SceneManager.LoadScene("Main");

    }




    
}
