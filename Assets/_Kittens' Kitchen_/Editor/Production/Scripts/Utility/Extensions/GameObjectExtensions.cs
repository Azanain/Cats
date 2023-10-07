using UnityEngine;

namespace _Kittens__Kitchen.Editor.Scripts.Utility.Extensions
{
    public static class GameObjectExtensions
    {
        public static void Activate(this GameObject gameObject)
        {
            gameObject.SetActive(true);
        }
        
        public static void Deactivate(this GameObject gameObject)
        {
            gameObject.SetActive(false);
        }

        public static void Destroy(this GameObject gameObject, float delay = 0f)
        {
            Object.Destroy(gameObject, delay);
        }
    }
}