using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class SafeAreaHandler : MonoBehaviour
{
    private RectTransform panelSafeArea;
    private Rect currentSafeArea = new Rect();
    private ScreenOrientation currentOrientation = ScreenOrientation.AutoRotation;

    void Start()
    {
        panelSafeArea = GetComponent<RectTransform>();
        ApplySafeArea();
    }

    void Update()
    {
        if (currentSafeArea != Screen.safeArea || currentOrientation != Screen.orientation)
            ApplySafeArea();
    }

    void ApplySafeArea()
    {
        currentSafeArea = Screen.safeArea;
        currentOrientation = Screen.orientation;

        // Convert safe area rectangle from absolute pixels to normalised anchor coordinates
        var anchorMin = currentSafeArea.position;
        var anchorMax = currentSafeArea.position + currentSafeArea.size;
        anchorMin.x /= Screen.width;
        anchorMin.y /= Screen.height;
        anchorMax.x /= Screen.width;
        anchorMax.y /= Screen.height;

        // Apply safe area anchors
        panelSafeArea.anchorMin = anchorMin;
        panelSafeArea.anchorMax = anchorMax;
    }
}
