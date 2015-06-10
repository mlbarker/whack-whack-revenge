//-----------------------------
// ImperfectlyCoded © 2015
//-----------------------------

namespace Assets.Scripts.Persistence
{
    using System.Collections.Generic;
    using UnityEngine;
    using Assets.Scripts.Level;

    public class PersistentDataManager
    {
        #region Fields

        /* 
         *                  lock level      starType   star1   starType    star2   starType    star3         
         * 0000 0000-0000   0    000-0000   000        0       000         0       000         0    
         * 
         * 
         * 0000 0000 0000 0000 0000 0000 0000 0000
         * 
         * 
         * 0000 0000 0000 0000 0000 0000 0000 0000
         */

        private List<DataBlock> m_dataBlocks;
        private Dictionary<LevelZoneId, Dictionary<string, PersistentDataBlock>> m_persistentDataBlocks;
        private static PersistentDataManager m_instance;
        //private Dictionary<PersistenceKey, List<PersistentDataBlock>> m_persistentDataBlocks;

        #endregion

        #region Public Properties

        public static PersistentDataManager Instance
        {
            get
            {
                if(m_instance == null)
                {
                    m_instance = new PersistentDataManager();
                }

                return m_instance;
            }
        }

        #endregion

        #region Constructors

        private PersistentDataManager()
        {
            m_persistentDataBlocks = new Dictionary<LevelZoneId, Dictionary<string, PersistentDataBlock>>();
        }

        #endregion

        #region Public Methods

        public int CreateBlock(LevelZoneId zoneId, PersistentDataBlock block)
        {
            if(m_persistentDataBlocks.ContainsKey(zoneId))
            {
                m_persistentDataBlocks[zoneId].Add(block.Key, block);
                return 2;
            }

            Dictionary<string, PersistentDataBlock> dataBlockDictionary = new Dictionary<string, PersistentDataBlock>();
            dataBlockDictionary.Add(block.Key, block);
            m_persistentDataBlocks.Add(zoneId, dataBlockDictionary);
            return 1;
        }

        public int UpdateBlock(LevelZoneId zoneId, PersistentDataBlock block)
        {
            if(!m_persistentDataBlocks.ContainsKey(zoneId))
            {
                return 0;
            }

            if(!m_persistentDataBlocks[zoneId].ContainsKey(block.Key))
            {
                return 0;
            }

            m_persistentDataBlocks[zoneId][block.Key] = block;
            return 1;
        }

        public PersistentDataBlock GetBlock(LevelZoneId zoneId, string blockKey)
        {
            if (!m_persistentDataBlocks.ContainsKey(zoneId))
            {
                return null;
            }

            if (!m_persistentDataBlocks[zoneId].ContainsKey(blockKey))
            {
                return null;
            }

            PersistentDataBlock block = m_persistentDataBlocks[zoneId][blockKey];
            return block;
        }

        public int Load()
        {
            return 1;
        }

        public int Save()
        {
            return 1;
        }

        #endregion
    }
}
