using System;

namespace Sandbox.Player;

/// <summary>
/// Holds a player's vital stats: HP and Stamina.
/// </summary>
public sealed class Vitals : Component, IScenePhysicsEvents
{
	[Property] public float MaxHp { get; set; } = 100f;
	[Property] public float MaxStamina { get; set; } = 100f;
	[Property] private float StaminaDrainPerSec { get; set; } = 25f;
	[Property] private float StaminaRegenPerSec { get; set; } = 15f;
	[Property] private float MinStaminaToStartSprint { get; set; } = 10f;

	[Sync( SyncFlags.FromHost )] public float Hp { get; private set; }
	[Sync( SyncFlags.FromHost )] public float Stamina { get; private set; }
	[Sync( SyncFlags.FromHost )] public bool IsSprinting { get; private set; }

	private bool _hostSprintIntent;

	private PlayerController _controller;

	public float HpFraction => MaxHp > 0 ? Hp / MaxHp : 0f;
	public float StaminaFraction => MaxStamina > 0 ? Stamina / MaxStamina : 0f;

	protected override void OnAwake()
	{
		base.OnAwake();

		_controller = GetComponent<PlayerController>();

		if ( !Networking.IsHost ) return;
		Hp = MaxHp;
		Stamina = MaxStamina;
	}

	/// <summary>
	/// Polls the sprint key on the owning client and forwards every change
	/// of intent to the host.
	/// </summary>
	protected override void OnUpdate()
	{
		if ( !Network.IsOwner ) return;

		if ( Input.Pressed( "Run" ) ) RequestSetSprintIntent( true );
		else if ( Input.Released( "Run" ) ) RequestSetSprintIntent( false );
	}

	/// <summary>
	/// Runs after the PlayerController computes its WishVelocity but before
	/// the physics step applies it. Clamps the wish to walk speed when the
	/// host has refused sprinting, leaving WalkSpeed and RunSpeed untouched
	/// so buffs and items can modify them freely.
	/// </summary>
	void IScenePhysicsEvents.PrePhysicsStep()
	{
		if ( !Network.IsOwner ) return;
		if ( _controller is null ) return;
		if ( IsSprinting ) return;

		var cap = _controller.WalkSpeed;
		if ( _controller.WishVelocity.Length > cap )
		{
			_controller.WishVelocity = _controller.WishVelocity.ClampLength( cap );
		}
	}

	/// <summary>
	/// Drains stamina while sprinting; regenerates only when the owner has
	/// released the sprint key. Holding sprint at zero stamina locks
	/// regeneration until release. Runs only on the host.
	/// </summary>
	protected override void OnFixedUpdate()
	{
		if ( !Networking.IsHost ) return;

		if ( IsSprinting )
		{
			Stamina = MathF.Max( 0, Stamina - StaminaDrainPerSec * Time.Delta );
			if ( Stamina <= 0 ) IsSprinting = false;
		}
		else if ( !_hostSprintIntent )
		{
			Stamina = MathF.Min( MaxStamina, Stamina + StaminaRegenPerSec * Time.Delta );
		}
	}

	/// <summary>
	/// Owner-side request to start or stop sprinting. The host stores the
	/// intent (used to lock out regeneration) and starts sprint only if
	/// <see cref="Stamina"/> is at least <see cref="MinStaminaToStartSprint"/>.
	/// </summary>
	[Rpc.Host]
	public void RequestSetSprintIntent( bool wants )
	{
		if ( Rpc.Caller != Network.Owner ) return;

		_hostSprintIntent = wants;

		if ( !wants )
		{
			IsSprinting = false;
			return;
		}

		if ( Stamina >= MinStaminaToStartSprint )
		{
			IsSprinting = true;
		}
	}
}
