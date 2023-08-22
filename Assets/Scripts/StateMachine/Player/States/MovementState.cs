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
    
    // �ҷ���propertyһ�����ڱ���ֻ����ĳ�������޸ĵĶ���
    // ���������룬�Լ�����Ϊ����ת����3D����
    protected Vector3 inputDirection;
    // Ŀ��Euler�Ƕ� �Ƕ�SmoothDampʱ�� �Ƕ�SmoothDamp��ʱ�� �Ƕ�SmoothDamp�ٶ�
    // TODO:����д���ݵ�ʱ��ɾ��


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
        // �Ƿ����뿪����
        if(groundedData.isTouchGround(collider.gameObject.layer))
        {
            // ����Ƿ�����뿪�����������״̬��һЩϸ��
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

    // �ƶ�ʱ�ŵ�����ת
    protected void Move()
    {
        // ��Ϊ��ʹ����Ƕ�Ϊ0��Ҳ�������������Ƕȣ�Ҳ���ƶ�����������ֱ��������Ϊ0������������ƶ�����
        if (StateMachine.ReusableData.input == Vector2.zero || StateMachine.ReusableData.speedMultiplier == 0f)
        {
            return;
        }

        float targetEulerAngle = Rotate();

        // ��Ŀ��Ƕ�ֵ ת��Ϊ Ŀ��Vector3�� ԭ������ת����Quaternion��֮���ٳ���Vector3.forward
        Vector3 playerMoveDirection = GetMovementDirection(targetEulerAngle);
        Vector3 inputVelocity = GetMoveVelocity(playerMoveDirection);
        Vector3 originVelocity = GetOriginVelocity();

        // ��Ϊ�ٶ��ǵ��� ���Եü�ȥ�������ٶ� ��Ȼ�Ҿ�������Ҳ����ֱ�ӽ��ٶȹ��� �ڼ����µ��ٶ�
        StateMachine.Controller.Rigidbody.AddForce(inputVelocity - originVelocity, ForceMode.VelocityChange);
    }



    protected float Rotate()
    {
        // �������Vector2ת��ΪY����ת��euler�Ƕ�ֵ
        float targetYEulerAngle = GetTartYEulerAngleBasedOnCamera();

        // �Ƿ������µ���תĿ��
        if (StateMachine.ReusableData.RecordEulerRoration.y != targetYEulerAngle)
        {
            // ���֮ǰ��¼��Ŀ����ת�Ƕ� �� �Ѿ���תʱ��
            ResetEulerSmoothDampData(targetYEulerAngle);
        }

        // ƽ���ƶ� ���� ת����Ŀ�귽��
        float frameEulerAngle = SmoothDampRotate();

        return frameEulerAngle;
    }

    protected void ResetEulerSmoothDampData(float targetYEulerAngle)
    {
        // ������תƽ��ʱ�� 
        StateMachine.ReusableData.TimerToReachTargetRotation.y = 0f;
        // ��¼�µ���תĿ��
        StateMachine.ReusableData.RecordEulerRoration.y = targetYEulerAngle;
    }

    // ������ƽ��ת��ReusableData�м�¼��Ŀ�귽��
    protected float SmoothDampRotate()
    {
        // Mathf�е�ר���ڽǶȵĲ�ֵ���� ������Lerp��ר������ֵͨ�Ĳ�ֵ���� ֮ǰ��������Camera��Zoom
        float frameEulerAngle = Mathf.SmoothDampAngle(StateMachine.Controller.transform.rotation.eulerAngles.y, StateMachine.ReusableData.RecordEulerRoration.y, ref StateMachine.ReusableData.EulerVelocity.y, StateMachine.ReusableData.TimeToReachTargetRotation.y - StateMachine.ReusableData.TimerToReachTargetRotation.y);
        StateMachine.ReusableData.TimerToReachTargetRotation.y += Time.deltaTime;

        // ��������ת�Ƕ�ת��ΪQuaternion
        Quaternion targetotation = Quaternion.Euler(0f, frameEulerAngle, 0f);

        StateMachine.Controller.Rigidbody.MoveRotation(targetotation);
        return frameEulerAngle;
    }

    // ��������õ������Unity��Y�����תEuler�Ƕ� bool Ϊ�Ƿ�������Ƕ�


    private float AddCameraEulerRotarion(float unityRotateEulerAngle)
    {
        // ��ȡ����Ļ����Ƕ�
        float cameraEulerAngle = StateMachine.Controller.CameraTransform.rotation.eulerAngles.y;

        // ���Ŀ��������ת�Ƕ�
        float targetEulerAngle = unityRotateEulerAngle + cameraEulerAngle;

        while (targetEulerAngle > 360f)
        {
            targetEulerAngle -= 360f;
        }

        return targetEulerAngle;
    }

    // ���������Vector3 ���� ����transform��Vector3 ת����Rotation��Euler����
    // Vector3 -> Euler angle
    // ��һ���� Euler angle -> Vector3
    // Euler->Quaternion * Vector3.forward
    protected float GetEulerRotation(Vector3 direction)
    {
        // ������ķ���ת��ΪUnity����ϵ�ķ��� ����Ĳ�����Mathf�м���õĽ�����ֵת��Ϊ�ǶȵĲ��� 
        // ��ȻҲ�н��Ƕ�ת��Ϊ���ȵĲ��� Marthf.Deg2Rad
        // ��Ϊ���������(y,x)������Unity�������������������Ƿ��ģ�
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

    // ����Ŀǰ��x����ת�Ƕ����ж϶�ӦcameraRecenteringList�м�¼���ٶ�
    protected void UpdateHorizontalRecenteringData(List<CameraRecenteringData> cameraRecenteringList)
    {
        float playerXEulerAngle = StateMachine.Controller.RecenteringUtility.VirtualCamera.gameObject.transform.rotation.eulerAngles.x;
        float speedModifier = StateMachine.ReusableData.speedMultiplier;
        float basedSpeedModifier = groundedData.DefaultSpeedMultiplier;

        if(speedModifier == 0f)
        {
            speedModifier = 1f;
        }

        // ���㶨Ϊ������euler angle��Ϊ��ΧΪ(-90f, 90f)�ĽǶ�ֵ
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

    // �жϽ����Ƿ�������Ӵ�����
    // ���ֻ�ǽ���һ���������ѷ� ��ô����false
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

    // ˮƽ�������
    protected virtual void HorizontalDecellerate()
    {
        // �õ���ǰ��ˮƽ�ٶ�
        Vector2 currentV2Speed = GetHorizontalSpeed();
        // ���ٶ�ת��ΪVector3
        Vector3 currentV3Speed = new Vector3(currentV2Speed.x, 0f, currentV2Speed.y);
        // ͨ��������� 
        StateMachine.Controller.Rigidbody.AddForce(-currentV3Speed * StateMachine.ReusableData.speedDecelerateMultiplier, ForceMode.Acceleration);
    }

    // ��ֱ�������
    protected virtual void VerticalDecellerate()
    {
        // �õ���ǰ��ˮƽ�ٶ�
        float currentV2Speed = GetVerticalSpeed();
        // ���ٶ�ת��ΪVector3
        Vector3 currentV3Speed = new Vector3(0f, currentV2Speed, 0f);
        // ͨ��������� 
        StateMachine.Controller.Rigidbody.AddForce(-currentV3Speed * StateMachine.ReusableData.speedDecelerateMultiplier, ForceMode.Acceleration);
    }

    // �ж��Ƿ����Ϊ0
    protected virtual bool IsHorizontalStopping(float stoppingLimit = 0.1f)
    {
        // �õ���ǰ��ˮƽ�ٶ�
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

    // ����Ŀ��ĽǶȵ���Ԫ�� * Vector3.forward ���Եõ������ǰ����λ(Vector3)
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
    // ��Ϥ��callback��Ϊ����
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
