using Easy.MessageHub;
using Microsoft.AspNetCore.SignalR;
using Data;
using Robot;

namespace WebServer.Hubs;

class InstrumentHub: Hub
{
    private readonly IHubContext<InstrumentHub>? _hubContext;
    private readonly IMessageHub? _messageHub;
    private SequenceManager _sequenceManager;

    public InstrumentHub(IHubContext<InstrumentHub> hubContext, IMessageHub messageHub, SequenceManager sequenceManager)
    {
        _hubContext = hubContext;
        _messageHub = messageHub;
        _sequenceManager = sequenceManager;

        SubscribeEvent();
    }

    private void SubscribeEvent()
    {
        _sequenceManager.OnSequenceProgressChanged += OnSequenceProgressChangedHandler;
    }

    private void OnSequenceProgressChangedHandler(object sender, SequenceProgress dto)
    {
        if (_hubContext != null)
        {
            _hubContext.Clients.All.SendAsync(HubEvents.OnSequenceProgress, dto);
        }
    }
}