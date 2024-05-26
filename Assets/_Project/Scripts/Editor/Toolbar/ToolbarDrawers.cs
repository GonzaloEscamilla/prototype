using UnityEditor;
using UnityEngine;

namespace Pyros.Editor.Toolbar
{
    static class ToolbarDrawers
    {
        internal const float BUTTONS_SPACING = 5f;

        internal static void DrawUrlButton(GUIContent content, string url)
        {
            if(DrawButtonInternal(content))
            {
                Application.OpenURL(url);
            }

            GUILayout.Space(BUTTONS_SPACING);
        }

        internal static bool DrawButton(GUIContent content)
        {
            var isClicked = DrawButtonInternal(content);

            GUILayout.Space(BUTTONS_SPACING);

            return isClicked;
        }

        static bool DrawButtonInternal(GUIContent content)
        {
            const float ButtonWidth = 24f;
            const float ButtonHeight = 24f;

            return GUILayout.Button(content, EditorStyles.toolbarButton, GUILayout.Width(ButtonWidth), GUILayout.Height(ButtonHeight));
        }
    }
}