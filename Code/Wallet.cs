using Sandbox;

public sealed class Wallet : Component
{
	[Sync( SyncFlags.FromHost )] [Property] public float Money { get; private set; }

	/// <summary>
	/// Add money to wallet. Host-authoritative: only the host mutates Money,
	/// callers can only act on a wallet they own.
	/// </summary>
	/// <param name="amount">Amount of money (must be > 0)</param>
	[Rpc.Host]
	public void RequestAddMoney( float amount )
	{
		if ( amount <= 0 ) return;
		if ( Rpc.Caller != Network.Owner ) return;
		Money += amount;
	}

	/// <summary>
	/// Remove money from wallet. Refuses silently if balance is insufficient.
	/// Host-authoritative.
	/// </summary>
	/// <param name="amount">Amount of money (must be > 0)</param>
	[Rpc.Host]
	public void RequestRemoveMoney( float amount )
	{
		if ( amount <= 0 ) return;
		if ( Rpc.Caller != Network.Owner ) return;
		if ( Money < amount ) return;
		Money -= amount;
	}

	/// <summary>
	/// Does the wallet have enough money to afford the given amount.
	/// Client-side check; the host re-validates inside RequestRemoveMoney.
	/// </summary>
	public bool CanAfford( float amount ) => Money >= amount;
}
