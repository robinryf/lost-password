using System;

namespace RobinBird.Utilities.Unity.Configuration.Attributes
{
	/// <summary>
	/// Decorate field in <see cref="BuildProperties"/> to be filled during build by an environment variable
	/// </summary>
	[AttributeUsage(AttributeTargets.Field)]
	public class BuildEnvironmentVariableAttribute : Attribute
	{
		public string EnvVariableName { get; }

		/// <summary>
		/// Specify name of environment variable
		/// </summary>
		/// <param name="envVariableName">e.g. UNITY_BUILD_MY_TOKEN</param>
		public BuildEnvironmentVariableAttribute(string envVariableName)
		{
			EnvVariableName = envVariableName;
		}
	}
}