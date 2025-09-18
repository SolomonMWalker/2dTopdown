using Godot;
using System;
using System.Linq;

public partial class Enemy : CharacterBody2D
{
    //nav help from https://docs.godotengine.org/en/stable/tutorials/navigation/navigation_introduction_2d.html
    [Export] public Node2D point1;
    [Export] public Node2D point2;
    [Export] public int health = 3;
    public int movementSpeed = 100;
    public Vector2 MovementTarget
    {
        get { return _navigationAgent.TargetPosition; }
        set { _navigationAgent.TargetPosition = value; }
    }
    
    private Area2D _area2D;
    private CollisionShape2D _collision;
    private Sprite2D _sprite2D;
    private NavigationAgent2D _navigationAgent;
    private AnimationPlayer _animationPlayer;

    public override void _Ready()
    {
        base._Ready();
        _area2D = GetNode<Area2D>("Area2D");
        _collision = GetNode<CollisionShape2D>("CollisionShape2D");
        _sprite2D = GetNode<Sprite2D>("Sprite2D");
        _navigationAgent = GetNode<NavigationAgent2D>("NavigationAgent2D");
        _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        
        // These values need to be adjusted for the actor's speed
        // and the navigation layout.
        _navigationAgent.PathDesiredDistance = 4.0f;
        _navigationAgent.TargetDesiredDistance = 4.0f;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        CheckForCollisions();
    }
    
    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

        if (_navigationAgent.IsNavigationFinished())
        {
            MovementTarget = MovementTarget == point1.GetGlobalPosition() 
                ? point2.GetGlobalPosition() : point1.GetGlobalPosition();
        }

        Vector2 currentAgentPosition = GlobalTransform.Origin;
        Vector2 nextPathPosition = _navigationAgent.GetNextPathPosition();

        Velocity = currentAgentPosition.DirectionTo(nextPathPosition) * movementSpeed;
        MoveAndSlide();
    }
    
    private async void ActorSetup()
    {
        // Wait for the first physics frame so the NavigationServer can sync.
        await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);

        // Now that the navigation map is no longer empty, set the movement target.
        MovementTarget = point1.GlobalPosition;
    }

    public void Hit()
    {
        if (!_animationPlayer.IsPlaying())
        {
            _animationPlayer.Play("Hit");
            health -= 1;
        }

        if (health <= 0)
        {
            QueueFree();
        }
    }

    public void CheckForCollisions()
    {
        var overlapAreas = _area2D.GetOverlappingAreas();
        if (overlapAreas.Any(a => a is Weapon))
        {
            Hit();
        }
    }
}
