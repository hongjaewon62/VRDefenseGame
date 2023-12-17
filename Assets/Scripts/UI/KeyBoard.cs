using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class KeyBoard : MonoBehaviour
{
    public TMP_InputField inputField;
    public GameObject[] buttons;
    void Start()
    {
        // �� ��ư�� Ŭ�� �̺�Ʈ �ڵ鷯 �߰�
        for (int i = 0; i < buttons.Length; i++)
        {
            int buttonIndex = i; // Ŭ������ ����Ͽ� ���� �ε��� ����
            buttons[i].GetComponent<Button>().onClick.AddListener(() => NumClick(buttonIndex));
        }
    }

    public void NumClick(int buttonIndex)
    {
        switch(buttonIndex)
        {
            case 0:
                inputField.text += 0;
                break;
            case 1:
                inputField.text += 1;
                break;
            case 2:
                inputField.text += 2;
                break;
            case 3:
                inputField.text += 3;
                break;
            case 4:
                inputField.text += 4;
                break;
            case 5:
                inputField.text += 5;
                break;
            case 6:
                inputField.text += 6;
                break;
            case 7:
                inputField.text += 7;
                break;
            case 8:
                inputField.text += 8;
                break;
            case 9:
                inputField.text += 9;
                break;
            // ���� ��ư
            case 10:
                DeleteLastChar();
                break;
            // �Ϸ� ��ư
            case 11:
                gameObject.GetComponent<Transform>().GetChild(0).gameObject.SetActive(false);
                break;
            default:
                break ;
        }
    }

    private void DeleteLastChar()
    {
        string currentText = inputField.text;
        if (!string.IsNullOrEmpty(currentText))
        {
            // �ؽ�Ʈ �� ���� �� ���ڸ� ����
            inputField.text = currentText.Substring(0, currentText.Length - 1);
        }
    }
}
