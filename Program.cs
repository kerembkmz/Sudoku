public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        // Hizmetleri ekle...
        builder.Services.AddRazorPages();

        var app = builder.Build();
        // Middleware'leri ekle...
        app.UseExceptionHandler("/Error");
        // ...

        app.UseAuthorization();
        app.MapRazorPages();

        // app.Run() yerine baþka bir Endpoint çaðýrabilirsiniz.
        app.MapGet("/", () => Results.Text("Hello, world!"));
        Board sudokuBoard = new Board(1);
        sudokuBoard.printBoard();


        app.Run();

       
       
        
    }


}
