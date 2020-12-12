using System;
using System.Collections.Generic;
using System.Threading;

namespace Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            Test test1 = new Test("Pomme");
            Test test2 = new Test("Poire");

            List<Test> tests = new List<Test> { test1, test2 };
            foreach(Test test in tests)
            {
                Console.WriteLine(test);
            }

            Test test3 = tests[1];
            test3.Title = "Ça marche";

            Console.WriteLine("\n\n");
            foreach (Test test in tests)
            {
                Console.WriteLine(test);
            }
        }
    }
}
