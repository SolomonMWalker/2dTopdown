using Godot;
using System;

public partial class Weapon : Area2D
{
    private CollisionPolygon2D _collisionPolygon2D;
    private Sprite2D _sprite2D;
    private AnimationPlayer _animationPlayer;

    public override void _Ready()
    {
        base._Ready();
        _sprite2D = GetNode<Sprite2D>("Sprite2D");
        _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        _collisionPolygon2D = GetNode<CollisionPolygon2D>("CollisionPolygon2D");
        _collisionPolygon2D.Disabled = true;
        Visible = false;
    }

    public void Swing()
    {
        if (!_animationPlayer.IsPlaying())
        {
            _animationPlayer.Play("WeaponSwing");
        }

    }
}
