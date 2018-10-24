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

        [Fact]
        public void WebHostBuilds()
        {
            Program.CreateWebHostBuilder(new string[0]).Build().Should().NotBeNull();
        }
    }
}
