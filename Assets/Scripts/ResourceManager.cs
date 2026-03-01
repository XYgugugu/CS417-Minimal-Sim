using UnityEngine;
using System.Collections;
using TMPro;

public class ResourceManager : MonoBehaviour
{

    [Header("Resource")]
    [SerializeField] private TMP_Text resource_1;
    [SerializeField] private TMP_Text resource_2;
    [SerializeField] private string resource_name_1 = "Resource 1";
    [SerializeField] private string resource_name_2 = "Resource 2";
    
    [SerializeField] private float delta_source_per_sec_1 = 1.0f;
    [SerializeField] private float delta_source_per_sec_2 = 2.0f;

    private float source_value_1 = 0.0f;
    private float source_value_2 = 0.0f;

    private Coroutine resourceCoroutine;
    
    void Start()
    {
        resourceCoroutine = StartCoroutine(UpdateResourceRoutine());
    }

    private IEnumerator UpdateResourceRoutine()
    {
        while (true)
        {
            source_value_1 += delta_source_per_sec_1;
            source_value_2 += delta_source_per_sec_2;
            resource_1.text = $"{resource_name_1} ({delta_source_per_sec_1}/s): {source_value_1}";
            resource_2.text = $"{resource_name_2} ({delta_source_per_sec_2}/s): {source_value_2}";
            yield return new WaitForSeconds(1f);
        }
    }

    void Update()
    {
        
    }

}
