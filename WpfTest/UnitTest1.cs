using CommunityToolkit.Mvvm;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.DependencyInjection.DryIoc;
using CommunityToolkit.Mvvm.DependencyInjection.Microsoft;
using CommunityToolkit.Mvvm.Modularity;
using DryIoc;
using DryIoc.Microsoft.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel;
using System.Windows;
using WpfTest.ViewModels;
using WpfTest.Views;
using Xunit;

namespace WpfTest
{
   
    public class UnitTest1
    {
         
        [WpfFact]
        public void TestMethod1()
        {
            var moduleA = new Item("Module A");
            var moduleA1 = new Item("Module A");
            var moduleC = new Item("Module C", moduleA);
            var moduleB = new Item("Module B", moduleC);
            var moduleE = new Item("Module E", moduleB);
            var moduleF = new Item("Module F", moduleA1);
            var moduleD = new Item("Module D", moduleE, moduleF);
         
            var unsorted = new[] { moduleE, moduleA, moduleD, moduleB, moduleC, moduleF };

            var sorted =  DAGSort(unsorted, x => x.Dependencies);

            foreach (var item in sorted)
            {
                Console.WriteLine(item.Name);
            }

            //Console.ReadLine();


            var boot = new MockBootstrapper();
            boot.Run();
            var k = Ioc.Default.GetService<IServiceProviderIsKeyedService>();
            var k2 = Ioc.Default.GetService<IServiceProviderIsService>();
            var kk = k == k2;
            var m = k.IsKeyedService(typeof(ITestz), "A");
            var m1 = k.IsKeyedService(typeof(ITestz), "C");
            var a = Ioc.Default.IsRegistrationType("A");

            var win = Ioc.Default.GetService<MockView>();
            ViewModelLocationProvider.AutoWireViewModelChanged(win, (v, vm) =>
            {
                Assert.NotNull(v);
                Assert.NotNull(vm);
                Assert.IsType<MockViewModel>(vm);
            });
            
        }


        class MockBootstrapper : DryIocInitialzation
        {
            protected override void ConfigService(IServiceProvider serviceProvider)
            {
               
            }

            protected override DependencyObject CreateShell(IServiceProvider serviceProvider)
            {
                return serviceProvider.GetService<MockView>();
            }
            

            protected override void RegisterTypes(DryIoc.IContainer container,IServiceCollection serviceDescriptors)
            {
                container.Register<ITestz, A>(Reuse.Transient, serviceKey: "A");
                container.Register<ITestz, A>(Reuse.Transient, serviceKey: "C");

                container.Register<ITestz, B>(Reuse.Transient, serviceKey: "B");
 
                AddModule(serviceDescriptors, (s, manger) =>
                {
                    manger.AddModule<TestBModule>();
                    manger.AddModule<TestCModule>();
                    manger.BuilderModule = (s, t) =>
                    {
                        return container.Resolve(t) as IModule;
                    };
                    manger.RegisterTypes = ( t) =>
                    {
                        foreach(var i in t)
                        {
                            container.Register(i,i,Reuse.Singleton );
                        }
                    };
                });

                
                
                //serviceDescriptors.BuilderViewAndViewModelByDryIoc(typeof(ITestz).Assembly);
            }
        }




        public static IList<T> DAGSort<T>(IEnumerable<T> source, Func<T, IEnumerable<T>> getDependencies)
        {
            var sorted = new List<T>();
            var visited = new Dictionary<T, bool>();

            foreach (var item in source)
            {
                Visit(item, getDependencies, sorted, visited);
            }

            return sorted;
        }

        private static void Visit<T>(T item, Func<T, IEnumerable<T>> getDependencies, List<T> sorted, Dictionary<T, bool> visited)
        {
            bool inProcess;
            var alreadyVisited = visited.TryGetValue(item, out inProcess);

            // ����Ѿ����ʸö��㣬��ֱ�ӷ���
            if (alreadyVisited)
            {
                // ��������Ϊ��ǰ�ڵ㣬��˵������ѭ������
                if (inProcess)
                {
                    throw new ArgumentException("Cyclic dependency found.");
                }
            }
            else
            {
                // ���ڴ���ǰ����
                visited[item] = true;

                // �������������
                var dependencies = getDependencies(item);
                // ���������ϲ�Ϊ�գ����������������ڵ�
                if (dependencies != null)
                {
                    foreach (var dependency in dependencies)
                    {
                        // �ݹ��������
                        Visit(dependency, getDependencies, sorted, visited);
                    }
                }

                // ���������Ϊ false
                visited[item] = false;
                sorted.Add(item);
            }
        }
    }

    public interface ITestz
    {
        void Foo();
    }

    public class A : ITestz
    {
        public void Foo()
        {
            Console.WriteLine("A");

        }
    }

    public class B : ITestz
    {
        public void Foo()
        {
            Console.WriteLine("b");

        }
    }

    public class TestAModule : IModule
    {
        public void ConfigService(IServiceCollection services)
        {
            
        }

        public void OnInitialized(IServiceProvider serviceProvider)
        {
            
        }
    }

    [ModuleDependency(typeof(TestAModule))]
    public class TestBModule : IModule
    {
        public void ConfigService(IServiceCollection services)
        {

        }

        public void OnInitialized(IServiceProvider serviceProvider)
        {

        }
    }
 
    [ModuleDependency(typeof(TestBModule))]
    [ModuleDependency(typeof(TestDModule))]
    public class TestCModule : IModule
    {
        public void ConfigService(IServiceCollection services)
        {

        }

        public void OnInitialized(IServiceProvider serviceProvider)
        {

        }
    }

    public class TestDModule : IModule
    {
        public void ConfigService(IServiceCollection services)
        {

        }

        public void OnInitialized(IServiceProvider serviceProvider)
        {

        }
    }
    // Item ����
    public class Item
    {
        // ��Ŀ����
        public string Name { get; private set; }
        // ������
        public Item[] Dependencies { get; set; }

        public Item(string name, params Item[] dependencies)
        {
            Name = name;
            Dependencies = dependencies;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}

namespace WpfTest.Views
{
    public class MockView :FrameworkElement{ }
}

namespace WpfTest.ViewModels
{
    public class MockViewModel { }
}