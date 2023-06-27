namespace RobinBird.Utilities.Unity.Helper
{
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// Helper class to reuse <see cref="YieldInstruction" />s for Coroutines in Unity. All yield instructions can be reused at
    /// any time. <see cref="WaitForSeconds" /> can also be used by multiple routines at the same time because the logice when
    /// to step into the next instruction set is contained within the enumerator and not within the
    /// <see cref="YieldInstruction" />. This prevents allocation that would normally happen when creating these objects.
    /// </summary>
    public static class YieldHelper
    {
        private static readonly Dictionary<float, WaitForSeconds> CachedTimeIntervals = new Dictionary<float, WaitForSeconds>(50);

        public static readonly WaitForEndOfFrame EndOfFrame = new WaitForEndOfFrame();

        public static readonly WaitForFixedUpdate FixedUpdate = new WaitForFixedUpdate();

        public static WaitForSeconds WaitForSeconds(float seconds)
        {
            if (!CachedTimeIntervals.ContainsKey(seconds))
                CachedTimeIntervals.Add(seconds, new WaitForSeconds(seconds));
            return CachedTimeIntervals[seconds];
        }
    }
}