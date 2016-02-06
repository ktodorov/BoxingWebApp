using System;
using Microsoft.AspNet.SignalR;
using BoxingWebApp.Helpers;
using System.Threading.Tasks;
using System.Linq;

public class ChatHub : Hub
{
    private readonly static ConnectionMapping<string> _connections =
        new ConnectionMapping<string>();

    private string _username
    {
        get
        {
            return Context.QueryString["userName"];
        }
    }

    public void SendChatMessage(string who, string message, string from)
    {
        // Then it's chat group for all users, so we send message to all
        if (who == "all")
        {
            Clients.All.addChatMessage(message, DateTime.Now.ToString("HH:mm, dd/MM/yyyy"), from, true);
            return;
        }

        Clients.Client(Context.ConnectionId).addChatMessage(message, DateTime.Now.ToString("HH:mm, dd/MM/yyyy"), from);

        foreach (var connectionId in _connections.GetConnections(who))
        {
            Clients.Client(connectionId).addChatMessage(message, DateTime.Now.ToString("HH:mm, dd/MM/yyyy"), from);
        }
    }

    public override Task OnConnected()
    {
        // We show all currently connected users
        var currentUsers = _connections.AllConnectedUsers();

        foreach (var user in currentUsers)
        {
            if (user != _username)
            {
                Clients.Client(Context.ConnectionId).userConnected(user);
            }
        }

        _connections.Add(_username, Context.ConnectionId);

        // We inform all other users that current user has connected
        Clients.AllExcept(Context.ConnectionId).userConnected(_username);

        return base.OnConnected();
    }

    public override Task OnDisconnected(bool stopCalled)
    {
        _connections.Remove(_username, Context.ConnectionId);

        Clients.AllExcept(Context.ConnectionId).userDisconnected(_username);

        return base.OnDisconnected(stopCalled);
    }

    public override Task OnReconnected()
    {
        if (!_connections.GetConnections(_username).Contains(Context.ConnectionId))
        {
            // We show all currently connected users
            var currentUsers = _connections.AllConnectedUsers();

            foreach (var user in currentUsers)
            {
                if (user != _username)
                {
                    Clients.Client(Context.ConnectionId).userConnected(user);
                }
            }

            _connections.Add(_username, Context.ConnectionId);

            // We inform all other users that current user has connected
            Clients.AllExcept(Context.ConnectionId).userConnected(_username);
        }

        return base.OnReconnected();
    }
}