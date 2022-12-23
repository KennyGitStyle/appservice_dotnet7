
partial class Program
{
    static void SectionTitle(string title)
    {
        ConsoleColor previousColor = ForegroundColor;
        ForegroundColor = ConsoleColor.DarkYellow;
        WriteLine("*");
        WriteLine($"*{title}");
        WriteLine("*");
        ForegroundColor = previousColor;
    }
    static void TaskTitle(string title)
    {
        ConsoleColor previousColor = ForegroundColor;
            ForegroundColor = ConsoleColor.Green;
            WriteLine($"{title}");
            ForegroundColor = previousColor;
     }
    static void OutputThreadInfo()
    {
        Thread thread = Thread.CurrentThread;
        ConsoleColor previousColor = ForegroundColor;
        ForegroundColor = ConsoleColor.DarkCyan;
        WriteLine("Thread Id: {0}, Priority: {1}, Background: {2}, Name: {3}",
            thread.ManagedThreadId, thread.Priority, thread.IsBackground, thread.Name ?? "null");
        ForegroundColor = previousColor;
        
    }
    
}
