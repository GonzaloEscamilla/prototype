using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Pyros.Editor.Toolbar
{
    [CreateAssetMenu(menuName = "ToolbarConfig", fileName = "ToolbarConfig", order = 0)]
    class ToolbarConfig : ScriptableObject
    {
        [SerializeReference, SubclassSelector]
        private List<IToolbarObject> _leftToolbarObjects = new List<IToolbarObject>();

        [SerializeReference, SubclassSelector]
        private List<IToolbarObject> _rightToolbarObjects = new List<IToolbarObject>();

        [SerializeField]
        private ToolbarIcons _toolbarIcons;

        private static ToolbarConfig _instance;

        internal static ToolbarConfig Instance
        {
            get
            {
                const string ToolbarConfigPath = "Assets/_Project/Editor/Toolbar/ToolbarConfig.asset";

                if(_instance == null)
                {
                    _instance = AssetDatabase.LoadAssetAtPath<ToolbarConfig>(ToolbarConfigPath);
                }

                return _instance;
            }
        }

        internal static ToolbarIcons ToolbarIcons => Instance == null ? default : Instance._toolbarIcons;

        private void OnValidate()
        {
            Setup();
        }

        public void Setup()
        {
            foreach(var toolbarObject in _leftToolbarObjects)
            {
                try
                {
                    toolbarObject?.Setup();
                }
                catch(Exception ex)
                {
                    Debug.LogException(ex);
                }
            }
            foreach(var toolbarObject in _rightToolbarObjects)
            {
                try
                {
                    toolbarObject?.Setup();
                }
                catch(Exception ex)
                {
                    Debug.LogException(ex);
                }
            }
        }
        public void DrawLeft()
        {
            foreach(var toolbarObject in _leftToolbarObjects)
            {
                try
                {
                    toolbarObject?.Draw();
                }
                catch(Exception ex)
                {
                    Debug.LogException(ex);
                }
            }
        }
        public void DrawRight()
        {
            foreach(var toolbarObject in _rightToolbarObjects)
            {
                try
                {
                    toolbarObject?.Draw();
                }
                catch(Exception ex)
                {
                    Debug.LogException(ex);
                }
            }
        }
    }
}