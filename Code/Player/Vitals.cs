using Sandbox;

namespace Sandbox.Player;

/// <summary>
/// Holds a player's vital stats: HP and Stamina.
/// </summary>
public sealed class Vitals : Component
{
	[Property] public float MaxHp { get; set; } = 100f;
	[Property] public float MaxStamina { get; set; } = 100f;

	[Sync( SyncFlags.FromHost )] public float Hp { get; private set; }
	[Sync( SyncFlags.FromHost )] public float Stamina { get; private set; }

	public float HpFraction => MaxHp > 0 ? Hp / MaxHp : 0f;
	public float StaminaFraction => MaxStamina > 0 ? Stamina / MaxStamina : 0f;

	protected override void OnAwake()
	{
		base.OnAwake();
		if ( !Networking.IsHost ) return;
		Hp = MaxHp;
		Stamina = MaxStamina;
	}
}
