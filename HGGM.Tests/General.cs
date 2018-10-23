using System;
using FluentAssertions;
using Xunit;

namespace HGGM.Tests
{
    public class General
    {
        [Fact]
        public void ProjectExists()
        {
            new Program().Should().NotBeNull();
        }
    }
}
