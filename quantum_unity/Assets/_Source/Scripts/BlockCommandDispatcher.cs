using UnityEngine;
using Quantum;
using Input = UnityEngine.Input;

namespace SoccerGame
{
    public class BlockCommandDispatcher : MonoBehaviour
    {
        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                var actorNumber = ConnectionManager.Client.LocalPlayer.ActorNumber;
                var playerRef = QuantumRunner.Default.Game.Frames.Verified.ActorIdToFirstPlayer(actorNumber);

                var command = new BlockCommand()
                {
                    playerRef = playerRef.Value
                };

                QuantumRunner.Default.Game.SendCommand(command);
            }
        }
    }
}
