using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;
using PutridParrot.Options;

namespace Tests.PutridParrot.Options
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class ResultTests
    {
        // Some of these tests look like I'm testing the compiler, but
        // they were written to check the syntax etc. suits my use cases

        [Test]
        public void SuccessUsingPatternMatching()
        {
            var l = -1;
            Result<string, string> result = new Success<string, string>("Succeeded");
            switch (result)
            {
                case Success<string, string> success:
                    l = success.Value.Length;
                    break;
                case Failure<string, string> failure:
                    l = 0;
                    break;
            }

            Assert.AreEqual("Succeeded".Length, l);
        }

        [Test]
        public void FailureUsingPatternMatching()
        {
            var l = -1;
            Result<string, string> result = new Failure<string, string>("Failed");
            switch (result)
            {
                case Success<string, string> success:
                    l = success.Value.Length;
                    break;
                case Failure<string, string> failure:
                    l = 0;
                    break;
            }

            Assert.AreEqual(0, l);
        }

    }
}
