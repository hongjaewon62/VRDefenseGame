using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyMonster : MonoBehaviour
{
    public AnimationController animController;
    public FacialAnimationController facialAnimationController;
    public Animator monsterAnim;
    // 외형 카테고리들을 저장할 배열의 배열
    public GameObject[][] appearanceOptions;
    // 외형 카테고리들
    public GameObject[] hairOptions;
    public GameObject[] topOptions;
    public GameObject[] bottomOptions;
    public GameObject[] shoeOptions;
    private int randomBaseFacial;
    //메테리얼
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
        InitializeAppearanceOptions(); // 외형 카테고리 배열 초기화
        SetRandomAppearance(); // 랜덤하게 바디, 의상, 헤어 등을 선택하여 적용
        facialAnimationController.SetFacial("NormalMonster", randomBaseFacial);
    }


    private void InitializeAppearanceOptions()
    {
        // 외형 카테고리 배열 생성과 초기화
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

    private void SetRandomAppearance() // 랜덤 의상 생성 코드
    {
        for (int i = 0; i < appearanceOptions.Length; i++)
        {
            // 해당 카테고리의 배열 길이가 0 이상인 경우에만 랜덤한 인덱스 선택
            if (appearanceOptions[i].Length > 0)
            {
                int randomIndex = UnityEngine.Random.Range(0, appearanceOptions[i].Length);
                GameObject selectedAppearancePrefab = appearanceOptions[i][randomIndex];
                GameObject selectedAppearance = Instantiate(selectedAppearancePrefab, transform.position, transform.rotation, transform);
                selectedAppearance.transform.localScale = clothsScale;

                for (int k = 0; k < selectedAppearance.transform.childCount; k++) // 랜덤 메테리얼 적용 코드
                {
                    if (selectedAppearance.transform.GetChild(k).GetComponent<SkinnedMeshRenderer>() != null)
                    {
                        Renderer[] renderers = selectedAppearance.GetComponentsInChildren<Renderer>(); // 모든 하위 렌더러 컴포넌트를 가져옵니다.
                        int randomMaterialIndex = UnityEngine.Random.Range(0, appearanceMaterials[i].Length);
                        foreach (Renderer rend in renderers)
                        {
                            Material[] materials = new Material[rend.sharedMaterials.Length];

                            for (int l = 0; l < materials.Length; l++)
                            {
                                materials[l] = appearanceMaterials[i][randomMaterialIndex]; // 새로운 메테리얼로 모든 메테리얼을 교체합니다.
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
