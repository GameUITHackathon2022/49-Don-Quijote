using LittleFoxLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.IO;
using UnityEngine.UI;
using UnityEngine.ProBuilder;

namespace LittleFoxLite
{

    public class InteractiveHandel : MonoBehaviour
    {
        public static InteractiveHandel instance;
        StateManager stateManager;
        [SerializeField] LayerMask interRackMask;
        [SerializeField] float CapRate = 0.15f;
        [SerializeField] Canvas UIcanvas;
        Animator animator;
        // Animator
        int aniFloatType;
        int aniBoolGrab;
        int aniBoolGive;
        int aniBoolTalking;
        void AssignAnimationID()
        {
            aniFloatType = Animator.StringToHash("TalkType");
            aniBoolGrab = Animator.StringToHash("IsGrab");
            aniBoolGive = Animator.StringToHash("IsGive");
            aniBoolTalking = Animator.StringToHash("IsTalking");
            ;
        }
        private void Awake()
        {
            if (instance == null)
                instance = this;
            animator = GetComponent<Animator>();
            AssignAnimationID();
        }
        // Start is called before the first frame update
        void Start()
        {
            stateManager = PlayerController.Instance.stateManager;
            InvokeRepeating(nameof(SOnInteractive), 0, CapRate);
            UIcanvas.gameObject.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {

        }
        [SerializeField] Vector3 deltaUI;
        [SerializeField] LayerMask DialogueMask;
        [SerializeField] List<Sprite> sprites;
        Collider coli = null;
        int Type = -1;
        public void SOnInteractive()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, CapRadius, interRackMask);
            Collider[] collidersDia = Physics.OverlapSphere(transform.position, CapRadius, DialogueMask);

            if (colliders.Length == 0 && collidersDia.Length == 0)
            {
                UIcanvas.gameObject.SetActive(false);
                coli = null;
                Type = -1;
                return;
            }
            float maxvalue = -1f;
            foreach (var k in colliders)
            {
                Vector3 deltaPos = k.transform.position - transform.position;
                float Value = Vector3.Dot(transform.forward, deltaPos.normalized);
                if (Value >= maxvalue)
                {
                    coli = k;
                    maxvalue = Value;
                    Type = 0;
                }
            }
            foreach (var k in collidersDia)
            {
                Vector3 deltaPos = k.transform.position - transform.position;
                float Value = Vector3.Dot(transform.forward, deltaPos.normalized);
                if (Value >= maxvalue)
                {
                    coli = k;
                    maxvalue = Value;
                    Type = 1;
                }
            }
            if (coli != null && Type > -1)
            {
                UIcanvas.gameObject.SetActive(true);
                UIcanvas.transform.position = coli.transform.position + deltaUI;
                UIcanvas.GetComponentInChildren<Image>().sprite = sprites[Type];
            }
        }
        public void PerformDialogue()
        {
            OnDiagloguePerform();
        }
        [SerializeField] float GrabTime = 1f;
        public void PerformInteractive()
        {
            if (Type == 0)
                OnGrabPerform();
            if (Type == 1)
                PerformDialogue();
        }
        public void OnGrabPerform()
        {
            if (coli == null) return;
            if (animator.GetBool(aniBoolGrab)) return;
            lastState = stateManager.CurrentType;
            stateManager.SwitchState(PlayerStateType.Event);
            animator.SetBool(aniBoolGrab, true);
            Vector3 deltaPos = coli.transform.position - transform.position;
            transform.DORotate(Quaternion.LookRotation(deltaPos.normalized).eulerAngles, GrabTime).OnComplete(OnGarabComplete);
        }
        public bool isInDialogue = false;
        DialogueActor actor;
        public void OnDiagloguePerform()
        {
            if (coli == null) return;
            isInDialogue = true;
            actor = coli.GetComponent<DialogueActor>();
            if (actor == null) return;
            actor.StartDialogueAction();
            ChangeToEventState();
        }
        public void OutDialoguePerform()
        {
            isInDialogue = false;
            if (DialogueManager.instance.dialogue == null) return;
            DialogueManager.instance.dialogue.EndDialogueAction();
            ChangeToNormalState();
        }
        void OnGarabComplete()
        {

            animator.SetBool(aniBoolGrab, false);
            ChangeToNormalState();
            Destroy(coli.gameObject);
        }
        PlayerStateType lastState = PlayerStateType.Normal;
        public void ChangeToNormalState()
        {
            if(lastState == PlayerStateType.Event) lastState = PlayerStateType.Normal;
            stateManager.SwitchState(lastState);
        }
        public void ChangeToEventState()
        {
            lastState = stateManager.CurrentType;
            stateManager.SwitchState(PlayerStateType.Event);
        }
        [SerializeField] float CapRadius;
    }

}