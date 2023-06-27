using RobinBird.Utilities.Unity.Extensions;
using RobinBird.Utilities.Unity.Helper;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace RobinBird.Utilities.Unity.Editor.Inspectors
{
	[CustomEditor(typeof(ParticleSystemGrid))]
	public class ParticleSystemGridInspector : GenericInspector<ParticleSystemGrid>
	{
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			if (GUILayout.Button("Recreate Grid"))
			{
				Target.transform.DestroyChilds();

				ComponentUtility.CopyComponent(Target.ParticleTemplate);
				for (int x = 0; x < Target.GridSize.x; x++)
				{
					for (int y = 0; y < Target.GridSize.y; y++)
					{
						if (x == 0 && y == 0)
							continue; // Skip first cell because our original sits here
						var gridElement = new GameObject($"[{x}/{y}]Generated ParticleSystem");
						ComponentUtility.PasteComponentAsNew(gridElement);
						gridElement.transform.SetParent(Target.transform);
						gridElement.transform.localPosition = new Vector3(x * Target.CellSize.x, y * Target.CellSize.y, 0);
					}
				}
			}
		}
	}
}