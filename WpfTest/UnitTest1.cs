using CommunityToolkit.Mvvm;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.DependencyInjection.DryIoc;
using CommunityToolkit.Mvvm.DependencyInjection.Microsoft;
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

        
        [UIFact]
        public void TestMethod1()
        {


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

            protected override void RegisterTypes(DryIoc.IContainer serviceDescriptors)
            {
                serviceDescriptors.Register<ITestz, A>(Reuse.Transient, serviceKey: "A");
                serviceDescriptors.Register<ITestz, A>(Reuse.Transient, serviceKey: "C");

                serviceDescriptors.Register<ITestz, B>(Reuse.Transient, serviceKey: "B");
                serviceDescriptors.BuilderViewAndViewModelByDryIoc(typeof(ITestz).Assembly);
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

    
}

namespace WpfTest.Views
{
    public class MockView :FrameworkElement{ }
}

namespace WpfTest.ViewModels
{
    public class MockViewModel { }
}