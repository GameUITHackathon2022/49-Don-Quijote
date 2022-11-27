using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine;

namespace LittleFoxLite
{
    public class DialogueActor : MonoBehaviour
    {
        public TextMeshProUGUI text;
        [TextArea]
        public List<string> diaElements;
        public int Currentindex = 0;
        Coroutine currentCou;
        [SerializeField] int iiType;
    // Start is called before the first frame update
        void Start()
        {
           
        }
        public float TimeDelay = 0.02f;
        IEnumerator Type()
        {
            if (Currentindex > diaElements.Count-1) Currentindex = diaElements.Count-1;
            text.text = "";
            foreach(char a in diaElements[Currentindex].ToCharArray())
            {
                text.text += a;
                yield return new WaitForSeconds(TimeDelay);
            }
        }
        private void Update()
        {
            Vector3 delta = PlayerController.Instance.transform.position - transform.position;
            transform.rotation = Quaternion.LookRotation(delta.normalized);
        }
        public void StartDialogueAction()
        {
            DialogueManager.instance.dialogue = this;
            DialogueManager.instance.StartDialog();
            text = DialogueManager.instance.gText;
            currentCou = StartCoroutine(Type());
        }
        public void EndDialogueAction()
        {
            if (iiType == 2) GotoNext();
            if (currentCou != null) StopCoroutine(currentCou);
            DialogueManager.instance.StopDialog();
            DialogueManager.instance.dialogue = null;
            InteractiveHandel.instance.ChangeToNormalState();
            if (InteractiveHandel.instance.isInDialogue)
            {
                InteractiveHandel.instance.OutDialoguePerform();
            }

        }
        void GotoNext()
        {
            SceneManager.LoadScene(2);
        }
        public void NextDialogue()
        {
            Currentindex++;
            if (Currentindex == diaElements.Count)
            {
                Currentindex = diaElements.Count - 1;
                EndDialogueAction();
            }
            if (currentCou != null) StopCoroutine(currentCou);
            StopAllCoroutines();
            StartCoroutine(Type());
        }
    }

}
