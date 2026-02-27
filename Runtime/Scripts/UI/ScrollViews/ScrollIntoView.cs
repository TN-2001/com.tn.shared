using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

namespace Library.UI
{
    /// <summary>
    /// ScrollRect内で選択されたUI要素が常に表示されるようにスクロールするコンポーネント
    /// </summary>
    [RequireComponent(typeof(ScrollRect))]
    public class ScrollIntoView : MonoBehaviour
    {
        private ScrollRect scrollRect;
        private InputAction move;
        private CancellationTokenSource holdToken;

        [SerializeField, Tooltip("スクロールアニメーション時間（秒）")]
        private float scrollDuration = 0.15f;
        [SerializeField] private bool isBackPosition = false; // OnEnableでスクロールをもとに戻すか


        private void Awake()
        {
            scrollRect = GetComponent<ScrollRect>();
        }

        private void OnEnable()
        {
            if (isBackPosition)
            {
                scrollRect.normalizedPosition = new Vector2(0, 1);
            }
            WaitForInputModuleAsync().Forget();
        }

        private void OnDisable()
        {
            if (move != null)
            {
                move.performed -= OnMovePerformed;
                move.canceled -= OnMoveCanceled;
            }
            holdToken?.Cancel();
        }


        private async UniTask WaitForInputModuleAsync()
        {
            // EventSystemが存在するまで待つ
            await UniTask.WaitUntil(() => EventSystem.current != null);

            InputSystemUIInputModule inputModule = null;

            // InputSystemUIInputModuleが存在し、moveアクションが有効になるまで待つ
            await UniTask.WaitUntil(() =>
            {
                inputModule = EventSystem.current.currentInputModule as InputSystemUIInputModule;
                return inputModule != null && inputModule.move != null && inputModule.move.action != null;
            });

            move = inputModule.move.action;
            move.performed += OnMovePerformed;
            move.canceled += OnMoveCanceled;
        }


        private void OnMovePerformed(InputAction.CallbackContext context)
        {
            EnsureVisibleWithTween();
            CheckHoldAsync().Forget();
        }

        private void OnMoveCanceled(InputAction.CallbackContext context)
        {
            holdToken?.Cancel();
        }


        private async UniTaskVoid CheckHoldAsync()
        {
            holdToken?.Cancel();
            holdToken = new CancellationTokenSource();

            GameObject selectObj = EventSystem.current.currentSelectedGameObject;

            // 長押しループ
            while (!holdToken.IsCancellationRequested)
            {
                await UniTask.WaitUntil(() => selectObj != EventSystem.current.currentSelectedGameObject, cancellationToken: holdToken.Token);
                selectObj = EventSystem.current.currentSelectedGameObject;
                EnsureVisibleWithTween();
            }
        }


        /// <summary>
        /// 現在選択されている要素をScrollRect内に収まるようにスムーズに移動
        /// </summary>
        private void EnsureVisibleWithTween()
        {
            var eventSystem = EventSystem.current;
            if (eventSystem == null || eventSystem.currentSelectedGameObject == null)
                return;

            RectTransform selectedRect = eventSystem.currentSelectedGameObject.GetComponent<RectTransform>();
            if (selectedRect == null)
                return;

            RectTransform viewport = scrollRect.viewport;
            RectTransform content = scrollRect.content;

            Vector3[] viewportCorners = new Vector3[4];
            Vector3[] targetCorners = new Vector3[4];
            viewport.GetWorldCorners(viewportCorners);
            selectedRect.GetWorldCorners(targetCorners);

            float viewportMinY = viewportCorners[0].y;
            float viewportMaxY = viewportCorners[1].y;
            float targetMinY = targetCorners[0].y;
            float targetMaxY = targetCorners[1].y;

            float diff = 0f;

            if (targetMinY < viewportMinY)
            {
                // 下方向にスクロール
                diff = viewportMinY - targetMinY;
            }
            else if (targetMaxY > viewportMaxY)
            {
                // 上方向にスクロール
                diff = targetMaxY - viewportMaxY;
                diff = -diff; // contentの方向が逆
            }

            if (Mathf.Abs(diff) > 1e-2f)
            {
                // DOTweenでスムーズに移動
                Vector3 targetPos = content.localPosition + new Vector3(0, diff, 0);
                content.DOKill(); // 前回のTweenを中断
                content.DOLocalMoveY(targetPos.y, scrollDuration).SetEase(Ease.OutQuad);
            }
        }
    }
}