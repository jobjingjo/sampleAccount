using System;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace sampleAccount.TestHelpers
{
    public static class MoqExtensions
    {
        public static IServiceCollection AddMock<T>(this IServiceCollection serviceCollection) where T : class
        {
            return serviceCollection.AddMock<T>(MockBehavior.Strict);
        }

        public static IServiceCollection AddMock<T>(this IServiceCollection serviceCollection,
            MockBehavior behavior) where T : class
        {
            var mock = new Mock<T>(behavior);
            serviceCollection.AddSingleton(mock);
            serviceCollection.AddSingleton(mock.Object);
            return serviceCollection;
        }

        public static Mock<T> GetMock<T>(this IServiceProvider serviceProvider) where T : class
        {
            return serviceProvider.GetRequiredService<Mock<T>>();
        }
    }
}