# CommunityToolkit.Mvvm.Wpf

# 免责申明：只是为了帮助开发人员简化开发流程、提高开发效率，请勿使用此框架做任何违法国家法律的事情。使用者所做任何事情也与本框架的作者无关。
## 迁移prism导航,IOC,自动注入Vm,用法跟prism一样.
#### 链接:https://github.com/PrismLibrary/Prism

#### 注册服务/配置服务
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