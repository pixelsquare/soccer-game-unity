// <auto-generated>
// This code was auto-generated by a tool, every time
// the tool executes this code will be reset.
//
// If you need to extend the classes generated to add
// fields or methods to them, please create partial  
// declarations in another file.
// </auto-generated>

using Quantum;
using UnityEngine;

[CreateAssetMenu(menuName = "Quantum/MovementConfig", order = Quantum.EditorDefines.AssetMenuPriorityStart + 312)]
public partial class MovementConfigAsset : AssetBase {
  public Quantum.MovementConfig Settings;

  public override Quantum.AssetObject AssetObject => Settings;
  
  public override void Reset() {
    if (Settings == null) {
      Settings = new Quantum.MovementConfig();
    }
    base.Reset();
  }
}

public static partial class MovementConfigAssetExts {
  public static MovementConfigAsset GetUnityAsset(this MovementConfig data) {
    return data == null ? null : UnityDB.FindAsset<MovementConfigAsset>(data);
  }
}
