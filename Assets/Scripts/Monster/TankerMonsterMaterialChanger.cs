using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankerMonsterMaterialChanger : MonoBehaviour
{

    public GameObject body;

    public Material[] shirtMaterials;
    public Material[] pantMaterials;

    private void Start()
    {
        ChangeMaterial();
    }
    public void ChangeMaterial()
    {
        SkinnedMeshRenderer bodyRenderer = body.GetComponent<SkinnedMeshRenderer>();

        int randomMaterialInstance = Random.Range(0, pantMaterials.Length);
        if (pantMaterials.Length > 0)
        {
            bodyRenderer.materials[0].color = pantMaterials[randomMaterialInstance].color;
            bodyRenderer.materials[0].SetColor("_EmissionColor", pantMaterials[randomMaterialInstance].color);
        }
    
        randomMaterialInstance = Random.Range(0, shirtMaterials.Length);
        if (shirtMaterials.Length > 0)
        {
            bodyRenderer.materials[2].color = pantMaterials[randomMaterialInstance].color;
            bodyRenderer.materials[2].SetColor("_EmissionColor", pantMaterials[randomMaterialInstance].color);
        }
    }

}
