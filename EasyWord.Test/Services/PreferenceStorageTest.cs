using System;
using EasyWord.Library.Services;
using EasyWord.Services;
using Xamarin.Essentials;
using Xunit;

namespace EasyWord.Test.Services
{
    public class PreferenceStorageTest
    {
        [Fact]
        public void SetAndGet_IntValue_ShouldReturnCorrectValue()
        {
            // Arrange
            var preferenceStorage = new PreferenceStorage();
            var key = "TestKey";
            var value = 42;

            // Act
            preferenceStorage.Set(key, value);
            var result = preferenceStorage.Get(key, 0);

            // Assert
            Assert.Equal(value, result);
        }

        [Fact]
        public void SetAndGet_StringValue_ShouldReturnCorrectValue()
        {
            // Arrange
            var preferenceStorage = new PreferenceStorage();
            var key = "TestKey";
            var value = "TestValue";

            // Act
            preferenceStorage.Set(key, value);
            var result = preferenceStorage.Get(key, string.Empty);

            // Assert
            Assert.Equal(value, result);
        }

        [Fact]
        public void SetAndGet_DateTimeValue_ShouldReturnCorrectValue()
        {
            // Arrange
            var preferenceStorage = new PreferenceStorage();
            var key = "TestKey";
            var value = DateTime.Now;

            // Act
            preferenceStorage.Set(key, value);
            var result = preferenceStorage.Get(key, DateTime.MinValue);

            // Assert
            Assert.Equal(value, result, TimeSpan.FromSeconds(1)); // Allow 1 second difference for DateTime
        }

        [Fact]
        public void Get_DefaultValue_ShouldReturnDefaultValue()
        {
            // Arrange
            var preferenceStorage = new PreferenceStorage();
            var key = "NonExistentKey";
            var defaultValue = "Default";

            // Act
            var result = preferenceStorage.Get(key, defaultValue);

            // Assert
            Assert.Equal(defaultValue, result);
        }
    }
}
