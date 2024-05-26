using System;
using Pyros.Editor.Toolbar;
using UnityEngine;

namespace _Project.Scripts.Editor.Toolbar.Objects
{
    [Serializable]
    public class DrawButtonsSpacingToolbarObject : IToolbarObject
    {
        public void Setup()
        {
        }

        public void Draw()
        {
            GUILayout.Space(ToolbarDrawers.BUTTONS_SPACING);
        }
    }
}