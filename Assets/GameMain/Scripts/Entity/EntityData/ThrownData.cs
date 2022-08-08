using System;
using UnityEngine;

namespace NetworkBasedFPS
{
    [Serializable]
    public class ThrownData : AccessoryObjectData
    {
        public ThrownData(int entityId, int typeId, int ownerId, CampType ownerCamp)
            : base(entityId, typeId, ownerId, ownerCamp)
        {

        }
    }
}
