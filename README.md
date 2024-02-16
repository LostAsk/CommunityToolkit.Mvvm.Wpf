# CommunityToolkit.Mvvm.Wpf

# ����������ֻ��Ϊ�˰���������Ա�򻯿������̡���߿���Ч�ʣ�����ʹ�ô˿�����κ�Υ�����ҷ��ɵ����顣ʹ���������κ�����Ҳ�뱾��ܵ������޹ء�
## Ǩ��prism����,IOC,�Զ�ע��Vm,�÷���prismһ��.
#### ����:https://github.com/PrismLibrary/Prism

#### ע�����/���÷���
``` CSharp
//MicrosoftIocInitialzationExtensions
//���ݲ�ͬ��IOCʵ�ְ����� ע�� ����
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