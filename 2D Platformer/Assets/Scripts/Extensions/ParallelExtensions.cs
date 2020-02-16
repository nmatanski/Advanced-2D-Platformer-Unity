using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace Platformer.Extensions
{
    public static class ParallelExtensions
    {
        /// <summary>
        /// Executes a for loop in which max may run in parallel.
        /// Example: new Action<int>((i) => { //body to iterate }).ParallelFor(0, 5);
        /// </summary>
        /// <param name="max"></param>
        /// <param name="function"></param>
        public static void ParallelFor(this Action<int> function, int minIndex, int max)
        {
            int iterationsPassed = 0;
            var resetEvent = new ManualResetEvent(false);

            for (int i = minIndex; i < max; i++)
            {
                ThreadPool.QueueUserWorkItem(
                    (state) =>
                    {
                        int currentIteration = (int)state;

                        try
                        {
                            function(currentIteration);
                        }
                        catch (Exception e)
                        {
                            Debug.LogException(e);
                        }

                        if (Interlocked.Increment(ref iterationsPassed) == max)
                            resetEvent.Set();
                    },
                i);
            }

            resetEvent.WaitOne();
        }


        /// <summary>
        /// Executes a for loop in which max may run in parallel.
        /// Example: new Action<int>((i) => { //body to iterate }).ParallelFor(0, 5);
        /// </summary>
        /// <param name="max"></param>
        /// <param name="function"></param>
        public static void ParallelFor(this Action<int> function, int iterations)
        {
            int iterationsPassed = 0;
            var resetEvent = new ManualResetEvent(false);

            for (int i = 0; i < iterations; i++)
            {
                ThreadPool.QueueUserWorkItem(
                    (state) =>
                    {
                        int currentIteration = (int)state;

                        try
                        {
                            function(currentIteration);
                        }
                        catch (Exception e)
                        {
                            Debug.LogException(e);
                        }

                        if (Interlocked.Increment(ref iterationsPassed) == iterations)
                            resetEvent.Set();
                    },
                i);
            }

            resetEvent.WaitOne();
        }


        /// <summary>
        /// Executes a foreach loop in which max may run in parallel.
        /// function: (collectionItem) => { // Do something with collectionItem }
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="function"></param>
        public static void ParallelForEach<T>(this IEnumerable<T> collection, Action<T> function)
        {
            int iterations = 0;
            int iterationsPassed = 0;
            var resetEvent = new ManualResetEvent(false);

            foreach (var item in collection)
            {
                Interlocked.Increment(ref iterations);
                ThreadPool.QueueUserWorkItem(
                    (state) =>
                    {
                        var subject = (T)state;

                        try
                        {
                            function(subject);
                        }
                        catch (Exception e)
                        {
                            Debug.LogException(e);
                        }

                        if (Interlocked.Increment(ref iterationsPassed) == iterations)
                            resetEvent.Set();
                    },
                item);
            }

            resetEvent.WaitOne();
        }


        ///Examples
        //new Action<int>(
        //    (i) =>
        //    {
        //        print(numbers[i]);
        //    }).ParallelFor(0, 5);
        //
        //new Action<int>(
        //    (i) =>
        //    {
        //        print(numbers[i]);
        //    }).ParallelFor(5);
    }
}
