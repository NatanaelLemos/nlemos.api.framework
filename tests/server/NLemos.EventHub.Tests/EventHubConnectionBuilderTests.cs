using System;
using NLemos.EventHub.Configuration;
using Xunit;

namespace NLemos.EventHub.Tests
{
    public class EventHubConnectionBuilderTests
    {
        [Fact]
        public void AddConnection_Should_Add_Connection_Successfully()
        {
            try
            {
                var connectionBuilder = new EventHubConnectionBuilder();
                connectionBuilder.AddConnection("host", "user", "pass", EventHubConnectionType.Server);
                Assert.True(true);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.ToString());
            }
        }

        [Fact]
        public void AddQueue_Should_Add_Queue_Successfully()
        {
            try
            {
                var connectionBuilder = new EventHubConnectionBuilder()
                    .AddConnection("host", "user", "pass", EventHubConnectionType.Server)
                    .AddQueue("queue");
                Assert.True(true);
            }
            catch (Exception ex)
            {
                Assert.True(false, ex.ToString());
            }
        }

        [Fact]
        public void AddQueue_Throws_NullReferenceException_If_No_Connection_Setup()
        {
            Assert.Throws<NullReferenceException>(
                () => new EventHubConnectionBuilder()
                    .AddQueue("queue")
            );
        }

        [Fact]
        public void AddQueue_Throws_ArgumentException_If_QueueName_Empty()
        {
            var connectionBuilder = new EventHubConnectionBuilder()
                .AddConnection("host", "user", "pass", EventHubConnectionType.Server);

            Assert.Throws<ArgumentException>(() => connectionBuilder.AddQueue(""));
        }

        [Fact]
        public void GetServiceConnection_Should_Retrieve_Added_Connection()
        {
            const string expectedHostname = "host";
            const string expectedUsername = "user";
            const string expectedPassword = "password";
            const string expectedQueue = "queue";

            var connectionBuilder = new EventHubConnectionBuilder()
                .AddConnection(expectedHostname, expectedUsername, expectedPassword, EventHubConnectionType.Server)
                .AddQueue(expectedQueue);

            var connection = connectionBuilder.GetServiceConnection(expectedQueue);
            Assert.Equal(expectedHostname, connection.Hostname);
            Assert.Equal(expectedUsername, connection.Username);
            Assert.Equal(expectedPassword, connection.Password);
        }

        [Fact]
        public void GetServiceConnection_Should_Retrieve_Multiple_Added_Connection()
        {
            var connectionBuilder = new EventHubConnectionBuilder();

            for (var i = 0; i < 10; i++)
            {
                connectionBuilder.AddConnection(i.ToString(), i.ToString(), i.ToString(), EventHubConnectionType.Server);

                for (var j = 0; j < 10; j++)
                {
                    connectionBuilder.AddQueue(i.ToString() + j.ToString());
                }
            }

            for (var i = 0; i < 10; i++)
            {
                for (var j = 0; j < 10; j++)
                {
                    var connection = connectionBuilder.GetServiceConnection(i.ToString() + j.ToString());
                    Assert.NotNull(connection);
                    Assert.Equal(i.ToString(), connection.Hostname);
                    Assert.Equal(i.ToString(), connection.Username);
                    Assert.Equal(i.ToString(), connection.Password);
                }
            }
        }

        [Fact]
        public void GetServiceConnection_Returns_Null_If_No_Connection_Found()
        {
            var connectionBuilder = new EventHubConnectionBuilder();
            var connection = connectionBuilder.GetServiceConnection("thisQueueDoesntExist");
            Assert.Null(connection);
        }
    }
}
