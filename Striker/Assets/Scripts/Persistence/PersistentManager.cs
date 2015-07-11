//-----------------------------
// ImperfectlyCoded © 2015
//-----------------------------

namespace Assets.Scripts.Persistence
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Text;
    using Assets.Scripts.Level;

    public class PersistentManager
    {
        #region Fields

        private const int m_playerKey = 9999;
        private const string m_filename = "/player_data.dat";
        private static PersistentManager m_instance;

        // key-player, (key-level, value-IDataBlock)
        // key-zone, (key-level, value-IDataBlock)
        private Dictionary<int, Dictionary<int, IDataBlock>> m_blocks;

        #endregion

        #region Public Properties

        public static PersistentManager Instance
        {
            get
            {
                if(m_instance == null)
                {
                    m_instance = new PersistentManager();
                }

                return m_instance;
            }
        }

        public static int PlayerKey
        {
            get
            {
                return m_playerKey;
            }
        }

        public static bool ModifiedData
        {
            get;
            private set;
        }

        #endregion

        #region Constructors

        private PersistentManager()
        {
            m_blocks = new Dictionary<int, Dictionary<int, IDataBlock>>();
            ModifiedData = false;
        }

        #endregion

        #region Public Methods

        public void Save(string path)
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            string fullPath = path + m_filename;
            FileStream file = File.Create(fullPath);

            binaryFormatter.Serialize(file, m_blocks);
            file.Close();

            ModifiedData = false;
        }

        public void Load(string path)
        {
            string fullPath = path + m_filename;
            if (!File.Exists(fullPath))
            {
                CreateEmptyBlocks();
                Save(path);
                return;
            }

            using (FileStream file = File.Open(fullPath, FileMode.Open))
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                m_blocks = (Dictionary<int, Dictionary<int, IDataBlock>>)binaryFormatter.Deserialize(file);
                binaryFormatter.Serialize(file, m_blocks);
            }
        }

        public int GetValue(int key1, int key2, DataIndex dataIndex)
        {
            if (!m_blocks.ContainsKey(key1))
            {
                return -1;
            }

            if (!m_blocks[key1].ContainsKey(key2))
            {
                return -1;
            }

            int index = (int)dataIndex;
            return m_blocks[key1][key2].GetValues()[index];
        }

        public void SetValue(int key1, int key2, DataIndex dataIndex, int value)
        {
            if(!m_blocks.ContainsKey(key1))
            {
                return;
            }

            if(!m_blocks[key1].ContainsKey(key2))
            {
                return;
            }

            m_blocks[key1][key2].StoreValues(dataIndex, value);
        }

        #endregion

        #region Private Methods

        private void CreateEmptyBlocks()
        {
            PlayerDataBlock playerBlock = new PlayerDataBlock();
            for (int index = 0; index < (int)DataIndex.MaxAmountPlayerData; ++index)
            {
                playerBlock.StoreValues((DataIndex)index, 0);
            }

            int playerKey = PersistentManager.PlayerKey;
            PersistentManager.Instance.AddBlock(playerKey, playerKey, playerBlock);

            for (int zoneIndex = 0; zoneIndex < (int)LevelZoneId.MaxZones; ++zoneIndex)
            {
                for (int levelIndex = 0; levelIndex < LevelZone.MAX_LEVELS; ++levelIndex)
                {
                    LevelDataBlock levelBlock = new LevelDataBlock();
                    for (int index = 0; index < (int)DataIndex.MaxAmountLevelData; ++index)
                    {
                        // Unlock initial level
                        if(levelIndex == 0 && index == (int)DataIndex.Unlocked)
                        {
                            levelBlock.StoreValues((DataIndex)index, 1);
                            continue;
                        }

                        levelBlock.StoreValues((DataIndex)index, 0);
                    }

                    // offset for level IDs
                    int levelKey = (int)LevelId.Plains1 + levelIndex;
                    levelKey += (zoneIndex * LevelZone.MAX_LEVELS);

                    PersistentManager.Instance.AddBlock(zoneIndex, levelKey, levelBlock);
                }
            }
        }

        #endregion

        #region Private Methods

        private IDataBlock GetDataBlock(int key, int levelKey)
        {
            if (!m_blocks.ContainsKey(key))
            {
                return null;
            }

            if (!m_blocks[key].ContainsKey(levelKey))
            {
                return null;
            }

            return m_blocks[key][levelKey];
        }

        private void AddBlock(int key1, int key2, IDataBlock block)
        {
            if (block == null || m_blocks == null)
            {
                return;
            }

            if (!m_blocks.ContainsKey(key1))
            {
                Dictionary<int, IDataBlock> blocks = new Dictionary<int, IDataBlock>();
                blocks.Add(key2, block);

                m_blocks.Add(key1, blocks);
                ModifiedData = true;
                return;
            }

            if (!m_blocks[key1].ContainsKey(key2))
            {
                m_blocks[key1].Add(key2, block);
                ModifiedData = true;
                return;
            }

            m_blocks[key1][key2] = block;
            ModifiedData = true;
        }

        #endregion
    }
}
