#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

using _Project.Scripts.Editor.Tools;

namespace Pyros.Editor.Toolbar.Objects
{
    [Serializable]
    public class FocusPlayerToolbarObject : IToolbarObject
    {
        private GUIContent _content;

        public void Setup()
        {
            _content = ToolbarGUIContentFactory.CreateContentWithBuiltInEditorTexture("Find_Player_Tool_Icon.png",
                "Selects and focus on the Player", "Focus");
        }

        public void Draw()
        {
            using(new EditorGUI.DisabledScope(Application.isPlaying))
            {
                EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));

                if(GUILayout.Button(_content, EditorStyles.toolbarButton, GUILayout.Width(110.0f)))
                {
                    MonoBehaviour player;

                    SearchLevelsWindow.ShowWindow();
                }

                EditorGUILayout.EndHorizontal();
            }
        }
    }
}
#endif