﻿using MegaMan.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MegaMan.Editor.Mediator
{
    public class StageAddedEventArgs : EventArgs
    {
        public StageLinkInfo Stage { get; set; }
    }
}
