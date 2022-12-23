using BenchmarkDotNet.Attributes;
using System.Globalization;
using System.Text;

namespace Benchmarking
{
    public class StringBenchmarks
    {
        int[] numbers;
        public StringBenchmarks()
        {
            numbers = Enumerable.Range(start: 1, count: 20).ToArray();
        }

        [Benchmark(Baseline = true)]
        public string ConcatenationStringTest()
        {
            string str = string.Empty;
            for(int i = 0; i < numbers.Length; i++)
            {
                str += numbers[i] + ", ";
            }
            return str;
        }

        [Benchmark]
        public string StringBuilderTest()
        {
            StringBuilder builder = new();
            for(int i = 0; i < numbers.Length; i++)
            {
                builder.Append(numbers[i]);
                builder.Append(", ");
            }
            return builder.ToString();
        }

    }
}
