using Sandbox.UI;

namespace Sandbox.Player;

public sealed class PlayerManager : Component
{
	[Property] private PlayerInfosUI PlayerInfosUi;
	[RequireComponent] private Wallet Wallet { get; set;  }
	
	public float Money => Wallet.Money;

	protected override void OnAwake()
	{
		base.OnAwake();
		PlayerInfosUi.Player = this;
	}
}
