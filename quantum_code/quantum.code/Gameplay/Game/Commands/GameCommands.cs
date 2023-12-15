using Photon.Deterministic;

namespace Quantum
{
    public class BlockCommand : DeterministicCommand
    {
        public PlayerRef playerRef;

        public override void Serialize(BitStream stream)
        {
            stream.Serialize(ref playerRef);
        }

        public void Execute(Frame f)
        {
            f.Signals.OnBlockedUsed(playerRef);
        }
    }
}
