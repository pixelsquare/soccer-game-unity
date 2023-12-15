using Quantum;
using UnityEngine;
using Photon.Deterministic;

namespace SoccerGame
{
    public class InputPoller : MonoBehaviour
    {
        private const string AxisHorizontal = "Horizontal";
        private const string AxisVertical = "Vertical";

        public void OnEnable()
        {
            QuantumCallback.Subscribe<CallbackPollInput>(this, callback => PollInput(callback));
        }

        public void OnDisable()
        {
            QuantumCallback.UnsubscribeListener<CallbackPollInput>(this);
        }

        private void PollInput(CallbackPollInput callback)
        {
            var input = new Quantum.Input
            {
                MoveForward = UnityEngine.Input.GetAxisRaw(AxisVertical) > 0,
                MoveBack = UnityEngine.Input.GetAxisRaw(AxisVertical) < 0,
                MoveLeft = UnityEngine.Input.GetAxisRaw(AxisHorizontal) < 0,
                MoveRight = UnityEngine.Input.GetAxisRaw(AxisHorizontal) > 0
            };

            callback.SetInput(input, DeterministicInputFlags.Repeatable);
        }
    }
}
