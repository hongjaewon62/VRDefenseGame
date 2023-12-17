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
        // 왼손 회전 감지
        Quaternion leftHandRotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.LTouch);

        // 왼손 위치 감지
        Vector3 cameraForward = cameraTransform.forward;

        watchUi.transform.position = cameraTransform.position + cameraForward * 1.5f;
        watchUi.transform.rotation = Quaternion.LookRotation(cameraForward);

        // 손목을 돌렸을 때 UI 활성화
        if (leftHandRotation.eulerAngles.z > 200f && leftHandRotation.eulerAngles.z < 300f && !uiActive)
        {
            watchUi.SetActive(true);
            UiManager.uiManager.UpdateHealthText(GameManager.gameManager.currentHealth, GameManager.gameManager.maxHealth);
            UiManager.uiManager.UpdateCurrencyText(GameManager.gameManager.currency);
            uiActive = true;
        }

        // 손목을 원래 위치로 되돌렸을 때 UI 비활성화
        if ((leftHandRotation.eulerAngles.z <= 200f || leftHandRotation.eulerAngles.z >= 300f) && uiActive)
        {
            Debug.Log("watch off");
            watchUi.SetActive(false);
            uiActive = false;
        }
    }
}
