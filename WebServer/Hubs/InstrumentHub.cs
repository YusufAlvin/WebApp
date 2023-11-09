using Easy.MessageHub;
using Microsoft.AspNetCore.SignalR;
using Data;

namespace WebServer.Hubs;

class InstrumentHub: Hub
{
    private readonly IHubContext<InstrumentHub>? _hubContext;
    private readonly IMessageHub? _messageHub;

    public InstrumentHub(IHubContext<InstrumentHub> hubContext, IMessageHub messageHub)
    {
        _hubContext = hubContext;
        _messageHub = messageHub;

        _messageHub.Subscribe<SequenceProgress>(OnSequenceProgress);
    }

    private void OnSequenceProgress(SequenceProgress dto)
    {
        if (_hubContext != null)
        {
            _hubContext.Clients.All.SendAsync(HubEvents.OnSequenceProgress, dto);
        }
    }
}