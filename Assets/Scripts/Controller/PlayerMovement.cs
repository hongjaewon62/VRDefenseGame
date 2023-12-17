using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private OVRPlayerController playerController;
    private void Start()
    {
        playerController = GetComponent<OVRPlayerController>();
    }

    private void Update()
    {
        // 점프 임시로 제거
        //if(OVRInput.Get(OVRInput.RawButton.X))
        //{
        //    playerController.Jump();
        //}
    }

    public IEnumerator SpeedIncreaseCoroutine(float duration, float defaultSpeed, float increaseSpeed)
    {
        //gameObject.SetActive(false);
        float time = 0.0f;
        while (time < duration)
        {
            playerController.Acceleration = increaseSpeed;
            time += Time.deltaTime;
            yield return null;
        }

        playerController.Acceleration = defaultSpeed;
        StopCoroutine(SpeedIncreaseCoroutine(duration, defaultSpeed, increaseSpeed));
    }
}