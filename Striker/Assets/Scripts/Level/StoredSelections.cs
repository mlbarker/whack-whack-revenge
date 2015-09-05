//-----------------------------
// ImperfectlyCoded © 2015
//-----------------------------

namespace Assets.Scripts.Level
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    internal static class StoredSelections
    {
        #region Static Members

        private static Dictionary<string, bool> m_selections = new Dictionary<string, bool>();

        #endregion

        #region Static Methods

        public static void SetSelection(string selection, bool selected)
        {
            if (!m_selections.ContainsKey(selection))
            {
                m_selections.Add(selection, selected);
            }
            else
            {
                m_selections[selection] = selected;
            }
        }

        public static bool GetSelection(string selection)
        {
            if (!m_selections.ContainsKey(selection))
            {
                m_selections.Add(selection, false);
            }

            return m_selections[selection];
        }

        #endregion
    }
}
