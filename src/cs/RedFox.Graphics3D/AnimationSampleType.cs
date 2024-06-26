﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedFox.Graphics3D
{
    /// <summary>
    /// A class to handle sampling an <see cref="Animation"/> at arbitrary frames or in a linear fashion.
    /// </summary>
    public enum AnimationSampleType
    {
        Percentage,
        AbsoluteFrameTime,
        AbsoluteTime,
        DeltaTime,
    }
}
