using System;
using System.Collections.Generic;
using System.Text;

using Xunit;

namespace Astron.Files.Tests
{
    public class PathValidationTests
    {
        [Theory]
        [InlineData(@"C:\Users\")]
        [InlineData(@"C:\Windows\notepad.exe")]
        public void IsValid_ShouldBeTrue(string path)
        {
            var validate = new PathValidation();
            Assert.True(validate.IsValid(path));
        }

        [Theory]
        [InlineData(@"454:*/")]
        [InlineData(@"C:\?:")]
        public void IsValid_ShouldBeFalse(string path)
        {
            var validate = new PathValidation();
            Assert.False(validate.IsValid(path));
        }
    }
}
