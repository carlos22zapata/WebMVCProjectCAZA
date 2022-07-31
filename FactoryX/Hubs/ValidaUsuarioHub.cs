using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace FactoryX.Hubs
{   
    public class ValidaUsuarioHub : Hub
    {
        public override Task OnConnected()
        {
            ConnectedUser.Ids.Add(Context.ConnectionId);
            return base.OnConnected();
        }
        public override Task OnDisconnected(bool stopCalled)
        {
            ConnectedUser.Ids.Remove(Context.ConnectionId);
            return base.OnDisconnected(true);
        }
    }

    public static class ConnectedUser
    {
        public static List<string> Ids = new List<string>();
    }
}
