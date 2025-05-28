using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using InfServer.Game;
using InfServer.Script;
using InfServer.Script.Bots;

namespace InfServer.Script.GameType_chaosineol;
{
    public class chaosineol : Script
    {
        private Bots _botHandler;

        public void Init(Arena arena)
        {
            _botHandler = new Bots(arena);

            string configPath = Path.Combine("Arcade - Bug Hunt XD", "assets", "chaosineol.cfg");

            if (File.Exists(configPath))
            {
                var lines = File.ReadAllLines(configPath);
                foreach (var line in lines)
                {
                    if (line.StartsWith("BotSpawn"))
                    {
                        try
                        {
                            var parts = line.Split('=')[1].Split(',');
                            ushort vehicleID = ushort.Parse(parts[0]);
                            int teamIndex = int.Parse(parts[1]);
                            int x = int.Parse(parts[2]);
                            int y = int.Parse(parts[3]);

                            var teams = arena.Teams.ToArray();
                            if (teamIndex >= 0 && teamIndex < teams.Length)
                            {
                                _botHandler.SpawnBot(vehicleID, teams[teamIndex], x, y);
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

        public void Poll(Arena arena)
        {
            _botHandler.Poll();
        }
    }
}
