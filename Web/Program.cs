using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

var builder = WebApplication.CreateBuilder();
var app = builder.Build();
app.UseRouting();
app.UseDefaultFiles();
app.UseStaticFiles();

app.Run(async (context) =>
{
    var response = context.Response;
    var request = context.Request;
    if (request.Path == "/api/setting")
    {
        var record = "Incorrect data";

        if (request.HasJsonContentType())
        {
            var jsonOptions = new JsonSerializerOptions();
            jsonOptions.Converters.Add(new GlitchConverter());
            var glitch = await request.ReadFromJsonAsync<Glitch>(jsonOptions);
            if (glitch != null)
            {
                record =
                    $"{glitch.Duration},{glitch.FirstColor},{glitch.SecondColor},{glitch.TextColor}";
                await using (StreamWriter streamWriter = new(File.Open("wwwroot/resource.txt", FileMode.Append)))
                {
                    streamWriter.WriteLine(record);
                }
            }
        }
    }
    else if (request.Path == "/api/pull")
    {
        List<Glitch> glitches = new List<Glitch>();
        using (StreamReader streamReader = new(File.Open("wwwroot/resource.txt", FileMode.Open)))
        {
            string str;
            while ((str = streamReader.ReadLineAsync().Result) != null)
            {
                string[] record = str.Split(',');
                glitches.Add(new Glitch(Int32.Parse(record[0]), record[1], record[2], record[3]));
                response.ContentType = "application/json; charset=utf-8";
            }
        }

        await response.WriteAsJsonAsync(glitches.ToArray());
    }
    else if (request.Path == "/api/delete")
    {
        await using (FileStream fileStream = File.Create("wwwroot/resource.txt"))
        {
        }
    }
    else
    {
        response.ContentType = "text/html; charset=utf-8";
        await response.SendFileAsync("wwwroot/science.html");
    }
});

app.Run();

public record Glitch(int Duration, string FirstColor, string SecondColor, string TextColor);

public class GlitchConverter : JsonConverter<Glitch>
{
    public override Glitch Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var glitchDuration = 0;
        string glitchFirst = "#000000";
        string glitchSecond = "#000000";
        string glitchText = "#000000";

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.PropertyName)
            {
                var propertyName = reader.GetString();
                reader.Read();
                switch (propertyName)
                {
                    case "duration" when reader.TokenType == JsonTokenType.Number:
                        glitchDuration = reader.GetInt32();
                        break;
                    case "duration" when reader.TokenType == JsonTokenType.String:
                        string? stringValue = reader.GetString();
                        if (int.TryParse(stringValue, out int value))
                        {
                            glitchDuration = value;
                        }

                        break;
                    case "firstColor":
                        string? prop1 = reader.GetString();
                        if (prop1 != null)
                            glitchFirst = prop1;
                        break;
                    case "secondColor":
                        string? prop2 = reader.GetString();
                        if (prop2 != null)
                            glitchSecond = prop2;
                        break;
                    case "textColor":
                        string? prop3 = reader.GetString();
                        if (prop3 != null)
                            glitchText = prop3;
                        break;
                }
            }
        }

        return new Glitch(glitchDuration, glitchFirst, glitchSecond, glitchText);
    }

    public override void Write(Utf8JsonWriter writer, Glitch glitch, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteNumber("duration",glitch.Duration);
        writer.WriteString("firstColor", glitch.FirstColor);
        writer.WriteString("secondColor", glitch.SecondColor);
        writer.WriteString("textColor", glitch.TextColor);
        writer.WriteEndObject();
    }
}