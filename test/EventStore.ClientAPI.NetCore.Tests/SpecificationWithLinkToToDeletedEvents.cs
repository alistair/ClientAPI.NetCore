﻿using System;
using System.Text;
using EventStore.ClientAPI;
using EventStore.ClientAPI.Common;

namespace Eventstore.ClientAPI.Tests
{
    public abstract class SpecificationWithLinkToToDeletedEvents : SpecificationWithConnection
    {
        protected string LinkedStreamName;
        protected string DeletedStreamName;
        protected override void Given()
        {
            var creds = DefaultData.AdminCredentials;
            LinkedStreamName = Guid.NewGuid().ToString();
            DeletedStreamName = Guid.NewGuid().ToString();
            _conn.AppendToStreamAsync(DeletedStreamName, ExpectedVersion.Any, creds,
                new EventData(Guid.NewGuid(), "testing", true, Encoding.UTF8.GetBytes("{'foo' : 4}"), new byte[0])).Wait();
            _conn.AppendToStreamAsync(LinkedStreamName, ExpectedVersion.Any, creds,
                new EventData(Guid.NewGuid(), SystemEventTypes.LinkTo, false, Encoding.UTF8.GetBytes("0@" + DeletedStreamName), new byte[0])).Wait();
            _conn.DeleteStreamAsync(DeletedStreamName, ExpectedVersion.Any).Wait();
        }
    }
}