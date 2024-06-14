#if UNITY_EDITOR

using System;
using UnityEditor;
using UnityEngine;

using UnityEditor.SceneManagement;
using Pyros.Editor.Toolbar;

namespace _Project.Scripts.Editor.Toolbar.Objects
{
    [Serializable]
    public class GoToMainMenuSceneToolbarObject : IToolbarObject
    {
        private GUIContent _content;

        public void Setup()
        {
            _content = ToolbarGUIContentFactory.CreateContentWithBuiltInEditorTexture("SliderJoint2D Icon",
                "Open Main Menu Scene");
        }

        public void Draw()
        {
            if(ToolbarDrawers.DrawButton(_content))
            {
                if(EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                {
                    var mainMenuScenePath = EditorBuildSettings.scenes[0].path;
                    EditorSceneManager.OpenScene(mainMenuScenePath, OpenSceneMode.Single);
                }
            }
        }
    }
}
#endif
