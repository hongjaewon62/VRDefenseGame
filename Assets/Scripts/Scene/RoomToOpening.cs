using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class RoomToOpening : MonoBehaviour
{
    public void moveScene()
    {
        SceneManager.LoadScene("Cine_OP1_Fail");
    }
}
