using System;
using System.Collections.Generic;
using UnityEngine;

namespace Library.UI
{
    /// <summary>
    /// UIページ管理
    /// </summary>
    public class PageManager : MonoBehaviour
    {
        [SerializeField] private GameObject[] pages;
        private int currentIndex = -1;

        // ページ履歴
        private readonly Stack<int> history = new();


        private void Start()
        {
            ShowPage(0, false); // 最初は履歴に残さない
        }

        /// <summary>
        /// ページを表示する
        /// </summary>
        private void ShowPage(int index, bool addToHistory = true)
        {
            if (index < 0 || index >= pages.Length) return;

            if (currentIndex >= 0)
            {
                pages[currentIndex].SetActive(false);

                if (addToHistory)
                {
                    history.Push(currentIndex); // 履歴に残す
                }
            }

            pages[index].SetActive(true);
            currentIndex = index;
        }
        public void ShowPage(int index)
        {
            ShowPage(index, true);
        }
        public void ShowPage(String name)
        {
            int index = Array.FindIndex(pages, p => p.name == name);
            ShowPage(index, true);
        }

        /// <summary>
        /// ひとつ前のページに戻る
        /// </summary>
        public void Back()
        {
            if (history.Count == 0) return;

            int prev = history.Pop();
            ShowPage(prev, false); // 戻るときは履歴に追加しない
        }
    }
}
