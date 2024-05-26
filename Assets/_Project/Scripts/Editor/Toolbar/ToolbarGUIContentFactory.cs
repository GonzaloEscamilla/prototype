using UnityEditor;
using UnityEngine;

namespace Pyros.Editor.Toolbar
{
    static class ToolbarGUIContentFactory
    {
        internal static GUIContent CreateContentWithBuiltInEditorTexture(string textureName, string tooltip, string text = null)
        {
            // icon content reference: https://github.com/halak/unity-editor-icons
            var icon = EditorGUIUtility.Load(textureName) as Texture2D;
            return CreateContentWithIconTexture(icon, tooltip, text);
        }

        internal static GUIContent CreateContentWithIconTexture(Texture2D icon, string tooltip, string text = null)
        {
            if(icon == null)
            {
                icon = DefaultIcon;
            }
            return text != null ? new GUIContent(text, icon, tooltip) : new GUIContent(icon, tooltip);
        }

        static Texture2D DefaultIcon => EditorGUIUtility.Load("BuildSettings.Broadcom") as Texture2D;

    }
}