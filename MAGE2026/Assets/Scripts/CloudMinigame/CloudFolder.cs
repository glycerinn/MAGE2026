using UnityEngine;

public class CloudFolder : MonoBehaviour
{
    public CloudType cloudType;

    public bool Accepts(CloudType currentType)
    {
        bool accepted = currentType == cloudType;
        return accepted;
    }
}