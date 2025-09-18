using Godot;
using System;

public partial class Player : CharacterBody2D
{
    [Export] public int speed = 300;
    private Weapon _weapon;

    public enum MovementState
    {
        Walking,
        Crouching,
        Dashing,
        Sprinting,
        Attacking
    } //TODO: implement dash going into sprinting

    public override void _Ready()
    {
        _weapon = GetNode<Weapon>("Weapon");
    }
    
    public override void _Process(double delta)
    {
        base._Process(delta);

        var movement = Vector2.Zero;
        
        if (Input.IsKeyPressed(Key.W))
        {
            movement.Y -= 1;
        }
        if (Input.IsKeyPressed(Key.S))
        {
            movement.Y += 1;
        }
        if (Input.IsKeyPressed(Key.D))
        {
            movement.X += 1;
        }
        if (Input.IsKeyPressed(Key.A))
        {
            movement.X -= 1;
        }

        Velocity = movement * speed;

        MoveAndSlide();
        
        LookAt(GetGlobalMousePosition());

        if (Input.IsMouseButtonPressed(MouseButton.Left))
        {
            _weapon.Swing();
        }
    }
}
