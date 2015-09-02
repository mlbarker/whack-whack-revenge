//-----------------------------
// ImperfectlyCoded © 2015
//-----------------------------

namespace Assets.Scripts.Persistence
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Assets.Scripts.Level;

    [Serializable]
    internal class LevelDataBlock : IDataBlock
    {
        #region Fields

        private List<int> m_values;

        #endregion
        
        #region Public Properties
        
        public LevelInfo LevelInfoData
        {
            get;
            private set;
        }
        
        
        #endregion

        #region Constructors

        public LevelDataBlock()
        {
            m_values = new List<int>((int)DataIndex.MaxAmountLevelData);

            for (int n = 0; n < m_values.Capacity; ++n)
            {
                m_values.Add(0);
            }
        }

        #endregion

        #region IDataBlock Methods

        public void StoreValues(DataIndex dataIndex, int value)
        {
            m_values[(int)dataIndex] = value;
        }

        public List<int> GetValues()
        {
            List<int> values = new List<int>();
            values.AddRange(m_values);

            return values;
        }

        #endregion
        
        #region Public Methods
        
        public void StoreLevelInfoData(LevelInfo levelInfo)
        {
            if(levelInfo.LevelIdNum == 0)
            {
                return;
            }
            
            LevelInfoData = levelInfo;
        }
        
        #endregion
    }
}
