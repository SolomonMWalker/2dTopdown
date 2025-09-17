using Godot;
using System;

public partial class Player : CharacterBody2D
{
    [Export] public int speed = 300;
    private AnimationPlayer _animationPlayer;
    private Weapon _weapon;

    public override void _Ready()
    {
        _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
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

        if (!_animationPlayer.IsPlaying() && Input.IsMouseButtonPressed(MouseButton.Left))
        {
            _animationPlayer.Play("Weapon swing");
        }
    }
}
