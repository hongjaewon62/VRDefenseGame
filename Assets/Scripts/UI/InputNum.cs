using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InputNum : MonoBehaviour
{
    public TMP_InputField inputField;
    public void SaveId()
    {
        GameManager.gameManager.studentId = inputField.text;
        inputField.text = "";
        Debug.Log(GameManager.gameManager.studentId);
        gameObject.SetActive(false);
    }
}
