using System;
using System.Collections.Generic;
using System.Text;
using System.Xaml;
using System.ComponentModel;
using System.Diagnostics;

namespace XamlToCode.DOM
{
    [DebuggerDisplay("{Member.Name}")]
    [System.Windows.Markup.ContentProperty("ItemNodes")]
    public class MemberNode : DomNode
    {
        [DefaultValue(null)]
        public XamlMember Member { get; set; }
        [DefaultValue(null)]
        public string Prefix { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ObjectNode ParentObjectNode { get; set; }

        private NodeCollection<ItemNode> _ValueNodes;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public NodeCollection<ItemNode> ItemNodes
        {
            get
            {
                if (_ValueNodes == null)
                    _ValueNodes = new NodeCollection<ItemNode>(this);
                return _ValueNodes;
            }
        }

        private Dictionary<string, NamespaceDeclaration> _NamespaceNodes;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Dictionary<string, NamespaceDeclaration> NamespaceNodes
        {
            get
            {
                if (_NamespaceNodes == null)
                    _NamespaceNodes = new Dictionary<string, NamespaceDeclaration>();
                return _NamespaceNodes;
            }
        }

        internal string LookupNamespaceByPrefix(string prefix)
        {
            if (_NamespaceNodes != null && _NamespaceNodes.ContainsKey(prefix))
            {
                return _NamespaceNodes[prefix].Namespace;
            }
            else
            {
                return this.ParentObjectNode.LookupNamespaceByPrefix(prefix);
            }
        }

        public XamlSchemaContext SchemaContext
        {
            get
            {
                return ParentObjectNode.SchemaContext;
            }
        }
    }
}
