await foreach (int number in GetNumbersAsync())
{
    WriteLine($"Number: {number}");
}
async static IAsyncEnumerable<int> GetNumbersAsync()
{
    Random random = new Random();
    await Task.Delay(random.Next(1500, 3000));
    yield return random.Next(0, 1001);
    await Task.Delay(random.Next(1500, 3000));
    yield return random.Next(0, 1001);
    await Task.Delay(random.Next(1500, 3000));
    yield return random.Next(0, 1001);
    
}