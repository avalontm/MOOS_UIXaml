using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Media;
using System.Xaml;
using Microsoft.CSharp;
using Microsoft.VisualBasic;
using x2006 = System.Windows.Markup;
using System.Xml;
using System.Diagnostics;
using XamlToCode.DOM;
using System.Windows.Input;
using System.Windows;

namespace XamlToCode
{
    public class XamlToCodeConverter
    {
        private CodeTypeDeclaration RootObject { get; set; }
        private Dictionary<Type, string> TypeConverterDictionary { get; set; }
        private const String CultureInfoString = "EnglishCultureInfo";
        private SortedDictionary<string, object> NamespacesToUse;
        private int objectNumber;
        private XamlSchemaContext _schemaContext;
        private CodeDomProvider cscProvider;
        private CodeCompileUnit ccu;
        private Dictionary<string, CodeDomObjectNode> PublicObjects { get; set; }

        public string MainCodeClassName { get; set; }

        public string Convert(string Xaml)
        {
            using (StringReader sr = new StringReader(Xaml))
            {
                using (XmlTextReader reader = new XmlTextReader(sr))
                {
                    using (XamlXmlReader xmlReader = new XamlXmlReader(reader))
                    {
                        return Convert(xmlReader);
                    }
                }
            }
        }

        public string Convert(XamlReader reader)
        {
            /*
            // Select the language provider based on the users choice
            switch (Properties.Settings.Default.TargetLanguage)
            {
                case "CSharp":
                    cscProvider = new CSharpCodeProvider();
                    break;
                case "VB":
                    cscProvider = new VBCodeProvider();
                    break;
            }
            */
            cscProvider = new CSharpCodeProvider();

            return Convert(reader, cscProvider);
        }

        public string Convert(XamlReader reader, CodeDomProvider cscProvider)
        {
            MainCodeClassName = string.Empty;
            StringBuilder sb = new StringBuilder();
            StringWriter stringWriter = new StringWriter(sb);
            NamespacesToUse = new SortedDictionary<string, object>();

            // TODO: XamlXmlReader can sometimes be null, if there is misformed XML in the first sections of XML
            _schemaContext = reader != null ? reader.SchemaContext : new XamlSchemaContext();
            CodeDomDomWriter codeDomDomWriter = new CodeDomDomWriter(_schemaContext);

            // Load XAML into a specialized XAML DOM, for analysis and processing
            Debug.WriteLine("Building codeDOM from XAML...");
            while (reader.Read())
            {
                codeDomDomWriter.WriteNode(reader);
            }
            Debug.WriteLine("codeDOM complete.");
            CodeDomObjectNode objectNode = (CodeDomObjectNode)codeDomDomWriter.Result;

            //DumpNodeTree(objectNode);

            // Initialize CodeDom constructs
            ICodeGenerator cscg = cscProvider.CreateGenerator(stringWriter);
            ccu = new CodeCompileUnit();
            CodeNamespace cns = new CodeNamespace();
            ccu.Namespaces.Add(cns);

            // Go process XAML DOM
            CodeMemberMethod initMethod = CreateInitializeMethod(cns, objectNode);
            GenerateUsings(cns, objectNode);
            CreateClass(cns, objectNode, initMethod);
            AddPublicObjectMembers();

            // Create code from codeDOM
            cscg.GenerateCodeFromCompileUnit(ccu, stringWriter, new CodeGeneratorOptions());
            string returnText = sb.ToString();

            return returnText;
        }

        /// <summary>
        /// Compiles the assembly from the last code compile unit generated using the language compiler provided
        /// </summary>
        public CompilerResults CompileAssemblyFromLastCodeCompileUnit()
        {
            // Compile the code
            CompilerParameters comparam = new CompilerParameters(new string[] { "mscorlib.dll", "System.dll", "System.Core.dll"});
            comparam.GenerateInMemory = true;

            // Add all the required referenced assemblies
            // TODO: Should look these up dynamically
            comparam.ReferencedAssemblies.Add(@"C:\Program Files\Reference Assemblies\Microsoft\Framework\v3.0\PresentationCore.dll");
            comparam.ReferencedAssemblies.Add(@"C:\Program Files\Reference Assemblies\Microsoft\Framework\v3.0\PresentationFramework.dll");
            comparam.ReferencedAssemblies.Add(@"C:\Program Files\Reference Assemblies\Microsoft\Framework\v3.0\WindowsBase.dll");

            CompilerResults compres = cscProvider.CompileAssemblyFromDom(comparam, ccu);

            return compres;
        }

        private void GenerateUsings(CodeNamespace cns, CodeDomObjectNode objectNode)
        {
            cns.Imports.Add(new CodeNamespaceImport("System"));
            cns.Imports.Add(new CodeNamespaceImport("System.Windows.Input"));
            foreach (string namespaceName in NamespacesToUse.Keys)
            {
                cns.Imports.Add(new CodeNamespaceImport(namespaceName));
            }
        }

        private void CreateClass(CodeNamespace cns, CodeDomObjectNode objectNode, CodeMemberMethod initComponentMethod)
        {
            CodeTypeDeclaration rootType;
            if (objectNode.XClassNode == null)
            {
                rootType = new CodeTypeDeclaration("MOOS" + objectNode.Type.Name);
            }
            else
            {
                rootType = new CodeTypeDeclaration((String)((ValueNode)objectNode.XClassNode.ItemNodes[0]).Value);
            }
            // Keep a copy
            MainCodeClassName = rootType.Name;

            rootType.BaseTypes.Add(new CodeTypeReference(objectNode.Type.UnderlyingType));
            rootType.IsPartial = true;
            rootType.Attributes = MemberAttributes.Public;
            cns.Types.Add(rootType);
            rootType.Members.Add(initComponentMethod);

            CodeMemberField cmf = new CodeMemberField(typeof(CultureInfo), CultureInfoString);
            cmf.InitExpression = new CodeObjectCreateExpression(typeof(CultureInfo), new CodeSnippetExpression("\"en-us\""), new CodePrimitiveExpression(false));

            rootType.Members.Add(cmf);

            RootObject = rootType;
        }

        private void AddPublicObjectMembers()
        {
            // Type, Name
            foreach (string targetName in PublicObjects.Keys)
            {
                CodeDomObjectNode targetObjectNode = PublicObjects[targetName];
                if (targetObjectNode.Type.IsUnknown)
                {
                    throw new Exception("Unknown type " + targetObjectNode.Type.Name + " found.");
                }
                CodeMemberField cmf = new CodeMemberField(targetObjectNode.Type.UnderlyingType, targetName);
                cmf.Attributes = MemberAttributes.Public;

                RootObject.Members.Add(cmf);
            }
        }

        private CodeMemberMethod CreateInitializeMethod(CodeNamespace cns, CodeDomObjectNode objectNode)
        {
            TypeConverterDictionary = new Dictionary<Type, string>();
            PublicObjects = new Dictionary<string, CodeDomObjectNode>();

            CodeMemberMethod initComponentMethod = new CodeConstructor();
            initComponentMethod.Attributes = MemberAttributes.Public;

            CodeThisReferenceExpression thisExpression = new CodeThisReferenceExpression();

            AddMembers(initComponentMethod, thisExpression, objectNode);
            return initComponentMethod;
        }

        private void AddMembers(CodeMemberMethod initComponentMethod, CodeExpression targetExpression, CodeDomObjectNode objectNode)
        {
            NodeCollection<MemberNode> members = objectNode.MemberNodes;
            foreach (MemberNode member in members)
            {
                if (member.Member.Name == "Implementation")
                {
                    GenerateImplementation(initComponentMethod, member, targetExpression);
                }
                else
                {
                    GenerateMemberValue(initComponentMethod, member, targetExpression);
                }
            }
        }

        private void GenerateImplementation(CodeMemberMethod initComponentMethod, MemberNode member, CodeExpression targetExpression)
        {
            foreach (var node in member.ItemNodes)
            {
                ProcessItemNode(node, initComponentMethod, member, targetExpression);
            }
        }

        private void GenerateMemberValue(CodeMemberMethod initComponentMethod,
            MemberNode member, CodeExpression targetExpression)
        {
            if (member.Member.IsUnknown)
            {
                //throw new Exception("Unknown member " + member.Member.Name);
                return;
            }
            if (member.Member == XamlLanguage.Class ||
                member.Member == XamlLanguage.Initialization ||
                member.Member == XamlLanguage.PositionalParameters ||
                member.Member == XamlLanguage.Arguments ||
                member.Member == XamlLanguage.FactoryMethod)
            {
                return;
            }
            else
            {
                foreach (ItemNode itemNode in member.ItemNodes)
                {
                    ValueNode valueNode = itemNode as ValueNode;
                    if (valueNode != null)
                    {
                        string value = valueNode.Value as String;
                        if (value != null)
                        {
                            if (member.Member.IsUnknown)
                            {
                                throw new Exception("Code can not be generated for unknown member: " + member.Member.Name);
                            }
                            bool shouldConvert = ShouldUseTypeConverter(value, member.Member.Type.UnderlyingType);
                            if (shouldConvert)
                            {
                                string tcName = GenerateTypeConverter(initComponentMethod, member.Member.Type);   //TextSyntax

                                GenerateMemberAssignment(initComponentMethod,
                                    member,
                                    targetExpression,
                                    GetTypeConverteredValue(member.Member.Type, value, member.Member.Type.UnderlyingType, tcName),
                                    null);
                            }
                            else
                            {
                                GenerateMemberAssignment(initComponentMethod,
                                       member,
                                       targetExpression,
                                       GenerateExpressionForString(value, member.Member.Type.UnderlyingType),
                                       null);
                            }

                        }
                        else
                        {
                            //We shouldn't have this happen.  XXR should only give us real numbers.
                            throw new NotImplementedException();
                        }
                    }
                    else
                    {
                        ProcessItemNode(itemNode, initComponentMethod, member, targetExpression);
                    }
                }
            }
        }

        private void ProcessItemNode(ItemNode itemNode, CodeMemberMethod initComponentMethod,
            MemberNode member, CodeExpression targetExpression)
        {
            CodeDomObjectNode objectNode = (CodeDomObjectNode)itemNode;
            XamlType xamlType = objectNode.Type;
            if (xamlType == XamlLanguage.Static)
            {
                //TODO: this could be posParams or named params...                            
                ValueNode valueNode2 = objectNode.XPosParamsNode.ItemNodes[0] as ValueNode;
                string xamlTypeReference = valueNode2.Value as string;
                string memberName = null;
                string typeName = null;
                int period = xamlTypeReference.IndexOf('.');
                if (period > -1)
                {
                    memberName = xamlTypeReference.Substring(period + 1);
                    typeName = xamlTypeReference.Substring(0, period);
                }
                Type resolvedType = objectNode.Resolve(typeName);
                //TODO: don't forget to make sure a using happens for the referencedXamlType
                string typeName2 = resolvedType != null ? resolvedType.Name : xamlTypeReference;
                //CodeTypeReference ctr = new CodeTypeReference(typeName2);
                CodeTypeReferenceExpression ctre = new CodeTypeReferenceExpression(typeName2);
                CodePropertyReferenceExpression cpre =
                    new CodePropertyReferenceExpression(ctre, memberName);

                GenerateMemberAssignment(initComponentMethod,
                                         member, targetExpression, cpre, objectNode);
            }
            else if (xamlType == XamlLanguage.Null)
            {
                CodePrimitiveExpression nullExpression = new CodePrimitiveExpression(null);
                GenerateMemberAssignment(initComponentMethod,
                                         member, targetExpression, nullExpression, objectNode);
            }
            else if (xamlType == XamlLanguage.Type)
            {
                //TODO: this could be posParams or named params...                            
                ValueNode valueNode2 = objectNode.XPosParamsNode.ItemNodes[0] as ValueNode;
                string xamlTypeReference = valueNode2.Value as string;
                Type resolvedType = objectNode.Resolve(xamlTypeReference);
                //TODO: don't forget to make sure a using happens for the referencedXamlType
                string typeName = resolvedType != null ? resolvedType.Name : xamlTypeReference;
                CodeTypeReference ctr = new CodeTypeReference(typeName);
                CodeTypeOfExpression typeof1 = new CodeTypeOfExpression(ctr);
                GenerateMemberAssignment(initComponentMethod,
                                member, targetExpression, typeof1, objectNode);
            }
            else if (xamlType == XamlLanguage.Reference)
            {

            }
            else
            {
                // We have something other than a ValueNode.  Must be a StartObject or GetObject
                initComponentMethod.Statements.Add(new CodeCommentStatement("---------------------------"));
                if (objectNode.Type != null)
                {
                    // Handle special case for Bindings
                    if (objectNode.Type.Name == "Binding")
                    {
                        GenerateBindingObject(initComponentMethod, targetExpression, objectNode, member);
                    }
                    else
                    {
                        // StartObject case
                        GenerateObject(initComponentMethod, targetExpression, objectNode, member);
                    }
                }
                else
                {
                    // GetObject
                    GenerateGetObject(initComponentMethod, targetExpression, objectNode, member);
                }
            }
        }

        //  MyData myDataObject = new MyData(DateTime.Now);      
        //  Binding myBinding = new Binding("MyDataProperty");
        //  myBinding.Source = myDataObject;
        //  myText.SetBinding(TextBlock.TextProperty, myBinding);

        private void GenerateBindingObject(CodeMemberMethod initComponentMethod, CodeExpression parentExpression, CodeDomObjectNode targetObjectNode, MemberNode parentMember)
        {
            // Generate new binding name
            string targetName = GenerateUniqueName(targetObjectNode.Type);
            string elementName = string.Empty;
            string path = string.Empty;

            // Grab all the attributes of the binding
            NodeCollection<MemberNode> members = targetObjectNode.MemberNodes;
        
            foreach (MemberNode member in members)
            {
                switch (member.Member.Name)
                {
                    case "ElementName":
                        elementName = ExtractItemMemberValue(member.ItemNodes[0]);
                        break;
                    case "Path":
                        path = ExtractItemMemberValue(member.ItemNodes[0]);
                        break;
                }
            }
  
            // Create a constructor for the binding adding the Path, if it exists
            CodeExpression ctor = new CodeObjectCreateExpression(targetObjectNode.Type.UnderlyingType.Name, new CodePrimitiveExpression(path));
            CodeVariableDeclarationStatement cvds = new CodeVariableDeclarationStatement(targetObjectNode.Type.UnderlyingType.Name, targetName, ctor);
            initComponentMethod.Statements.Add(cvds);

            // Set the source property on the binding
            CodeStatement cs = new CodeAssignStatement(new CodePropertyReferenceExpression(new CodeVariableReferenceExpression(targetName), "Source"), new CodeVariableReferenceExpression(elementName));
            initComponentMethod.Statements.Add(cs);

            // Set the binding on the target object
            CodeFieldReferenceExpression cfre = new CodeFieldReferenceExpression(new CodeTypeReferenceExpression(parentMember.ParentObjectNode.Type.UnderlyingType), parentMember.Member.Name + "Property");

            CodeMethodInvokeExpression cmie = new CodeMethodInvokeExpression(parentExpression, "SetBinding", new CodeExpression[] { cfre, new CodeVariableReferenceExpression(targetName) });
            initComponentMethod.Statements.Add(cmie);
        }

        private string ExtractItemMemberValue(ItemNode node)
        {
            if (node is ValueNode)
            {
                return ((ValueNode)node).Value.ToString ();
            }

            return string.Empty;
        }

        private void GenerateGetObject(CodeMemberMethod initComponentMethod, CodeExpression parentExpression, CodeDomObjectNode targetObjectNode, MemberNode parentMember)
        {
            if (parentMember.Member.IsAttachable)
            {
                throw new NotImplementedException();
            }
            CodePropertyReferenceExpression cpfe = new CodePropertyReferenceExpression(parentExpression, parentMember.Member.Name);
            Type type = parentMember.Member.Type.UnderlyingType;
            string targetName = GenerateUniqueName(type);
            CodeVariableDeclarationStatement cvds = new CodeVariableDeclarationStatement(type.Name, targetName, cpfe);
            initComponentMethod.Statements.Add(cvds);

            CodeVariableReferenceExpression cvre = new CodeVariableReferenceExpression(targetName);
            AddMembers(initComponentMethod, cvre, targetObjectNode);
        }

        private void GenerateObject(CodeMemberMethod initComponentMethod, CodeExpression parentExpression, CodeDomObjectNode targetObjectNode, MemberNode parentMember)
        {
            string targetName;
            bool isPublicObject = false;
            if (targetObjectNode.XNameNode == null)
                targetName = GenerateUniqueName(targetObjectNode.Type);
            else
            {
                ValueNode valueNode = targetObjectNode.XNameNode.ItemNodes[0] as ValueNode;
                targetName = (string)valueNode.Value;
                isPublicObject = true;
            }
            CodeExpression ctor = null;
            if (targetObjectNode.XInitNode != null)
            {
                string tcName = GenerateTypeConverter(initComponentMethod, targetObjectNode.Type);   // TextSyntax
           
                ctor = GetTypeConverteredValue(targetObjectNode.Type, ((ValueNode)targetObjectNode.XInitNode.ItemNodes[0]).Value, targetObjectNode.Type.UnderlyingType, tcName);
            }
            else if (targetObjectNode.XFactoryMethodNode != null)
            {
                throw new NotImplementedException();
            }
            else if (targetObjectNode.XPosParamsNode != null)
            {
                IList<XamlType> types = targetObjectNode.Type.GetPositionalParameters(targetObjectNode.XPosParamsNode.ItemNodes.Count);
                CodeObjectCreateExpression constructor = new CodeObjectCreateExpression(targetObjectNode.Type.UnderlyingType.Name);
                for (int i = 0; i < types.Count; i++)
                {
                    string tcName = GenerateTypeConverter(initComponentMethod, types[i]);
                
                    constructor.Parameters.Add(GetTypeConverteredValue(types[i], ((ValueNode)targetObjectNode.XPosParamsNode.ItemNodes[i]).Value, types[i].UnderlyingType, tcName));
                }
                ctor = constructor;
            }
            else if (targetObjectNode.XArgumentsNode != null)
            {
                throw new NotImplementedException();
            }
            else
            {
                if (targetObjectNode.Type.IsUnknown)
                    throw new Exception("Unknown type " + targetObjectNode.Type.Name + " found.");
                ctor = new CodeObjectCreateExpression(targetObjectNode.Type.UnderlyingType.Name);
            }

            if (!isPublicObject)
            {
                CodeVariableDeclarationStatement cvds = new CodeVariableDeclarationStatement(targetObjectNode.Type.UnderlyingType.Name, targetName, ctor);
                initComponentMethod.Statements.Add(cvds);
            }
            else
            {
                CodeAssignStatement cas = new CodeAssignStatement(new CodeVariableReferenceExpression(targetName), ctor);
                initComponentMethod.Statements.Add(cas);

                // Needs to be public
                PublicObjects.Add(targetName, targetObjectNode);
            }

            CodeVariableReferenceExpression cvre = new CodeVariableReferenceExpression(targetName);

            bool isUsuable = GetIsUsableDuringInitialization(targetObjectNode.Type);

            if (isUsuable)
            {
                GenerateMemberAssignmentWithProvideValueIfNecessary(initComponentMethod, parentExpression, targetObjectNode, parentMember, cvre);
            }

            AddMembers(initComponentMethod, cvre, targetObjectNode);

            if (!isUsuable)
            {
                GenerateMemberAssignmentWithProvideValueIfNecessary(initComponentMethod, parentExpression, targetObjectNode, parentMember, cvre);
            }
        }

        private void GenerateMemberAssignmentWithProvideValueIfNecessary(CodeMemberMethod initComponentMethod, CodeExpression parentExpression, CodeDomObjectNode targetObjectNode,
            MemberNode parentMember, CodeVariableReferenceExpression cvre)
        {
            //if (parentMember.Member.IsUnknown)
            //{
            //    throw new Exception("Unknown member " + parentMember.Member.Name);
            //}
            XamlType xamlType = targetObjectNode.Type;
            if (xamlType.IsMarkupExtension)
            {
                //call ProvideValue on the ME
                GenerateMemberAssignment(initComponentMethod, parentMember, parentExpression,
                    new CodeMethodInvokeExpression(cvre, "ProvideValue", new CodeExpression[] { 
                        new CodeVariableReferenceExpression("context") 
                    })
                    , targetObjectNode);
            }
            else
            {
                GenerateMemberAssignment(initComponentMethod, parentMember, parentExpression, cvre, targetObjectNode);
            }
        }

        private void GenerateMemberAssignment(CodeMemberMethod initComponentMethod, MemberNode member, CodeExpression targetExpression, CodeExpression valueExpression, CodeDomObjectNode targetObjectNode)
        {
            CodeStatement cs = null;

            //if (member.Member.IsUnknown)
            //{
            //    throw new Exception("Unknown member " + member.Member.Name);
            //}
            if (member.Member == XamlLanguage.Items)
            {
                ObjectNode parentObjectNode = member.ParentObjectNode;
                XamlType parentType = null;
                if (parentObjectNode.IsGetObject)
                {
                    parentType = parentObjectNode.ParentMemberNode.Member.Type;
                }
                else
                {
                    parentType = parentObjectNode.Type;
                }
                if (parentType.IsDictionary)
                {
                    if (!typeof(IDictionary).IsAssignableFrom(parentType.UnderlyingType))
                    {
                        throw new NotImplementedException("Support non-IDictionary adds");
                    }
                    CodeExpression keyExpression;
                    if (targetObjectNode.XKeyNode != null)
                    {
                        keyExpression = new CodeSnippetExpression("\"" + ((ValueNode)targetObjectNode.XKeyNode.ItemNodes[0]).Value + "\"");
                    }
                    else
                    {
                        if (targetObjectNode.DictionaryKeyProperty == null)
                        {
                            throw new NotSupportedException("No key on dictionary entry");
                        }
                        throw new NotImplementedException();
                    }
                    cs = new CodeExpressionStatement(new CodeMethodInvokeExpression(new CodeCastExpression(typeof(IDictionary), targetExpression), "Add", keyExpression, valueExpression));
                }
                else
                {
                    if (!typeof(IList).IsAssignableFrom(parentType.UnderlyingType))
                    {
                        throw new NotImplementedException("Support non-IList adds");
                    }
                    //TODO: calling Add directly is how I'll leave it for now...
                    //cs = new CodeExpressionStatement(new CodeMethodInvokeExpression(new CodeCastExpression(typeof(IList), targetExpression), "Add", valueExpression));
                    cs = new CodeExpressionStatement(new CodeMethodInvokeExpression(targetExpression, "Add", valueExpression));
                }
            }
            else if (member.Member.IsEvent)
            {
                 throw new NotImplementedException();
               //cs = new CodeAssignStatement(new CodePropertyReferenceExpression(targetExpression, member.Member.Name), valueExpression);
            }
            else
            {
                if (member.Member.IsAttachable)
                {
                    cs = new CodeExpressionStatement(new CodeMethodInvokeExpression(
                            new CodeMethodReferenceExpression(new CodeTypeReferenceExpression(member.Member.DeclaringType.UnderlyingType.Name),
                                                            "Set" + member.Member.Name), targetExpression, valueExpression));
                }
                else //normal property
                {
                    cs = new CodeAssignStatement( new CodePropertyReferenceExpression(targetExpression, member.Member.Name), valueExpression);
                }
            }
            initComponentMethod.Statements.Add(cs);
        }


        private bool ShouldUseTypeConverter(string value, Type type)
        {
            if (type == typeof(String))
            {
                return false;
            }
            else if (type == typeof(Double))
            {
                double doubleValue;
                bool isDouble = Double.TryParse(value, out doubleValue);
                if (isDouble)
                {
                    return false;
                }
            }
            else if (type == typeof(Int32))
            {
                int intValue;
                bool isInt = Int32.TryParse(value, out intValue);
                if (isInt)
                {
                    return false;
                }
            }
            else if (type == typeof(Boolean))
            {
                bool boolValue;
                bool isBool = Boolean.TryParse(value, out boolValue);
                if (isBool)
                {
                    return false;
                }
            }
            else if (type == typeof(Brush))
            {
                PropertyInfo propertyInfo = typeof(Brushes).GetProperty(value,
                    BindingFlags.IgnoreCase | BindingFlags.Static | BindingFlags.Public);
                if (propertyInfo != null)
                    return false;
            }
            else if (type == typeof(Color))
            {
                PropertyInfo propertyInfo = typeof(Colors).GetProperty(value);
                if (propertyInfo != null)
                    return false;
            }
            else if (type.IsEnum)
            {
                FieldInfo fieldInfo = type.GetField(value);
                if (fieldInfo != null)
                    return false;
            }
            return true;
        }

        private string GenerateTypeConverter(CodeMemberMethod initComponentMethod,
            XamlType xamlBaseType)
        {
            //Type typeConverterType = XamlSchemaContext.GetClrType(xamlTextSyntax);
            Type typeConverterType = (xamlBaseType == null ? null : (xamlBaseType.TypeConverter == null ? null : xamlBaseType.TypeConverter.ConverterType));
            if (typeConverterType == null)
            {
                return null;
            }
            string name;
            if (!TypeConverterDictionary.TryGetValue(typeConverterType, out name))
            {
                name = GenerateName(typeConverterType);
                if (!typeConverterType.IsEnum && typeConverterType.Name != "EnumConverter")
                {
                    CodeVariableDeclarationStatement cvds = new CodeVariableDeclarationStatement(
                        new CodeTypeReference(typeConverterType.Name),
                        name,
                        new CodeObjectCreateExpression(typeConverterType.Name));

                    initComponentMethod.Statements.Add(cvds);

                    TypeConverterDictionary[typeConverterType] = name;
                }
            }

            return name;
        }

        private CodeExpression GetTypeConverteredValue(XamlType xamlBaseType, object value, Type type, string tcName)
        {
            CodeExpression valueExpression = new CodeSnippetExpression("\"" + value + "\"");
            if (xamlBaseType != null)
            {
                Type typeConverter = (xamlBaseType == null ? null : (xamlBaseType.TypeConverter == null ? null : xamlBaseType.TypeConverter.ConverterType));
                if (typeConverter != null)
                {
                    if (typeConverter.IsEnum)
                    {
                        //todo: add typeConverter to usings
                        return new CodeFieldReferenceExpression(
                            new CodeTypeReferenceExpression(typeConverter.Name), (string)value);
                    }
                    else
                    {
                        //Avoid LengthConverter if it appears that it would be a valid double
                        bool shouldConvert = ShouldUseTypeConverter((string)value, type);
                    
                        if (shouldConvert)
                        {
                            CodeVariableReferenceExpression cvrf = new CodeVariableReferenceExpression(tcName);
                            CodeMethodInvokeExpression cmie = new CodeMethodInvokeExpression(
                                new CodeMethodReferenceExpression(cvrf, "ConvertFrom"),
                                    new CodePrimitiveExpression(null),
                                    new CodeFieldReferenceExpression(null, CultureInfoString),
                                    new CodeSnippetExpression("\"" + value + "\""));

                            return new CodeCastExpression(type.Name, cmie);
                        }
                        else
                        {
                            return GenerateExpressionForString((string)value, type);
                        }
                    }
                }
            }

            return valueExpression;
        }

        private CodeExpression GenerateExpressionForString(string value, Type type)
        {
            if (type == typeof(String))
            {
                return new CodePrimitiveExpression(value);
            }
            else if (type == typeof(Double))
            {
                int doubleValue;
                bool isDouble = int.TryParse((string)value, out doubleValue);
                if (isDouble)
                {
                    return new CodePrimitiveExpression(doubleValue);
                }
            }
            else if (type == typeof(Int32))
            {
                int intValue;
                bool isInt = Int32.TryParse((string)value, out intValue);
                if (isInt)
                {
                    return new CodePrimitiveExpression(intValue);
                }
            }
            else if (type == typeof(Boolean))
            {
                bool boolValue;
                bool isBool = Boolean.TryParse(value, out boolValue);
                if (isBool)
                {
                    return new CodePrimitiveExpression(boolValue);
                }
            }
            else if (type == typeof(Brush))
            {
                Type brushes = typeof(Brushes);
                PropertyInfo propertyInfo = brushes.GetProperty(value,
                    BindingFlags.IgnoreCase | BindingFlags.Static | BindingFlags.Public);
                if (propertyInfo != null)
                {
                    // Make sure we have a namespace reference to the Brushes type
                    if (!NamespacesToUse.ContainsKey(brushes.Namespace))
                    {
                        NamespacesToUse.Add(brushes.Namespace, null);
                    }

                    CodeTypeReferenceExpression ctre = new CodeTypeReferenceExpression(brushes.Name);
                    return new CodeFieldReferenceExpression(ctre, propertyInfo.Name);
                }
            }
            else if (type == typeof(Color))
            {
                Type colors = typeof(Colors);
                PropertyInfo propertyInfo = colors.GetProperty(value);
                if (propertyInfo != null)
                {
                    // Make sure we have a namespace reference to the Color type
                    if (!NamespacesToUse.ContainsKey(colors.Namespace))
                    {
                        NamespacesToUse.Add(colors.Namespace, null);
                    }
                    CodeTypeReferenceExpression ctre = new CodeTypeReferenceExpression(colors.Name);
                    return new CodeFieldReferenceExpression(ctre, propertyInfo.Name);
                }
            }
            else if (type.IsEnum)
            {
                FieldInfo fieldInfo = type.GetField(value);
                if (fieldInfo != null)
                {
                    CodeTypeReferenceExpression ctre = new CodeTypeReferenceExpression(type.Name);
                    return new CodeFieldReferenceExpression(ctre, fieldInfo.Name);
                }
            }

                throw new Exception("Type " + type.Name + " isn't supported by GenerateExpressionForString");
        }

        private bool GetIsUsableDuringInitialization(XamlType xamlType)
        {
            object[] attrs = xamlType.UnderlyingType.GetCustomAttributes(typeof(x2006.UsableDuringInitializationAttribute), true);
            if (attrs != null && attrs.Length == 1)
            {
                x2006.UsableDuringInitializationAttribute udia = (x2006.UsableDuringInitializationAttribute)attrs[0];

                return udia.Usable;
            }

            return false;
        }

        private string GenerateUniqueName(XamlType xamlType)
        {
            if (xamlType.UnderlyingType != null)
                return GenerateName(xamlType.UnderlyingType, true);
            else
                return "_" + xamlType.Name.Substring(0, 1).ToLower() + xamlType.Name.Substring(1);
        }

        private string GenerateUniqueName(Type type)
        {
            return GenerateName(type, true);
        }

        private string GenerateName(Type type)
        {
            return GenerateName(type, false);
        }

        private string GenerateName(Type type, bool addUniqueSuffix)
        {
            if (!NamespacesToUse.ContainsKey(type.Namespace))
                NamespacesToUse.Add(type.Namespace, null);

            string varName = "_" + type.Name.Substring(0, 1).ToLower() + type.Name.Substring(1);
            if (addUniqueSuffix)
                varName += objectNumber++;
            return varName;
        }

        #region Dump Data
        private void DumpNodeTree(CodeDomObjectNode rootNode)
        {
            if (rootNode.Type != null)
            {
                Debug.WriteLine(rootNode.Type.Name);
            }

            NodeCollection<MemberNode> members = rootNode.MemberNodes;
            foreach (MemberNode member in members)
            {
                Debug.WriteLine("Member={0}, Type={1}", new object[] { member.Member.Name, member.Member.Type.Name });

                foreach (ItemNode itemNode in member.ItemNodes)
                {
                    ValueNode valueNode = itemNode as ValueNode;

                    if (valueNode != null)
                    {
                        string value = valueNode.Value as String;
                        Debug.WriteLine("Underlying Type={0}, Value={1}", new object[] { member.Member.Type.UnderlyingType, value });
                    }
                    else
                    {
                        CodeDomObjectNode objectNode = (CodeDomObjectNode)itemNode;
                        XamlType xamlType = objectNode.Type;

                        if (xamlType == XamlLanguage.Static)
                        {
                            ValueNode valueNode2 = objectNode.XPosParamsNode.ItemNodes[0] as ValueNode;
                            string xamlTypeReference = valueNode2.Value as string;
                            string memberName = null;
                            string typeName = null;
                            int period = xamlTypeReference.IndexOf('.');
                            if (period > -1)
                            {
                                memberName = xamlTypeReference.Substring(period + 1);
                                typeName = xamlTypeReference.Substring(0, period);
                            }
                            Type resolvedType = objectNode.Resolve(typeName);
                            //TODO: don't forget to make sure a using happens for the referencedXamlType
                            string typeName2 = resolvedType != null ? resolvedType.Name : xamlTypeReference;

                            Debug.WriteLine("TypeName={0}, MemberName={1}", new object[] { typeName2, memberName });
                        }
                        else if (xamlType == XamlLanguage.Null)
                        {
                            Debug.WriteLine("NULL Expression");
                        }
                        else if (xamlType == XamlLanguage.Type)
                        {
                            ValueNode valueNode2 = objectNode.XPosParamsNode.ItemNodes[0] as ValueNode;
                            string xamlTypeReference = valueNode2.Value as string;
                            Type resolvedType = objectNode.Resolve(xamlTypeReference);
                            //TODO: don't forget to make sure a using happens for the referencedXamlType
                            string typeName = resolvedType != null ? resolvedType.Name : xamlTypeReference;

                            Debug.WriteLine("Type Ref={0}", new object[] { typeName });
                        }
                        else if (xamlType == XamlLanguage.Reference)
                        {

                        }
                        else
                        {
                            if (objectNode.Type != null)
                            {
                                // StartObject case
                                DumpNodeTree(objectNode);
                            }
                            else
                            {
                                // GetObject
                                DumpNodeTree(objectNode);
                            }
                        }
                    }
                }
            }
        }
        #endregion

    }
}
