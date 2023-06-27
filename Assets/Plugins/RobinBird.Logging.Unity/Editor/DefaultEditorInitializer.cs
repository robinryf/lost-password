#region Disclaimer
// <copyright file="DefaultEditorInitializer.cs">
// Copyright (c) 2019 - 2019 All Rights Reserved
// </copyright>
// <author>Robin Fischer</author>
#endregion


namespace RobinBird.Logging.Unity.Editor
{
    using UnityEditor;
    
#if ROBIN_BIRD_EDITOR_UTILS
    [InitializeOnLoad]
    public static class DefaultEditorInitializer
    {
        static DefaultEditorInitializer()
        {
            DefaultInitializer.OnRuntimeMethodLoad();
        }
    }
#endif
}