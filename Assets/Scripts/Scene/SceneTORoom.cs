using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneTORoom : MonoBehaviour
{
    private void Start()
    {
        Invoke("moveScenne", 20);
    }

   private void moveScenne()
    {
        SceneManager.LoadScene("Player_Room");
    }
}
