using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SalmonMovement : MonoBehaviour
{
    [SerializeField] private float m_MovementSpeed = 5f;
    private const float Multiplier = 0.7f;

    public void MoveSalmon(Vector3 _startPosition, Vector3 _endPosition, SystemComponents _newSystem)
    {
        gameObject.SetActive(true);
        StartCoroutine(MovementProcess(_startPosition, _endPosition, _newSystem));
    }

    private IEnumerator MovementProcess(Vector3 _startPosition, Vector3 _endPosition, SystemComponents _newSystem)
    {
        float time = 0f;
        float speed = Vector3.Distance(_startPosition, _endPosition) / m_MovementSpeed * Multiplier;

        while (time < speed)
        {
            time += Time.deltaTime;
            transform.position = Vector3.Lerp(_startPosition, _endPosition, time / speed);
            yield return null;
        }

        _newSystem.Info.OnSalmonRecieved(this, GetComponent<SalmonInfo>());
    }
}
