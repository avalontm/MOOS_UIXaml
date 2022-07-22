using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Windows.Markup;
using System.Xaml;
using System.Xaml.Schema;

namespace XamlToCode.DOM
{
    [DebuggerDisplay("<{Type.Name}>")]
    [System.Windows.Markup.ContentProperty("MemberNodes")]
    public class ObjectNode : ItemNode, IXamlTypeResolver
    {
        [DefaultValue(null)]
        public XamlType Type { get; set; }

        public bool IsGetObject { get; set; }

        [DefaultValue(null)]
        public string XmlLang { get; set; }

        public string LookupNamespaceByPrefix(string prefix)
        {
            if (_NamespaceNodes != null && _NamespaceNodes.ContainsKey(prefix))
            {
                return _NamespaceNodes[prefix].Namespace;
            }
            else
            {
                if (this.ParentMemberNode != null)
                    return this.ParentMemberNode.LookupNamespaceByPrefix(prefix);
                else
                    return null;
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

        private NodeCollection<MemberNode> _PropertyNodes;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public NodeCollection<MemberNode> MemberNodes
        {
            get
            {
                if (_PropertyNodes == null)
                    _PropertyNodes = new NodeCollection<MemberNode>(this);
                return _PropertyNodes;
            }
        }

        private XamlSchemaContext _SchemaContext;
        public XamlSchemaContext SchemaContext
        {
            get
            {
                if (_SchemaContext != null)
                    return _SchemaContext;
                else
                    if (ParentMemberNode != null)
                        return ParentMemberNode.SchemaContext;
                    else
                        return null;
            }
            set
            {
                _SchemaContext = value;
            }
        }
        public Type Resolve(string qualifiedTypeName)
        {
            int colon = qualifiedTypeName.IndexOf(':');
            string prefix = "";
            if (colon > -1)
            {
                prefix = qualifiedTypeName.Substring(0, colon);
            }
            string xmlNs = this.LookupNamespaceByPrefix(prefix);
            if (xmlNs == null)
                return null;
            string typeName = qualifiedTypeName.Substring(colon + 1);

            XamlType referencedXamlType = SchemaContext.GetXamlType(new XamlTypeName(xmlNs, typeName));

            return (referencedXamlType == null || referencedXamlType.UnderlyingType == null)
                ? null : referencedXamlType.UnderlyingType;
        }
    }
}
