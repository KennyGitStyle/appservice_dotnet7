
partial class Program
{
    static void MethodA()
    {
        TaskTitle("Starting Method A...");
        OutputThreadInfo();
        Thread.Sleep(3000);
        TaskTitle("Finished Method A.");
    }

    static void MethodB()
    {
        TaskTitle("Starting Method B...");
        OutputThreadInfo();
        Thread.Sleep(2000);
        TaskTitle("Finished Method B.");
    }

    static void MethodC()
    {
        TaskTitle("Starting Method C...");
        OutputThreadInfo();
        Thread.Sleep(1000);
        TaskTitle("Finished Method C.");
    }

    static decimal CallWebService()
    {
        TaskTitle("Starting call to web service...");
        OutputThreadInfo();
        Thread.Sleep((new Random()).Next(2000, 4000));
        TaskTitle("Finished call to web service");
        return 89.99M;
    }

    static string CallStoredProcedures(decimal amount)
    {
        TaskTitle("Starting calling to stored procedures...");
        OutputThreadInfo();
        Thread.Sleep((new Random()).Next(2000, 4000));
        TaskTitle("Finished call to stored procedures.");
        return $"12 products cost more than {amount:C}";
    }

    static void OuterMethod()
    {
        TaskTitle("Outer method starting...");
        Task innerTask = Task.Factory.StartNew(InnerMethod, TaskCreationOptions.AttachedToParent);
        TaskTitle("Outer method finished.");
    }

    static void InnerMethod()
    {
        TaskTitle("Inner method starting...");
        Thread.Sleep(2000);
        TaskTitle("Inner method finished.");
    }
}

