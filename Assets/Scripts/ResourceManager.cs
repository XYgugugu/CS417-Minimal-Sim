using UnityEngine;
using System.Collections;
using TMPro;

public class ResourceManager : MonoBehaviour
{
    public enum ChangeMode
    {
        ADD,
        SCALE
    }

    [Header("Resource")]
    [SerializeField] private TMP_Text resource_1;
    [SerializeField] private TMP_Text resource_2;
    [SerializeField] private string resource_name_1 = "Resource 1";
    [SerializeField] private string resource_name_2 = "Resource 2";
    
    [SerializeField] private float delta_source_per_sec_1 = 1.0f;
    [SerializeField] private float delta_source_per_sec_2 = 0.0f;
    [SerializeField] private float unlockRequirementForResource2 = 500f;

    private float resource_value_1 = 0.0f;
    private float resource_value_2 = 0.0f;

    private Coroutine resourceCoroutine;
    
    void Start()
    {
        resourceCoroutine = StartCoroutine(UpdateResourceRoutine());
        resource_2.enabled = false;
    }

    private IEnumerator UpdateResourceRoutine()
    {
        while (true)
        {
            resource_value_1 += delta_source_per_sec_1;
            resource_value_2 += delta_source_per_sec_2;
            resource_1.text = $"{resource_name_1} ({delta_source_per_sec_1}/s): {resource_value_1}";
            resource_2.text = $"{resource_name_2} ({delta_source_per_sec_2}/s): {resource_value_2}";
            yield return new WaitForSeconds(1f);
        }
    }

    public void UnlockResource2()
    {
        if (unlockRequirementForResource2 < 0f) return;
        if (resource_value_1 < unlockRequirementForResource2) return;

        resource_value_1 -= unlockRequirementForResource2;
        resource_1.text = $"{resource_name_1} ({delta_source_per_sec_1}/s): {resource_value_1}";
        unlockRequirementForResource2 = -1f;
        resource_2.enabled = true;
        delta_source_per_sec_2 = 1f;
    }

    public void ChangeResourceRate1(float delta, ChangeMode mode)
    {
        float newRate = mode == ChangeMode.ADD ? delta_source_per_sec_1 + delta : delta_source_per_sec_1 * delta;
        delta_source_per_sec_1 = newRate < 0.0f ? delta_source_per_sec_1 : newRate;
        resource_1.text = $"{resource_name_1} ({delta_source_per_sec_1}/s): {resource_value_1}";
    }
    public void ChangeResourceRate2(float delta, ChangeMode mode)
    {
        float newRate = mode == ChangeMode.ADD ? delta_source_per_sec_2 + delta : delta_source_per_sec_2 * delta;
        delta_source_per_sec_2 = newRate < 0.0f ? delta_source_per_sec_2 : newRate;
        resource_2.text = $"{resource_name_2} ({delta_source_per_sec_2}/s): {resource_value_2}";
    }
    public bool ChangeResource1(float delta)
    {
        if (resource_value_1 + delta < 0.0f) return false;
        resource_value_1 += delta;
        resource_1.text = $"{resource_name_1} ({delta_source_per_sec_1}/s): {resource_value_1}";
        return true;
    }
    public bool ChangeResource2(float delta)
    {
        if (resource_value_2 + delta < 0.0f) return false;
        resource_value_2 += delta;
        resource_2.text = $"{resource_name_2} ({delta_source_per_sec_2}/s): {resource_value_2}";
        return true;
    }


    void Update()
    {
        
    }

}
