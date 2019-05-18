using System;
using Xunit;

namespace Astron.Files.Tests
{
    public class FileNameValidationTests
    {
        [Theory]
        [InlineData(@"file.exe")]
        [InlineData(@"log.txt")]
        public void IsValid_ShouldBeTrue(string path)
        {
            var validate = new FileNameValidation();
            Assert.True(validate.IsValid(path));
        }

        [Theory]
        [InlineData(@"file:first.exe")]
        [InlineData(@"isThisAfile?.txt")]
        public void IsValid_ShouldBeFalse(string path)
        {
            var validate = new FileNameValidation();
            Assert.False(validate.IsValid(path));
        }
    }
}
