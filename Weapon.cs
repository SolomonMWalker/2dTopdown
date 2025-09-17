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
        
        //TODO: move area2d to root node and move weapon hit code to weapon, not player
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        if (!collisionPolygon2D.Disabled && area2D.HasOverlappingBodies())
        {
            var bodies = area2D.GetOverlappingBodies();
            foreach (var body in bodies)
            {
                if (body is Enemy enemy)
                {
                    enemy.Hit();    
                }
            }
        }
    }
}
