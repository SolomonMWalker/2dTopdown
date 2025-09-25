using Godot;
using System;
using System.Linq;

public partial class Enemy : CharacterBody2D
{
    //nav help from https://docs.godotengine.org/en/stable/tutorials/navigation/navigation_introduction_2d.html
    [Export] public Node2D Point1;
    [Export] public Node2D Point2;
    [Export] public int Health = 3;
    [Export] public int RotateRadius = 96/2;
    [Export] public int AnglePerNavPoint = 3;
    public int MovementSpeed = 100;
    public Vector2 MovementTarget
    {
        get { return _navigationAgent.TargetPosition; }
        set => _navigationAgent.TargetPosition = value;
    }

    public enum NavMode
    {
        Chase,
        RotateAround
    }
    
    private Area2D _area2D;
    private CollisionShape2D _collision;
    private Sprite2D _sprite2D;
    private NavigationAgent2D _navigationAgent;
    private AnimationPlayer _animationPlayer;
    private NavMode _navMode;
    private float _degreesSoFar = 0;
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
        _navigationAgent.TargetDesiredDistance = 1.0f;
        _navMode = NavMode.Chase;
        
        Callable.From(ActorSetup).CallDeferred();
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        CheckForCollisions();
    }
    
    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

        var switched = false;
        
        var distance = GlobalTransform.Origin.DistanceTo(Point1.GlobalPosition);
        
        if (_navMode == NavMode.Chase)
        {
            if (distance < RotateRadius)
            {
                _navMode = NavMode.RotateAround;
                switched = true;
                //Get current degrees of incoming enemy
                _degreesSoFar = Mathf.RadToDeg(Mathf.Atan2(
                    GlobalTransform.Origin.Y - Point1.GlobalPosition.Y,
                    GlobalTransform.Origin.X - Point1.GlobalPosition.X));
            }
        }

        if (_navMode == NavMode.RotateAround)
        {
            if (distance > RotateRadius + 10)
            {
                _navMode = NavMode.Chase;
            }
        }
        
        if (switched || _navigationAgent.IsNavigationFinished())
        {
            if (_navMode == NavMode.Chase)
            {
                MovementTarget = Point1.GlobalPosition;
            }
            else if (_navMode == NavMode.RotateAround)
            {
                var angle = (AnglePerNavPoint + _degreesSoFar) % 360;
                _degreesSoFar = angle;
                MovementTarget = GetNextPathInCircle(angle, Point1.GlobalPosition, RotateRadius);
            }
        }

        Vector2 currentAgentPosition = GlobalTransform.Origin;
        Vector2 nextPathPosition = _navigationAgent.GetNextPathPosition();

        Velocity = currentAgentPosition.DirectionTo(nextPathPosition) * MovementSpeed;
        MoveAndSlide();
    }
    
    private async void ActorSetup()
    {
        // Wait for the first physics frame so the NavigationServer can sync.
        await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);

        // Now that the navigation map is no longer empty, set the movement target.
        MovementTarget = Point1.GlobalPosition;
    }

    private Vector2 GetNextPathInCircle(float angleInDeg, Vector2 center, float radius)
    {
        var angleInRads = Mathf.DegToRad(angleInDeg);
        var x = radius * Mathf.Cos(angleInRads);
        var y = radius * Mathf.Sin(angleInRads);
        return center + new Vector2(x, y);
    }

    public void Hit()
    {
        if (!_animationPlayer.IsPlaying())
        {
            _animationPlayer.Play("Hit");
            Health -= 1;
        }

        if (Health <= 0)
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
