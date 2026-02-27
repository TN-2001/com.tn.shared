using UnityEngine;
using UnityEngine.Events;
using TMPro;
using Cysharp.Threading.Tasks;
using System.Threading;

namespace Library.UI
{
    /*
        タイピングゲームのテキストを表示するクラス
        入力された文字に応じて、色を変更する
    */

    public class TypingText : MonoBehaviour
    {
        // コンポーネント
        private TextMeshProUGUI displayText;

        // パラメータ
        [SerializeField] private Color onColor = Color.white;
        [SerializeField] private Color offColor = Color.grey;

        // イベント
        public UnityEvent onCorrect = new();
        public UnityEvent onIncorrect = new();


        /// <summary>
        /// ゲッター
        /// </summary>
        /// <returns></returns>
        public string GetText()
        {
            return displayText.text;
        }


        private void Awake()
        {
            // コンポーネントの取得
            displayText = GetComponent<TextMeshProUGUI>();
        }


        public void ChangeText(string text, bool isOn = true)
        {
            if (isOn)
            {
                displayText.text = $"<color=#{ColorUtility.ToHtmlStringRGBA(onColor)}>" + text;
            }
            else
            {
                displayText.text = $"<color=#{ColorUtility.ToHtmlStringRGBA(offColor)}>" + text;
            }
        }

        public async UniTask UpdateTyping(string text, CancellationToken token = new())
        {
            int index = 0;

            while (index < text.Length)
            {
                // テキストの色更新（正しく入力されたところまで grey に）
                displayText.text = text;
                displayText.text = displayText.text.Insert(index, $"<color=#{ColorUtility.ToHtmlStringRGBA(onColor)}>");
                displayText.text = $"<color=#{ColorUtility.ToHtmlStringRGBA(offColor)}>" + displayText.text;

                // 無視する文字はスキップ
                if (Typing.IsIgnoreChar(text[index]))
                {
                    index++;
                    continue;
                }

                // 入力があるまで待機
                await UniTask.WaitUntil(() => UnityEngine.Input.anyKeyDown, cancellationToken: token);

                if (Time.timeScale == 0f)
                {
                    await UniTask.WaitUntil(() => Time.timeScale == 1f, cancellationToken: token);
                    continue;
                }

                // 文字列以外の入力を無視
                string inputString = UnityEngine.Input.inputString;
                if (string.IsNullOrEmpty(inputString))
                    continue;

                // インデックス更新
                foreach (char input in inputString)
                {
                    char lowerInput = char.ToLower(input);
                    char lowerText = char.ToLower(text[index]);

                    if (lowerInput == lowerText)
                    {
                        onCorrect?.Invoke();

                        index++;
                        if (index >= text.Length)
                        {
                            break;
                        }
                    }
                    else
                    {
                        onIncorrect?.Invoke();
                    }
                }
                
                displayText.text = $"<color=#{ColorUtility.ToHtmlStringRGBA(offColor)}>" + displayText.text;
            }
        }
    }
}
