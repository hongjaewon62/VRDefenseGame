using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyMonster : MonoBehaviour
{
    public AnimationController animController;
    public FacialAnimationController facialAnimationController;
    public Animator monsterAnim;
    // ���� ī�װ����� ������ �迭�� �迭
    public GameObject[][] appearanceOptions;
    // ���� ī�װ���
    public GameObject[] hairOptions;
    public GameObject[] topOptions;
    public GameObject[] bottomOptions;
    public GameObject[] shoeOptions;
    private int randomBaseFacial;
    //���׸���
    public Material[][] appearanceMaterials;
    public Material[] hairMaterials;
    public Material[] topMaterials;
    public Material[] bottomMaterials;
    public Material[] shoeMaterials;
    public Vector3 clothsScale = new Vector3(11.0f, 11.0f, 9.0f);
    // Start is called before the first frame update
    void Start()
    {
        randomBaseFacial = UnityEngine.Random.Range(0, 2);
        InitializeAppearanceOptions(); // ���� ī�װ� �迭 �ʱ�ȭ
        SetRandomAppearance(); // �����ϰ� �ٵ�, �ǻ�, ��� ���� �����Ͽ� ����
        facialAnimationController.SetFacial("NormalMonster", randomBaseFacial);
    }


    private void InitializeAppearanceOptions()
    {
        // ���� ī�װ� �迭 ������ �ʱ�ȭ
        appearanceOptions = new GameObject[4][];
        appearanceOptions[0] = hairOptions;
        appearanceOptions[1] = topOptions;
        appearanceOptions[2] = bottomOptions;
        appearanceOptions[3] = shoeOptions;
        appearanceMaterials = new Material[4][];
        appearanceMaterials[0] = hairMaterials;
        appearanceMaterials[1] = topMaterials;
        appearanceMaterials[2] = bottomMaterials;
        appearanceMaterials[3] = shoeMaterials;
    }

    private void SetRandomAppearance() // ���� �ǻ� ���� �ڵ�
    {
        for (int i = 0; i < appearanceOptions.Length; i++)
        {
            // �ش� ī�װ��� �迭 ���̰� 0 �̻��� ��쿡�� ������ �ε��� ����
            if (appearanceOptions[i].Length > 0)
            {
                int randomIndex = UnityEngine.Random.Range(0, appearanceOptions[i].Length);
                GameObject selectedAppearancePrefab = appearanceOptions[i][randomIndex];
                GameObject selectedAppearance = Instantiate(selectedAppearancePrefab, transform.position, transform.rotation, transform);
                selectedAppearance.transform.localScale = clothsScale;

                for (int k = 0; k < selectedAppearance.transform.childCount; k++) // ���� ���׸��� ���� �ڵ�
                {
                    if (selectedAppearance.transform.GetChild(k).GetComponent<SkinnedMeshRenderer>() != null)
                    {
                        Renderer[] renderers = selectedAppearance.GetComponentsInChildren<Renderer>(); // ��� ���� ������ ������Ʈ�� �����ɴϴ�.
                        int randomMaterialIndex = UnityEngine.Random.Range(0, appearanceMaterials[i].Length);
                        foreach (Renderer rend in renderers)
                        {
                            Material[] materials = new Material[rend.sharedMaterials.Length];

                            for (int l = 0; l < materials.Length; l++)
                            {
                                materials[l] = appearanceMaterials[i][randomMaterialIndex]; // ���ο� ���׸���� ��� ���׸����� ��ü�մϴ�.
                            }

                            rend.sharedMaterials = materials;
                        }
                    }
                }

                Animator appearanceAnim = selectedAppearance.GetComponent<Animator>();
                if (appearanceAnim == null)
                {
                    appearanceAnim = selectedAppearance.AddComponent<Animator>();
                    animController.SetAnimator(i, appearanceAnim);
                }

                appearanceAnim.runtimeAnimatorController = GetComponent<Animator>().runtimeAnimatorController;


            }

        }
    }
}
