using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonsFunctions : MonoBehaviour
{
    public void CameraMovementButton(Transform _newPosition)
    {
        GameManager.Instance.CameraManager.MoveCameraToPosition(_newPosition.position);
    }
}
