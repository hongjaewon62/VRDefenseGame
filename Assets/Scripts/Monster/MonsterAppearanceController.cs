using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAppearanceController : MonoBehaviour
{
    public SkinnedMeshRenderer bodyRenderer;
    public SkinnedMeshRenderer faceRenderer;

    public void SetBodyMesh(Mesh bodyMesh, Material bodyMaterial)
    {
        bodyRenderer.sharedMesh = bodyMesh;
        bodyRenderer.material = bodyMaterial;
    }

    public void SetFaceMesh(Mesh faceMesh, Material faceMaterial)
    {
        faceRenderer.sharedMesh = faceMesh;
        faceRenderer.material = faceMaterial;
    }
}
