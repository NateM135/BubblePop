using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerEvents : MonoBehaviour
{

    private int Score;
    public static int endScore;
    public Text ScoreText;
    public bool possibletoscore;
    public int timeLeft = 5;
    public Text countdownText;

    Scene currentScene;
    string sceneName;

    #region Events
    public static UnityAction OnTouchpadUp = null;
    public static UnityAction OnTouchpadDown = null;
    public static UnityAction<OVRInput.Controller, GameObject> OnControllerSource = null;
    #endregion

    #region Anchors
    public GameObject m_LeftAnchor;
    public GameObject m_RightAnchor;
    public GameObject m_HeadAnchor;
    #endregion

    #region Input
    private Dictionary<OVRInput.Controller, GameObject> m_ControllerSets = null;
    private OVRInput.Controller m_InputSource = OVRInput.Controller.None;
    private OVRInput.Controller m_Controller = OVRInput.Controller.None;
    private bool m_InputActive = true;
    #endregion

    public void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        sceneName = currentScene.name;
        if (sceneName == "Main")
        {
            Score = 0;
        }
        ScoreText.text = "Score: 0";
        DontDestroyOnLoad(this.gameObject);
        StartCoroutine(Timer());

    }

    private IEnumerator Timer()
    {
        if (sceneName == "Main")
        {
            yield return new WaitForSeconds(30.0f);
            endScore = Score;
            SceneManager.LoadScene("GameEnd");
            ScoreText.text = "Score: " + endScore;

        }
    }

    public void UpdateScore()
    {

            Score += 5;
            ScoreText.text = "Score: " + Score.ToString();
        
    }

    private void Awake()
    {
        // Player 
        OVRManager.HMDMounted += PlayerFound;
        OVRManager.HMDUnmounted += PlayerLost;

        // Create pairing for controller, and gameObject
        m_ControllerSets = CreateControllerSets();
    }

    private void OnDestroy()
    {
        // Player 
        OVRManager.HMDMounted -= PlayerFound;
        OVRManager.HMDUnmounted -= PlayerLost;
    }

    private void Update()
    {
        // If input is inactive, do nothing
        if (!m_InputActive)
            return;

        // Check for controller connection
        CheckForController();

        // Check for input source
        CheckInputSource();

        // Check for actual input
        Input();
    }

    #region Controller
    private void CheckForController()
    {
        // Temporary storage
        OVRInput.Controller controllerCheck = m_Controller;

        // Right remote
        if (OVRInput.IsControllerConnected(OVRInput.Controller.RTrackedRemote))
            controllerCheck = OVRInput.Controller.RTrackedRemote;

        // Left remote
        if (OVRInput.IsControllerConnected(OVRInput.Controller.LTrackedRemote))
            controllerCheck = OVRInput.Controller.LTrackedRemote;

        // If no controllers, headset
        if (!OVRInput.IsControllerConnected(OVRInput.Controller.RTrackedRemote) &&
            !OVRInput.IsControllerConnected(OVRInput.Controller.LTrackedRemote))
            controllerCheck = OVRInput.Controller.Touchpad;

        // Update
        m_Controller = UpdateSource(controllerCheck, m_Controller);
    }

    private void CheckInputSource()
    {
        // Temporary storage
        // OVRInput.Controller.Active
        /*
        OVRInput.Controller inputCheck = m_InputSource;

        // Right Remote
        if (OVRInput.GetDown(OVRInput.Button.Any, OVRInput.Controller.RTrackedRemote))
            inputCheck = OVRInput.Controller.RTrackedRemote;

        // Left Remote
        if (OVRInput.GetDown(OVRInput.Button.Any, OVRInput.Controller.LTrackedRemote))
            inputCheck = OVRInput.Controller.LTrackedRemote;

        // Headset
        if (OVRInput.GetDown(OVRInput.Button.Any, OVRInput.Controller.Touchpad))
            inputCheck = OVRInput.Controller.Touchpad;
        */

        // Update
        m_InputSource = UpdateSource(OVRInput.GetActiveController(), m_InputSource);
    }

    private void Input()
    {
        // Touchpad down
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger))
        {
            if (OnTouchpadDown != null)
                OnTouchpadDown();
        }

        // Touchpad up
        if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger))
        {
            if (OnTouchpadUp != null)
                OnTouchpadUp();
        }
    }

    private OVRInput.Controller UpdateSource(OVRInput.Controller check, OVRInput.Controller previous)
    {
        // If the values are the same, do nothing
        if (check == previous)
            return previous;

        // Get controller object, default to head
        GameObject controllerObject = null;
        m_ControllerSets.TryGetValue(check, out controllerObject);

        // If no object, default to head
        if(controllerObject == null)
            controllerObject = m_HeadAnchor;

        // Send out event with new value
        if (OnControllerSource != null)
            OnControllerSource(check, controllerObject);

        // Return new value to set
        return check;
    }
    #endregion

    #region Utility
    private void PlayerFound()
    {
        Debug.Log("Player Found");
        m_InputActive = true;
    }

    private void PlayerLost()
    {
        Debug.Log("Player Lost");
        m_InputActive = false;
    }

    private Dictionary<OVRInput.Controller, GameObject> CreateControllerSets()
    {
        Dictionary<OVRInput.Controller, GameObject> newSets = new Dictionary<OVRInput.Controller, GameObject>()
        {
            { OVRInput.Controller.LTrackedRemote, m_LeftAnchor },
            { OVRInput.Controller.RTrackedRemote, m_RightAnchor },
            { OVRInput.Controller.Touchpad, m_HeadAnchor }

            // Optional
            // { OVRInput.Controller.None, m_HeadAnchor }
        };

        return newSets;
    }
    #endregion
}

