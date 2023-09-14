using System.Buffers;
using System.IO.Pipelines;
using System.Net;
using System.Text;

namespace PaymentGateWayService.Middleware;

public static class MyCustomMiddlewareExtensions
{
    public static IApplicationBuilder UseMyCustomMiddleware(
        this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<MyCustomMiddleware>();
    }
}

public class MyCustomMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<MyCustomMiddleware> _logger;

    public MyCustomMiddleware(RequestDelegate next, ILogger<MyCustomMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
            if (context.Response.StatusCode == (int)HttpStatusCode.Unauthorized)
                throw new UnauthorizedAccessException();
        }
        catch (Exception ex)
        {
            _logger.LogError("({errorCode})Message Error: {ex}\r\nqueryString/Body:{queryString}", "500", ex, await GetInfo(context));
        }
    }

    private async Task<string> GetInfo(HttpContext context)
    {
        var request = context.Request;
        string strInfoBody = string.Empty;
        bool infoBody = request.ContentLength > 0;
        if (infoBody)
        {
            request.EnableBuffering();
            request.Body.Position = 0;
            List<string> tmp = await GetListOfStringFromPipe(request.BodyReader);
            request.Body.Position = 0;

            strInfoBody = string.Concat("\r\nBody: ", string.Join("", tmp.ToArray()));
        }

        return string.Concat('[', request.Method, "]: ", request.Path, '/', request.QueryString,(infoBody ? strInfoBody : string.Empty));
    }
    
    private async Task<List<string>> GetListOfStringFromPipe(PipeReader reader)
    {
        List<string> results = new List<string>();

        while (true)
        {
            ReadResult readResult = await reader.ReadAsync();
            var buffer = readResult.Buffer;

            SequencePosition? position = null;

            do
            {
                // Look for a EOL in the buffer
                position = buffer.PositionOf((byte)'\n');

                if (position != null)
                {
                    var readOnlySequence = buffer.Slice(0, position.Value);
                    AddStringToList(results, in readOnlySequence);

                    // Skip the line + the \n character (basically position)
                    buffer = buffer.Slice(buffer.GetPosition(1, position.Value));
                }
            }
            while (position != null);


            if (readResult.IsCompleted && buffer.Length > 0)
            {
                AddStringToList(results, in buffer);
            }

            reader.AdvanceTo(buffer.Start, buffer.End);

            // At this point, buffer will be updated to point one byte after the last
            // \n character.
            if (readResult.IsCompleted)
            {
                break;
            }
        }

        return results;
    }

    private static void AddStringToList(List<string> results, in ReadOnlySequence<byte> readOnlySequence)
    {
        // Separate method because Span/ReadOnlySpan cannot be used in async methods
        ReadOnlySpan<byte> span = readOnlySequence.IsSingleSegment ? readOnlySequence.First.Span : readOnlySequence.ToArray().AsSpan();
        results.Add(Encoding.UTF8.GetString(span));
    }
}