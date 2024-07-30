using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScaler : MonoBehaviour
{
    private Transform parentPlatform;
    private Vector3 originalPlayerScale;
    private Vector3 originalLocalScale;

    void Start()
    {
        originalPlayerScale = transform.localScale;
    }

    public void SetParentPlatform(Transform platform)
    {
        parentPlatform = platform;
        originalLocalScale = transform.localScale;
        transform.SetParent(platform);
        UpdateScale();
    }

    public void ClearParentPlatform()
    {
        transform.SetParent(null);
        transform.localScale = originalPlayerScale;
        parentPlatform = null;
    }

    void Update()
    {
        if (parentPlatform != null)
        {
            UpdateScale();
        }
    }

    void UpdateScale()
    {
        Vector3 inversePlatformScale = new Vector3(
            1f / parentPlatform.lossyScale.x,
            1f / parentPlatform.lossyScale.y,
            1f / parentPlatform.lossyScale.z
        );

        Vector3 newScale = Vector3.Scale(originalLocalScale, inversePlatformScale);

        // Adjust the scale based on the player's rotation
        float angleY = transform.eulerAngles.y;
        float cosAngle = Mathf.Cos(angleY * Mathf.Deg2Rad);
        float sinAngle = Mathf.Sin(angleY * Mathf.Deg2Rad);

        transform.localScale = new Vector3(
            newScale.x * Mathf.Abs(cosAngle) + newScale.z * Mathf.Abs(sinAngle),
            newScale.y,
            newScale.x * Mathf.Abs(sinAngle) + newScale.z * Mathf.Abs(cosAngle)
        );
    }
}
