using UnityEngine;

namespace _Project.Scripts.Utilities.Math
{
    public class CameraUtilities
    {
        
        public static Vector3 WorldToLocal(Vector3 aCamPos, Quaternion aCamRot, Vector3 aPos)
        {
            return Quaternion.Inverse(aCamRot) * (aPos - aCamPos);
        }
        public static Vector3 Project(Vector3 aPos, float aFov, float aAspect)
        {
            float f = 1f / Mathf.Tan(aFov * Mathf.Deg2Rad * 0.5f);
            f /= aPos.z;
            aPos.x *= f / aAspect;
            aPos.y *= f;
            return aPos;
        }
        public static Vector3 ClipSpaceToViewport(Vector3 aPos)
        {
            aPos.x = aPos.x * 0.5f + 0.5f;
            aPos.y = aPos.y * 0.5f + 0.5f;
            return aPos;
        }
 
        public static Vector3 WorldToViewport(Vector3 aCamPos, Quaternion aCamRot, float aFov, float aAspect, Vector3 aPos)
        {
            Vector3 p = WorldToLocal(aCamPos, aCamRot, aPos);
            p = Project(p, aFov, aAspect);
            return ClipSpaceToViewport(p);
        }
 
        public static Vector3 WorldToScreenPos(Vector3 aCamPos, Quaternion aCamRot, float aFov, float aScrWidth, float aScrHeight, Vector3 aPos)
        {
            Vector3 p = WorldToViewport(aCamPos, aCamRot, aFov, aScrWidth / aScrHeight, aPos);
            p.x *= aScrWidth;
            p.y *= aScrHeight;
            return p;
        }
 
        public static Vector3 WorldToGUIPos(Vector3 aCamPos, Quaternion aCamRot, float aFov, float aScrWidth, float aScrHeight, Vector3 aPos)
        {
            Vector3 p = WorldToScreenPos(aCamPos, aCamRot, aFov, aScrWidth, aScrHeight, aPos);
            p.y = aScrHeight - p.y;
            return p;
        }
    }
}