using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using _Utilities;
using UnityEngine;
using UnityEngine.Rendering;
using Debug = UnityEngine.Debug;
using Random = System.Random;

namespace Utilities
{
    public static partial class Extensions
    {
        #region Fields

        private static Random rng = new Random();

        #endregion

        #region Extension Methods

        public static Vector2 ToVector2(this Vector3 vector)
        {
            return new Vector2(vector.x, vector.z);
        }

        public static string SubstringBefore(this string input, string indexValue)
        {
            var inputSpan = input.AsSpan();
            var range = inputSpan.IndexOf(indexValue);
            range = range == -1 ? inputSpan.Length : range;
            var sliced = inputSpan[..range];
            return sliced.ToString();
        }

        public static Int32 ToInt32(this string str)
        {
            return Int32.Parse(str, CultureInfo.CurrentCulture);
        }

        public static string FormatNumber(this int num)
        {
            if (num >= 100000000)
            {
                return ((float)num / 1000000).ToString("#.0M");
            }

            if (num >= 10000000)
            {
                return ((float)num / 1000000).ToString("0.#M");
            }

            if (num >= 100000)
            {
                return ((float)num / 1000).ToString("#.0K");
            }

            if (num >= 10000)
                return ((float)num / 1000).ToString("0.#K");

            return $"{num:N0}";
        }

        /// <summary>
        /// Bu metod FormatNumber metodunun tersi olarak çalışır. Örnek: 1.1M -> 1100000
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ReformatString(this string str)
        {
            if (str.Contains("M"))
            {
                str = str.Replace("M", "");
                str = str.Replace(".", "");
                str += "000000";
            }
            else if (str.Contains("K"))
            {
                str = str.Replace("K", "");
                str = str.Replace(".", "");
                str += "000";
            }

            return str;
        }


        /// <summary>
        /// <para>Has been made to convert decimal version numbers into integers.</para>
        /// <para>Example: Android version 1.1.6 will be converted to 116</para>
        /// </summary>

        public static float FloatingToIntegral(this string value)
        {
            if (float.TryParse(value, out var tmp))
                return tmp;
            return -1;
        }

        public static string FormatNumber(this long num)
        {
            if (num >= 100000000)
            {
                return ((double)num / 1000000).ToString("#.0M");
            }

            if (num >= 1000000)
            {
                return ((float)num / 1000000).ToString("#.0M");
            }

            if (num >= 100000)
            {
                return ((float)num / 1000).ToString("#.0K");
            }

            return $"{num:N0}";
        }


        public static string AddDotsToNumber(this string numberString)
        {
            string finalString = "";
            int counter = 0;
            Debug.Log(numberString);
            for (int i = numberString.Length - 1; i > -1; i--)
            {
                Debug.Log(numberString[i]);
                if (counter == 3)
                {
                    finalString = '.' + finalString;
                    counter = 0;
                }

                finalString = numberString[i] + finalString;
                counter++;
            }

            return finalString;
        }

        /// <summary>
        /// Cuts string if over desired length
        /// </summary>
        /// <param name="text">text</param>
        /// <param name="maxLenght">max length</param>
        /// <param name="startIndex"> start index of cut operation</param>
        /// <returns></returns>
        public static string ClampString(this string text, int maxLenght, int startIndex = 0)
        {
            if (text.Length > maxLenght) return text.Substring(startIndex, maxLenght);
            return text;
        }

        /// <summary>
        /// Extension version of DestroyAllChildren
        /// </summary>
        /// <param name="transform"></param>
        public static void RemoveAllChildren(this Transform transform)
        {
            Helper.DestroyAllChildren(transform);
        }

        /// <summary>
        /// Recursive search in all the children and subchildren
        /// </summary>
        /// <param name="parent">Parent transform</param>
        public static Transform FindInAllChildren(this Transform parent, string childName)
        {
            foreach (Transform child in parent)
            {
                if (child.name == childName)
                {
                    return child;
                }
                else
                {
                    Transform found = FindInAllChildren(child, childName);
                    if (found != null)
                    {
                        return found;
                    }
                }
            }

            return null;
        }


        /// <summary>
        /// Returns String by adding spaces before captials
        /// </summary>
        /// <param name="text"></param>
        /// <param name="preserveAcronyms"></param>
        /// <returns></returns>
        public static string AddSpacesToSentence(this string text, bool preserveAcronyms)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;
            StringBuilder newText = new StringBuilder(text.Length * 2);
            newText.Append(text[0]);
            for (int i = 1; i < text.Length; i++)
            {
                if (char.IsUpper(text[i]))
                    if ((text[i - 1] != ' ' && !char.IsUpper(text[i - 1])) || (preserveAcronyms && char.IsUpper(text[i - 1]) && i < text.Length - 1 && !char.IsUpper(text[i + 1])))
                        newText.Append(' ');
                newText.Append(text[i]);
            }

            return newText.ToString();
        }

        /// <summary>
        /// Random shuffle Extension for lists
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        /// <summary>
        /// Makes the material opaque. Source: https://forum.unity.com/threads/change-rendering-mode-via-script.476437/#post-5235491
        /// </summary>
        /// <param name="material">Material to be made opaque.</param>
        public static void ToOpaqueMode(this Material material)
        {
            material.SetOverrideTag("RenderType", "");
            material.SetInt("_SrcBlend", (int)BlendMode.One);
            material.SetInt("_DstBlend", (int)BlendMode.Zero);
            material.SetInt("_ZWrite", 1);
            material.DisableKeyword("_ALPHATEST_ON");
            material.DisableKeyword("_ALPHABLEND_ON");
            material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            material.renderQueue = -1;
        }

        /// <summary>
        /// Makes the material transparent. Source: https://forum.unity.com/threads/change-rendering-mode-via-script.476437/#post-5235491
        /// </summary>
        /// <param name="material">Material to be made transparent.</param>
        public static void ToFadeMode(this Material material)
        {
            material.SetOverrideTag("RenderType", "Transparent");
            material.SetInt("_SrcBlend", (int)BlendMode.SrcAlpha);
            material.SetInt("_DstBlend", (int)BlendMode.OneMinusSrcAlpha);
            material.SetInt("_ZWrite", 0);
            material.DisableKeyword("_ALPHATEST_ON");
            material.EnableKeyword("_ALPHABLEND_ON");
            material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            material.renderQueue = (int)RenderQueue.Transparent;
        }

        /// <summary>
        /// Clones a list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="listToClone"></param>
        /// <returns></returns>
        public static IList<T> Clone<T>(this IList<T> listToClone) where T : ICloneable
        {
            return listToClone.Select(item => (T)item.Clone()).ToList();
        }

        ///<summary>Finds the index of the first item matching an expression in an enumerable.</summary>
        ///<param name="items">The enumerable to search.</param>
        ///<param name="predicate">The expression to test the items against.</param>
        ///<returns>The index of the first matching item, or -1 if no items match.</returns>
        public static int FindIndex<T>(this IEnumerable<T> items, Func<T, bool> predicate)
        {
            if (items == null) throw new ArgumentNullException("items");
            if (predicate == null) throw new ArgumentNullException("predicate");

            int retVal = 0;
            foreach (var item in items)
            {
                if (predicate(item)) return retVal;
                retVal++;
            }

            return -1;
        }

        ///<summary>Finds the index of the first occurrence of an item in an enumerable.</summary>
        ///<param name="items">The enumerable to search.</param>
        ///<param name="item">The item to find.</param>
        ///<returns>The index of the first matching item, or -1 if the item was not found.</returns>
        public static int IndexOf<T>(this IEnumerable<T> items, T item)
        {
            return items.FindIndex(i => EqualityComparer<T>.Default.Equals(item, i));
        }

        /// <summary>
        /// Shorthand method to check if the value is in range
        /// </summary>
        /// <param name="item"></param>
        /// <param name="start">Start of the range</param>
        /// <param name="end">End of the range</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool IsBetween<T>(this T item, T start, T end)
        {
            return Comparer<T>.Default.Compare(item, start) >= 0 && Comparer<T>.Default.Compare(item, end) <= 0;
        }

        /// <summary>
        /// Picks a random member of the list
        /// </summary>
        /// <param name="list"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T PickRandom<T>(this IList<T> list)
        {
            int pickedIndex = UnityEngine.Random.Range(0, list.Count);
            return list[pickedIndex];
        }

        /// <summary>
        /// Extension: Picks a random member of the array
        /// </summary>
        /// <param name="array"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T PickRandom<T>(this T[] array)
        {
            int pickedIndex = UnityEngine.Random.Range(0, array.Length);
            return array[pickedIndex];
        }

        /// <summary>
        /// Extension method to convert a list to string
        /// </summary>
        /// <param name="list"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static string ConvertToString<T>(this IList<T> list)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append($"List[{list.Count}] :\n");

            foreach (var item in list)
            {
                stringBuilder.AppendLine($"[{item.ToString()}]");
            }

            return stringBuilder.ToString();
        }

        public static void AddIfNotExist<T>(this List<T> list, T item)
        {
            if (list.Contains(item) is false)
                list.Add(item);
        }

        public static void StopParticleEmission(this ParticleSystem particle)
        {
            List<ParticleSystem> particles = particle.GetComponentsInChildren<ParticleSystem>().ToList();
            particles.Add(particle);

            foreach (var particleSystem in particles)
            {
                var emissionModule = particleSystem.emission;
                emissionModule.enabled = false;
            }
        }

        public static Vector3 GetRandomPointInBounds(this Bounds bounds)
        {
            return new Vector3(UnityEngine.Random.Range(bounds.min.x, bounds.max.x), UnityEngine.Random.Range(bounds.min.y, bounds.max.y), UnityEngine.Random.Range(bounds.min.z, bounds.max.z));
        }

        public static int StopAndGetElapsedMs(this Stopwatch stopwatch)
        {
            stopwatch.Stop();
            return (int)stopwatch.ElapsedMilliseconds;
        }

        public static bool IsPlayingAnimatorController(this Animator animator)
        {
            return animator.isActiveAndEnabled;
        }

        public static bool HasParameter(this Animator animator, string paramName)
        {
            foreach (AnimatorControllerParameter param in animator.parameters)
            {
                if (param.name == paramName)
                    return true;
            }
            return false;
        }
        
        public static bool IsInViewport(this Camera cam, Vector3 position, float margin = 0)
        {
            Vector3 viewportPoint = cam.WorldToViewportPoint(position);
            return viewportPoint.x >= -margin && viewportPoint.x <= 1 + margin && viewportPoint.y >= -margin && viewportPoint.y <= 1 + margin &&
                viewportPoint.z > 0;
        }


        #endregion
    }
}
