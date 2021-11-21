using Autofac;

public class WebBootstrap : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<WeeklyOrdersService>().As<IWeeklyOrdersService>();
    }
}