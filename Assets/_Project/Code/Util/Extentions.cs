using UnityEngine;

namespace _Project.Code.Util
{
    public static class Extentions
    {
        public static T AddOrGet<T>(this GameObject mono) where T : Component => 
            mono.GetComponent<T>() ?? mono.gameObject.AddComponent<T>();
        
        public static void SetActive(this Transform transform, bool value) => 
            transform.gameObject.SetActive(value);
        
        public static void SetActive(this MonoBehaviour mono, bool value) => 
            mono.gameObject.SetActive(value);
    }
}