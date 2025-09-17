using Godot;
using System;

public partial class Enemy : CharacterBody2D
{
    private CollisionShape2D _collision;
    private Sprite2D _sprite2D;

    public override void _Ready()
    {
        base._Ready();
        _collision = GetNode<CollisionShape2D>("CollisionShape2D");
        _sprite2D = GetNode<Sprite2D>("Sprite2D");
    }

    public void Hit()
    {
        _sprite2D.Modulate = Colors.Red;
    }
}
