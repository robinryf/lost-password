#if TEXT_MESH_PRO_PACKAGE
using TMPro;
using UnityEngine;

namespace RobinBird.Utilities.Unity.Configuration
{
    public class BuildPropertiesView : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI versionText;
        
        private void Awake()
        {
            var properties = BuildProperties.Default;
            versionText.text = properties.GetFullVersionName();
        }
    }
}
#endif