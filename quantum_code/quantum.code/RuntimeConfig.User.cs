using Photon.Deterministic;

namespace Quantum
{
    partial class RuntimeConfig
    {
        public AssetRefEntityPrototype ballPrototype;

        partial void SerializeUserData(BitStream stream)
        {
            stream.Serialize(ref ballPrototype);
        }
    }
}
