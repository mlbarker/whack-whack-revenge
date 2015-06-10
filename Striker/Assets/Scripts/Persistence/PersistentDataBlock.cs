//-----------------------------
// ImperfectlyCoded © 2015
//-----------------------------

namespace Assets.Scripts.Persistence
{
    using System;

    public class PersistentDataBlock
    {
        #region Fields

        public const int INVALID = -9999;

        #endregion

        #region Public Properties

        public int ValueInt
        {
            get;
            private set;
        }

        public string ValueString
        {
            get;
            private set;
        }

        public string Key
        {
            get;
            private set;
        }

        #endregion

        #region Constructors

        private PersistentDataBlock()
        {
        }

        public PersistentDataBlock(string key, int value)
        {
            Key = key;
            ValueInt = value;
            ValueString = string.Empty;
        }

        public PersistentDataBlock(string key, string value)
        {
            Key = key;
            ValueString = value;
            ValueInt = INVALID;
        }

        #endregion
    }
}