using Sandbox.Player;

public sealed class MoneyGiver : Component, Component.ITriggerListener
{
	[Property] private float Amount { get; set; }
	[Property] public float Cooldown { get; set; } = 0.1f;
	
	private TimeSince _timeSinceLastGive = 999f;
	
	public void OnTriggerEnter( Collider other )
	{
		if ( _timeSinceLastGive < Cooldown ) return;
		
		var wallet = other.Components.GetInParent<Wallet>();
		if ( wallet is null ) return;
		if (!wallet.Network.IsOwner) return;
		
		wallet.AddMoney(Amount);
		_timeSinceLastGive = 0;
	}
}
