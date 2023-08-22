using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace GenshinImpactMovement
{
    public class EnemyController : MonoBehaviour
    {
        [field: Header("Reference")]
        public EnemyData_SO enemyData_SO;
        public EnemyStatemachine EnemyStatemachine { get; private set; }
        [field: Header("NavMesh")]
        public NavMeshAgent navMeshAgent { get; private set; }
        [field: Header("Common Ref")]
        public GameObject player { get; set; }
        [field: Header("Animator")]
        public Animator animator { get; private set; }

        public Rigidbody rb { get; private set; }

        private void Awake()
        {
            // Ä¬ÈÏ×´Ì¬idle
            navMeshAgent = GetComponent<NavMeshAgent>();
            animator = GetComponentInChildren<Animator>();
            EnemyStatemachine = new EnemyStatemachine(this);
            EnemyStatemachine.ChangeState(new EIdleState(EnemyStatemachine));
            rb = GetComponentInChildren<Rigidbody>();
        }

        private void Start()
        {
            EnemyStatemachine.reusableData.originalPos = EnemyStatemachine.controller.transform.position;
        }

        private void Update()
        {
            EnemyStatemachine.Update();
        }

        private void FixedUpdate()
        {
            EnemyStatemachine.PhysicalUpdate();
        }

        private void OnTriggerEnter(Collider other)
        {
            EnemyStatemachine.OnTriggerEnter(other);
        }

        private void OnTriggerExit(Collider other)
        {
            EnemyStatemachine.OnTriggerExit(other);
        }

        public void AnimationEnter()
        {
            EnemyStatemachine.OnAnimationEnterEvent();
        }

        public void AnimationExit()
        {
            EnemyStatemachine.OnAnimationExitEvent();
        }

        public void AnimationTrasition()
        {
            EnemyStatemachine.OnAnimationTransactionEvent();
        }

    }
}
