using Sandbox;

public sealed class Wallet : Component
{
	[Sync] [Property] public float Money { get; private set; }

	/// <summary>
	/// Add money to wallet
	/// </summary>
	/// <param name="amount">Amount of money</param>
	public void AddMoney( float amount )
	{
		Money += amount;
	}

	/// <summary>
	/// Remove money from wallet
	/// </summary>
	/// <param name="amount">Amount of money</param>
	public void RemoveMoney( float amount )
	{
		Money -= amount;
		if (Money < 0 ) Money = 0;
	}

	/// <summary>
	/// Does the wallet has enough money in it
	/// </summary>
	/// <param name="amount">Amount of money</param>
	/// <returns></returns>
	public bool HasEnoughMoney( float amount )
	{
		return Money >= amount;
	}
}
