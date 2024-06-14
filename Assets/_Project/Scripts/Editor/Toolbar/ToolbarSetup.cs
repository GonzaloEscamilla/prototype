#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;
using UnityToolbarExtender;

namespace Pyros.Editor.Toolbar
{
    static class ToolbarSetup
    {
        private static ToolbarConfig _toolbarConfig;

        [InitializeOnLoadMethod]
        static void Setup()
        {
            if(Application.isBatchMode)
            {
                return;
            }

            _toolbarConfig = ToolbarConfig.Instance;
            if(_toolbarConfig != null)
            {
                _toolbarConfig.Setup();

                ToolbarExtender.LeftToolbarGUI.Add(OnDrawLeftToolbarGUI);
                ToolbarExtender.RightToolbarGUI.Add(OnDrawRightToolbarGUI);
            }
        }

        static void OnDrawLeftToolbarGUI()
        {
            _toolbarConfig.DrawLeft();
        }

        static void OnDrawRightToolbarGUI()
        {
            _toolbarConfig.DrawRight();
        }

    }
}
#endif
