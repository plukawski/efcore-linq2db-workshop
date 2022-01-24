using System;
using System.Diagnostics;

namespace Linq2DbSynergyWorkshop.Helpers
{
    public class Stopper : IDisposable
    {
        private readonly string operationName;
        private readonly Stopwatch stopwatch;

        public Stopper(string operationName)
        {
            this.operationName = operationName;
            this.stopwatch = Stopwatch.StartNew();
        }

        public void Dispose()
        {
            this.stopwatch.Stop();
            Console.WriteLine($"Operation '{operationName}' ended in {stopwatch.ElapsedMilliseconds}ms");
        }
    }
}
