namespace Northwind.Shared;

public class Animal
{
    [Coder("Kenneth Hendricks", "24 December 2022")]
    [Coder("Ali Sajjad", "28 November 2022")]
    [Obsolete($"use {nameof(SpeakBetter)} instead.")]
    public void Speak()
    {
        WriteLine("Woof...");
    }

    public void SpeakBetter()
    {
        WriteLine("Wooooooooof...");
    }
}

