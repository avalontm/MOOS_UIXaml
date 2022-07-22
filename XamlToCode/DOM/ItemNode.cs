using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace XamlToCode.DOM
{
    public abstract class ItemNode : DomNode
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [DefaultValue(null)]
        public MemberNode ParentMemberNode { get; set; }
    }
}
