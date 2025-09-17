using Godot;
using System;

public partial class Weapon : Node2D
{
    public Area2D area2D;
    public CollisionPolygon2D collisionPolygon2D;
    public Sprite2D sprite2D;

    public override void _Ready()
    {
        base._Ready();
        area2D = GetNode<Area2D>("Area2D");
        sprite2D = GetNode<Sprite2D>("Sprite2D");
        collisionPolygon2D = area2D.GetNode<CollisionPolygon2D>("CollisionPolygon2D");
        collisionPolygon2D.Disabled = true;
        Visible = false;
    }
}
