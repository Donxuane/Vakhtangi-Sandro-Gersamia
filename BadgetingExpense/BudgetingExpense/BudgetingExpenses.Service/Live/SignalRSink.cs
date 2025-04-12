using Microsoft.AspNetCore.SignalR;
using Serilog.Core;
using Serilog.Events;

namespace BudgetingExpenses.Service.Live;
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
