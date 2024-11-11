using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Extensions.OpenAI.Assistants;
using Microsoft.Extensions.Logging;
using System.Numerics.Tensors;

namespace AssistantSample;

/// <summary>
/// Defines assistant skills that can be triggered by the assistant chat bot.
/// </summary>
public class AssistantSkills
{
    readonly ILogger<AssistantSkills> logger;

    public AssistantSkills(ILogger<AssistantSkills> logger)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }


    [Function(nameof(GetCosh))]
    public String GetCosh(
        [AssistantSkillTrigger(
            "Calculate the Cosh of x",
            Model = "%CHAT_MODEL_DEPLOYMENT_NAME%"
        )]
            string x
    )
    {
        // Log the location for which the weather is being requested
        this.logger.LogInformation("Calculating the Cosh of: {0}", x);

        float xValue = float.Parse(x);

        // Create an array of floats
        float[] inputArray = { xValue };

        // Create ReadOnlySpan from the input array
        ReadOnlySpan<float> inputSpan = new ReadOnlySpan<float>(inputArray);

        // Create an output array to store results
        float[] outputArray = new float[inputArray.Length];

        // Create Span from the output array
        Span<float> outputSpan = new Span<float>(outputArray);

        // Calculate hyperbolic cosines
        TensorPrimitives.Cosh(inputSpan, outputSpan);

        // Get results

        string resultCosh = "Hyperbolic Cosines: ";
        foreach (var value in outputArray)
        {
            resultCosh += value.ToString() + " ";
        }

        // return the calulated hyperbolic cosines
        return resultCosh;
    }


    [Function(nameof(GetTime))]
    public String GetTime(
        [AssistantSkillTrigger(
            "Get the current time of location using time zone",
            Model = "%CHAT_MODEL_DEPLOYMENT_NAME%"
        )]
            string timeZone
    )
    {
        // Log the time zone for which the current time is being requested
        this.logger.LogInformation("Getting time in {0}", timeZone);

        // Find the system time zone by the provided time zone ID
        TimeZoneInfo timeZoneResult = TimeZoneInfo.FindSystemTimeZoneById(timeZone);

        // Get the current UTC time
        DateTime utcNow = DateTime.UtcNow;

        // Convert the UTC time to the local time in the specified time zone
        DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(utcNow, timeZoneResult);

        // Return the local time as a string in short time format
        return localTime.ToString("t");
    }

    [Function(nameof(GetWeather))]
    public String GetWeather(
    [AssistantSkillTrigger(
            "Get the weather in location",
            Model = "%CHAT_MODEL_DEPLOYMENT_NAME%"
        )]
            string location
)
    {
        // Log the location for which the weather is being requested
        this.logger.LogInformation("Getting weather for location: {0}", location);

        // Return a mock weather response for the given location
        return $"The current weather in {location} is 72 degrees and sunny.";
    }
}
