﻿using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace sampleAccount.TestHelpers
{
    public static class MoqExtensions
    {
        public static IServiceCollection AddMock<T>(this IServiceCollection serviceCollection, MockBehavior behavior = MockBehavior.Strict) where T : class
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