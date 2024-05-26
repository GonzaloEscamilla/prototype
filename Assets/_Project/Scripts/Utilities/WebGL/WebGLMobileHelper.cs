using UnityEngine;

namespace _Project.Scripts.Utilities.WebGL
{
    public static class WebGLMobileHelper
    {
        public static bool IsRunningOnMobile() =>
            Application.platform == RuntimePlatform.WebGLPlayer && Application.isMobilePlatform;
    }
}