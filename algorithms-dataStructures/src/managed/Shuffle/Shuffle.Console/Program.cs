﻿using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;

namespace Shuffle.Console
{
    using console = System.Console;
    class Program
    {
        static void Main(string[] args)
        {
            int numberOfListsAndElements = 2;
            console.WriteLine("Enter the number of lists with the number of elements for shuffle testing : ");
            numberOfListsAndElements = int.Parse(console.ReadLine());

            ShuffleNListsWithNElements(numberOfListsAndElements);
            ShuffleHugeList();
         
            console.ReadKey();
        }

        static void ShuffleNListsWithNElements(int numberOfListsAndElements)
        {
            var watch = Stopwatch.StartNew();
            Enumerable.Range(2, numberOfListsAndElements)
             .ToList()
             .ForEach(numberOfListElements =>
             {
                 var list = Enumerable.Range(0, numberOfListElements).ToList();
                 var beforeListArray = list.ToArray();
                 list.Shuffle(shuffleType: ShuffleType.AllElementsShouldBeOnDifferentPositions);
                 TestResults.Add(list.SequenceEqual(beforeListArray));
                 console.WriteLine($"Testing shuffling with list of {numberOfListElements}.");
             });

            watch.Stop();
            console.WriteLine($"Process took {watch.ElapsedMilliseconds} ms!");

            console.WriteLine(!TestResults.All(_ => _) ? "All lists shuffled correctly." : "Tests failed!");
        }

        static void ShuffleHugeList()
        {
            console.WriteLine($"Testing shuffling with list of 5000000 elements.");
            var watch = Stopwatch.StartNew();
            var list = Enumerable.Range(0, 5000000).ToList(); // 5 million elements
            var beforeListArray = list.ToArray();
            list.Shuffle(shuffleType: ShuffleType.AllElementsShouldBeOnDifferentPositions);

            console.WriteLine(!beforeListArray.SequenceEqual(list) ? "All lists shuffled correctly." : "Tests failed!");
            watch.Stop();
            console.WriteLine($"Process took {watch.ElapsedMilliseconds} ms!");

        }
        public static ICollection<bool> TestResults = new HashSet<bool>();
    }
}
