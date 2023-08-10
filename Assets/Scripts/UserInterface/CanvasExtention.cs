using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TPSHorror.UserInterface
{
    public static class CanvasExtention 
    {
        public static Vector2 WorldPositionToCanvasPosition(RectTransform canvasRect, Camera camera, Vector3 worldPosition)
        {
            Vector2 viewPos = camera.WorldToViewportPoint(worldPosition);

            viewPos.x *= canvasRect.sizeDelta.x;
            viewPos.y *= canvasRect.sizeDelta.y;

            viewPos.x -= canvasRect.sizeDelta.x * canvasRect.pivot.x;
            viewPos.y -= canvasRect.sizeDelta.y * canvasRect.pivot.y;

            return viewPos;
        }

    }
}
