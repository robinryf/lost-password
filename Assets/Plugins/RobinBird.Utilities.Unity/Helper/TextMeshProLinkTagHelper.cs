using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RobinBird.Utilities.Unity.Helper
{
	[RequireComponent(typeof(TextMeshProUGUI))]
	public class TextMeshProLinkTagHelper : MonoBehaviour, IPointerClickHandler
	{
		private TextMeshProUGUI textMeshProUGUI;
		private Camera uiCamera;

		private void InitVariables()
		{
			textMeshProUGUI = GetComponent<TextMeshProUGUI>();
			var canvas = GetComponentInParent<Canvas>();
			if (canvas.isRootCanvas == false)
			{
				canvas = canvas.rootCanvas;
			}

			uiCamera = canvas.worldCamera;
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			if (textMeshProUGUI == null)
			{
				InitVariables();
			}
			
			int linkIndex = TMP_TextUtilities.FindIntersectingLink(textMeshProUGUI, Input.mousePosition, uiCamera);
			if( linkIndex != -1 ) { // was a link clicked?
				TMP_LinkInfo linkInfo = textMeshProUGUI.textInfo.linkInfo[linkIndex];

				// open the link id as a url, which is the metadata we added in the text field
				string htmlLink = linkInfo.GetLinkID();
				if (htmlLink.StartsWith("http") == false)
				{
					return;
				}
				Application.OpenURL(htmlLink);
			}
		}
	}
}