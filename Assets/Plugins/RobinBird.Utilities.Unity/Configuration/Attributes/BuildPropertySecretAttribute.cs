using System;

namespace RobinBird.Utilities.Unity.Configuration.Attributes
{
	/// <summary>
	/// Decorate field in <see cref="BuildProperties"/> to indicate a secret value
	/// Secret values are removed from builds in our CI environment when not explicitly set via the CI environment
	/// This is to protect secrets that leak out but at the same time make sure we can use the secrets in a debug
	/// environment
	/// </summary>
	[AttributeUsage(AttributeTargets.Field)]
	public class BuildPropertySecretAttribute : Attribute
	{
		
	}
}