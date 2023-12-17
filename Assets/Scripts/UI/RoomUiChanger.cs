using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RoomUiChanger : MonoBehaviour
{
    public Sprite[] TutorialPic;
    int clickCount;
    int maxCount;

    void Start()
    {
        clickCount = 0;
        GameObject.Find("main").GetComponent<Image>().sprite = TutorialPic[clickCount];
        maxCount = TutorialPic.Length - 1;      //배열 길이 - 1
    }
   public void RightBU()
    {
        Debug.Log("R");
        clickCount++;
        if (clickCount > maxCount)
            clickCount = maxCount;
        GameObject.Find("main").GetComponent<Image>().sprite = TutorialPic[clickCount];
    }
    public void LefttBU()
    {
        Debug.Log("L");
        clickCount--;
        if (clickCount < 0)
            clickCount = 0;
        GameObject.Find("main").GetComponent<Image>().sprite = TutorialPic[clickCount];
    }

}
