using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reticule : MonoBehaviour
{
    [Header("Scene")]
    public Pointer m_Pointer;

    [Header("Object")]
    public SpriteRenderer m_CircleRenderer;

    [Header("Assets")]
    public Sprite m_OpenSprite;
    public Sprite m_CloseSprite;

    private Camera m_Camera = null;

    private void Awake()
    {
        m_Pointer.OnPointerUpdate += UpdateSprite;

        m_Camera = Camera.main;
    }

    private void Update()
    {
        transform.LookAt(m_Camera.gameObject.transform);
    }

    private void OnDestroy()
    {
        m_Pointer.OnPointerUpdate -= UpdateSprite;
    }

    private void UpdateSprite(Vector3 point, GameObject hitObject)
    {
        // Set reticule to end of laser
        transform.position = point;

        // Switch sprites
        if (hitObject)
        {
            m_CircleRenderer.sprite = m_CloseSprite;
        }
        else
        {
            m_CircleRenderer.sprite = m_OpenSprite;
        }
    }
}

