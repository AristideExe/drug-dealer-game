using Sandbox;

public sealed class Wallet : Component
{
	[Property] private float money;

	/// <summary>
	/// Amount of money in wallet
	/// </summary>
	public float Money => money;

	/// <summary>
	/// Add money to wallet
	/// </summary>
	/// <param name="amount">Amount of money</param>
	public void AddMoney( float amount )
	{
		money += amount;
	}

	/// <summary>
	/// Remove money from wallet
	/// </summary>
	/// <param name="amount">Amount of money</param>
	public void RemoveMoney( float amount )
	{
		money -= amount;
	}

	/// <summary>
	/// Does the wallet has enough money in it
	/// </summary>
	/// <param name="amount">Amount of money</param>
	/// <returns></returns>
	public bool HasEnoughMoney( float amount )
	{
		return money >= amount;
	}
}
