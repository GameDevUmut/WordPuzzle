using System.Threading;
using System.Threading.Tasks;

namespace _Utilities
{
    public static class AsyncHelper
    {
        public delegate bool ConditionHandler();

        public static async Task WaitWhile(ConditionHandler condition, int pollingRateMs = 50)
        {
            while (condition.Invoke())
            {
                await Task.Delay(pollingRateMs);
            }
        }

        public static async Task WaitUntil(ConditionHandler condition, int pollingRateMs = 50)
        {
            while (!condition.Invoke())
            {
                await Task.Delay(pollingRateMs);
            }
        }

        public static async Task WaitWhileOrTimeout(ConditionHandler condition, int pollingRateMs = 50, int timeoutMs = 5000)
        {
            await Task.WhenAny(WaitWhile(condition, pollingRateMs), Task.Delay(timeoutMs));
        }

        public static Task GetCancellationTokenTask(CancellationToken cancelToken)
        {
            Task cancellationTask = Task.Run(() =>
                {
                    cancelToken.WaitHandle.WaitOne();
                },
                cancelToken);

            return cancellationTask;
        }
    }
}
