# CommunityToolkit.Mvvm.Wpf

# 免责申明：只是为了帮助开发人员简化开发流程、提高开发效率，请勿使用此框架做任何违法国家法律的事情。使用者所做任何事情也与本框架的作者无关。
## 迁移prism导航,IOC,自动注入Vm,用法跟prism一样.
#### 链接:https://github.com/PrismLibrary/Prism

#### 注册服务/配置服务
``` CSharp
//MicrosoftIocInitialzationExtensions
//根据不同的IOC实现包进行 注册 配置
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
```

``` xml
<Window
    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    xmlns:tookit="http://CommunityToolkit.Mvvm.Wpf/"
    tookit:ViewModelLocator.AutoWireViewModel="True"
    >
```