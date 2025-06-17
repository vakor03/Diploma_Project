using System;
using _Project.Features.PlayerModule;
using _Project.Features.StatsModule;
using UnityEngine;
using Zenject;

public class PlayerMovement : MonoBehaviour
{
    public PlayerRunData Data;
    private int _jumpsRemaining;

    [field:SerializeField] public Rigidbody2D RB { get; private set; }

    public bool IsFacingRight { get; private set; }
    public bool IsJumping { get; private set; }
    public bool IsWallJumping { get; private set; }
    public bool IsSliding { get; private set; }

    public float LastOnGroundTime { get; private set; }
    public float LastOnWallTime { get; private set; }
    public float LastOnWallRightTime { get; private set; }
    public float LastOnWallLeftTime { get; private set; }

    private bool _isJumpCut;
    private bool _isJumpFalling;

    private float _wallJumpStartTime;
    private int _lastWallJumpDir;

    private Vector2 _moveInput;
    public float LastPressedJumpTime { get; private set; }

    private IStatService<EntityStats> _statService;

    [Header("Checks")] 
    [SerializeField] private Transform _groundCheckPoint;
    [SerializeField] private Vector2 _groundCheckSize = new Vector2(0.49f, 0.03f);
    [Space(5)]
    [SerializeField] private Transform _frontWallCheckPoint;
    [SerializeField] private Transform _backWallCheckPoint;
    [SerializeField] private Vector2 _wallCheckSize = new Vector2(0.5f, 1f);

    [Header("Layers & Tags")]
    [SerializeField] private LayerMask _groundLayer;

    [SerializeField] private Transform _playerVisuals;

    [Inject]
    private void InjectDependencies(IStatService<EntityStats> statService)
    {
        _statService = statService;
    }

    private void Start()
    {
        InitializePlayer();
    }

    private void InitializePlayer()
    {
        SetGravityScale(Data.CalculateGravityScale(_statService.GetStat(EntityStats.JumpHeight)));
        IsFacingRight = true;
        ResetJumps();
    }
    
    private void ResetJumps()
    {
        _jumpsRemaining = (int)_statService.GetStat(EntityStats.MaxJumps);
    }

    private void Update()
    {
        UpdateTimers();
        HandleInput();
        CheckCollisions();
        HandleJumpLogic();
        HandleSlideLogic();
        UpdateGravity();
    }

    private void UpdateTimers()
    {
        LastOnGroundTime -= Time.deltaTime;
        LastOnWallTime -= Time.deltaTime;
        LastOnWallRightTime -= Time.deltaTime;
        LastOnWallLeftTime -= Time.deltaTime;
        LastPressedJumpTime -= Time.deltaTime;
        
        // Reset jumps when touching ground
        if (LastOnGroundTime > 0)
        {
            ResetJumps();
        }
    }

    private void HandleInput()
    {
        _moveInput.x = Input.GetAxisRaw("Horizontal");
        _moveInput.y = Input.GetAxisRaw("Vertical");

        if (_moveInput.x != 0)
            CheckDirectionToFace(_moveInput.x > 0);

        CheckJumpInputs();
    }

    private void CheckJumpInputs()
    {
        if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.C) || Input.GetKeyDown(KeyCode.J))
        {
            OnJumpInput();
        }

        if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.C) || Input.GetKeyUp(KeyCode.J))
        {
            OnJumpUpInput();
        }
    }

    private void CheckCollisions()
    {
        CheckGroundContact();
        CheckWallContact();
    }

    private void CheckGroundContact()
    {
        if (Physics2D.OverlapBox(_groundCheckPoint.position, _groundCheckSize, 0, _groundLayer) && !IsJumping)
        {
            LastOnGroundTime = Data.coyoteTime;
        }
    }

    private void CheckWallContact()
    {
        if (IsJumping || IsWallJumping) return;
        
        // Direct check of front and back wall contact
        bool frontWallContact = Physics2D.OverlapBox(_frontWallCheckPoint.position, _wallCheckSize, 0, _groundLayer);
        bool backWallContact = Physics2D.OverlapBox(_backWallCheckPoint.position, _wallCheckSize, 0, _groundLayer);
        
        // Determine right/left wall contact based on facing direction
        if (IsFacingRight)
        {
            // When facing right: front = right wall, back = left wall
            if (frontWallContact && !IsWallJumping) LastOnWallRightTime = Data.coyoteTime;
            if (backWallContact && !IsWallJumping) LastOnWallLeftTime = Data.coyoteTime;
        }
        else
        {
            // When facing left: front = left wall, back = right wall
            if (frontWallContact && !IsWallJumping) LastOnWallLeftTime = Data.coyoteTime;
            if (backWallContact && !IsWallJumping) LastOnWallRightTime = Data.coyoteTime;
        }
        
        // Set generic wall time to max of left/right wall times
        LastOnWallTime = Mathf.Max(LastOnWallLeftTime, LastOnWallRightTime);
    }
    
    private void HandleJumpLogic()
    {
        UpdateJumpingState();
        ProcessJumpRequests();
    }

    private void UpdateJumpingState()
    {
        // End jumping state when velocity becomes negative
        if (IsJumping && RB.linearVelocity.y < 0)
        {
            IsJumping = false;

            if(!IsWallJumping)
                _isJumpFalling = true;
        }

        // End wall jumping state after time expires
        if (IsWallJumping && Time.time - _wallJumpStartTime > Data.wallJumpTime)
        {
            IsWallJumping = false;
        }

        // Reset jump cut and fall state on ground
        if (LastOnGroundTime > 0 && !IsJumping && !IsWallJumping)
        {
            _isJumpCut = false;

            if(!IsJumping)
                _isJumpFalling = false;
        }
    }

    private void ProcessJumpRequests()
    {
        // Process regular jumps (including multi-jumps)
        if (CanJump() && LastPressedJumpTime > 0)
        {
            PerformJump();
        }
        // Process wall jumps
        else if (CanWallJump() && LastPressedJumpTime > 0)
        {
            PerformWallJump();
        }
    }

    private void PerformJump()
    {
        IsJumping = true;
        IsWallJumping = false;
        _isJumpCut = false;
        _isJumpFalling = false;
        _jumpsRemaining--;
        Jump();
    }

    private void PerformWallJump()
    {
        IsWallJumping = true;
        IsJumping = false;
        _isJumpCut = false;
        _isJumpFalling = false;
        _wallJumpStartTime = Time.time;
        
        // Determine direction to jump - away from wall
        int jumpDir = DetermineWallJumpDirection();
        
        WallJump(jumpDir);
        
        // Wall jumps also reset air jumps (for gameplay feel)
        ResetJumps();
    }
    
    private int DetermineWallJumpDirection()
    {
        // More reliable wall jump direction determination based on which wall was touched
        if (LastOnWallRightTime > LastOnWallLeftTime)
        {
            _lastWallJumpDir = -1; // Jump left (away from right wall)
        }
        else
        {
            _lastWallJumpDir = 1;  // Jump right (away from left wall)
        }
        
        return _lastWallJumpDir;
    }

    private void HandleSlideLogic()
    {
        // Only slide when against a wall, pressing toward it, and not jumping
        bool shouldSlide = CanSlide() && 
                         ((LastOnWallLeftTime > 0 && _moveInput.x < 0) || 
                          (LastOnWallRightTime > 0 && _moveInput.x > 0));
        
        IsSliding = shouldSlide;
    }

    private void UpdateGravity()
    {
        if (IsSliding)
        {
            SetGravityScale(0);
        }
        else if (IsFastFalling())
        {
            ApplyFastFallGravity();
        }
        else if (_isJumpCut)
        {
            ApplyJumpCutGravity();
        }
        else if (IsAtJumpApex())
        {
            ApplyJumpApexGravity();
        }
        else if (IsFalling())
        {
            ApplyFallingGravity();
        }
        else
        {
            SetGravityScale(Data.CalculateGravityScale(_statService.GetStat(EntityStats.JumpHeight)));
        }
    }

    private bool IsFastFalling()
    {
        return RB.linearVelocity.y < 0 && _moveInput.y < 0;
    }

    private void ApplyFastFallGravity()
    {
        SetGravityScale(Data.CalculateGravityScale(_statService.GetStat(EntityStats.JumpHeight)) * Data.fastFallGravityMult);
        RB.linearVelocity = new Vector2(RB.linearVelocity.x, Mathf.Max(RB.linearVelocity.y, -Data.maxFastFallSpeed));
    }

    private void ApplyJumpCutGravity()
    {
        SetGravityScale(Data.CalculateGravityScale(_statService.GetStat(EntityStats.JumpHeight)) * Data.jumpCutGravityMult);
        RB.linearVelocity = new Vector2(RB.linearVelocity.x, Mathf.Max(RB.linearVelocity.y, -Data.maxFallSpeed));
    }

    private bool IsAtJumpApex()
    {
        return (IsJumping || IsWallJumping || _isJumpFalling) && 
               Mathf.Abs(RB.linearVelocity.y) < Data.jumpHangTimeThreshold;
    }

    private void ApplyJumpApexGravity()
    {
        SetGravityScale(Data.CalculateGravityScale(_statService.GetStat(EntityStats.JumpHeight)) * Data.jumpHangGravityMult);
    }

    private bool IsFalling()
    {
        return RB.linearVelocity.y < 0;
    }

    private void ApplyFallingGravity()
    {
        SetGravityScale(Data.CalculateGravityScale(_statService.GetStat(EntityStats.JumpHeight)) * Data.fallGravityMult);
        RB.linearVelocity = new Vector2(RB.linearVelocity.x, Mathf.Max(RB.linearVelocity.y, -Data.maxFallSpeed));
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }
    
    private void HandleMovement()
    {
        if (IsWallJumping)
            Run(Data.wallJumpRunLerp);
        else
            Run(1);

        if (IsSliding)
            Slide();
    }

    public void OnJumpInput()
    {
        LastPressedJumpTime = Data.jumpInputBufferTime;
    }

    public void OnJumpUpInput()
    {
        if (CanJumpCut() || CanWallJumpCut())
            _isJumpCut = true;
    }

    public void SetGravityScale(float scale)
    {
        RB.gravityScale = scale;
    }

    private void Run(float lerpAmount)
    {
        float currentSpeed = _statService.GetStat(EntityStats.CurrentSpeed);
        float targetSpeed = _moveInput.x * currentSpeed;
        targetSpeed = Mathf.Lerp(RB.linearVelocity.x, targetSpeed, lerpAmount);

        float accelRate = CalculateAccelerationRate(targetSpeed);
        ApplyRunForce(targetSpeed, accelRate);
    }

    private float CalculateAccelerationRate(float targetSpeed)
    {
        float currentSpeed = _statService.GetStat(EntityStats.CurrentSpeed);
        float accelRate;
        bool isAccelerating = Mathf.Abs(targetSpeed) > 0.01f;

        // Ground acceleration rates
        if (LastOnGroundTime > 0)
        {
            accelRate = isAccelerating ? Data.CalculateRunAccelAmount(currentSpeed) : Data.CalculateRunDeccelAmount(currentSpeed);
        }
        // Air acceleration rates
        else
        {
            accelRate = isAccelerating ? 
                Data.CalculateRunAccelAmount(currentSpeed) * Data.accelInAir : 
                Data.CalculateRunDeccelAmount(currentSpeed) * Data.deccelInAir;
        }

        // Apply jump apex acceleration bonus
        if (IsAtJumpApex())
        {
            accelRate *= Data.jumpHangAccelerationMult;
            targetSpeed *= Data.jumpHangMaxSpeedMult;
        }

        // Conserve momentum when moving faster than max speed in air
        if (ShouldConserveMomentum(targetSpeed))
        {
            accelRate = 0;
        }

        return accelRate;
    }

    private bool ShouldConserveMomentum(float targetSpeed)
    {
        return Data.doConserveMomentum && 
               Mathf.Abs(RB.linearVelocity.x) > Mathf.Abs(targetSpeed) && 
               Mathf.Sign(RB.linearVelocity.x) == Mathf.Sign(targetSpeed) && 
               Mathf.Abs(targetSpeed) > 0.01f && 
               LastOnGroundTime < 0;
    }

    private void ApplyRunForce(float targetSpeed, float accelRate)
    {
        float speedDif = targetSpeed - RB.linearVelocity.x;
        float movement = speedDif * accelRate;
        RB.AddForce(movement * Vector2.right, ForceMode2D.Force);
    }

    private void Turn()
    {
        Vector3 scale = _playerVisuals.localScale; 
        scale.x *= -1;
        _playerVisuals.localScale = scale;

        IsFacingRight = !IsFacingRight;
    }

    private void Jump()
    {
        // Reset jump timer but not ground time for multi-jumps
        LastPressedJumpTime = 0;
        
        // Only reset ground time on first jump
        if (_jumpsRemaining == (int)_statService.GetStat(EntityStats.MaxJumps) - 1)
        {
            LastOnGroundTime = 0;
        }

        // Calculate jump force with velocity compensation
        float force = Data.CalculateJumpForce(_statService.GetStat(EntityStats.JumpHeight));
        // Compensate for downward velocity to ensure consistent jump height
        if (RB.linearVelocity.y < 0)
            force -= RB.linearVelocity.y;

        // Apply jump force
        RB.AddForce(Vector2.up * force, ForceMode2D.Impulse);
    }

    private void WallJump(int dir)
    {
        // Reset jump and wall timers
        LastPressedJumpTime = 0;
        LastOnGroundTime = 0;
        LastOnWallRightTime = 0;
        LastOnWallLeftTime = 0;

        // Create directional force vector
        Vector2 force = new Vector2(Data.wallJumpForce.x, Data.wallJumpForce.y);
        force.x *= dir; // Apply horizontal direction

        // Neutralize opposing velocity
        if (Mathf.Sign(RB.linearVelocity.x) != Mathf.Sign(force.x))
            force.x -= RB.linearVelocity.x;

        // Compensate for falling velocity
        if (RB.linearVelocity.y < 0)
            force.y -= RB.linearVelocity.y;

        // Apply wall jump force
        RB.AddForce(force, ForceMode2D.Impulse);
    }

    private void Slide()
    {
        float speedDif = Data.slideSpeed - RB.linearVelocity.y;    
        float movement = speedDif * Data.slideAccel;
        
        // Clamp movement to prevent over-acceleration
        float maxForce = Mathf.Abs(speedDif) * (1 / Time.fixedDeltaTime);
        movement = Mathf.Clamp(movement, -maxForce, maxForce);

        RB.AddForce(movement * Vector2.up);
    }

    public void CheckDirectionToFace(bool isMovingRight)
    {
        if (isMovingRight != IsFacingRight)
            Turn();
    }

    private bool CanJump()
    {
        // Either on ground (first jump) OR have remaining air jumps
        return (LastOnGroundTime > 0 && !IsJumping) || (_jumpsRemaining > 0 && !IsWallJumping);
    }

    private bool CanWallJump()
    {
        bool hasJumpInput = LastPressedJumpTime > 0;
        bool isOnWall = LastOnWallTime > 0;
        bool isNotOnGround = LastOnGroundTime <= 0;
        
        // Either first wall jump or attempting to wall jump off opposite wall
        bool validWallJumpDirection = !IsWallJumping || 
                                     (LastOnWallRightTime > 0 && _lastWallJumpDir == 1) || 
                                     (LastOnWallLeftTime > 0 && _lastWallJumpDir == -1);
        
        return hasJumpInput && isOnWall && isNotOnGround && validWallJumpDirection;
    }

    private bool CanJumpCut()
    {
        return IsJumping && RB.linearVelocity.y > 0;
    }

    private bool CanWallJumpCut()
    {
        return IsWallJumping && RB.linearVelocity.y > 0;
    }

    public bool CanSlide()
    {
        return LastOnWallTime > 0 && !IsJumping && !IsWallJumping && LastOnGroundTime <= 0;
    }

    private void OnDrawGizmosSelected()
    {
        // Draw ground check area
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(_groundCheckPoint.position, _groundCheckSize);
        
        // Draw wall check areas
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(_frontWallCheckPoint.position, _wallCheckSize);
        Gizmos.DrawWireCube(_backWallCheckPoint.position, _wallCheckSize);
    }
}