using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NLemos.EventHub.Configuration;
using Xunit;

namespace NLemos.EventHub.Tests
{
    public class EventHubHandlerTests : EventHubTestsBase
    {
        public EventHubHandlerTests() : base(new EventHubConnectionBuilder()
            .AddConnection("local.dev", "servicesUser", "servicesPwd", EventHubConnectionType.Server))
        {
        }

        #region Golden paths

        [Fact]
        public void Event_Should_Be_Sent_Successfully()
        {
            const string queue = "test1";

            try
            {
                var body = new
                {
                    Property1 = "We hope there's no problems here"
                };

                var expectedResponse = JsonConvert.SerializeObject(body);

                _defaultConnection.AddQueue(queue);

                using (var handler = new EventHubHandler(_defaultConnection))
                {
                    handler.SendEvent(queue, body);
                    var response = ReceiveEvent(queue);
                    Assert.Equal(expectedResponse, response);
                }
            }
            finally
            {
                CleanPipes(queue);
            }
        }

        [Fact]
        public void Event_Should_Be_Sent_Raw_Successfully()
        {
            const string queue = "test1";

            try
            {
                var body = "{\"Property1\": \"We hope there's no problems here\"}";

                _defaultConnection.AddQueue(queue);

                using (var handler = new EventHubHandler(_defaultConnection))
                {
                    handler.SendRawEvent(queue, body);
                    var response = ReceiveEvent(queue);
                    Assert.Equal(body, response);
                }
            }
            finally
            {
                CleanPipes(queue);
            }
        }

        [Fact]
        public void Event_Should_Be_Received_Successfully()
        {
            const string queue = "test2";
            const string expectedResult = "this is a test message";

            try
            {
                var testWorked = false;
                _defaultConnection.AddQueue(queue);

                using (var handler = new EventHubHandler(_defaultConnection))
                {
                    handler.OnEventReceived<TestClass1>(queue, result =>
                    {
                        Assert.Equal(expectedResult, result.Property);
                        testWorked = true;
                        return Task.CompletedTask;
                    });

                    SendEvent(queue, JsonConvert.SerializeObject(new TestClass1
                    {
                        Property = expectedResult
                    }));

                    Thread.Sleep(200);
                }

                if (!testWorked)
                {
                    Assert.True(false, "Test didn't work");
                }
            }
            finally
            {
                CleanPipes(queue);
            }
        }

        [Fact]
        public void Event_Should_Be_Received_And_Deserialized_To_All_Observer_Classes()
        {
            const string queue = "test3";
            const string expectedProperty = "this is a test message";
            const string expectedProperty1 = "this is another test message";

            try
            {
                var testWorked = new List<bool>();
                _defaultConnection.AddQueue(queue);

                using (var handler = new EventHubHandler(_defaultConnection))
                {
                    handler.OnEventReceived<TestClass1>(queue, result =>
                    {
                        Assert.Equal(expectedProperty, result.Property);
                        testWorked.Add(true);
                        return Task.CompletedTask;
                    });

                    handler.OnEventReceived<TestClass2>(queue, result =>
                    {
                        Assert.Equal(expectedProperty, result.Property);
                        Assert.Equal(expectedProperty1, result.Property1);
                        testWorked.Add(true);
                        return Task.CompletedTask;
                    });

                    SendEvent(queue, JsonConvert.SerializeObject(new TestClass2
                    {
                        Property = expectedProperty,
                        Property1 = expectedProperty1
                    }));

                    Thread.Sleep(300);
                }

                if (testWorked.Count < 2 || testWorked.Any(t => !t))
                {
                    Assert.True(false, "Test didn't work");
                }
            }
            finally
            {
                CleanPipes(queue);
            }
        }

        #endregion Golden paths

        #region Validations

        [Fact]
        public void SendEvent_Throws_ArgumentException_For_Empty_QueueName()
        {
            using (var handler = new EventHubHandler(_defaultConnection))
            {
                Assert.Throws<ArgumentException>(() => handler.SendEvent("", new { }));
            }
        }

        [Fact]
        public void SendEvent_Throws_ArgumentException_If_Connection_Not_Found()
        {
            using (var handler = new EventHubHandler(_defaultConnection))
            {
                Assert.Throws<ArgumentException>(() => handler.SendEvent("thisQueueDoesntExist", new { }));
            }
        }

        #endregion Validations

        #region Helper classes

        private class TestClass1
        {
            public string Property { get; set; }
        }

        private class TestClass2
        {
            public string Property { get; set; }
            public string Property1 { get; set; }
        }

        #endregion Helper classes
    }
}
