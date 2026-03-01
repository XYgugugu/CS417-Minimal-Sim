using UnityEngine;

public class Generator : MonoBehaviour
{
    
    [SerializeField] private ResourceManager resourceManager;


    public void SampleGenerateType1_10Cost_2ExtraRate()
    {
        Generate(1, 10, 2);
    }
    public void Generate(int source_type, float cost, float delta_rate)
    {
        if (source_type == 1)
        {
            if (resourceManager.ChangeResource1(-cost))
            {
                resourceManager.ChangeResourceRate1(delta_rate, ResourceManager.ChangeMode.ADD);
            }
            return;
        }
        if (source_type == 2)
        {
            if (resourceManager.ChangeResource2(-cost))
            {
                resourceManager.ChangeResourceRate2(delta_rate, ResourceManager.ChangeMode.ADD);
            }
            return;
        }
    }
}
