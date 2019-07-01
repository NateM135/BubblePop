using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Interactable : MonoBehaviour
{
    public AudioClip clank1;
    private bool abletobescored;

    //private int Score;
    //public Text ScoreText;
    public GameObject myObject;


    public void Start()
    {
        abletobescored = true;
    }

    public void Pressed()
    {
        StartCoroutine(HideShow());
        
    }


    private IEnumerator HideShow()
    {
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        //play sound 
        AudioSource audio = gameObject.AddComponent<AudioSource>();
        
        // end of audio code

        //makes the bubble visible/invisible after a 3 second delay
        renderer.enabled = false;

        PlayerEvents script = myObject.GetComponent<PlayerEvents>();
        if(abletobescored==true)
        {
            audio.PlayOneShot(clank1, 0.5F);
            script.UpdateScore();
            //sound only plays when points are awarded.
        }

        abletobescored = false;





        yield return new WaitForSeconds(3.0f);
        renderer.enabled = true;
        abletobescored = true;

    }

    
}
