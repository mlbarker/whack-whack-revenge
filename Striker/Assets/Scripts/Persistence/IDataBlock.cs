//-----------------------------
// ImperfectlyCoded © 2015
//-----------------------------

namespace Assets.Scripts.Persistence
{
    using System.Collections.Generic;

    public interface IDataBlock
    {
        void StoreValues(DataIndex dataIndex, int value);
        List<int> GetValues();
    }
}
