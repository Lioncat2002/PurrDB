using System.Text;
using System.Text.Json;
using GameServer.Schemas;
using GameServer.Services;
using QuicNet.Streams;

namespace GameServer.Controllers;

public class Player
{
    private byte[] _response=new byte[1024];
    public void GetPlayers(QuicStream stream,int playerId)
    {
        var players=Services.Player.GetPlayersInRange(playerId,10);
        var json=JsonSerializer.Serialize(players);
        _response = Encoding.UTF8.GetBytes(json);//Encoding.UTF8.GetBytes($"Received Player {newPlayer.Id} at ({newPlayer.X}, {newPlayer.Y})");
        stream.Send(_response);
        //stream.Send(Encoding.UTF8.GetBytes("<EOF>"));

        
    }

    public void Join(QuicStream stream,PlayerData player)
    {
        var newPlayer = new Models.Player{X = player.x, Y = player.y};
        var players = Services.Player.Insert(newPlayer);
        Console.WriteLine("Player "+players.Id+"Joined the room");
        var json = JsonSerializer.Serialize(players);
        _response = Encoding.UTF8.GetBytes(json);
        stream.Send(_response);

    }

    public void Leave(QuicStream stream,PlayerData player)
    {
        _response = Encoding.UTF8.GetBytes("ok");
        stream.Send(_response);
    }

    public void UpdatePosition(QuicStream stream,PlayerData player)
    {
        var newPlayer=new Models.Player{Id = player.id,X=player.x,Y=player.y};
        try
        {
            Services.Player.UpdatePlayer(newPlayer);
            _response = Encoding.UTF8.GetBytes("ok");
            Console.WriteLine("Player "+newPlayer.Id+"moved");
            stream.Send(_response);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            _response = Encoding.UTF8.GetBytes("failure");
            stream.Send(_response);

        }
        
    }
}