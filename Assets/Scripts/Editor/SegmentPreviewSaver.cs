using System.IO;
using System.Linq;
using Runtime.Components.Segments;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class SegmentPreviewSaver : UnityEditor.Editor
    {
        [MenuItem("Tools/Save Segment Preview")]
        private static void SaveSegmentPreview()
        {
            var selectedObjects = Selection.gameObjects;
            var segments = selectedObjects
                .Where(g => g.GetComponent<Segment>())
                .Select(g => g.GetComponent<Segment>());

            const string folderPath = "Assets/Prefabs/Segments/Previews";
        
            if (!AssetDatabase.IsValidFolder(folderPath))
            {
                Debug.LogWarning("Invalid Folder Path: " + folderPath);
                return;
            }

            foreach (var segment in segments)
            {
                var preview = AssetPreview.GetAssetPreview(segment.gameObject);
                if (preview == null)
                {
                    Debug.LogWarning($"Could not generate preview for {segment.name}");
                    continue;
                }

                var readableTexture = new Texture2D(preview.width, preview.height, TextureFormat.RGBA32, false);
                Graphics.CopyTexture(preview, readableTexture);

                var spritePath = Path.Combine(folderPath, segment.name + "_Preview.png");

                File.WriteAllBytes(spritePath, readableTexture.EncodeToPNG());
                AssetDatabase.Refresh();

                var importer = AssetImporter.GetAtPath(spritePath) as TextureImporter;
                if (importer != null)
                {
                    importer.textureType = TextureImporterType.Sprite;
                    importer.spriteImportMode = SpriteImportMode.Single;
                    AssetDatabase.ImportAsset(spritePath, ImportAssetOptions.ForceUpdate);

                    segment.Preview = AssetDatabase.LoadAssetAtPath<Sprite>(spritePath);
                    EditorUtility.SetDirty(segment);
                }
                else
                {
                    Debug.LogError($"Failed to import sprite at {spritePath}");
                }
            }
        
            Debug.Log("Segment previews saved successfully.");
        }
    }
}
