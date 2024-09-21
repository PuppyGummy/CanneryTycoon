using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The profile of the worker
/// </summary>
public class Worker : MonoBehaviour
{
    [Tooltip("How much processing time will cut off with an extra worker of this type.")]
    [Range(0, 0.3f)] public float Efficiency;
    [SerializeField] private bool m_CanMove;
    [SerializeField] private float m_StepTime = 3f;
    [SerializeField] private Vector2 m_WaitingRange;

    private void Start()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, 0f);

        if (m_CanMove)
        {
            StartCoroutine(MovementProcess(true, GameManager.Instance.GeneralReferences.StartingPoints[Random.Range(0, GameManager.Instance.GeneralReferences.StartingPoints.Count)].position));
        }
    }

    private IEnumerator MovementProcess(bool _forward, Vector3 _startPosition)
    {
        float time = 0;
        Vector2 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
        Vector3 endPosition = _forward ? GameManager.Instance.GeneralReferences.EndingPoints[Random.Range(0, GameManager.Instance.GeneralReferences.EndingPoints.Count)].position
            : GameManager.Instance.GeneralReferences.StartingPoints[Random.Range(0, GameManager.Instance.GeneralReferences.StartingPoints.Count)].position;

        while(time < m_StepTime)
        {
            time += Time.deltaTime;
            float t = time / m_StepTime;
            transform.position = Vector3.Lerp(_startPosition, endPosition, t);
            yield return null;
        }

        yield return new WaitForSeconds(Random.Range(m_WaitingRange.x, m_WaitingRange.y));
        StartCoroutine(MovementProcess(!_forward, endPosition));
    }
}
