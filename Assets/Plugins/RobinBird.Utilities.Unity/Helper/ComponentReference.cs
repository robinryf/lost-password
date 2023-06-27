#if ADDRESSABLES_PACKAGE
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace RobinBird.Utilities.Unity.Helper
{
    public class ComponentReference<TComponent> : AssetReference
    {
	    private AsyncOperationHandle<TComponent> chainOp;
	    
        public ComponentReference(string guid) : base(guid)
        {
        }
        
        public new AsyncOperationHandle<TComponent> InstantiateAsync(Vector3 position, Quaternion rotation, Transform parent = null)
        {
            chainOp = Addressables.ResourceManager.CreateChainOperation<TComponent, GameObject>(base.InstantiateAsync(position, Quaternion.identity, parent), GameObjectReady);
            return chainOp;
        }
       
        public new AsyncOperationHandle<TComponent> InstantiateAsync(Transform parent = null, bool instantiateInWorldSpace = false)
        {
	        chainOp = Addressables.ResourceManager.CreateChainOperation<TComponent, GameObject>(base.InstantiateAsync(parent, instantiateInWorldSpace), GameObjectReady);
	        return chainOp;
        }
        public AsyncOperationHandle<TComponent> LoadAssetAsync()
        {
	        chainOp = Addressables.ResourceManager.CreateChainOperation<TComponent, GameObject>(base.LoadAssetAsync<GameObject>(), GameObjectReady);
	        return chainOp;
        }

        AsyncOperationHandle<TComponent> GameObjectReady(AsyncOperationHandle<GameObject> arg)
        {
            var comp = arg.Result.GetComponent<TComponent>();
            return Addressables.ResourceManager.CreateCompletedOperation<TComponent>(comp, string.Empty);
        }

        public override bool ValidateAsset(Object obj)
        {
            var go = obj as GameObject;
            return go != null && go.GetComponent<TComponent>() != null;
        }
        
        public override bool ValidateAsset(string path)
        {
    #if UNITY_EDITOR
            //this load can be expensive...
            var go = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(path);
            return go != null && go.GetComponent<TComponent>() != null;
    #else
                return false;
    #endif
        }

        public override void ReleaseAsset()
        {
	        base.ReleaseAsset();
	        // Also release the chain operation
	        if (chainOp.IsValid())
		        Addressables.Release(chainOp);
        }
    }
}
#endif