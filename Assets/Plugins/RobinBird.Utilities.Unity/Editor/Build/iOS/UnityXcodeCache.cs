using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace RobinBird.Utilities.Unity.Editor.Build.iOS
{
    [CreateAssetMenu(fileName = nameof(UnityXcodeCache), menuName = CreateAssetMenuItemName + nameof(UnityXcodeCache))]
    public class UnityXcodeCache : AbstractCIBuildCallbackHandler
    {
        public override bool OnPreBuild(BuildTarget target, BuildOptions buildOptions, string buildPath)
        {
            if (target == BuildTarget.iOS)
                ReadCache(GetCachePath(buildPath), buildPath);
            return true;
        }

        public override void OnPostBuild(BuildReport report, BuildTarget target, BuildOptions buildOptions, string buildPath)
        {
            if (target == BuildTarget.iOS)
                WriteCache(GetCachePath(buildPath), buildPath);
        }
    
        private static void WriteCache(string cacheDir, string xcodeProjectDir)
        {
            var sw = new Stopwatch();
            sw.Start();

            var output = new StringBuilder();
            var psi = new ProcessStartInfo();
            psi.FileName = "/usr/bin/rsync";
            psi.Arguments = string.Format("--archive --delete {0} {1}", xcodeProjectDir, cacheDir);
            psi.UseShellExecute = false;
            psi.RedirectStandardError = true;

            var process = new Process();
            process.StartInfo = psi;
            process.ErrorDataReceived += (sender, e) => {
                if(string.IsNullOrEmpty(e.Data))
                    return;

                output.AppendLine(e.Data);
            };
            process.Start();
            process.BeginErrorReadLine();
            process.WaitForExit();

            if (process.ExitCode != 0)
            {
                UnityEngine.Debug.LogError(output);
            }

            sw.Stop();
            UnityEngine.Debug.Log(string.Format("UnityXcodeCache: Write takes {0} ms", sw.ElapsedMilliseconds));
        }

        private static void ReadCache(string cacheDir, string xcodeProjectDir)
        {
            var sw = new Stopwatch();
            sw.Start();

            CompareDirectory(cacheDir, xcodeProjectDir);

            sw.Stop();
            UnityEngine.Debug.Log(string.Format("UnityXcodeCache: Read takes {0} ms", sw.ElapsedMilliseconds));
        }

        static bool CompareDirectory(string cacheDir, string targetDir)
        {
            bool hasChanged = false;

            var targetDI = new DirectoryInfo(targetDir);
            var cachedDI = new DirectoryInfo(Path.Combine(cacheDir, targetDir));

            if (cachedDI.Exists)
            {
                foreach (var dir in Directory.GetDirectories(targetDir))
                {
                    hasChanged |= CompareDirectory(cacheDir, dir);
                }

                foreach (var file in Directory.GetFiles(targetDir))
                {
                    hasChanged |= CompareFile(cacheDir, file);
                }
            }
            else
            {
                hasChanged = true;
            }

            if (!hasChanged)
            {
                if (targetDI.CreationTime != cachedDI.CreationTime)
                    targetDI.CreationTime = cachedDI.CreationTime;

                if (targetDI.LastWriteTime != cachedDI.LastWriteTime)
                    targetDI.LastWriteTime = cachedDI.LastWriteTime;

                if (targetDI.LastAccessTime != cachedDI.LastAccessTime)
                    targetDI.LastAccessTime = cachedDI.LastAccessTime;
            }

            return hasChanged;
        }

        private static bool CompareFile(string cacheDir, string targetFilePath)
        {
            string cachedFilePath = Path.Combine(cacheDir, targetFilePath);

            var targetFileInfo = new FileInfo(targetFilePath);
            var cachedFileInfo = new FileInfo(cachedFilePath);

            bool hasChanged = !FilesContentsAreEqual(targetFileInfo, cachedFileInfo);
            if (!hasChanged)
            {
                if (targetFileInfo.CreationTime != cachedFileInfo.CreationTime)
                    targetFileInfo.CreationTime = cachedFileInfo.CreationTime;

                if (targetFileInfo.LastWriteTime != cachedFileInfo.LastWriteTime)
                    targetFileInfo.LastWriteTime = cachedFileInfo.LastWriteTime;

                if (targetFileInfo.LastAccessTime != cachedFileInfo.LastAccessTime)
                    targetFileInfo.LastAccessTime = cachedFileInfo.LastAccessTime;
            }

            return hasChanged;
        }

        public static bool FilesContentsAreEqual(FileInfo fileInfo1, FileInfo fileInfo2)
        {
            if (fileInfo1 == null)
            {
                throw new ArgumentNullException("fileInfo1");
            }

            if (fileInfo2 == null)
            {
                throw new ArgumentNullException("fileInfo2");
            }

            if (!fileInfo1.Exists || !fileInfo2.Exists)
            {
                return false;
            }

            if (string.Equals(fileInfo1.FullName, fileInfo2.FullName, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            if (fileInfo1.Length != fileInfo2.Length)
            {
                return false;
            }
            else
            {
                using (var file1 = fileInfo1.OpenRead())
                using (var file2 = fileInfo2.OpenRead())
                {
                    return StreamsContentsAreEqual(file1, file2);
                }
            }
        }

        private static int ReadFullBuffer(Stream stream, byte[] buffer)
        {
            int bytesRead = 0;
            while (bytesRead < buffer.Length)
            {
                int read = stream.Read(buffer, bytesRead, buffer.Length - bytesRead);
                if (read == 0)
                {
                    // Reached end of stream.
                    return bytesRead;
                }

                bytesRead += read;
            }

            return bytesRead;
        }

        private static bool StreamsContentsAreEqual(Stream stream1, Stream stream2)
        {
            const int bufferSize = 1024 * sizeof(Int64);
            var buffer1 = new byte[bufferSize];
            var buffer2 = new byte[bufferSize];

            while (true)
            {
                int count1 = ReadFullBuffer(stream1, buffer1);
                int count2 = ReadFullBuffer(stream2, buffer2);

                if (count1 != count2)
                {
                    return false;
                }

                if (count1 == 0)
                {
                    return true;
                }

                int iterations = (int) Math.Ceiling((double) count1 / sizeof(Int64));
                for (int i = 0; i < iterations; i++)
                {
                    if (BitConverter.ToInt64(buffer1, i * sizeof(Int64)) !=
                        BitConverter.ToInt64(buffer2, i * sizeof(Int64)))
                    {
                        return false;
                    }
                }
            }
        }

        private string GetCachePath(string buildPath)
        {
            return buildPath + "Cache";
        }
    }
}
