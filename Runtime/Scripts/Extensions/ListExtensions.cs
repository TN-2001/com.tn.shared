using System.Collections.Generic;
using UnityEngine;

namespace Library.Extensions
{
    public static class ListExtensions
    {
        /// <summary>
        /// リストをシャッフルする（Fisher–Yatesアルゴリズム）
        /// </summary>
        public static void Shuffle<T>(this List<T> list)
        {
            if (list == null || list.Count <= 1) return;

            for (int i = list.Count - 1; i > 0; --i)
            {
                // 0以上i以下のランダムな整数を取得
                int j = Random.Range(0, i + 1);

                // i番目とj番目の要素を交換する
                (list[j], list[i]) = (list[i], list[j]);
            }
        }

        /// <summary>
        /// リストから重複しないランダムな要素を指定した数だけ取得する
        /// </summary>
        public static List<T> TakeRandomUnique<T>(this List<T> list, int count)
        {
            if (list == null || list.Count == 0) return new List<T>();

            List<T> temp = new List<T>(list);
            temp.Shuffle(); // Shuffle 拡張メソッドを使う
            count = Mathf.Clamp(count, 0, temp.Count);
            return temp.GetRange(0, count);
        }
    }
}
