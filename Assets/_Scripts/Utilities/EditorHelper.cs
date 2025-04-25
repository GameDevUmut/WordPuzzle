using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR

namespace _Utilities
{
    /// <summary>
    /// Provides helper methods for the Unity Editor.
    /// </summary>
    public static class EditorHelper
    {
        /// <summary>
        /// Draws Debug Text, can only be used in OnDrawGizmos
        /// </summary>
        /// <param name="text">text to display</param>
        /// <param name="worldPos">Text position</param>
        /// <param name="textColor">Text color</param>
        /// <returns></returns>
        public static void DrawString(string text, Vector3 worldPos, Color? textColor = null)
        {
            if (textColor.HasValue) Handles.color = textColor.Value;
            Handles.Label(worldPos, text);
        }

        /// <summary>
        /// Draws a cube to represent bounds
        /// </summary>
        /// <param name="b">Bounds</param>
        /// <param name="delay">Time to stay alive</param>
        public static void DrawBounds(Bounds b, float delay = 0)
        {
            // bottom
            var p1 = new Vector3(b.min.x, b.min.y, b.min.z);
            var p2 = new Vector3(b.max.x, b.min.y, b.min.z);
            var p3 = new Vector3(b.max.x, b.min.y, b.max.z);
            var p4 = new Vector3(b.min.x, b.min.y, b.max.z);

            Debug.DrawLine(p1, p2, Color.blue, delay);
            Debug.DrawLine(p2, p3, Color.red, delay);
            Debug.DrawLine(p3, p4, Color.yellow, delay);
            Debug.DrawLine(p4, p1, Color.magenta, delay);

            // top
            var p5 = new Vector3(b.min.x, b.max.y, b.min.z);
            var p6 = new Vector3(b.max.x, b.max.y, b.min.z);
            var p7 = new Vector3(b.max.x, b.max.y, b.max.z);
            var p8 = new Vector3(b.min.x, b.max.y, b.max.z);

            Debug.DrawLine(p5, p6, Color.blue, delay);
            Debug.DrawLine(p6, p7, Color.red, delay);
            Debug.DrawLine(p7, p8, Color.yellow, delay);
            Debug.DrawLine(p8, p5, Color.magenta, delay);

            // sides
            Debug.DrawLine(p1, p5, Color.white, delay);
            Debug.DrawLine(p2, p6, Color.gray, delay);
            Debug.DrawLine(p3, p7, Color.green, delay);
            Debug.DrawLine(p4, p8, Color.cyan, delay);
        }

        /// <summary>
        /// Draws 2d rectangle to represent bounds bottom area
        /// </summary>
        /// <param name="b">Bounds</param>
        /// <param name="color">Color of the rectangle</param>
        /// <param name="time">Time to stay alive</param>
        public static void DrawBounds2D(Bounds b, Color color, float time = 0.1f)
        {
            // bottom
            var p1 = new Vector3(b.min.x, b.min.y, b.min.z);
            var p2 = new Vector3(b.max.x, b.min.y, b.min.z);
            var p3 = new Vector3(b.max.x, b.min.y, b.max.z);
            var p4 = new Vector3(b.min.x, b.min.y, b.max.z);

            Debug.DrawLine(p1, p2, color, time);
            Debug.DrawLine(p2, p3, color, time);
            Debug.DrawLine(p3, p4, color, time);
            Debug.DrawLine(p4, p1, color, time);
        }
    }
}

#endif
