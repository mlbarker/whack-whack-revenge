//-----------------------------
// ImperfectlyCoded © 2015
//-----------------------------

namespace Assets.Scripts.Persistence
{
    public enum PersistentBitMask
    {
        LevelStar3Shift     = 0,    //0x1
        LevelStar3TypeShift = 1,    //0x2

        LevelStar2Shift     = 4,    //0x10
        LevelStar2TypeShift = 5,    //0x20

        LevelStar1Shift     = 8,    //0x100
        LevelStar1TypeShift = 9,    //0x200

        LevelIdShift        = 12,   //0x1000
        LevelLockShift      = 19    //0x80000
    }
}