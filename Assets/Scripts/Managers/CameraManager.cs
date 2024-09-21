using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private Transform m_Camera;
    [SerializeField] private float m_MovementSpeed = 2f;
    [SerializeField] private AnimationCurve m_SmoothnessCurve;
    private bool m_CanMove = true;

    private void Awake()
    {
        m_Camera = Camera.main.transform;
    }

    public void MoveCameraToPosition(Vector2 _newPosition)
    {
        if (!m_CanMove) return;

        StartCoroutine(MovementProcess(_newPosition));
    }

    private IEnumerator MovementProcess(Vector2 _newPosition)
    {
        float time = 0f;
        Vector2 startPosition = m_Camera.position;

        while (time < m_MovementSpeed)
        {
            time += Time.deltaTime;
            float t = m_SmoothnessCurve.Evaluate(time / m_MovementSpeed);
            m_Camera.position = Vector2.Lerp(startPosition, _newPosition, t);
            yield return null;
        }
    }
}
