using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xaml;
using XamlToCode.DOM;

namespace XamlToCode
{
    public class CodeDomDomWriter : XamlWriter
    {
        public CodeDomDomWriter(XamlSchemaContext schemaContext)
        {
            _schemaContext = schemaContext;
        }
        Stack<DomNode> writerStack = new Stack<DomNode>();

        #region XamlWriter Members
        public ObjectNode RootNode = null;
        XamlSchemaContext _schemaContext;

        public override void WriteGetObject()
        {
            WriteObject(null, true);
        }

        public override void WriteStartObject(XamlType xamlType)
        {
            WriteObject(xamlType, false);
        }

        void WriteObject(XamlType xamlType, bool isGetObject)
        {
            CodeDomObjectNode objectNode = null;
            MemberNode propertyNode = writerStack.Peek() as MemberNode;

            if (writerStack.Count > 0)
            {
                objectNode = writerStack.Peek() as CodeDomObjectNode;
                if (!(objectNode != null && objectNode.NamespaceNodes.Count > 0))
                {
                    objectNode = new CodeDomObjectNode();
                    writerStack.Push(objectNode);
                }
                else
                {
                    //root node
                    objectNode.SchemaContext = SchemaContext;
                }
            }
            else
            {
                //root node
                objectNode = new CodeDomObjectNode();
                objectNode.SchemaContext = SchemaContext;
                writerStack.Push(objectNode);
            }
            objectNode.Type = xamlType;
            objectNode.IsGetObject = isGetObject;

            if (RootNode != null)
            {
                propertyNode.ItemNodes.Add(objectNode);
            }
            else
                RootNode = objectNode;
        }

        public override void WriteEndObject()
        {
            writerStack.Pop();
        }

        public override void WriteStartMember(XamlMember property)
        {
            MemberNode propertyNode = new MemberNode();
            propertyNode.Member = property;

            CodeDomObjectNode objectNode = (CodeDomObjectNode)writerStack.Peek();

            writerStack.Push(propertyNode);

            if (property == XamlLanguage.Name || (property.DeclaringType != null && property == property.DeclaringType.GetAliasedProperty(XamlLanguage.Name)))
            {
                objectNode.XNameNode = propertyNode;
            }
            else if (property == XamlLanguage.Class)
            {
                objectNode.XClassNode = propertyNode;
            }
            else if (property == XamlLanguage.Initialization)
            {
                objectNode.XInitNode = propertyNode;
            }
            else if (property == XamlLanguage.PositionalParameters)
            {
                objectNode.XPosParamsNode = propertyNode;
            }
            else if (property == XamlLanguage.FactoryMethod)
            {
                objectNode.XFactoryMethodNode = propertyNode;
            }
            else if (property == XamlLanguage.Arguments)
            {
                objectNode.XArgumentsNode = propertyNode;
            }
            else if (property == XamlLanguage.Key)
            {
                objectNode.XKeyNode = propertyNode;
            }
            else
            {
                if (property.DeclaringType != null && property == property.DeclaringType.GetAliasedProperty(XamlLanguage.Key))
                {
                    objectNode.DictionaryKeyProperty = propertyNode;
                }
                objectNode.MemberNodes.Add(propertyNode);
            }
        }

        public override void WriteEndMember()
        {
            writerStack.Pop();
        }

        public override void WriteValue(object value)
        {
            ValueNode valueNode = new ValueNode();
            valueNode.Value = value;

            //text should always be inside of a property...
            MemberNode propertyNode = (MemberNode)writerStack.Peek();
            propertyNode.ItemNodes.Add(valueNode);
        }

        public override void WriteNamespace(NamespaceDeclaration namespaceDeclaration)
        {
            CodeDomObjectNode objectNode = null;
            if (writerStack.Count == 0)
            {
                objectNode = new CodeDomObjectNode();
                writerStack.Push(objectNode);
            }
            else
            {
                objectNode = writerStack.Peek() as CodeDomObjectNode;
                if (objectNode.Type != null)
                {
                    objectNode = new CodeDomObjectNode();
                    writerStack.Push(objectNode);
                }
            }
            objectNode.NamespaceNodes.Add(namespaceDeclaration.Prefix, namespaceDeclaration);
        }

        public object Result { get { return RootNode; } }

        public override XamlSchemaContext SchemaContext
        {
            get
            {
                return _schemaContext;
            }
            //set
            //{
            //    //base.CheckSettingSchemaContext(_schemaContext, value);
            //    _schemaContext = value;
            //}
        }

        #endregion
    }
}
