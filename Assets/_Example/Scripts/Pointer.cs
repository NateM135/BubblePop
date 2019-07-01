using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Pointer : MonoBehaviour
{
    public float m_Distance = 10.0f;
    public LineRenderer m_LineRenderer = null;
    public LayerMask m_EverythingMask = 0;
    public LayerMask m_InteractableMask = 0;
    public UnityAction<Vector3, GameObject> OnPointerUpdate = null;

    private Transform m_CurrentOrigin = null;
    private GameObject m_CurrentObject = null;

    Scene currentScene;
    string sceneName;
    int x;



    private void Awake()
    {
        PlayerEvents.OnControllerSource += UpdateOrigin;
        PlayerEvents.OnTouchpadDown += ProcessTouchpadDown;
    }

    private void Start()
    {
        SetLineColor();
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;
        x = 0;
        if(sceneName=="Main")
        {
            x++;
        }
    }

    private void OnDestroy()
    {
        PlayerEvents.OnControllerSource -= UpdateOrigin;
        PlayerEvents.OnTouchpadDown -= ProcessTouchpadDown;
    }

    private void Update()
    {
        // Line renderer
        Vector3 hitPoint = UpdateLine();

        // Is the pointer hitting anything?
        m_CurrentObject = UpdatePointerStatus();

        // Send out update
        if (OnPointerUpdate != null)
            OnPointerUpdate(hitPoint, m_CurrentObject);
    }

    #region Updaters
    private Vector3 UpdateLine()
    {
        // Create ray
        RaycastHit hit = CreateRaycast(m_EverythingMask);

        // Default end
        Vector3 endPosition = m_CurrentOrigin.position + (m_CurrentOrigin.forward * m_Distance);

        // Or based on hit
        if (hit.collider != null)
            endPosition = hit.point;

        // Set positions
        m_LineRenderer.SetPosition(0, m_CurrentOrigin.position);
        m_LineRenderer.SetPosition(1, endPosition);

        return endPosition;
    }

    private void UpdateOrigin(OVRInput.Controller controller, GameObject controllerObject)
    {
        // Set the origin of the pointer
        m_CurrentOrigin = controllerObject.transform;

        // Is the laser visible?
        if (controller == OVRInput.Controller.Touchpad)
        {
            m_LineRenderer.enabled = false;
        }
        else
        {
            m_LineRenderer.enabled = true;
        }
    }

    private GameObject UpdatePointerStatus()
    {
        // Create ray
        RaycastHit hit = CreateRaycast(m_InteractableMask);

        // If hit, true
        if (hit.collider)
            return hit.collider.gameObject;

        // Return nothing
        return null;
    }
    #endregion

    #region Utility
    private RaycastHit CreateRaycast(int layer)
    {
        RaycastHit hit;
        Ray ray = new Ray(m_CurrentOrigin.position, m_CurrentOrigin.forward);
        Physics.Raycast(ray, out hit, m_Distance, layer);

        return hit;
    }

    private void SetLineColor()
    {
        if (!m_LineRenderer)
            return;

        Color endColor = Color.white;
        endColor.a = 0.0f;

        m_LineRenderer.endColor = endColor;
    }

    #endregion

    #region Basic Interaction
    private void ProcessTouchpadDown()
    {
        
        if (!m_CurrentObject)
            return;
        if (x == 0)
        {
            StartButton s = m_CurrentObject.GetComponent<StartButton>();
            s.StartPressed();
        }
        else
        {

            Interactable interactable = m_CurrentObject.GetComponent<Interactable>();
            interactable.Pressed();
            x++;
        }
    }
    #endregion
}

