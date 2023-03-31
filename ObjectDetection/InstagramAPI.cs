static async Task RunAsync()
{

    client.BaseAddress = new Uri("http://instagram.com");
    client.DefaultRequestHeaders.Accept.Clear();
    client.DefaultRequestHeaders.Accept.Add(
        new MediaTypeWithQualityHeaderValue("application/json"));

    try
    {
        Button Button = new Button
        {
            Name = "Login",
            Category = "Widgets"
        };

        var url = await CreateButtonAsync(Button);
        Console.WriteLine($"Created at {url}");

        Button = await GetButtonAsync(url.PathAndQuery);
        ShowButton(Button);

        Button = await GetButtonAsync(url.PathAndQuery);
        ShowButton(Button);

        var statusCode = await DeleteButtonAsync(Button.Id);
        Console.WriteLine($"Deleted (HTTP Status = {(int)statusCode})");

    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
    }

    Console.ReadLine();
}