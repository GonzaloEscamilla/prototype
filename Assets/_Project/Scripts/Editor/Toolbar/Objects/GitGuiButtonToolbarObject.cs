using System;
using Pyros.Editor.Toolbar;
using UnityEngine;

namespace Stumble.Editor.Toolbar.Objects
{
    [Serializable]
    public class GitGuiButtonToolbarObject : IToolbarObject
    {
        private GUIContent _content;

        public void Setup()
        {
            _content = ToolbarGUIContentFactory.CreateContentWithIconTexture(ToolbarConfig.ToolbarIcons.GitIcon, "Open GitHub");
        }

        public void Draw()
        {
            const string url = "https://github.com/GonzaloEscamilla/prototype.git";
            ToolbarDrawers.DrawUrlButton(_content, url);
        }
    }
}