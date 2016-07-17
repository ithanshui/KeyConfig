using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using KeyConfig;
using KeyConfig.ConfigSources;

namespace KeyConfigTests
{
    /// <summary>
    /// Simply tests retrieving and setting values.
    /// </summary>
    [TestClass]
    public class RetrievingSettingValues
    {
        /// <summary>
        /// Test fixture.
        /// </summary>
        public class TestConfigClass
        {
            [ConfigKey(KeyName = "IpAddress", Required = true)]
            public string Name { get; set; }

            [ConfigKey]
            public string Occupation { get; set; }
        }

        /// <summary>
        /// Tests getting values.
        /// </summary>
        [TestMethod]
        public void TestGetValues()
        {
            var instance = ConfigManager<TestConfigClass>.GetConfig(new AppSettingsSource()); 

            Assert.AreEqual("Tim Reynolds", instance.Name);
            Assert.AreEqual("Tester", instance.Occupation);
        }

        /// <summary>
        /// Tests setting values.
        /// </summary>
        [TestMethod]
        public void TestSetValues()
        {
            var source = new AppSettingsSource();
            var instance = ConfigManager<TestConfigClass>.GetConfig(source);
            instance.Name = "Bob";

            ConfigManager<TestConfigClass>.SaveConfig(source, instance);
            ConfigManager<TestConfigClass>.FulfillConfig(source, instance);

            Assert.AreEqual("Bob", instance.Name);
        }
    }
}
