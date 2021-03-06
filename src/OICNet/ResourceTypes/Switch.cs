﻿using System;
using System.Collections.Generic;
using System.Text;

using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace OICNet.ResourceTypes
{
    [OicResourceType("oic.r.switch.binary")]
    public class SwitchBinary : OicBaseResouece<bool>
    {
        public SwitchBinary()
            : base(OicResourceInterface.Baseline | OicResourceInterface.Actuator, "oic.r.switch.binary")
        { }
    }
}
