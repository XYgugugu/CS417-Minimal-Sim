using UnityEngine;

public class Exchanger : MonoBehaviour
{
    [SerializeField] private ResourceManager resourceManager;


    public void SampleExchangeType1_100Cost_HalfRate()
    {
        Exchange(1, 100, 0.5f);
    }

    public void Exchange(int source_type, float cost, float scale)
    {
        if (source_type == 1)
        {
            if (resourceManager.ChangeResource1(-cost))
            {
                resourceManager.ChangeResourceRate1(scale, ResourceManager.ChangeMode.SCALE);
            }
            return;
        }
        if (source_type == 2)
        {
            if (resourceManager.ChangeResource2(-cost))
            {
                resourceManager.ChangeResourceRate2(scale, ResourceManager.ChangeMode.SCALE);
            }
            return;
        }
    }
}
