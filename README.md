# CommunityToolkit.Mvvm.Wpf

# ����������ֻ��Ϊ�˰���������Ա�򻯿������̡���߿���Ч�ʣ�����ʹ�ô˿�����κ�Υ�����ҷ��ɵ����顣ʹ���������κ�����Ҳ�뱾��ܵ������޹ء�
## Ǩ��prism����,IOC,�Զ�ע��Vm,�÷���prismһ��.
#### ����:https://github.com/PrismLibrary/Prism

#### ע�����/���÷���
``` CSharp
 public partial class App : Application
 {
     protected override void OnStartup(StartupEventArgs e)
     {
         base.OnStartup(e);
         var boot = new MockBootstrapper();
         boot.Run();
     }

     class MockBootstrapper : DryIocInitialzation
     {
         protected override void ConfigService(IServiceProvider serviceProvider)
         {

         }

         protected override DependencyObject CreateShell(IServiceProvider serviceProvider)
         {
             return serviceProvider.GetService<MainWindow>();
         }

         protected override void RegisterTypes(DryIoc.IContainer serviceDescriptors)
         {
             
             serviceDescriptors.BuilderViewAndViewModelByDryIoc(typeof(App).Assembly);
         }
     }
 }
```

``` xml
<Window
    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    xmlns:tookit="http://CommunityToolkit.Mvvm.Wpf/"
    tookit:ViewModelLocator.AutoWireViewModel="True"
    >
```