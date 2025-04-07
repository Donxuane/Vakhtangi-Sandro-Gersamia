using Microsoft.AspNetCore.SignalR;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

namespace BudgetingExpenses.Service.Live;

public class LiveLogsForwarder
{
    private readonly IHubContext<AppLiveHub> _hubContext;

    public LiveLogsForwarder(IHubContext<AppLiveHub> hubContext) {  _hubContext = hubContext; }

    public void Start()
    {
        Log.Logger = new LoggerConfiguration()
             .WriteTo.Console(theme: AnsiConsoleTheme.Code)
              .WriteTo.File("Logs/ErrorLogs.log", restrictedToMinimumLevel: LogEventLevel.Error)
              //.WriteTo.File("Logs/InformationLogs.log", restrictedToMinimumLevel: LogEventLevel.Information)
              .WriteTo.Sink(new SignalRSink(_hubContext))
            .CreateLogger();
    }
}
public class SignalRSink : ILogEventSink
{
    private readonly IHubContext<AppLiveHub> _hubContext;

    public SignalRSink(IHubContext<AppLiveHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public void Emit(LogEvent logEvent)
    {
        var message = $"[{logEvent.Timestamp:HH:mm:ss}] {logEvent.Level}: {logEvent.RenderMessage()}";
        _hubContext.Clients.All.SendAsync("ReceiveLog", message); 
    }
}
