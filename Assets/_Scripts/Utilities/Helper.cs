using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utilities;

namespace _Utilities
{
    /// <summary>
    /// Implements general-use static methods to use in other scripts.
    /// </summary>
    public static class Helper
    {
        public static Color GetRandomColor()
        {
            Color[] colors = { Color.blue, Color.magenta, Color.cyan, Color.green, Color.red, Color.yellow, Color.white };

            return colors.PickRandom();
        }

        public static void MoveToGround(Transform transform)
        {
            transform.position += new Vector3(0, 20, 0);
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity, ~LayerMask.GetMask("CommandAreas")))
            {
                transform.position -= new Vector3(0, hit.distance, 0);
            }
            else
            {
                transform.position -= new Vector3(0, 20, 0);
            }
        }

        public static Vector3 SampleParabola(Vector3 start, Vector3 end, float height, float t)
        {
            float parabolicT = t * 2 - 1;
            if (Mathf.Abs(start.y - end.y) < 0.1f)
            {
                //start and end are roughly level, pretend they are - simpler solution with less steps
                Vector3 travelDirection = end - start;
                Vector3 result = start + t * travelDirection;
                result.y += (-parabolicT * parabolicT + 1) * height;
                return result;
            }
            else
            {
                //start and end are not level, gets more complicated
                Vector3 travelDirection = end - start;
                Vector3 levelDirecteion = end - new Vector3(start.x, end.y, start.z);
                Vector3 right = Vector3.Cross(travelDirection, levelDirecteion);
                Vector3 up = Vector3.Cross(right, travelDirection);
                if (end.y > start.y) up = -up;
                Vector3 result = start + t * travelDirection;
                result += ((-parabolicT * parabolicT + 1) * height) * up.normalized;
                return result;
            }
        }

        /// <summary>
        /// Clamps an angle value to minimum and maximum values
        /// </summary>
        /// <param name="angle">input angle</param>
        /// <param name="min">minimum value to clamp</param>
        /// <param name="max">maximum value to clamp</param>
        /// <returns></returns>
        public static float ClampAngle(float angle, float min, float max)
        {
            if (angle < -360F)
                angle += 360F;
            if (angle > 360F)
                angle -= 360F;
            return Mathf.Clamp(angle, min, max);
        }

        public static string FormatBigNumbers(int num)
        {
            if (num > 999999999 || num < -999999999)
            {
                return num.ToString("0,,,.###B", CultureInfo.InvariantCulture);
            }
            else if (num > 999999 || num < -999999)
            {
                return num.ToString("0,,.##M", CultureInfo.InvariantCulture);
            }
            else if (num > 999 || num < -999)
            {
                return num.ToString("0,.#K", CultureInfo.InvariantCulture);
            }
            else
            {
                return num.ToString(CultureInfo.InvariantCulture);
            }
        }

        /// <summary>
        /// Remaps input range to output range
        /// </summary>
        /// <param name="input">Input value</param>
        /// <param name="minInput">Minimum possible input value</param>
        /// <param name="maxInput">Maximum possible input value</param>
        /// <param name="minOutput">Minimum possible output value after scaling</param>
        /// <param name="maxOutput">Maximum possible output value after scaling</param>
        /// <returns>Scaled output</returns>
        public static float Remap(float input, float minInput, float maxInput, float minOutput, float maxOutput, bool invertOutput = false)
        {
            float result = (((maxOutput - minOutput) * (input - minInput)) / (maxInput - minInput)) + minOutput;
            if (invertOutput)
            {
                result = maxOutput - result + minOutput;
            }

            return result;
        }

        /// <summary>
        /// Prevents the object to launch unity events
        /// </summary>
        /// <param name="ev"></param>
        public static void MuteUnityEvent(UnityEventBase ev)
        {
            int count = ev.GetPersistentEventCount();
            for (int i = 0; i < count; i++)
            {
                ev.SetPersistentListenerState(i, UnityEventCallState.Off);
            }
        }

        /// <summary>
        /// Allows the object to launch unity events
        /// </summary>
        /// <param name="ev"></param>
        public static void UnmuteUnityEvent(UnityEventBase ev)
        {
            int count = ev.GetPersistentEventCount();
            for (int i = 0; i < count; i++)
            {
                ev.SetPersistentListenerState(i, UnityEventCallState.RuntimeOnly);
            }
        }

        /// <summary>
        /// A clamp method that works well with 359 degree system
        /// </summary>
        /// <param name="angle"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static float ClampEulerAngles(float angle, float min, float max)
        {
            float realmin = Mathf.Min(min, max);
            float realmax = Mathf.Max(min, max);

            //make the angle positive and between 0-359
            if (angle < 0)
                angle = 360 + angle;
            else if (angle > 360)
            {
                angle = angle % 360;
            }

            if (angle >= realmin && angle <= realmax)
            {
                if (Mathf.Abs(min - angle) > Mathf.Abs(max - angle))
                {
                    return max;
                }

                return min;
            }
            else
            {
                return angle;
            }
        }

        /// <summary>
        /// Destroys all children of a parent transform
        /// </summary>
        /// <param name="parent">Parent transform</param>
        public static void DestroyAllChildren(Transform parent)
        {
            //Debug.Log("#33#: DestroyAllChildren");
            if (parent == null) return;
            foreach (Transform child in parent)
            {
                if (child != null)
                    GameObject.Destroy(child.gameObject);
            }
        }

        /// <summary>
        /// Checks if the pointer is on an UI object (Canvas) or not
        /// </summary>
        /// <param name="canvases">Canvas list to process</param>
        /// <param name="screenPosition">Clicked position</param>
        /// <returns></returns>
        public static bool IsPointerOverUIObject(List<Canvas> canvases, Vector2 screenPosition)
        {
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = screenPosition;

            int resultCount = 0;

            foreach (Canvas canvas in canvases)
            {
                GraphicRaycaster uiRaycaster = canvas.gameObject.GetComponent<GraphicRaycaster>();
                List<RaycastResult> results = new List<RaycastResult>();
                uiRaycaster.Raycast(eventDataCurrentPosition, results);
                resultCount += results.Count;
            }

            return resultCount > 0;
        }

        /// <summary>
        /// Checks if the pointer is on an UI object (Canvas) or not
        /// </summary>
        /// <param name="canvas">Canvas list to process</param>
        /// <param name="screenPosition">Clicked position</param>
        /// <returns></returns>
        public static bool IsPointerOverUIObject(Canvas canvas, Vector2 screenPosition)
        {
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = screenPosition;
            int resultCount = 0;

            GraphicRaycaster uiRaycaster = canvas.gameObject.GetComponent<GraphicRaycaster>();
            List<RaycastResult> results = new List<RaycastResult>();
            uiRaycaster.Raycast(eventDataCurrentPosition, results);
            resultCount += results.Count;

            return resultCount > 0;
        }

        /// <summary>
        /// Calculates a random chance and returns a bool
        /// </summary>
        /// <param name="percentage">Percentage of probability</param>
        /// <returns>true if the RNG chance occurs, false for vice versa</returns>
        public static bool CalculateRngChange(float percentage)
        {
            float rand = Random.Range(0f, 100f);

            if (percentage == 100f || rand < percentage && percentage != 0)
                return true;

            return false;
        }

        public static string GetGameObjectPath(GameObject obj)
        {
            string path = "/" + obj.name;
            while (obj.transform.parent != null)
            {
                obj = obj.transform.parent.gameObject;
                path = "/" + obj.name + path;
            }

            return path;
        }

        /// <summary>
        /// Calculates a random chance and picks a key from the given dictionary.
        /// Chances must be given as the value of the dictionary. Dictionary total should be equal to 100.
        /// </summary>
        /// <param name="dict">Dictionary to calculate rng chances from</param>
        /// <typeparam name="T">Return type of the method</typeparam>
        /// <returns>Key type of the dictionary</returns>
        public static T CalculateRNGWithOptionDictionary<T>(Dictionary<T, float> dict)
        {
            var totalChances = 0f;
            int roll = Random.Range(0, 100);

            foreach (var keyVal in dict.Where(keyVal => keyVal.Value > 0f))
            {
                totalChances += keyVal.Value;
                if (roll <= totalChances)
                {
                    return keyVal.Key;
                }
            }

            return default(T);
        }

        /// <summary>
        /// Finds the center point of a list of transforms
        /// </summary>
        /// <param name="transforms">List of transforms</param>
        /// <returns>Center point of the transforms</returns>
        public static Vector3 FindCenterPointOfTransforms(List<Transform> transforms)
        {
            Vector3 center = Vector3.zero;
            foreach (Transform t in transforms)
            {
                if (t)
                    center += t.position;
            }

            center /= transforms.Count;
            return center;
        }
    }
}
