using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using HGGM.Models;
using HGGM.Services;
using LiteDB;
using Xunit;

namespace HGGM.UnitTests.Services
{
    public class AuditServiceTests
    {
        private readonly LiteRepository db = new LiteRepository(new MemoryStream());

        private class First : AuditEntryBase
        {
            public override string Message { get; }
        }
        private class Second : AuditEntryBase
        {
            public override string Message { get; }
        }
        [Fact]
        public void AuditItemKeepsType()
        {
            var audit = new AuditService(db);
            audit.Add(new First());
            audit.Add(new Second());
            var list = audit.GetAll();
            list.OfType<First>().Should().HaveCount(1);
            list.OfType<Second>().Should().HaveCount(1);
        }
    }
}
