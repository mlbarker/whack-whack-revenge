//-----------------------------
// ImperfectlyCoded © 2015
//-----------------------------

namespace Assets.Scripts.Persistence
{
    using System.Collections.Generic;
    using UnityEngine;
    using Assets.Scripts.Level;

    public class PlayerPrefsManager : MonoBehaviour
    {
        #region Unity Methods

        void Start()
        {
        }

        #endregion

        #region Public Methods

        public void Store(LevelZoneId zoneKey, LevelId levelId, List<LevelStarInfo> starInfo)
        {
            int[] starTypes = new int [starInfo.Count];
            int[] starsAchieved = new int[starInfo.Count];

            for(int index = 0; index < starInfo.Count; ++index)
            {
                starTypes[index] = (int)starInfo[index].starType;
                starsAchieved[index] = starInfo[index].requirementAchieved ? 1 : 0;
            }

            int level = (int)levelId;

            level <<= (int)PersistentBitMask.LevelIdShift;
            starTypes[0] <<= (int)PersistentBitMask.LevelStar1Shift;
            starTypes[0] <<= (int)PersistentBitMask.LevelStar1TypeShift;
            starTypes[1] <<= (int)PersistentBitMask.LevelStar2Shift;
            starTypes[1] <<= (int)PersistentBitMask.LevelStar2TypeShift;
            starTypes[2] <<= (int)PersistentBitMask.LevelStar3Shift;
            starTypes[2] <<= (int)PersistentBitMask.LevelStar3TypeShift;

            int levelSaveData = 0;
            for(int index = 0; index < starInfo.Count; ++index)
            {
                levelSaveData += starTypes[index];
                levelSaveData += starsAchieved[index];
            }

            levelSaveData += level;

            PersistentDataBlock block = new PersistentDataBlock(levelId.ToString(), levelSaveData);
            PersistentDataManager.Instance.UpdateBlock(zoneKey, block);
        }

        #endregion
    }
}