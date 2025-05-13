using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace _Project.Extensions.UniTasks {
    public static class UniTaskHelper
    {
        /// <summary>
        /// Executes the specified action after waiting for one frame.
        /// </summary>
        /// <param name="action">The action to execute.</param>
        /// <param name="timing">Optional: The timing to wait. Defaults to PlayerLoopTiming.Update.</param>
        /// <param name="cancellationToken">Optional: Token to cancel the operation.</param>
        /// <returns>A UniTask that can be awaited.</returns>
        public static async UniTask ExecuteAfterOneFrame(
            Action action, 
            PlayerLoopTiming timing = PlayerLoopTiming.Update, 
            CancellationToken cancellationToken = default)
        {
            // Wait for one frame
            await UniTask.Yield(timing, cancellationToken);
        
            // Execute the action if not cancelled
            if (!cancellationToken.IsCancellationRequested)
            {
                action?.Invoke();
            }
        }
    
        /// <summary>
        /// Executes the specified async action after waiting for one frame.
        /// </summary>
        /// <param name="asyncAction">The async action to execute.</param>
        /// <param name="timing">Optional: The timing to wait. Defaults to PlayerLoopTiming.Update.</param>
        /// <param name="cancellationToken">Optional: Token to cancel the operation.</param>
        /// <returns>A UniTask that can be awaited.</returns>
        public static async UniTask ExecuteAfterOneFrame(
            Func<UniTask> asyncAction, 
            PlayerLoopTiming timing = PlayerLoopTiming.Update, 
            CancellationToken cancellationToken = default)
        {
            // Wait for one frame
            await UniTask.Yield(timing, cancellationToken);
        
            // Execute the async action if not cancelled
            if (!cancellationToken.IsCancellationRequested && asyncAction != null)
            {
                await asyncAction();
            }
        }
    
        /// <summary>
        /// Executes the specified action after waiting for one frame and returns a value.
        /// </summary>
        /// <typeparam name="T">The type of value to return.</typeparam>
        /// <param name="func">The function to execute.</param>
        /// <param name="timing">Optional: The timing to wait. Defaults to PlayerLoopTiming.Update.</param>
        /// <param name="cancellationToken">Optional: Token to cancel the operation.</param>
        /// <returns>A UniTask with the result value.</returns>
        public static async UniTask<T> ExecuteAfterOneFrame<T>(
            Func<T> func, 
            PlayerLoopTiming timing = PlayerLoopTiming.Update, 
            CancellationToken cancellationToken = default)
        {
            // Wait for one frame
            await UniTask.Yield(timing, cancellationToken);
        
            // Execute the function if not cancelled
            if (!cancellationToken.IsCancellationRequested && func != null)
            {
                return func();
            }
        
            return default;
        }
    
        /// <summary>
        /// Executes the specified async function after waiting for one frame and returns a value.
        /// </summary>
        /// <typeparam name="T">The type of value to return.</typeparam>
        /// <param name="asyncFunc">The async function to execute.</param>
        /// <param name="timing">Optional: The timing to wait. Defaults to PlayerLoopTiming.Update.</param>
        /// <param name="cancellationToken">Optional: Token to cancel the operation.</param>
        /// <returns>A UniTask with the result value.</returns>
        public static async UniTask<T> ExecuteAfterOneFrame<T>(
            Func<UniTask<T>> asyncFunc, 
            PlayerLoopTiming timing = PlayerLoopTiming.Update, 
            CancellationToken cancellationToken = default)
        {
            // Wait for one frame
            await UniTask.Yield(timing, cancellationToken);
        
            // Execute the async function if not cancelled
            if (!cancellationToken.IsCancellationRequested && asyncFunc != null)
            {
                return await asyncFunc();
            }
        
            return default;
        }
    }
}