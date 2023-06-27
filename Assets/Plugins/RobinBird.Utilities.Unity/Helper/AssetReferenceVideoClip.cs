#if ADDRESSABLES_PACKAGE
using System;
using UnityEngine.AddressableAssets;
using UnityEngine.Video;

namespace RobinBird.Utilities.Unity.Helper
{
	[Serializable]
    public class AssetReferenceVideoClip : AssetReferenceT<VideoClip>
    {
	    public AssetReferenceVideoClip(string guid) : base(guid)
	    {
	    }
    }
}
#endif