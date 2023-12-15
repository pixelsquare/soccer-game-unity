using Photon.Deterministic;

namespace Quantum
{
    partial class RuntimePlayer
    {
        public AssetRefEntityPrototype playerPrototype;

        partial void SerializeUserData(BitStream stream)
        {
            stream.Serialize(ref playerPrototype);
        }
    }
}
