namespace ToDoWeb.Application.Services
{
    public interface ISingletonGenerator
    {
        Guid Generate();
    }

    public class SingletonGenerator : ISingletonGenerator
    {
        private readonly IServiceProvider _serviceProvider;

        public SingletonGenerator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Guid Generate()
        {
            var guidGenerator = _serviceProvider.GetService<IGuidGenerator>();
            return guidGenerator.Generate();
        }
    }
}
