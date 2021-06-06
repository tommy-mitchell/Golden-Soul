using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Settings", menuName = "Game/Settings/Player")]
public class PlayerSettings : ScriptableObject
{
    [SerializeField, Tooltip("The maximum health of the player. The number of hearts is half as much, due to half-hearts.")]
    private int _maximumHealthValue = 8;
    public int MaximumHealthValue => _maximumHealthValue;

    [SerializeField, Tooltip("Movement speed of the player.")]
    private float _movementSpeed = 6.5f;
    public float MovementSpeed => _movementSpeed;

    [SerializeField, Tooltip("Force to be applied when the player is jumping.")]
    private float _jumpForce = 9f;
    public float JumpForce => _jumpForce;

    [SerializeField, Tooltip("Force to be applied when the player is falling.")]
    private float _fallMultiplier = 3f;
    public float FallMultiplier => _fallMultiplier;

    [SerializeField, Tooltip("Minimum force applied when the player performs a short jump.")]
    private float _lowJumpMultiplier = 4f;
    public float LowJumpMultiplier => _lowJumpMultiplier;

    [SerializeField, Tooltip("The minimum required downward velocity for fall damage.")]
    private float _damageFallSpeed = 25f;
    public float DamageFallSpeed => _damageFallSpeed;

    [SerializeField, Tooltip("The minimum required downward velocity to kill the player with fall damage.")]
    private float _fatalFallSpeed = 32f;
    public float FatalFallSpeed => _fatalFallSpeed;

    [SerializeField, Tooltip("The grace period (in seconds) during which a player can jump while technically in the air after leaving the ground, for more user-friendly input. See: Wile E. Coyote")]
    private float _coyoteTimeGraceTimer = .2f;
    public float CoyoteTimeGraceTimer => _coyoteTimeGraceTimer;

    [SerializeField, Tooltip("The time (in seconds) before another sword throw is allowed, after either teleporting or throwing the sword at an enemy.")]
    private float _throwCooldownTime = 1f;
    public float ThrowCooldownTime => _throwCooldownTime;

    [SerializeField, Tooltip("todo")] // TODO
    private float _attackLengthTime = 2f;
    public float AttackLengthTime => _attackLengthTime;

    [SerializeField, Tooltip("todo")] // TODO
    private float _attackCooldown = .3f;
    public float AttackCooldown => _attackCooldown;

    [SerializeField, Tooltip("The time (in seconds) that the player flashes for when damaged.")]
    private float _damageFlashTime = .25f;
    public float DamageFlashTime => _damageFlashTime;

    [SerializeField, Tooltip("The gravity scale of the attached RigidBody2D on the player object.")]
    private float _rbGravityScale = 2f;
    public float RB_GravityScale => _rbGravityScale;

    /*[SerializeField, Tooltip("The minimum y-value necessary for the raw aim input to snap to straight up/down.")] // maybe move to input settings?
    private float _aimSnapValue_Y = .95f;
    public float AimSnapValue_Y => _aimSnapValue_Y;

    [SerializeField, Tooltip("The minimum x-value necessary for the raw aim input to snap to straight left/right.")]
    private float _aimSnapValue_X = .99f;
    public float AimSnapValue_X => _aimSnapValue_X;*/

    [SerializeField, Tooltip("The value the sword's gravity scale is incremented by each update.")]
    private float _swordGravityScaleIncrement = .08f;
    public float SwordGravityScaleIncrement => _swordGravityScaleIncrement;
}