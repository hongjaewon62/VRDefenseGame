using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Craft
{
    public string craftName;
    public GameObject gameObjectPrefab;                 // 실제 설치될 프리팹
    public GameObject gameObjectPreviewPrefab;          // 미리보기 프리팹
}
public class InstallObject : MonoBehaviour
{
    [SerializeField]
    private Craft[] craftItem;
    private GameObject preview;
    private GameObject prefab;

    public int clickNum = 0;

    private Transform playerTransform;

    public bool isPreviewActivated = false;

    private RaycastHit hitInfo;
    [SerializeField]
    private LayerMask layerMask;
    [SerializeField]
    private float range;                // RaycastHit 범위

    private void Awake()
    {
        playerTransform = gameObject.transform.root;
    }

    private void Update()
    {
        if(isPreviewActivated)
        {
            PreviewPositionUpdate();
        }
    }

    public void Installation(int itemNumber)                // 미리보기 생성
    {
        preview = Instantiate(craftItem[itemNumber].gameObjectPreviewPrefab, playerTransform.position + playerTransform.forward, Quaternion.identity);
        prefab = craftItem[itemNumber].gameObjectPrefab;
        isPreviewActivated = true;
    }

    private void PreviewPositionUpdate()            // 미리보기 위치 지정
    {
        if(Physics.Raycast(gameObject.transform.position, gameObject.transform.forward, out hitInfo, range, layerMask))
        {
            if(hitInfo.transform != null)
            {
                Vector3 location = hitInfo.point;
                preview.transform.position = location;
            }
        }
    }

    public void Build()                         // 실제 프리팹 생성
    {
        if(isPreviewActivated)
        {
            GameObject buildObject = Instantiate(prefab, preview.transform.position + new Vector3(0, 0.05f, 0), Quaternion.identity);
            buildObject.GetComponent<HoverItem2>().itemRotation = false;
            Destroy(preview);
            isPreviewActivated = false;
            preview = null;
            prefab = null;
            clickNum = 0;
        }
    }
}
