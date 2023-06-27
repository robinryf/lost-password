using System;

namespace RobinBird.Utilities.Unity.Configuration.Attributes
{
	/// <summary>
	/// Decorate field with attribute to fill it during build from command line argument
	/// </summary>
	[AttributeUsage(AttributeTargets.Field)]
	public class BuildCommandLineArgAttribute : Attribute
	{
		public string CLIArgName { get; }

		/// <summary>
		/// Specify CLI argument name
		/// </summary>
		/// <param name="cliArgName">e.g. --my-token</param>
		public BuildCommandLineArgAttribute(string cliArgName)
		{
			CLIArgName = cliArgName;
		}
	}
}