using System.Xml;
using System.Xml.Xsl;

namespace XMLBrowser;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();
        app.UseStaticFiles();
        app.MapGet(("/xml"), (IWebHostEnvironment _env) =>
        {
            var xmlPath = Path.Combine(_env.ContentRootPath, ".", "tv.xml");
            var xsltPath = Path.Combine(_env.ContentRootPath, ".", "tv.xsl");

            var transform = new XslCompiledTransform();
            transform.Load(xsltPath);

            using var sw = new StringWriter();
            using var writer = XmlWriter.Create(sw, new XmlWriterSettings { Indent = true });

            transform.Transform(xmlPath, writer);

            return Results.Text(sw.ToString(), "text/html");
        });
        app.Run();
    }
}