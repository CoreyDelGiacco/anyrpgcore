using System.IO;
using UnityEditor;
using UnityEngine;

namespace Triarch.Editor
{
    public static class TriarchDataSyncEditor
    {
        private const string SourceDataFolder = "data";
        private const string StreamingDataFolder = "Assets/StreamingAssets/TriarchData";

        [MenuItem("Tools/Triarch/Sync Data to StreamingAssets")]
        public static void SyncData()
        {
            var projectRoot = Path.GetFullPath(Path.Combine(Application.dataPath, ".."));
            var sourcePath = Path.Combine(projectRoot, SourceDataFolder);
            var targetPath = Path.Combine(projectRoot, StreamingDataFolder);

            if (!Directory.Exists(sourcePath))
            {
                Debug.LogError($"Triarch data source folder not found: {sourcePath}");
                return;
            }

            CopyDirectory(sourcePath, targetPath);
            AssetDatabase.Refresh();
            Debug.Log($"Triarch data synced from {sourcePath} to {targetPath}");
        }

        private static void CopyDirectory(string sourcePath, string targetPath)
        {
            Directory.CreateDirectory(targetPath);

            foreach (var filePath in Directory.GetFiles(sourcePath))
            {
                if (filePath.EndsWith(".meta"))
                {
                    continue;
                }

                var fileName = Path.GetFileName(filePath);
                var destination = Path.Combine(targetPath, fileName);
                File.Copy(filePath, destination, true);
            }

            foreach (var directoryPath in Directory.GetDirectories(sourcePath))
            {
                var directoryName = Path.GetFileName(directoryPath);
                var destination = Path.Combine(targetPath, directoryName);
                CopyDirectory(directoryPath, destination);
            }
        }
    }
}
