using System.Threading;
using Cysharp.Threading.Tasks;

namespace _Utilities
{
    public static class UniTaskAsyncHelper
    {
        public delegate bool ConditionHandler();

        public static async UniTask WaitWhile(ConditionHandler condition, int pollingRateMs = 50)
        {
            while (condition.Invoke())
            {
                await UniTask.Delay(pollingRateMs);
            }
        }

        public static async UniTask WaitUntil(ConditionHandler condition, int pollingRateMs = 50)
        {
            while (!condition.Invoke())
            {
                await UniTask.Delay(pollingRateMs);
            }
        }

        public static async UniTask WaitWhileOrTimeout(ConditionHandler condition, int pollingRateMs = 50,
            int timeoutMs = 5000)
        {
            await UniTask.WhenAny(WaitWhile(condition, pollingRateMs), UniTask.Delay(timeoutMs));
        }

        public static UniTask GetCancellationTokenTask(CancellationToken cancelToken)
        {
            return UniTask.Run(() =>
                {
                    cancelToken.WaitHandle.WaitOne();
                },
                cancellationToken: cancelToken);
        }
    }
}
