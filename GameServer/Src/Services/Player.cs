using Dapper;
using GameServer.Config;

namespace GameServer.Services;

public class Player
{
    public static Models.Player Insert(Models.Player player)
    {
       player.Id= DB.Instance.Ctx.Execute("INSERT INTO Player (X,Y) VALUES (@X,@Y);",player);
       return player;
    }
    
    public static Models.Player[] GetPlayersInRange(int playerId,float range)
    {
        try
        {
            // Step 1: Get the center player's position
            var centerPlayer = DB.Instance.Ctx.QueryFirstOrDefault<Models.Player>(
                "SELECT * FROM Player WHERE Id = @Id", new { Id = playerId });

            if (centerPlayer == null)
                return Array.Empty<Models.Player>();

            float centerX = centerPlayer.X;
            float centerY = centerPlayer.Y;
            float radiusSquared = range * range;

            // Step 2: Query players within the range (excluding the player himself)
            return DB.Instance.Ctx.Query<Models.Player>(
                @"SELECT * FROM Player 
              WHERE Id != @Id AND ((X - @X) * (X - @X) + (Y - @Y) * (Y - @Y)) <= @RadiusSquared",
                new
                {
                    Id = playerId,
                    X = centerX,
                    Y = centerY,
                    RadiusSquared = radiusSquared
                }).ToArray();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
           return null;
        }
    }

    public static void UpdatePlayer(Models.Player player)
    {
        try
        {
            DB.Instance.Ctx.Query<Models.Player>("UPDATE Player SET X=@X, Y=@Y WHERE Id=@Id",player); 
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
            
    }
}