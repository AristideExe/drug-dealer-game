using Sandbox.UI;

namespace Sandbox.Player;

/// <summary>
/// Top-level player component. Owns the player's sub-components (wallet,
/// vitals) and exposes their values for UI consumption.
/// </summary>
public sealed class PlayerManager : Component
{
	[Property] private PlayerInfosUI PlayerInfosUi;
	[RequireComponent] private Wallet Wallet { get; set;  }
	[RequireComponent] private Vitals Vitals { get; set; }

	public float Money => Wallet.Money;

	public float Hp => Vitals.Hp;
	public float MaxHp => Vitals.MaxHp;
	public float Stamina => Vitals.Stamina;
	public float MaxStamina => Vitals.MaxStamina;
	public float HpFraction => Vitals.HpFraction;
	public float StaminaFraction => Vitals.StaminaFraction;

	protected override void OnAwake()
	{
		base.OnAwake();
		PlayerInfosUi.Player = this;
	}
}
