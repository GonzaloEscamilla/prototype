using System;
using Pyros.Editor.Toolbar;
using UnityEngine;

namespace _Project.Scripts.Editor.Toolbar.Objects
{
    [Serializable]
    public class DrawFlexibleSpaceToolbarObject : IToolbarObject
    {
        public void Setup()
        {
        }

        public void Draw()
        {
            GUILayout.FlexibleSpace();
        }
    }
}