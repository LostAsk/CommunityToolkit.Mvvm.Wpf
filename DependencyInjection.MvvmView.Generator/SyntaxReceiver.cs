using DependencyInjection.Annotation.SourceGenerator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Text;

namespace DependencyInjection.MvvmView.Generator
{
    sealed class SyntaxReceiver : ISyntaxReceiver
    {
        private static readonly string viewBindVmAttributeTypeName = "Microsoft.Extensions.DependencyInjection.ViewBindVmAttribute";

        private readonly List<TypeDeclarationSyntax> typeSyntaxList = new();

        public void OnVisitSyntaxNode(SyntaxNode syntaxNode)
        {
            // 声明 类node
            if (syntaxNode is ClassDeclarationSyntax classSyntax)
            {
                this.typeSyntaxList.Add(classSyntax);
            }
            //声明 record 类node
            else if (syntaxNode is RecordDeclarationSyntax recordSyntax)
            {
                this.typeSyntaxList.Add(recordSyntax);
            }
        }


        /// <summary>
        /// 获取服务描述
        /// </summary>
        /// <param name="compilation"></param>
        /// <returns></returns>
        public IEnumerable<ServiceDescriptor> GetServiceDescriptors(Compilation compilation)
        {
            var serviceAttributeClass = compilation.GetTypeByMetadataName(viewBindVmAttributeTypeName);
            if (serviceAttributeClass == null)
            {
                yield break;
            }

            foreach (var syntax in this.typeSyntaxList)
            {
                var symbol = compilation.GetSemanticModel(syntax.SyntaxTree).GetDeclaredSymbol(syntax);
                if (symbol is ITypeSymbol @class)
                {
                    foreach (var descriptor in GetServiceDescriptors(@class, serviceAttributeClass))
                    {
                        yield return descriptor;
                    }
                }
            }
        }

        private static IEnumerable<ServiceDescriptor> GetServiceDescriptors(ITypeSymbol @class, INamedTypeSymbol serviceAttributeClass)
        {
            foreach (var attr in @class.GetAttributes())
            {
                var attrClass = attr.AttributeClass;
                if (attrClass != null && attrClass.Equals(serviceAttributeClass, SymbolEqualityComparer.Default))
                {
                    var args = attr.ConstructorArguments;
                    if (args.Length > 0 &&
                        args[0].Kind == TypedConstantKind.Enum &&
                        args[0].Value is int intValue &&
                        Enum.IsDefined(typeof(ServiceLifetime), intValue))
                    {
                        var lifetime = (ServiceLifetime)intValue;
                        var descriptor = new ServiceDescriptor(lifetime, new TypeSymbol(@class), GetKeyString(args.Last()));

                        for (var i = 1; i < args.Length; i++)
                        {
                            if (args[i].Value is ITypeSymbol serviceType)
                            {
                                descriptor.ServiceTypes.Add(new TypeSymbol(serviceType));
                            }
                        }
                        yield return descriptor;
                    }
                }
            }
        }

        private static string? GetKeyString(TypedConstant keyTypedConstant)
        {
            object? value = keyTypedConstant.Value;
            if (value is null)
            {
                return null;
            }
            else if (keyTypedConstant.Kind == TypedConstantKind.Enum)
            {
                return $"global::{keyTypedConstant.ToCSharpString()}";
            }
            else if (value is string stringValue)
            {
                return $"\"{stringValue}\"";
            }
            else
            {
                return value.ToString();
            }
        }
    }
}
