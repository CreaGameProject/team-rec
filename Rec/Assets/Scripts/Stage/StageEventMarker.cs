using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Stage
{
    public abstract class StageEventMarker: MonoBehaviour
    {
        public abstract IStageEvent ToStageEvent();
    }
}
