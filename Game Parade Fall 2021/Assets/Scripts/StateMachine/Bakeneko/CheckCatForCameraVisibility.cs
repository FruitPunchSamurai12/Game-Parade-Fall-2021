using UnityEngine;

public class CheckCatForCameraVisibility:MonoBehaviour
{
    SkinnedMeshRenderer rend;

    private void Awake()
    {
        rend = GetComponent<SkinnedMeshRenderer>();
    }

    private void Update()
    {
        if(rend.isVisible)
        {
            Director.Instance.HandleCatCloseToBird();
        }
        else
        {
            Director.Instance.HandleCatAwayFromBird();
        }
    }
}