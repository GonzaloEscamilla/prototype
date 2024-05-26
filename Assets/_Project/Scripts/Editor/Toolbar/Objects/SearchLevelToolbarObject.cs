using System;
using _Project.Scripts.Editor.Tools;
using UnityEditor;
using UnityEngine;

namespace Pyros.Editor.Toolbar.Objects
{
    [Serializable]
    public class SearchLevelToolbarObject : IToolbarObject
    {
        private GUIContent _content;

        public void Setup()
        {
            _content = ToolbarGUIContentFactory.CreateContentWithBuiltInEditorTexture("Search On Icon",
                "Open Search Levels Window", "Search level ");
        }

        public void Draw()
        {
            using(new EditorGUI.DisabledScope(Application.isPlaying))
            {
                EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));

                if(GUILayout.Button(_content, EditorStyles.toolbarButton, GUILayout.Width(110.0f)))
                {
                    SearchLevelsWindow.ShowWindow();
                }

                EditorGUILayout.EndHorizontal();
            }
        }
    }
}