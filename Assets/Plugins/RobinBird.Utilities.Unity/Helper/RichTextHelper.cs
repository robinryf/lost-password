namespace RobinBird.Utilities.Unity.Helper
{
    /// <summary>
    /// Helper to apply TextMeshPro rich text tags to strings.
    /// <seealso cref="http://digitalnativestudios.com/textmeshpro/docs/rich-text/"/>
    /// </summary>
    public static class RichTextHelper
    {
        public const string PositiveNumberMaterial = "Positive";
        public const string NegativeNumberMaterial = "Negative";

        public static string EncloseInMaterial(string input, string materialName)
        {
            return $"<material={materialName}>{input}</material>";
        }
    }
}