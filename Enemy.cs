using Godot;
using System;

public partial class Enemy : CharacterBody2D
{
    private Area2D _area2D;
    private CollisionShape2D _collision;
    private Sprite2D _sprite2D;

    public override void _Ready()
    {
        base._Ready();
        _area2D = GetNode<Area2D>("Area2D");
        _collision = GetNode<CollisionShape2D>("CollisionShape2D");
        _sprite2D = GetNode<Sprite2D>("Sprite2D");
    }

    public void Hit()
    {
        _sprite2D.Modulate = Colors.Red;
    }

    public void CheckForCollisions()
    {
        var overlapBodies = _area2D.GetOverlappingBodies();
        var overlapAreas = _area2D.GetOverlappingAreas();
    }
}
