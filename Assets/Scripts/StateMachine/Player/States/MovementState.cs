using GenshinImpactMovement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class MovementState : IState
{

    protected PlayerStateMachine StateMachine { get; }
    protected GroundedData groundedData;
    protected AirborneData airborneData;
    
    // 我发现property一般用于保护只能在某个类中修改的对象
    // 物理方向输入，以及以其为基础转化的3D数据
    protected Vector3 inputDirection;
    // 目标Euler角度 角度SmoothDamp时间 角度SmoothDamp计时器 角度SmoothDamp速度
    // TODO:后面写数据的时候删除


    public MovementState(PlayerStateMachine stateMachine)
    {
        StateMachine = stateMachine;
        InitiateData();
        ResetDefaultRecenteringData();
    }


    #region Istate Methods
    public virtual void Enter()
    {
        //Debug.Log("Enter" + GetType().Name);
        UpdateHorizontalRecentering(StateMachine.ReusableData.input);
        AddInputReaction();
    }

    public virtual void Exit()
    {
        RemoveInputReaction();
    }

    public virtual void GetInput()
    {
        GetPhysicalInput();
    }


    public virtual void PhysicalUpdate()
    {
        Move();
    }

    public virtual void Update()
    {

    }
    public virtual void OnAnimationEnterEvent()
    {
        
    }

    public virtual void OnAnimationExitEvent()
    {
        
    }

    public virtual void OnAnimationTransactionEvent()
    {
        
    }

    public virtual void OnTriggerEnter()
    {
        
    }

    public virtual void OnTriggerEnter(Collider collider)
    {
        
    }

    public virtual void OnTriggerExit(Collider collider)
    {
        // 是否是离开地面
        if(groundedData.isTouchGround(collider.gameObject.layer))
        {
            // 检测是否真的离开地面进入下落状态的一些细节
            ExamineLeavingGround();
            return;
        }
    }

    #endregion

    #region Main Methods
    private void InitiateData()
    {
        groundedData = StateMachine.Controller.playerData_SO.PlayerGroundedData;
        airborneData = StateMachine.Controller.playerData_SO.AirborneData;
        SetBaseRotaionData();
    }

    private void GetPhysicalInput()
    {
        StateMachine.ReusableData.input = StateMachine.Controller.Input.PlayerActions.Movement.ReadValue<Vector2>();
    }

    // 移动时才调用旋转
    protected void Move()
    {
        // 因为即使输入角度为0，也会添加相机基础角度，也会移动，输入我们直接在输入为0的情况下跳出移动函数
        if (StateMachine.ReusableData.input == Vector2.zero || StateMachine.ReusableData.speedMultiplier == 0f)
        {
            return;
        }

        float targetEulerAngle = Rotate();

        // 将目标角度值 转换为 目标Vector3， 原理是先转换成Quaternion，之后再乘以Vector3.forward
        Vector3 playerMoveDirection = GetMovementDirection(targetEulerAngle);
        Vector3 inputVelocity = GetMoveVelocity(playerMoveDirection);
        Vector3 originVelocity = GetOriginVelocity();

        // 因为速度是叠加 所以得减去本来的速度 当然我觉得这里也可以直接将速度归零 在加上新的速度
        StateMachine.Controller.Rigidbody.AddForce(inputVelocity - originVelocity, ForceMode.VelocityChange);
    }



    protected float Rotate()
    {
        // 将输入的Vector2转换为Y轴旋转的euler角度值
        float targetYEulerAngle = GetTartYEulerAngleBasedOnCamera();

        // 是否有了新的旋转目标
        if (StateMachine.ReusableData.RecordEulerRoration.y != targetYEulerAngle)
        {
            // 清空之前记录的目标旋转角度 和 已经旋转时间
            ResetEulerSmoothDampData(targetYEulerAngle);
        }

        // 平滑移动 并且 转动的目标方向
        float frameEulerAngle = SmoothDampRotate();

        return frameEulerAngle;
    }

    protected void ResetEulerSmoothDampData(float targetYEulerAngle)
    {
        // 归零旋转平滑时间 
        StateMachine.ReusableData.TimerToReachTargetRotation.y = 0f;
        // 记录新的旋转目标
        StateMachine.ReusableData.RecordEulerRoration.y = targetYEulerAngle;
    }

    // 将人物平滑转向到ReusableData中记录的目标方向
    protected float SmoothDampRotate()
    {
        // Mathf中的专用于角度的插值函数 区别于Lerp是专用于普通值的插值函数 之前我们用于Camera的Zoom
        float frameEulerAngle = Mathf.SmoothDampAngle(StateMachine.Controller.transform.rotation.eulerAngles.y, StateMachine.ReusableData.RecordEulerRoration.y, ref StateMachine.ReusableData.EulerVelocity.y, StateMachine.ReusableData.TimeToReachTargetRotation.y - StateMachine.ReusableData.TimerToReachTargetRotation.y);
        StateMachine.ReusableData.TimerToReachTargetRotation.y += Time.deltaTime;

        // 将最终旋转角度转化为Quaternion
        Quaternion targetotation = Quaternion.Euler(0f, frameEulerAngle, 0f);

        StateMachine.Controller.Rigidbody.MoveRotation(targetotation);
        return frameEulerAngle;
    }

    // 根据输入得到玩家在Unity的Y轴的旋转Euler角度 bool 为是否考虑相机角度


    private float AddCameraEulerRotarion(float unityRotateEulerAngle)
    {
        // 获取相机的基础角度
        float cameraEulerAngle = StateMachine.Controller.CameraTransform.rotation.eulerAngles.y;

        // 获得目标最终旋转角度
        float targetEulerAngle = unityRotateEulerAngle + cameraEulerAngle;

        while (targetEulerAngle > 360f)
        {
            targetEulerAngle -= 360f;
        }

        return targetEulerAngle;
    }

    // 根据输入的Vector3 或者 任务transform的Vector3 转换成Rotation的Euler方向
    // Vector3 -> Euler angle
    // 举一反三 Euler angle -> Vector3
    // Euler->Quaternion * Vector3.forward
    protected float GetEulerRotation(Vector3 direction)
    {
        // 将输入的方向转化为Unity坐标系的方向 后面的参数是Mathf中计算好的将弧度值转换为角度的参数 
        // 当然也有将角度转换为弧度的参数 Marthf.Deg2Rad
        // 因为本身参数是(y,x)，但是Unity是右手螺旋定理所以是反的？
        float unityRotateEulerAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

        while (unityRotateEulerAngle < 0f)
        {
            unityRotateEulerAngle += 360f;
        }

        return unityRotateEulerAngle;
    }

    private Vector3 GetOriginVelocity()
    {
        Vector3 horizontalVelocity = StateMachine.Controller.Rigidbody.velocity;
        horizontalVelocity.y = 0f;
        return horizontalVelocity;
    }
    #endregion

    #region Reused Methods
    protected bool CanTransformToFlyingState()
    {
        Ray rayFromCerterToGround = new Ray(StateMachine.Controller.PlayerFloatUtility.CapsuleColliderData.CapsuleColliderRef.bounds.center, Vector3.down);
        RaycastHit hit;
        if (Physics.Raycast(rayFromCerterToGround, out hit, airborneData.LeastFlyingHeight, groundedData.floatLayerMask, QueryTriggerInteraction.Ignore))
        {
            return false;
        }

        return true;
    }

    protected void StartAnimation(int animationHash)
    {
        StateMachine.Controller.animator.SetBool(animationHash, true);
    }

    protected void StopAnimation(int animationHash)
    {
        StateMachine.Controller.animator.SetBool(animationHash, false);
    }

    protected void ResetDefaultRecenteringData()
    {
        StateMachine.ReusableData.BackwardsRecenteringData = StateMachine.Controller.BackwardsRecenteringData;
        StateMachine.ReusableData.SidewayRecenteringData = StateMachine.Controller.SidewayRecenteringData;
    }
    protected void UpdateHorizontalRecentering(Vector2 movement)
    {
        if(movement == Vector2.zero)
        {
            return;
        }

        if (movement == Vector2.up)
        {
            StateMachine.Controller.RecenteringUtility.ResetRecentering();
            return;
        }

        if(movement == Vector2.down)
        {
            UpdateHorizontalRecenteringData(StateMachine.ReusableData.BackwardsRecenteringData);
            return;
        }

        UpdateHorizontalRecenteringData(StateMachine.ReusableData.SidewayRecenteringData);
    }

    // 根据目前的x轴旋转角度来判断对应cameraRecenteringList中记录的速度
    protected void UpdateHorizontalRecenteringData(List<CameraRecenteringData> cameraRecenteringList)
    {
        float playerXEulerAngle = StateMachine.Controller.RecenteringUtility.VirtualCamera.gameObject.transform.rotation.eulerAngles.x;
        float speedModifier = StateMachine.ReusableData.speedMultiplier;
        float basedSpeedModifier = groundedData.DefaultSpeedMultiplier;

        if(speedModifier == 0f)
        {
            speedModifier = 1f;
        }

        // 将恒定为正数的euler angle改为范围为(-90f, 90f)的角度值
        while (playerXEulerAngle >= 270f)
        {
            playerXEulerAngle -= 360f;
        }

        playerXEulerAngle = Mathf.Abs(playerXEulerAngle);

        foreach (CameraRecenteringData cameraRecenteringData in cameraRecenteringList)
        {
            if (cameraRecenteringData.IsWithingLimimation(playerXEulerAngle))
            {
                EnableHorizontalRecentering(cameraRecenteringData.WaitingTime, cameraRecenteringData.RecenteringTime, speedModifier, basedSpeedModifier);
                return;
            }
        }

        StateMachine.Controller.RecenteringUtility.DisableRecentering();
    }

    protected virtual void EnableHorizontalRecentering(float waitTime = -1f, float recenteringTime = -1f, float baseSpeedModifier = 1f, float speedModifier = 1f)
    {
        if(recenteringTime != -1)
        {
            recenteringTime = baseSpeedModifier / speedModifier * recenteringTime;
        }
         
        StateMachine.Controller.RecenteringUtility.EnableRecentering(waitTime, recenteringTime);
    }

    protected virtual void DisableHorizontalRecentering()
    {
        StateMachine.Controller.RecenteringUtility.DisableRecentering();
    }

    // 判断脚下是否是整块接触地面
    // 如果只是脚下一个洞或者裂缝 那么返回false
    protected bool TouchTriggerGround()
    {
        BoxCollider boxCollider = StateMachine.Controller.PlayerFloatUtility.PlayerTriggerData.GroundCheckCollider;
        Collider[] colliders = Physics.OverlapBox(boxCollider.bounds.center, StateMachine.Controller.PlayerFloatUtility.PlayerTriggerData.GroundCheckColliderExtents, boxCollider.transform.rotation, groundedData.floatLayerMask);
        if( colliders.Length > 0 )
        {
            return true;
        }

        return false;
    }

    protected virtual void ExamineLeavingGround()
    {
        
    }

    public bool isUpMove(float minSpeed = 0.1f)
    {
        if(GetVerticalSpeed() > minSpeed)
        {
            return true;
        }
        return false;
    }

    public bool isDownMove(float minSpeed = 0.1f)
    {
        if (GetVerticalSpeed() < -minSpeed)
        {
            return true;
        }
        return false;
    }

    public bool isHorizontally(float minSpeed = 0.1f)
    {
        if (GetHorizontalSpeed().magnitude < minSpeed)
        {
            return true;
        }
        return false;
    }

    protected void SetBaseRotaionData()
    {
        StateMachine.ReusableData.RotationData = groundedData.PlayerRotationData;
        StateMachine.ReusableData.TimeToReachTargetRotation.y = StateMachine.ReusableData.RotationData.TimeToReachTargetRotation.y;
    }

    // 水平方向减速
    protected virtual void HorizontalDecellerate()
    {
        // 得到当前的水平速度
        Vector2 currentV2Speed = GetHorizontalSpeed();
        // 将速度转换为Vector3
        Vector3 currentV3Speed = new Vector3(currentV2Speed.x, 0f, currentV2Speed.y);
        // 通过刚体减速 
        StateMachine.Controller.Rigidbody.AddForce(-currentV3Speed * StateMachine.ReusableData.speedDecelerateMultiplier, ForceMode.Acceleration);
    }

    // 垂直方向减速
    protected virtual void VerticalDecellerate()
    {
        // 得到当前的水平速度
        float currentV2Speed = GetVerticalSpeed();
        // 将速度转换为Vector3
        Vector3 currentV3Speed = new Vector3(0f, currentV2Speed, 0f);
        // 通过刚体减速 
        StateMachine.Controller.Rigidbody.AddForce(-currentV3Speed * StateMachine.ReusableData.speedDecelerateMultiplier, ForceMode.Acceleration);
    }

    // 判断是否减速为0
    protected virtual bool IsHorizontalStopping(float stoppingLimit = 0.1f)
    {
        // 得到当前的水平速度
        Vector2 currentV2Speed = GetHorizontalSpeed();
        if(currentV2Speed.magnitude < stoppingLimit)
        {
            return true;
        }
        return false;
    }

    protected float GetMovementSpeed(bool isConsiderSlope = true)
    {
        float movementSpeedModifier = StateMachine.ReusableData.speedMultiplier;

        if(isConsiderSlope)
        {
            movementSpeedModifier *= StateMachine.ReusableData.speedSlopeMultiplier;
        }

        return movementSpeedModifier;
    }

    protected Vector3 GetRigidBodySpeed()
    {
        return StateMachine.Controller.Rigidbody.velocity;
    }

    protected float GetVerticalSpeed()
    {
        return StateMachine.Controller.Rigidbody.velocity.y;
    }

    protected Vector2 GetHorizontalSpeed()
    {
        return new Vector2(StateMachine.Controller.Rigidbody.velocity.x, StateMachine.Controller.Rigidbody.velocity.z);
    }

    protected virtual void ResetVerticalVelocity()
    {
        Vector2 horizontalSpeed = GetHorizontalSpeed();
        StateMachine.Controller.Rigidbody.velocity = new Vector3(horizontalSpeed.x, 0f, horizontalSpeed.y);
    }

    protected virtual void ResetVelocity()
    {
        StateMachine.Controller.Rigidbody.velocity = Vector3.zero;
    }

    protected virtual void AddInputReaction()
    {
        StateMachine.Controller.Input.PlayerActions.WalkToggle.started += ToggleReaction;
        StateMachine.Controller.Input.PlayerActions.Movement.canceled += MovementCanceled;
        StateMachine.Controller.Input.PlayerActions.Movement.performed += MovementPerformed;
        StateMachine.Controller.Input.PlayerActions.Look.started += MouseMoved;
    }


    protected virtual void RemoveInputReaction() 
    {
        StateMachine.Controller.Input.PlayerActions.WalkToggle.started -= ToggleReaction;
        StateMachine.Controller.Input.PlayerActions.Movement.canceled -= MovementCanceled;
        StateMachine.Controller.Input.PlayerActions.Movement.performed -= MovementPerformed;
        StateMachine.Controller.Input.PlayerActions.Look.started -= MouseMoved;
    }

    protected Vector3 GetMovementInput()
    {
        return new Vector3(StateMachine.ReusableData.input.x, 0f, StateMachine.ReusableData.input.y);
    }

    
    protected float GetTartYEulerAngleBasedOnCamera(bool isAddingCameraEulerAngle = true)
    {
        float targetYEulerAngle = GetEulerRotation(GetMovementInput());
        if (isAddingCameraEulerAngle)
        {
            targetYEulerAngle = AddCameraEulerRotarion(targetYEulerAngle);
        }
        return targetYEulerAngle;
    }

    // 人物目标的角度的四元数 * Vector3.forward 可以得到具体的前进方位(Vector3)
    protected Vector3 GetMovementDirection(float targetEulerAngle)
    {
        Quaternion tarhetQuaternion = Quaternion.Euler(0f, targetEulerAngle, 0f);
        return tarhetQuaternion * Vector3.forward;
    }
    protected Vector3 GetMoveVelocity(Vector3 direction)
    {
        return StateMachine.Controller.playerData_SO.PlayerGroundedData.DefaultSpeed * StateMachine.ReusableData.speedMultiplier * StateMachine.ReusableData.speedSlopeMultiplier * direction;
    }

    #endregion

    #region Input Methods
    // 据悉的callback行为函数
    protected virtual void ToggleReaction(InputAction.CallbackContext obj)
    {
        StateMachine.ReusableData.isToggle = !StateMachine.ReusableData.isToggle;
    }

    protected virtual void MovementCanceled(InputAction.CallbackContext obj)
    {
        DisableHorizontalRecentering();
    }

    private void MouseMoved(InputAction.CallbackContext obj)
    {
        UpdateHorizontalRecentering(StateMachine.ReusableData.input);
    }

    private void MovementPerformed(InputAction.CallbackContext obj)
    {
        UpdateHorizontalRecentering(obj.ReadValue<Vector2>());
    }
    #endregion
}
