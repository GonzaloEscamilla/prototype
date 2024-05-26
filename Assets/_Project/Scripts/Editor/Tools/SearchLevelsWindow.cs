using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project.Scripts.Editor.Tools
{
    public class SearchLevelsWindow : EditorWindow
    {
        [MenuItem("Tools/Stumble Guys/Levels/Search levels")]
        public static void ShowWindow()
        {
            var window = EditorWindow.GetWindow<SearchLevelsWindow>() ?? EditorWindow.CreateInstance<SearchLevelsWindow>();
            window.titleContent = new GUIContent("Search levels");
            window.minSize = window.maxSize = new Vector2(500, 600);
            window.Show();
        }
        private Vector2 scrollPosition;

        private void OnGUI()
        {
            GUILayout.Label("Scenes In Build", EditorStyles.boldLabel);

            // Get the scenes in build settings
            EditorBuildSettingsScene[] scenes = EditorBuildSettings.scenes;
            if (scenes.Length == 0)
            {
                GUILayout.Label("No scenes in build settings.");
                return;
            }

            scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, true);
            foreach (EditorBuildSettingsScene scene in scenes)
            {
                if (scene == null)
                    continue;

                string scenePath = scene.path;
                string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);

                GUILayout.BeginHorizontal();

                GUILayout.Label(sceneName, GUILayout.Width(200));

                if (GUILayout.Button("Open Scene"))
                {
                    OpenScene(scenePath);
                }

                GUILayout.EndHorizontal();
            }
            GUILayout.EndScrollView();
        }

        private void OpenScene(string scenePath)
        {
            // Check if the scene is currently open
            if (SceneManager.GetActiveScene().path != scenePath)
            {
                // Save current scene if it has unsaved changes
                if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                {
                    EditorSceneManager.OpenScene(scenePath);
                }
            }
            else
            {
                Debug.Log("Scene is already open.");
            }
        }
    }
}