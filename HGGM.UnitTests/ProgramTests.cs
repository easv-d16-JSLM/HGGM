using System;
using FluentAssertions;
using Xunit;

namespace HGGM.UnitTests
{
    public class ProgramTests
    {
        [Fact]
        public void ProjectExists()
        {
            new Program().Should().NotBeNull();
        }
    }
}
