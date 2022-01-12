using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

var builder = WebApplication.CreateBuilder();
var app = builder.Build();

app.Run(async (context) =>
{
    var response = context.Response;
    var request = context.Request;
    if (request.Path == "/api/setting")
    {
        var responseText = "Incorrect data";

        if (request.HasJsonContentType())
        {
            var jsonOptions = new JsonSerializerOptions();
            jsonOptions.Converters.Add(new GlitchConverter());
            var glitch = await request.ReadFromJsonAsync<Glitch>(jsonOptions);
            if (glitch != null)
                responseText =
                    $"Duration: {glitch.Duration}, first color: {glitch.FirstColor}, second color: {glitch.SecondColor}, text color: {glitch.TextColor}";
        }

        await response.WriteAsJsonAsync(new {text = responseText});
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
        writer.WriteNumber("duration", glitch.Duration);
        writer.WriteString("firstColor", glitch.FirstColor);
        writer.WriteString("secondColor", glitch.SecondColor);
        writer.WriteString("textColor", glitch.TextColor);

        writer.WriteEndObject();
    }
}