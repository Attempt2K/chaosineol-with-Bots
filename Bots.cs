using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using InfServer.Game;

namespace InfServer.Script.Bots
{
    public class BotHandler
    {
        private Arena _arena;
        private List<BotHandler> _bots = new List<BotHandler>();
        private Random _rand = new Random();

        public BotHandler(Arena arena)
        {
            _arena = arena;
        }

        public void SpawnBot(ushort vehicleID, Team team, int x, int y)
        {
            Bot bot = new Bot
            {
                VehicleID = vehicleID,
                Team = team,
                X = x,
                Y = y,
                TickCounter = 0
            };

            Console.WriteLine($"Spawning bot vehicleID:{vehicleID} for team index:{team._id} at ({x},{y})");

            _bots.Add(bot);
        }

        public void Poll()
        {
            foreach (var bot in _bots)
            {
                bot.TickCounter++;

                if (bot.TickCounter >= 20)
                {
                    bot.X += _rand.Next(-5, 6); // Move between -5 to +5 units
                    bot.Y += _rand.Next(-5, 6);

                    Console.WriteLine($"Bot {bot.VehicleID} moved to ({bot.X}, {bot.Y})");

                    bot.TickCounter = 0;
                }
            }
        }

        public void InitFromConfig()
        {
            string configPath = Path.Combine("Arcade - Bug Hunt XD", "assets", "chaosineol.cfg");

            if (File.Exists(configPath))
            {
                var lines = File.ReadAllLines(configPath);
                foreach (var line in lines)
                {
                    if (line.StartsWith("BotSpawn", StringComparison.OrdinalIgnoreCase))
                    {
                        try
                        {
                            var parts = line.Split('=')[1].Split(',');
                            ushort vehicleID = ushort.Parse(parts[0]);
                            int teamIndex = int.Parse(parts[1]);
                            int x = int.Parse(parts[2]);
                            int y = int.Parse(parts[3]);

                            var teams = _arena.Teams.ToArray();
                            if (teamIndex >= 0 && teamIndex < teams.Length)
                            {
                                SpawnBot(vehicleID, teams[teamIndex], x, y);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Warning: BotSpawn config line failed to parse: {line} ({ex.Message})");
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine($"Warning: Config file not found at {configPath}");
            }
        }

        private class Bot
        {
            public ushort VehicleID;
            public Team Team;
            public int X;
            public int Y;
            public int TickCounter;
        }
    }
}
