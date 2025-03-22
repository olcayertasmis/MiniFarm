/*using System.Resources;
using MiniFarm.Data.EnumData;
using MiniFarm.Managers;
using NUnit.Framework;
using UnityEngine.Assertions;
using Zenject.ReflectionBaking.Mono.Cecil;

namespace MiniFarm.Tests
{
    public class ResourceManagerTests
    {
        [Test]
        public void UpdateResource_AddsResourceCorrectly()
        {
            var resourceManager = new ResourceManager();
            resourceManager.UpdateResource(ResourceType.Hay, 10);

            Assert.AreEqual(10, resourceManager.GetResourceAmount(ResourceType.Hay));
        }

        [Test]
        public void HasEnoughResource_ReturnsFalseWhenNotEnough()
        {
            var resourceManager = new ResourceManager();
            resourceManager.UpdateResource(ResourceType.Hay, 5);

            Assert.IsFalse(resourceManager.HasEnoughResource(ResourceType.Hay, 10));
        }

        [Test]
        public void GetResourceAmount_ReturnsZeroForUninitializedResource()
        {
            var resourceManager = new ResourceManager();

            Assert.AreEqual(0, resourceManager.GetResourceAmount(ResourceType.Hay));
        }
    }
}*/