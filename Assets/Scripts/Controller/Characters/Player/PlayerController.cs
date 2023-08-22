using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

namespace GenshinImpactMovement
{
    /// <summary>
    /// 补充一个重要的知识点，父类包括子类可以直接引用子类，最好子类在创建时也传递父类引用
    /// 所以这里我们给状态机添加父类引用(PlayerController)
    /// </summary>
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerController : MonoBehaviour
    {
        [field:Header("Reference")]
        public PlayerData_SO playerData_SO;
        [field: Header("Recentering Utility")]
        public HorizontalRecenteringUtility RecenteringUtility;
        public List<CameraRecenteringData> BackwardsRecenteringData;
        public List<CameraRecenteringData> SidewayRecenteringData;
        [field: Header("Animator Utility")]
        [field:SerializeField] public AnimatorDataUtility animatorDataUtility { get; private set; }
        public Animator animator;
        [field:Header("Floating Utility")]
        [field:SerializeField] public ColliderUtility PlayerFloatUtility { get; private set; }
        public PlayerInput Input { get; private set; }
        public Rigidbody Rigidbody { get; private set; }
        // 这里不使用泛型父类的原因是因为我们需要使用特里面特定的cashing变量
        public PlayerStateMachine StateMachine { get; private set; }  

        public Transform CameraTransform { get; private set; }

        public CinemachineVirtualCamera VirtualCamera;

        private void Awake()
        {
            Input = GetComponent<PlayerInput>();
            Rigidbody = GetComponent<Rigidbody>();

            StateMachine = new PlayerStateMachine(this);
            CameraTransform = Camera.main.transform;

            PlayerFloatUtility.Initialize(gameObject);
            PlayerFloatUtility.CalculateFloatDemension();

            animator = GetComponentInChildren<Animator>();

            RecenteringUtility.Initialize();

            animatorDataUtility.Initialize();
        }

        private void Start()
        {
            StateMachine.ChangeState(StateMachine.IdelingState);
            InventoryManager.Instance.controller = this;
            QuestManager.Instance.controller = this;
            PauseManager.Instance.controller = this;
            AddInputAction();
        }

        private void Update()
        {
            StateMachine.Update();
        }

        private void FixedUpdate()
        {
            StateMachine.PhysicalUpdate();
        }

        private void OnDestroy()
        {
            RemoveInputAction();
        }

        private void AddInputAction()
        {
            Input.UIActions.Inventory.started += InventoryManager.Instance.ChangeStateAction;
            Input.UIActions.Quest.started += QuestManager.Instance.UpdateQuestPanel;
        }

        private void RemoveInputAction()
        {
            Input.UIActions.Inventory.started -= InventoryManager.Instance.ChangeStateAction;
            Input.UIActions.Quest.started -= QuestManager.Instance.UpdateQuestPanel;
        }

        // 如果有Inspector数值改变 那么重新计算我们floating capsule的长度和半径等数值
        private void OnValidate()
        {
            PlayerFloatUtility.Initialize(gameObject);
            PlayerFloatUtility.CalculateFloatDemension();
        }

        private void OnTriggerEnter(Collider other)
        {
            StateMachine.OnTriggerEnter(other);
        }

        private void OnTriggerExit(Collider other)
        {
            StateMachine.OnTriggerExit(other);
        }

        public void AnimationEnter()
        {
            StateMachine.OnAnimationEnterEvent();
        }

        public void AnimationExit()
        {
            StateMachine.OnAnimationExitEvent();
        }

        public void AnimationTransition()
        {
            StateMachine.OnAnimationTransactionEvent();
        }
    }
}
