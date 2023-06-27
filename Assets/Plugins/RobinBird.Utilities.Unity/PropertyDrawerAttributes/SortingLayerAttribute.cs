using UnityEngine;

namespace RobinBird.Utilities.Unity.PropertyDrawerAttributes
{
	/// <summary>
	/// Makes an integer show a dropdown for a Sprite SortingLayer selection <see cref="SortingLayer"/>
	/// </summary>
	public class SortingLayerAttribute : AbstractPropertyDrawerAttribute
	{
		public SortingLayerAttribute(string tooltip) : base(tooltip)
		{
		}
	}
}