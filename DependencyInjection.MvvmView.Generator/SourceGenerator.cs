using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace DependencyInjection.MvvmView.Generator
{
    [Generator]
    public sealed class SourceGenerator : ISourceGenerator
    {
        private static readonly string fileName = "ServiceCollectionViewBindVmExtensions.g.cs";
        private static readonly string className = "ServiceCollectionViewBindVmExtensions";
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="context"></param>
        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new SyntaxReceiver());
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="context"></param>
        public void Execute(GeneratorExecutionContext context)
        {
            //  System.Diagnostics.Debugger.Launch();

            if (context.SyntaxReceiver is SyntaxReceiver receiver)
            {
                var assemblyName = GetAssemblyName(context.Compilation);
                var code = GenerateCode(receiver, context.Compilation, assemblyName);
                context.AddSource(fileName, code);
            }
        }

        private static string GetAssemblyName(Compilation compilation)
        {
            var assemblyName = compilation.AssemblyName ?? string.Empty;
            return new string(assemblyName.Where(IsAllowChar).ToArray());

            static bool IsAllowChar(char c)
            {
                return ('0' <= c && c <= '9') || ('A' <= c && c <= 'Z') || ('a' <= c && c <= 'z');
            }
        }

        private static string GenerateCode(SyntaxReceiver receiver, Compilation compilation, string assemblyName)
        {
            var builder = new StringBuilder();
            builder.AppendLine("using CommunityToolkit.Mvvm.Wpf.Microsoft;");
            builder.AppendLine("using System;");
            builder.AppendLine(PadSpace(0, "using Microsoft.Extensions.DependencyInjection;"));
            builder.AppendLine(PadSpace(0, "using Microsoft.Extensions.DependencyInjection.Extensions;"));
            builder.AppendLine($"namespace {assemblyName}");
            builder.AppendLine("{");
            builder.AppendLine(PadSpace(4, $"""
            /// <summary>
            /// 注册view and vm
            /// </summary>
            """, true));
            builder.AppendLine(PadSpace(4, $"public static partial class {className}"));
            builder.AppendLine(PadSpace(4, "{"));
            #region 注册view and vm
            builder.AppendLine(PadSpace(8, $"""
            /// <summary>
            /// 将程序集{compilation.AssemblyName}的所有ViewBindVm标记类型注册到DI
            /// </summary>
            /// <param name="services"></param>
            /// <returns></returns>
    """, true));
            builder.AppendLine(PadSpace(8, $"public static IServiceCollection Add{assemblyName}ViewAndViewModel(this IServiceCollection services)"));
            builder.AppendLine(PadSpace(8, "{"));
            foreach (var descriptor in receiver.GetServiceDescriptors(compilation))
            {

                builder.AppendLine(PadSpace(12, $@"services.TryAdd(ServiceDescriptor.Describe(typeof({descriptor.DeclaredType}),typeof({descriptor.DeclaredType}),ServiceLifetime.{descriptor.Lifetime}));"));
                foreach(var view in descriptor.ServiceTypes)
                {
                    //瞬时
                    builder.AppendLine(PadSpace(12, $@"services.TryAdd(ServiceDescriptor.Describe(typeof({view}),typeof({view}),ServiceLifetime.Transient));"));
                    if (descriptor.Key != null)
                    {
                        builder.AppendLine(PadSpace(12, $"services.TryAdd(ServiceDescriptor.DescribeKeyed(typeof({view}),{descriptor.Key}, typeof({view}), ServiceLifetime.Transient));"));
                    }
                    else
                    {
                        builder.AppendLine(PadSpace(12, $"services.TryAdd(ServiceDescriptor.DescribeKeyed(typeof({view}),\"{view}\", typeof({view}), ServiceLifetime.Transient));"));
                    }
                    var key = string.IsNullOrWhiteSpace(descriptor.Key) ? "${view}" : descriptor.Key;
                    builder.AppendLine(PadSpace(12, $"VMLocationProvider.Instance.AddTypeMapping({key},typeof({view}));"));
                }
          

            }
            builder.AppendLine(PadSpace(12, "return services;"));
            builder.AppendLine(PadSpace(8, "}"));
            #endregion

            #region VmLocationProvider
            builder.AppendLine(PadSpace(8, $"""
            /// <summary>
            /// 将程序集{compilation.AssemblyName}的View 查找对应viewVm
            /// </summary>
            /// <param name="services"></param>
            /// <returns></returns>
    """, true));
            builder.AppendLine(PadSpace(8, $"public static object Use{assemblyName}DefaultViewModelFatory(object view, IServiceProvider serviceProvider)"));
            builder.AppendLine(PadSpace(8, "{"));
            builder.AppendLine(PadSpace(12, "return view switch"));
            builder.AppendLine(PadSpace(12, "{"));
            foreach (var descriptor in receiver.GetServiceDescriptors(compilation))
            {
                    foreach (var view in descriptor.ServiceTypes)
                    {
                        //瞬时
                        builder.AppendLine(PadSpace(16, $@"{view} => serviceProvider.GetService<{descriptor.DeclaredType}>(),"));
                    }
            }
            builder.AppendLine(PadSpace(16, $@"null => null,"));
            builder.AppendLine(PadSpace(16, $@"_ => null,"));
            builder.AppendLine(PadSpace(12, "};"));
            builder.AppendLine(PadSpace(8, "}"));




            builder.AppendLine(PadSpace(8, $"""
            /// <summary>
            /// 将程序集{compilation.AssemblyName}的 key 创建view
            /// </summary>
            /// <param name="services"></param>
            /// <returns></returns>
    """, true));
            builder.AppendLine(PadSpace(8, $"public static object Use{assemblyName}DefaultViewFatory(string view_key, IServiceProvider serviceProvider)"));
            builder.AppendLine(PadSpace(8, "{"));
            builder.AppendLine(PadSpace(12, "return view_key switch"));
            builder.AppendLine(PadSpace(12, "{"));
            foreach (var descriptor in receiver.GetServiceDescriptors(compilation))
            {
                if (string.IsNullOrEmpty(descriptor.Key)) continue;

                foreach (var view in descriptor.ServiceTypes)
                {
                    //瞬时
                    builder.AppendLine(PadSpace(16, $@"{descriptor.Key} => serviceProvider.GetKeyedService<{view}>({descriptor.Key}),"));
                }
            }
            builder.AppendLine(PadSpace(16, $@"null => null,"));
            builder.AppendLine(PadSpace(16, $@"_ => null,"));
            builder.AppendLine(PadSpace(12, "};"));
            builder.AppendLine(PadSpace(8, "}"));

            #endregion
            //namespace
            builder.AppendLine(PadSpace(4, "}"));
            builder.AppendLine(PadSpace(0, "}"));
            return builder.ToString();
        }








        private static string PadSpace(int num, string str = "", bool is_trim_start = false)
        {
            return string.Join("\n", str.Split(["\r", "\n"], StringSplitOptions.RemoveEmptyEntries).Select(p =>
            {
                var s = is_trim_start ? p.TrimStart() : p;
                return $"{new string(' ', num)}{s}";

            }));
        }
    }
}
