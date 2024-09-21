using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SystemMessage : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_MessageText;
    [SerializeField] private Animation m_Anim;

    public void DisplayMessage(string _message, Color _color)
    {
        m_MessageText.color = _color;
        m_MessageText.text = _message;
        m_Anim.Play();
    }
}
