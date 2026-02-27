using UnityEngine;

namespace Library.Extensions
{
    public static class GameObjectExtensions
    {
        // 子オブジェクトを全て削除する
        public static void DestroyChildren(this GameObject gameObject)
        {
            Transform root = gameObject.transform;

            if (!Application.isPlaying)
            {
                for (int i = root.childCount - 1; i >= 0; i--)
                {
                    Object.DestroyImmediate(root.GetChild(i).gameObject);
                }
            }
            else
            {
                for (int i = root.childCount - 1; i >= 0; i--)
                {
                    Object.Destroy(root.GetChild(i).gameObject);
                }
            }
        }
    }
}
