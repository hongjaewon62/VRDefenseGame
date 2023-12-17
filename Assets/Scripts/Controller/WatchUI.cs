using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WatchUI : MonoBehaviour
{
    [SerializeField]
    private GameObject watchUi;
    [SerializeField]
    private Transform cameraTransform;


    private bool uiActive = false;

    private void Update()
    {
        WatchUiActive();
    }

    private void WatchUiActive()
    {
        // �޼� ȸ�� ����
        Quaternion leftHandRotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.LTouch);

        // �޼� ��ġ ����
        Vector3 cameraForward = cameraTransform.forward;

        watchUi.transform.position = cameraTransform.position + cameraForward * 1.5f;
        watchUi.transform.rotation = Quaternion.LookRotation(cameraForward);

        // �ո��� ������ �� UI Ȱ��ȭ
        if (leftHandRotation.eulerAngles.z > 200f && leftHandRotation.eulerAngles.z < 300f && !uiActive)
        {
            watchUi.SetActive(true);
            UiManager.uiManager.UpdateHealthText(GameManager.gameManager.currentHealth, GameManager.gameManager.maxHealth);
            UiManager.uiManager.UpdateCurrencyText(GameManager.gameManager.currency);
            uiActive = true;
        }

        // �ո��� ���� ��ġ�� �ǵ����� �� UI ��Ȱ��ȭ
        if ((leftHandRotation.eulerAngles.z <= 200f || leftHandRotation.eulerAngles.z >= 300f) && uiActive)
        {
            Debug.Log("watch off");
            watchUi.SetActive(false);
            uiActive = false;
        }
    }
}
