using UnityEngine;

namespace _Project.Scripts.GameServices
{
    public interface IGameSettingsProvider
    {
        public GameSettings GameSettings { get; }
    }
    
    public class GameSettingsProvider: IGameSettingsProvider
    {
        public GameSettings GameSettings { get; }

        public GameSettingsProvider(GameSettings gameSettings)
        {
            GameSettings = gameSettings;
        }
    }
}