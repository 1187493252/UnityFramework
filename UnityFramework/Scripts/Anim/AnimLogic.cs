using UnityEngine;

namespace UnityFramework.Runtime
{
    public class AnimLogic : MonoBehaviour
    {
        private Animator animator;
        protected virtual void Awake()
        {
            animator = GetComponent<Animator>();
            ComponentEntry.Anim.AddAnimationEndEvent(animator, "AnimEnd");
            ComponentEntry.Anim.AddAnimationStartEvent(animator, "AnimStart");

        }
        void AnimEnd()
        {
            ComponentEntry.Anim.AnimPlayEndEvent?.Invoke();
        }
        void AnimStart()
        {
            ComponentEntry.Anim.AnimPlayStartEvent?.Invoke();
        }
    }
}