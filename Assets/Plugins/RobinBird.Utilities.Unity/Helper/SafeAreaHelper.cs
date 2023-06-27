using RobinBird.Logging.Runtime;

namespace RobinBird.Utilities.Unity.Helper
{
    using UnityEngine;
    
    /// <summary>
    ///     Applies the safe area to attached RectTransform. Attached RectTransform should be set to scale to parent bounds
    ///     Parent bounds should match the screen size
    /// </summary>
    public class SafeAreaHelper : MonoBehaviour
    {
	    /// <summary>
	    /// Applies this margin to all screens regardless if the device has a safe area or not.
	    /// With this solution we can design our interfaces to be flush to the safe area if one exists.
	    /// </summary>
	    public Vector4 NoSafeAreaMargin;

	    /// <summary>
	    /// The default safe area on the iPhone for the HomeBar (white bar at the bottom) is pretty big. We use this value to specify a custom one 
	    /// </summary>
	    public float iOSHomeBarSafeAreaOverride = 10;
	    
	    private void Awake()
        {
            ApplySafeArea();
        }

	    [ContextMenu("ApplySafeArea")]
        public void ApplySafeArea()
        {
            var rectTransform = GetComponent<RectTransform>();

            if (rectTransform == null)
            {
                return;
            }

            var screenResolution = new Rect(0, 0, Screen.width, Screen.height);
            Rect safeArea = Screen.safeArea;
            var cutouts = Screen.cutouts;
            bool doesSafeAreaExist = screenResolution != safeArea;
            bool doCutoutsExist = cutouts.Length != 0;
            bool allCutoutsAreZero = true;
            foreach (Rect cutout in cutouts)
            {
	            if (cutout.width > 0 || cutout.height > 0)
		            allCutoutsAreZero = false;
            }
            if (doesSafeAreaExist && (doCutoutsExist == false || allCutoutsAreZero))
            {
	            Log.Warn($"Detected device that has a safe area but no cutouts. SafeAreaExists: {doesSafeAreaExist.ToString()}, doCutoutsExist: {doCutoutsExist.ToString()} allCutoutsAreZero: {allCutoutsAreZero.ToString()}");
            }
            if (doesSafeAreaExist == false || doCutoutsExist == false || allCutoutsAreZero) // If we have a safe area but no cutouts we should fallback to the safe area
            {
	            if (Application.isPlaying == false)
	            {
		            safeArea = screenResolution;
		            safeArea.xMin += NoSafeAreaMargin.x;
		            safeArea.yMin += NoSafeAreaMargin.y;
		            safeArea.xMax -= NoSafeAreaMargin.z;
		            safeArea.yMax -= NoSafeAreaMargin.w;
	            }
            }
            else
            {
	            if (PlatformHelper.IsIOSEditorOrRuntime())
	            {
		            var topPart = new Rect(new Vector2(safeArea.xMin, safeArea.yMax),
			            new Vector2(safeArea.width, screenResolution.yMax - safeArea.yMax));
		            var bottomPart = new Rect(new Vector2(safeArea.xMin, screenResolution.yMin),
			            new Vector2(safeArea.width, safeArea.yMin - screenResolution.yMin));
		            
		            Debug.Log($"Got top part: {topPart.ToString()}");
		            Debug.Log($"Got bottom part: {bottomPart.ToString()}");
		            
		            bool topPartClear = true;
		            bool bottomPartClear = true;
		            foreach (Rect cutout in cutouts)
		            {
			            Debug.Log($"Got cutout: {cutout.ToString()}");
			            if (topPart.Overlaps(cutout))
			            {
				            topPartClear = false;
			            }
			            if (bottomPart.Overlaps(cutout))
			            {
				            bottomPartClear = false;
			            }
		            }

		            if (topPartClear)
		            {
			            safeArea.yMax = screenResolution.yMax - iOSHomeBarSafeAreaOverride;
		            }
		            if (bottomPartClear)
		            {
			            safeArea.yMin = screenResolution.yMin + iOSHomeBarSafeAreaOverride;
		            }
	            }
            }

            float xMin = safeArea.xMin;
            float xMax = safeArea.xMax - screenResolution.width;

            float yMin = safeArea.yMin;
            float yMax = safeArea.yMax - screenResolution.height;

            rectTransform.offsetMin = new Vector2(xMin, yMin);
            rectTransform.offsetMax = new Vector2(xMax, yMax);
        }
    }
}