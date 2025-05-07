using Application.Common.Composers;
using Infrastructure.Common.Composer;
using Microsoft.Extensions.DependencyInjection;

namespace Tests.UnitTests;

public class RegisterDepenciesTests
{
    
    public class RegisterDependenciesTests
    {
        [Fact]
        public void RegisterInfrastructure_WhenCalled_ReturnsServiceCollection()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            var result = services.RegisterInfrastructure();

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IServiceCollection>(result);
        }
        
        [Fact]
        public void RegisterApplication_WhenCalled_ReturnsServiceCollection()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            var result = services.RegisterApplication();

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IServiceCollection>(result);
        }
    }
}