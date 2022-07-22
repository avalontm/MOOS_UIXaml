using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.ComponentModel;

namespace XamlToCode.DOM
{
    [DebuggerDisplay("{Value}")]
    public class ValueNode : ItemNode
    {
        [DefaultValue(null)]
        public object Value { get; set; }
    }
}
