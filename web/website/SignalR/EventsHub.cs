using System;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

namespace Twilio.OwlFinance.Web.SignalR
{
    public class EventsHub : Hub
    {
        public void Send(string accountId, string message)
        {
            if (!string.IsNullOrEmpty(accountId))
            {
                Clients.OthersInGroup(accountId).MessageReceived(accountId, message);
            }
        }

        public void Subscribe(string accountId)
        {
            if (!string.IsNullOrEmpty(accountId))
            {
                Groups.Add(Context.ConnectionId, accountId);
            }
        }

        public void UnSubscribe(string accountId)
        {
            if (!string.IsNullOrEmpty(accountId))
            {
                Groups.Remove(Context.ConnectionId, accountId);
            }
        }

        public override Task OnConnected()
        {
            return base.OnConnected();
        }
    }
}