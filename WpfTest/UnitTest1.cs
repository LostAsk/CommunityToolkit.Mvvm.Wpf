using CommunityToolkit.Mvvm;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.DependencyInjection.DryIoc;
using CommunityToolkit.Mvvm.DependencyInjection.Microsoft;
using DryIoc;
using DryIoc.Microsoft.DependencyInjection;
using System.ComponentModel;
using System.Windows;
using WpfTest.ViewModels;
using WpfTest.Views;

namespace WpfTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            
            DryIocInitialzationExtensions.BuilderService((service) =>
            {
                service.Register<ITestz, A>(Reuse.Transient,serviceKey: "A");
                service.Register<ITestz, A>(Reuse.Transient, serviceKey: "C");
               
                service.Register<ITestz, B>(Reuse.Transient, serviceKey: "B");
                var typez = typeof(ITestz).Assembly.RegisterViewAndViewModel();
                //service.Register<MockView>(serviceKey: "MockView");
                //service.Register<MockView>(serviceKey: typeof(MockView).FullName);
                //service.Register<MockView>();
                service.PopulateKey(typez, (reg, ser) =>
                {
                    return ser.IsKeyedService ? reg.IsRegistered(ser.ServiceType, ser.ServiceKey) : reg.IsRegistered(ser.ServiceType);


                });

            }, (provider) => { 
            
            });
            var service = new ServiceCollection();
             
            //service.AddKeyedTransient<ITestz, A>("A")
            //     .AddKeyedTransient<ITestz, A>("C")
            //.AddSingleton<ServiceCollection>(service)
            //.AddKeyedTransient<ITestz, B>("B");
            //var provider=service.BuildServiceProvider();
            //Ioc.Default.ConfigureServices(provider);
             
            var k = Ioc.Default.GetService<IServiceProviderIsKeyedService>();
            var k2 = Ioc.Default.GetService<IServiceProviderIsService>();
            var kk = k == k2;
            var m = k.IsKeyedService(typeof(ITestz), "A");
            var m1 = k.IsKeyedService(typeof(ITestz), "C");
            var a = Ioc.Default.IsRegistrationType("A");

            var win = Ioc.Default.GetService<MockView>();
            //ViewModelLocationProvider.AutoWireViewModelChanged(win, (v, vm) =>
            //{
            //    Assert.IsNotNull(v);
            //    Assert.IsNotNull(vm);
            //    Assert.IsInstanceOfType<MockViewModel>(vm);
            //});
            //var b = Ioc.Default.IsRegistrationType("D");
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
    public class MockView { }
}

namespace WpfTest.ViewModels
{
    public class MockViewModel { }
}