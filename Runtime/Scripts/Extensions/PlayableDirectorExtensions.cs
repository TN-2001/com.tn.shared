using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.Playables;

namespace Library.Extensions
{
    public static class PlayableDirectorExtensions
    {
        /// <summary>
        /// Timelineの再生完了を待機する（UniTask対応）
        /// </summary>
        public static async UniTask PlayAsync(this PlayableDirector director, CancellationToken cancellationToken = default)
        {
            // 既に再生中なら一度停止
            if (director.state == PlayState.Playing)
                director.Stop();

            var tcs = new UniTaskCompletionSource();

            void OnStopped(PlayableDirector d)
            {
                if (d == director)
                {
                    d.stopped -= OnStopped;
                    tcs.TrySetResult();
                }
            }

            director.stopped += OnStopped;
            director.Play();

            try
            {
                await tcs.Task.AttachExternalCancellation(cancellationToken);
            }
            finally
            {
                director.stopped -= OnStopped;
            }
        }
    }
}
