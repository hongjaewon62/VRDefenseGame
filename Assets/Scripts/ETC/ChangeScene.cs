using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    [SerializeField]
    private string sceneName;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (GameManager.gameManager.weaponName == "Bat" || GameManager.gameManager.weaponName == "Racket" || GameManager.gameManager.weaponName == "Wrench")
            {
                GameManager.gameManager.ChangeWeaponSwapEnabled();
                GameManager.gameManager.PlusCurrentWave();
                SceneManager.LoadScene(sceneName);
            }
            else
                Debug.Log("무기 없음");
        }
    }
}
