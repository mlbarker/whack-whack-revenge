//-----------------------------
// ImperfectlyCoded © 2014-2015
//-----------------------------

namespace Assets.Scripts.Game
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using Assets.Scripts.Formation;
    using Assets.Scripts.Level;
    using Assets.Scripts.Mole;
    using Assets.Scripts.Persistence;
    using Assets.Scripts.Player;
    using Assets.Scripts.Projectile;
    using Assets.Scripts.Score;
    using Assets.Scripts.Utilities.Timers;

    public class Game : MonoBehaviour
    {
        #region Private Members

        private GameController m_gameController;
        private StarController m_starController;
        private LevelManager m_levelManager;
        private PersistentManager m_persistentManager;
        private List<GameObject> m_projectileObjects;
        private Mole[] m_moles;
        private bool m_dataSaved;
        private int m_zoneId;
        private int m_levelId;
        private string m_projectileTag = "Projectile";

        #endregion

        #region Public Properties

        public int Score
        {
            get
            {
                return m_gameController.CurrentScore;
            }
        }

        public float WhackPercentage
        {
            get
            {
                return m_gameController.CurrentPercentage;
            }
        }

        public int GameTimeSecondsLeft
        {
            get
            {
                return m_gameController.GameTimeSecondsLeft;
            }
        }

        public bool StartGameCalled
        {
            get;
            set;
        }

        public bool DisplayGameResults
        { 
            get; 
            private set; 
        }

        public bool DisplayDefeatedGameResults
        {
            get;
            private set;
        }

        public bool DisplayObjectives
        {
            get;
            set;
        }

        public bool CompletedLevel
        {
            get;
            private set;
        }

        public List<string> Objectives
        {
            get
            {
                return m_starController.Objectives;
            }
        }

        public int StarsAchievedCount
        {
            get
            {
                return m_starController.StarsAchievedCount;
            }
        }

        public int PlayerHits
        {
            get;
            private set;
        }

        public bool PlayerDefeated
        {
            get
            {
                return player.Health < 1;
            }
        }

        #endregion

        #region Editor Values

        public Player player;
        public BaseFormation formation;

        #endregion

        #region Unity Methods

        void Start()
        {
            Initialize();
        }

        void Update()
        {
            UpdateGame();
        }

        #endregion

        #region Private Methods

        private void Initialize()
        {
            InitializeFormation();
            InitializeMoles();
            InitializePlayer();
            InitializeGame();
            InitializeStars();
            InitializePersistence();
        }

        private void InitializeFormation()
        {
            if(formation == null)
            {
                throw new NullReferenceException("BaseFormation object is null");
            }

            formation.InitializePositions();
            m_moles = formation.Moles;
        }

        private void InitializeMoles()
        {
            if (m_moles == null)
            {
                throw new MoleException();
            }

            foreach (Mole mole in m_moles)
            {
                mole.Initialize();
            }
        }

        private void InitializePlayer()
        {
            if (player == null)
            {
                throw new PlayerException();
            }

            player.Initialize();
        }

        private void InitializeGame()
        {
            DisplayGameResults = false;
            DisplayObjectives = true;
            StartGameCalled = false;
            m_dataSaved = false;

            m_levelManager = LevelManager.Instance;
            m_zoneId = (int)m_levelManager.SelectedLevelInfo.ZoneId;
            m_levelId = (int)m_levelManager.SelectedLevelInfo.LevelIdNum;
            int gameTimeInSeconds = m_levelManager.SelectedLevelInfo.LevelTimeInSeconds;

            m_gameController = new GameController();
            m_gameController.SetGameTime(gameTimeInSeconds);
            m_gameController.SetOnGameEndCallback(EndGame);
            m_gameController.AddPlayerController(player.playerController);

            m_projectileObjects = new List<GameObject>();

            foreach(Mole mole in m_moles)
            {
                m_gameController.AddMoleController(mole.moleController);
            }

            m_gameController.Initialize();
        }

        private void InitializeStars()
        {
            m_starController = new StarController(m_gameController);
            m_starController.Initialize();
        }

        private void InitializePersistence()
        {
            m_persistentManager = PersistentManager.Instance;
        }

        private bool OnDisplayObjectives()
        {
            if (DisplayObjectives)
            {
                OnGamePaused();
                return true;
            }

            if (StartGameCalled)
            {
                OnGameResumed();

                StartGameCalled = false;
            }

            return false;
        }

        private void OnGamePaused()
        {
            PauseManager.Instance.GamePaused();
        }

        private void OnGameResumed()
        {
            PauseManager.Instance.GameResumed();
        }

        private void UpdateGame()
        {
            if(OnDisplayObjectives())
            {
                return;
            }

            UpdateProjectilesList();
            UpdatePlayerHealth();
            UpdateWhackedObjects();
            //UpdateProjectileWasWhacked();
            //UpdateMoleWasWhacked();
            //ClearPlayerHitCollision();
            UpdateGameController();
            UpdateStarsAchievements();
            ClearObjectHit();
            ClearMoleHit();
            CheckEndGameFinished();
        }

        private void UpdateProjectilesList()
        {
            GameObject[] projectileObjects = GameObject.FindGameObjectsWithTag(m_projectileTag);
            if(projectileObjects.Length != 0)
            {
                m_projectileObjects.Clear();
                m_projectileObjects.AddRange(projectileObjects);

                // send the list to the game controller
                List<Projectile> projectiles = new List<Projectile>();
                foreach(GameObject projectileObject in m_projectileObjects)
                {
                    projectiles.Add(projectileObject.GetComponent<Projectile>());
                }

                m_gameController.GetProjectilesList(projectiles);
            }
        }

        private void UpdatePlayerHealth()
        {
            // Reset player's previous damage
            PlayerHits = 0;

            GameObject[] projectiles = GameObject.FindGameObjectsWithTag(m_projectileTag);

            foreach(GameObject projectile in projectiles)
            {
                Projectile proj = projectile.GetComponent<Projectile>();
                if (proj == null)
                {
                    continue;
                }

                bool playerWasHit = proj.TravelTimeElapsed;
                if(playerWasHit)
                {
                    player.AdjustHealth();
                    ++PlayerHits;
                }
            }

            if(player.Health < 1)
            {
                m_gameController.StopGame();
                EndGame();
            }
        }

        private void UpdateWhackedObjects()
        {
            foreach (GameObject projectileObject in m_projectileObjects)
            {
                if (projectileObject == null)
                {
                    continue;
                }

                bool hit = projectileObject.GetComponent<Projectile>().Hit;
                int health = projectileObject.GetComponent<Projectile>().Health;
                if (hit)
                {
                    player.ObjectWasHit(true);
                    return;
                }
            }

            foreach (Mole mole in m_moles)
            {
                if (mole.Hit)
                {
                    player.ObjectWasHit(true);
                    return;
                }
            }
        }

        private void UpdateProjectileWasWhacked()
        {
            if (DisplayGameResults || DisplayDefeatedGameResults || player.HitCollisionId == -1)
            {
                return;
            }

            // check if any existing projectiles were hit by player
            GameObject[] projectiles = GameObject.FindGameObjectsWithTag(m_projectileTag);
            foreach (GameObject projectile in projectiles)
            {
                if(projectile.GetComponent<Collider2D>() == null)
                {
                    continue;
                }

                int hitCollision2dId = projectile.GetComponent<Collider2D>().GetInstanceID();
                if (player.HitCollisionId == hitCollision2dId)
                {
                    Debug.Log("HIT " + projectile.GetComponent<Collider2D>().tag + " | " + hitCollision2dId);
                    //projectile.GetComponent<Projectile>().DecrementHealth();
                    break;
                }
            }
        }

        private void UpdateMoleWasWhacked()
        {
            if (DisplayGameResults || DisplayDefeatedGameResults || player.HitCollisionId == -1)
            {
                return;
            }

            foreach (Mole mole in m_moles)
            {
                if (mole.GetComponent<Collider2D>() == null)
                {
                    continue;
                }

                int hitCollision2dId = mole.GetComponent<Collider2D>().GetInstanceID();
                // TODO: My God, I'm not liking this one bit - figure out different way of doing this
                if (player.HitCollisionId == hitCollision2dId && mole.moleController.InjuredAnimFinished)
                {
                    Debug.Log("HIT " + mole.GetComponent<Collider2D>().tag + " | " + hitCollision2dId);
                    //mole.moleController.Hit = true;
                    break;
                }
            }
        }

        private void ClearPlayerHitCollision()
        {
            player.ClearHitCollisionId();
        }

        private void ClearObjectHit()
        {
            player.ClearObjectHit();
        }

        private void ClearMoleHit()
        {
            foreach (Mole mole in m_moles)
            {
                mole.ClearHit();
            }
        }

        private void UpdateGameController()
        {
            if (DisplayGameResults || DisplayDefeatedGameResults)
            {
                return;
            }

            m_gameController.Update();

        }

        private void UpdateStarsAchievements()
        {
            m_starController.UpdateStarsAchievements();
        }

        private void GameIsOver()
        {
            if(PlayerDefeated)
            {
                DisplayDefeatedGameResults = true;
                return;
            }

            DisplayGameResults = true;

            Save();
        }

        private void EndGame()
        {
            EndGameManager.Instance.RunEndGame(PlayerDefeated);
        }

        private void CheckEndGameFinished()
        {
            if(EndGameManager.Instance.IsEndGameTimeDone)
            {
                GameIsOver();
                return;
            }

            EndGameManager.Instance.Update();
        }

        private void Save()
        {
            if(m_dataSaved)
            {
                return;
            }

            StoreStarTypes();
            StoreStarHighestValues();

            // check for persisted needed stars
            List<LevelStarType> neededStarTypes = GetPersistedNeededStarTypes();
            int newAchievedStarCount = GetNewAchievedStarCount(neededStarTypes);

            // updating player lifetime stats with end game stats
            EndGameStatUpdate(newAchievedStarCount);
            StoreAchievedStars();

            // set level as completed
            SetLevelCompleted();
            UnlockNextLevel();

            StorePlayerStats();


            m_dataSaved = true;
        }

        private void StoreStarTypes()
        {
            LevelInfo levelInfo = m_levelManager.SelectedLevelInfo;
            for (int index = 0; index < Level.MAX_STARS; ++index)
            {
                DataIndex dataIndex = DataIndex.Star1Type + index;
                int starTypeValue = (int)levelInfo.LevelStarInfos[index].StarType;
                m_persistentManager.SetValue(m_zoneId, m_levelId, dataIndex, starTypeValue);
            }
        }

        private void StoreStarHighestValues()
        {
            LevelInfo levelInfo = m_levelManager.SelectedLevelInfo;
            // save the highest values of the stars received
            for (int n = 0; n < Level.MAX_STARS; ++n)
            {
                DataIndex starValueIndex = DataIndex.Star1TypeBest + n;
                LevelStarType starType = levelInfo.LevelStarInfos[n].StarType;
                int value = EvaluateBestValueForStar(starType);

                if (value < 0)
                {
                    continue;
                }

                m_persistentManager.SetValue(m_zoneId, m_levelId, starValueIndex, value);
            }
        }

        private void StoreAchievedStars()
        {
            // only store stars if they are achieved
            LevelInfo levelInfo = m_levelManager.SelectedLevelInfo;
            for (int index = 0; index < Level.MAX_STARS; ++index)
            {
                int starAchievedIndex = (int)DataIndex.Star1Achieved + index;
                int starAchieved = m_starController.StarAchieved(levelInfo.LevelStarInfos[index].StarType) ? 1 : 0;

                // check if the persisted star was achieved
                int persistedStarAchieved = PersistentManager.Instance.GetValue(m_zoneId, m_levelId, (DataIndex)starAchievedIndex);

                if (starAchieved == 1 || persistedStarAchieved == 1)
                {
                    PersistentManager.Instance.SetValue(m_zoneId, m_levelId, (DataIndex)starAchievedIndex, 1);
                }
            }
        }

        private void UnlockNextLevel()
        {
            // first level id starts with 2
            int firstLevelId = 2;
            int nextLevelId = m_levelId + 1;
            int nextZoneId = m_zoneId + 1;
            int levelIdMax = (nextZoneId * LevelZone.MAX_LEVELS) + firstLevelId;
            if (nextLevelId < levelIdMax)
            {
                PersistentManager.Instance.SetValue(m_zoneId, nextLevelId, DataIndex.Unlocked, 1);
            }
        }

        private void StorePlayerStats()
        {
            int playerKey = PersistentManager.PlayerKey;
            PersistentManager.Instance.SetValue(playerKey, playerKey, DataIndex.StarsCollected, player.playerController.StarsCollected);
            PersistentManager.Instance.SetValue(playerKey, playerKey, DataIndex.LifetimeScore, player.playerController.LifetimeScore);
            PersistentManager.Instance.SetValue(playerKey, playerKey, DataIndex.LifetimeWhacks, player.playerController.LifetimeWhacks);
            PersistentManager.Instance.SetValue(playerKey, playerKey, DataIndex.LifetimeWhackAttempts, player.playerController.LifetimeWhackAttempts);
        }

        private int EvaluateBestValueForStar(LevelStarType starType)
        {
            // The offset for the star type value in the DataIndex
            int indexOffset = 3;
            int starBestValue = 0;

            for(int index = 0; index < Level.MAX_STARS; ++index)
            {
                int starTypeIndex = index + (int)DataIndex.Star1Type;
                int persistedStarType = PersistentManager.Instance.GetValue(m_zoneId, m_levelId, (DataIndex)starTypeIndex);

                if (persistedStarType != (int)starType)
                {
                    continue;
                }

                int bestValueDataIndex = starTypeIndex + indexOffset;
                starBestValue = PersistentManager.Instance.GetValue(m_zoneId, m_levelId, (DataIndex)bestValueDataIndex);
                break;
            }

            // check if there is a new best value for the star type
            int currentStarStat = m_starController.GetStarStat(starType);
            if (starBestValue >= currentStarStat)
            {
                return starBestValue;
            }

            // new best value
            int newBestValue = currentStarStat;
            return newBestValue;
        }

        //private int EvaluateBestValueForStar(LevelStarType starType)
        //{
        //    int zoneId = (int)m_levelManager.SelectedLevelInfo.zoneId;
        //    int levelId = (int)m_levelManager.SelectedLevelInfo.levelId;
        //    LevelDataBlock block = PersistentManager.Instance.GetDataBlock(zoneId, levelId) as LevelDataBlock;

        //    if(block == null)
        //    {
        //        return -1;
        //    }

        //    // check the star type
        //    List<int> levelValues = block.GetValues();
        //    int first = (int)DataIndex.Star1Type;
        //    int last = (int)DataIndex.Star3Type + 1;
        //    int indexOffset = 3;  // The offset for the star type value in the DataIndex
        //    int starBestValue = 0;

        //    // get the best star value from persistence
        //    for (int n = first; n < last; ++n)
        //    {
        //        if(levelValues[n] != (int)starType)
        //        {
        //            continue;
        //        }

        //        starBestValue = levelValues[n + indexOffset];
        //        break;
        //    }

        //    // check if there is a new best value for the star type
        //    int currentStarStat = m_starController.GetStarStat(starType);
        //    if (starBestValue >= currentStarStat)
        //    {
        //        return starBestValue;
        //    }

        //    // new best value
        //    int newBestValue = currentStarStat;
        //    return newBestValue;
        //}

        private List<LevelStarType> GetPersistedNeededStarTypes()
        {
            int zoneId = (int)m_levelManager.SelectedLevelInfo.ZoneId;
            int levelId = (int)m_levelManager.SelectedLevelInfo.LevelIdNum;
            List<LevelStarType> neededStarTypes = new List<LevelStarType>();

            for (int index = 0; index < Level.MAX_STARS; ++index)
            {
                int neededStarIndex = index + (int)DataIndex.Star1Achieved;
                int neededStarTypeIndex = index + (int)DataIndex.Star1Type;
                LevelStarType neededType = (LevelStarType)m_persistentManager.GetValue(zoneId, levelId, (DataIndex)neededStarTypeIndex);

                if(m_persistentManager.GetValue(zoneId, levelId, (DataIndex)neededStarIndex) == 0)
                {
                    neededStarTypes.Add(neededType);
                }
            }

            return neededStarTypes;
        }

        private int GetNewAchievedStarCount(List<LevelStarType> neededStarTypes)
        {
            if(neededStarTypes == null)
            {
                return 0;
            }

            int newAchievedStarCount = 0;
            for (int index = 0; index < neededStarTypes.Count; ++index)
            {
                LevelStarType neededStar = neededStarTypes[index];
                if(m_starController.StarAchieved(neededStar))
                {
                    ++newAchievedStarCount;
                }
            }

            return newAchievedStarCount;
        }

        private void EndGameStatUpdate(int starsAchieved)
        {
            m_gameController.EndGameStatUpdate(starsAchieved);
        }

        private void SetLevelCompleted()
        {
            PersistentManager.Instance.SetValue(m_zoneId, m_levelId, DataIndex.Completed, 1);
        }

        #endregion
    }
}
