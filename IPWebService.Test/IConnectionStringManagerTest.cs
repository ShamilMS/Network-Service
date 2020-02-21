﻿using IPWebService.Persistence;
using IPWebService.Services;
using IPWebService.Test.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace IPWebService.Test
{
    public sealed class ConnectionStringManagerTest:IClassFixture<WebAppTestFactory>
    {
        private readonly IConnectionStringManager connStringManager;
        private readonly AppDbOptions dbOptions;

        public ConnectionStringManagerTest(WebAppTestFactory factory)
        {
            connStringManager = factory.Services.GetService<IConnectionStringManager>();
            dbOptions = factory.Services.GetService<IOptionsSnapshot<AppDbOptions>>().Value;
        }



        [Fact(DisplayName = "IConnectionStringManager is registered")]
        public void IConnectionStringManagerIsRegistered()
        {
            Assert.True(connStringManager!=null, "IConnectionStringManager should be registered");
        }



        [Fact(DisplayName = "IConnectionStringManager set valid connectionString")]
        public void IConnectionStringManagerSetValidConnectionString()
        {
            string connString = "connectionString...";
            connStringManager.SetConnectionString(connString);
            Assert.True(connStringManager.CurrentConnectionString == connString, "Connection string should be assinged");
        }


        [Fact(DisplayName = "IConnectionStringManager throw Exception when setting invalid connectionString")]
        public void IConnectionStringManagerThrowExceptionWhenSettingInvalidConnString()
        {
            try
            {
                connStringManager.SetConnectionString("");
            }
            catch (Exception ex)
            {
                Assert.True(ex is ArgumentNullException, "ArgumentNullException should be thrown");
            }


            try
            {
                connStringManager.SetConnectionString(null);
            }
            catch (Exception ex)
            {
                Assert.True(ex is ArgumentNullException, "ArgumentNullException should be thrown");
            }
        }



        [Fact(DisplayName = "IConnectionStringManager build connectionstring ")]
        public void IConnectionStringManagerBuildsNewConnectionString()
        {
            string connectionstring = connStringManager.NewConnectionString(dbOptions);

            Assert.True(connectionstring != null, "Connection string should not be null");
            Assert.True(connectionstring.Contains("Database"), "Connection string should contain Database name");
            Assert.True(connectionstring.Contains(dbOptions.Host), "Connection string should contain Host");
            Assert.True(connectionstring.Contains(dbOptions.Password), "Connection string should contain Password");
            Assert.True(connectionstring.Contains(dbOptions.Port.ToString()), "Connection string should contain Port");
            Assert.True(connectionstring.Contains(dbOptions.UserName), "Connection string should contain UserName");
        }
    }
}