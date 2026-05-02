using Sandbox.UI;

namespace Sandbox.Player;

public sealed class PlayerManager : Component
{
	[RequireComponent] private Wallet Wallet { get; set;  }
	
	public float Money => Wallet.Money;
}
