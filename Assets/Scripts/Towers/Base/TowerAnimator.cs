using UnityEngine;
using System.Collections;

namespace SesiDefense.Towers.Base
{
    /// <summary>
    /// Handles tower animation state and transitions.
    /// Manages idle, attack, and upgrade animations.
    /// </summary>
    public class TowerAnimator : MonoBehaviour
    {
        private Animator animator;
        private int attackHash = Animator.StringToHash("Attack");
        private int upgradeHash = Animator.StringToHash("Upgrade");
        private int idleHash = Animator.StringToHash("Idle");

        private void Awake()
        {
            animator = GetComponent<Animator>();
            if (animator == null)
            {
                animator = gameObject.AddComponent<Animator>();
            }
        }

        public void PlayAttackAnimation()
        {
            if (animator != null)
            {
                animator.SetTrigger(attackHash);
            }
        }

        public void PlayUpgradeAnimation()
        {
            if (animator != null)
            {
                animator.SetTrigger(upgradeHash);
            }
        }

        public void PlayIdleAnimation()
        {
            if (animator != null)
            {
                animator.SetTrigger(idleHash);
            }
        }
    }
}
