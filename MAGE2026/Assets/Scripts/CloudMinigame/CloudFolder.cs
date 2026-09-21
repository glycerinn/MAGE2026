using UnityEngine;

public class CloudFolder : MonoBehaviour
{
    public CloudType cloudType;

    public bool Accepts(CloudType currentType)
    {
        return currentType == cloudType;
    }
}
