``` ini

BenchmarkDotNet=v0.13.2, OS=Windows 10 (10.0.19045.2364)
AMD A8-6600K APU with Radeon(tm) HD Graphics, 1 CPU, 4 logical and 2 physical cores
Frequency=14318180 Hz, Resolution=69.8413 ns, Timer=HPET
.NET SDK=7.0.101
  [Host]     : .NET 7.0.1 (7.0.122.56804), X64 RyuJIT AVX
  DefaultJob : .NET 7.0.1 (7.0.122.56804), X64 RyuJIT AVX


```
|                  Method |       Mean |    Error |   StdDev | Ratio |
|------------------------ |-----------:|---------:|---------:|------:|
| ConcatenationStringTest | 1,427.6 ns | 22.42 ns | 20.97 ns |  1.00 |
|       StringBuilderTest |   735.8 ns |  9.10 ns |  8.51 ns |  0.52 |
