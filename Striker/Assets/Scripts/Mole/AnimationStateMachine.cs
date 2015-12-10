//-----------------------------
// ImperfectlyCoded © 2015
//-----------------------------

namespace Assets.Scripts.Mole
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class AnimationStateMachine
    {
        #region Fields

        private Dictionary<string, bool> m_animations = new Dictionary<string, bool>();
        private string m_animationError;

        #endregion

        #region Constructor

        public AnimationStateMachine()
        {
            m_animations.Add(AnimationNames.Idle, false);
            m_animations.Add(AnimationNames.MoveUp, false);
            m_animations.Add(AnimationNames.MoveDown, false);
            m_animations.Add(AnimationNames.Swoon, false);
            m_animations.Add(AnimationNames.Injured, false);
            m_animations.Add(AnimationNames.ThrowStone, false);
        }

        #endregion

        #region Public Properties

        public string LastError
        {
            get
            {
                return m_animationError;
            }
        }

        #endregion

        #region Public Methods

        public void UpdateAnimationState(string animation, bool play)
        {
            ClearLastError();

            if (!m_animations.ContainsKey(animation))
            {
                m_animationError = animation + " does not exist";
                return;
            }

            m_animations[animation] = play;
        }

        public bool GetAnimationState(string animation)
        {
            ClearLastError();

            if (!m_animations.ContainsKey(animation))
            {
                m_animationError = animation + " does not exist";
                return false;
            }

            if(!OneAnimationPlaying())
            {
                m_animationError += "animations are playing"; 
            }

            return m_animations[animation];
        }

        #endregion

        #region Private Methods

        private bool OneAnimationPlaying()
        {
            int animationCounter = 0;
            foreach (KeyValuePair<string, bool> animationState in m_animations)
            {
                if(animationState.Value)
                {
                    m_animationError += animationState.Key;
                    m_animationError += "|";
                    ++animationCounter;
                }
            }

            return animationCounter == 1;
        }

        private void ClearLastError()
        {
            m_animationError = string.Empty;
        }

        #endregion
    }
}
