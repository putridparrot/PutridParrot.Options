using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using NUnit.Framework;
using PutridParrot.Options;

namespace Tests.PutridParrot.Options
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class OptionalTests
    {
        [Test]
        public void ConstructorWithNonNullExpectValidOption()
        {
            var optional = new Optional<string>("Hello World");

            Assert.That(optional.Value, Is.EqualTo("Hello World"));
        }

        [Test]
        public void ConstructorWithNullExpectArgumentException()
        {
            Assert.Throws<ArgumentNullException>(() => new Optional<string>(null));
        }

        [Test]
        public void IfWeTryToGetTheValueOnOptionalNoneExpectException()
        {
            var optional = Optional<string>.None;

            Assert.Throws<InvalidOperationException>(() =>
            {
                var r = optional.Value;
            });
        }

        [Test]
        public void IfWithNullPredicate()
        {
            var optional = "Hello World".ToOptional();

            Assert.Throws<ArgumentNullException>(() => optional.If((Predicate<string>)null));
        }

        [Test]
        public void OptionalNoneIsNoneShouldBeTrue()
        {
            var optional = new Optional<string>();

            Assert.That(optional.IsNone, Is.True);
        }

        [Test]
        public void OptionalSomeIsNoneShouldBeFalse()
        {
            var optional = new Optional<string>("Hello");

            Assert.That(optional.IsNone, Is.False);
        }

        [Test]
        public void IfWithSomeShouldReturnItself()
        {
            var result = new Optional<string>("Some Value");

            Assert.That(result.If(s => s == "Some Value"), Is.EqualTo(result));
        }

        [Test]
        public void IfWithDifferenceShouldReturnNone()
        {
            var result = new Optional<string>("Some Value");

            Assert.That(result.If(s => s == "Not Some Value"), Is.EqualTo(Optional<string>.None));
        }

        [Test]
        public void MapFromOptionalWithSomeToNewOptionalWithNewSome()
        {
            var optional = new Optional<string>("Some Value");

            var result = optional.Map(s => new Optional<Stream>(new MemoryStream()));

            Assert.That(result, Is.Not.EqualTo(Optional<Stream>.None));
        }

        [Test]
        public void MapWithOptionalNoneShouldReturnNone()
        {
            var optional = new Optional<string>();

            var result = optional.Map(s => new Optional<Stream>(new MemoryStream()));

            Assert.That(result, Is.EqualTo(Optional<Stream>.None));
        }

        [Test]
        public void MapWithMapperReturningNullExpectNullReferenceException()
        {
            var optional = "Hello World".ToOptional();

            Assert.Throws<NullReferenceException>(() => optional.Map(s => (Optional<string>)null));
        }


        [Test]
        public void MapWhereMapperFuncReturnsNullExpectOptionalNone()
        {
            var optional = new Optional<string>();

            var result = optional.Map(s => (Stream) null);

            Assert.That(result, Is.EqualTo(Optional<Stream>.None));
        }

        [Test]
        public void MapOverloadWhereMapperFuncReturnsNullExpectOptionalNone()
        {
            var optional = new Optional<string>();

            var result = optional.Map(s => (Optional<Stream>) null);

            Assert.That(result, Is.EqualTo(Optional<Stream>.None));
        }

        [Test]
        public void MapFromOptionalWithSomeButNullMapperExpectException()
        {
            var optional = new Optional<string>("Some Value");

            Assert.Throws<ArgumentNullException>(() => optional.Map((Func<string, Stream>) null));
        }

        [Test]
        public void MapOverloadFromOptionalWithSomeButNullMapperExpectException()
        {
            var optional = new Optional<string>("Some Value");

            Assert.Throws<ArgumentNullException>(() => optional.Map((Func<string, Optional<Stream>>) null));
        }

        [Test]
        public void OrWithOptionalSomeExpectSomeValue()
        {
            var optional = new Optional<string>("Some Value");

            var result = optional.Or("Not Some Value");

            Assert.That(result, Is.EqualTo("Some Value"));
        }

        [Test]
        public void OrWithOptionalNoneExpectOrValue()
        {
            var optional = new Optional<string>();

            var result = optional.Or("Not Some Value");

            Assert.That(result, Is.EqualTo("Not Some Value"));
        }

        [Test]
        public void OrWithOptionalSomeExpectOptionalSomeValue()
        {
            var optional = new Optional<string>("Some Value");

            var result = optional.Or(() => "Not Some Value");

            Assert.That(result, Is.EqualTo("Some Value"));
        }

        [Test]
        public void OrWithOptionalNoneExpectFunctionSuppliedValue()
        {
            var optional = new Optional<string>();

            var result = optional.Or(() => "Not Some Value");

            Assert.That(result, Is.EqualTo("Not Some Value"));
        }

        [Test]
        public void OrWithExceptionAndOptionalSomeExpectOptionalValue()
        {
            var optional = new Optional<string>("Some Value");

            var result = optional.OrException(() => new ArgumentException());

            Assert.That(result, Is.EqualTo("Some Value"));
        }

        [Test]
        public void OrWithExceptionAndOptionalNoneExpectException()
        {
            var optional = new Optional<string>();

            Assert.Throws<ArgumentException>(() => optional.OrException(() => new ArgumentException()));
        }

        [Test]
        public void OrWithNullFunctionExpectArgumentNullException()
        {
            var optional = new Optional<string>();

            Assert.Throws<ArgumentNullException>(() => optional.OrException((Func<Exception>)null));
        }

        [Test]
        public void EqualsWithItselfExpectTrue()
        {
            var a = new Optional<string>("Some Value");

            Assert.That(a.Equals(a), Is.True);
        }

        [Test]
        public void EqualsWithTwoOptionalsWithSameValuesExpectTrue()
        {
            var a = new Optional<string>("Some Value");
            var b = new Optional<string>("Some Value");

            Assert.That(a.Equals(b), Is.True);
        }

        [Test]
        public void EqualsWithTwoOptionalsWithDifferentValuesExpectFalse()
        {
            var a = new Optional<string>("Some Value");
            var b = new Optional<string>("Not Some Value");

            Assert.That(a.Equals(b), Is.False);
        }

        [Test]
        public void OperatorEqualsWithLeftNullAndValueExpectFalse()
        {
            var a = (Optional<string>) null;
            var b = new Optional<string>("Not Some Value");

            Assert.That(a == b, Is.False);
        }

        [Test]
        public void OperatorEqualsWithRightNullAndValueExpectFalse()
        {
            var a = new Optional<string>("Some Value");
            var b = (Optional<string>) null;

            Assert.That(a == b, Is.False);
        }

        [Test]
        public void OperatorEqualsWithDifferentValuesExpectFalse()
        {
            var a = new Optional<string>("Some Value");
            var b = new Optional<string>("Not Some Value");

            Assert.That(a == b, Is.False);
        }

        [Test]
        public void OperatorNotEqualsWithDifferentValuesExpectTrue()
        {
            var a = new Optional<string>("Some Value");
            var b = new Optional<string>("Not Some Value");

            Assert.That(a != b, Is.True);
        }

        [Test]
        public void DoWithNullActionExpectArgumentNullException()
        {
            var optional = new Optional<string>("Some Value");

            Assert.Throws<ArgumentNullException>(() => optional.Do(null));
        }

        [Test]
        public void DoWithOptionalSomeExpectActionInvoked()
        {
            var optional = new Optional<string>("Some Value");

            var hasValue = false;
            optional.Do(s => hasValue = true);

            Assert.That(hasValue, Is.True);
        }

        [Test]
        public void DoWithOptionalNoneExpectNoActionInvoked()
        {
            var optional = new Optional<string>();

            var hasValue = false;
            optional.Do(s => hasValue = true);

            Assert.That(hasValue, Is.False);
        }

        [Test]
        public void ToOptionalWithNonNullExpectNewOptionalWithValue()
        {
            var optional = "Hello World".ToOptional();

            Assert.That(optional.Value, Is.EqualTo("Hello World"));
        }

        [Test]
        public void ToOptionalWithNullValueExpectOptionalNone()
        {
            var optional = ((string) null).ToOptional();

            Assert.That(optional, Is.EqualTo(Optional<string>.None));
        }

        [Test]
        public void CastFromBaseToSuperExpectOptionalSome()
        {
            var optional = ((Stream) new MemoryStream()).ToOptional();
            var cast = optional.Cast<Stream, MemoryStream>();

            Assert.That(cast, Is.Not.EqualTo(Optional<Stream>.None));
        }

        [Test]
        public void CastFromInvalidTypeExpectInvalidCastException()
        {
            var optional = ((Stream) new MemoryStream()).ToOptional();
            Assert.Throws<InvalidCastException>(() => optional.Cast<Stream, FileStream>());
        }

        [Test]
        public void SafeCastFromInvalidTypeExpectOptionalNone()
        {
            var optional = ((Stream) new MemoryStream()).ToOptional();
            var cast = optional.SafeCast<Stream, FileStream>();

            Assert.That(cast, Is.EqualTo(Optional<FileStream>.None));
        }

        [Test]
        public void ToStringWithOptionalSomeExpectOptionalAndValue()
        {
            var optional = "Hello World".ToOptional();

            Assert.That(optional.ToString(), Is.EqualTo("Optional[Hello World]"));
        }

        [Test]
        public void ToStringWithOptionalNoneExpectOptionalNone()
        {
            var optional = Optional<string>.None;

            Assert.That(optional.ToString(), Is.EqualTo("Optional.None"));
        }

        [Test]
        public void IfTrueExpectOptionalSome()
        {
            var optional = "Hello World".ToOptional();

            Assert.That(optional.If(true), Is.EqualTo(optional));
        }

        [Test]
        public void IfFalseExpectOptionalNone()
        {
            var optional = "Hello World".ToOptional();

            Assert.That(optional.If(false), Is.EqualTo(Optional<string>.None));
        }

        [Test]
        public void GetValueOfDefaultIsOptionalIsSomeThenExpectTheOptionalValue()
        {
            var optional = "Hello World".ToOptional();

            Assert.That(optional.GetValueOfDefault(), Is.EqualTo("Hello World"));
        }

        [Test]
        public void GetValueOfDefaultIsOptionalIsNoneWithNoDefaultThenExpectDefaulOfT()
        {
            var optional = ((string) null).ToOptional();

            Assert.That(optional.GetValueOfDefault(), Is.EqualTo(default(string)));
        }

        [Test]
        public void GetValueOfDefaultIsOptionalIsNoneWithDefaultThenExpectDefaulOfT()
        {
            var optional = ((string) null).ToOptional();

            Assert.That(optional.GetValueOfDefault("Hello World"), Is.EqualTo("Hello World"));
        }

        [Test]
        public void MatchSomeThenWithSomeValueShouldReturnValueResult()
        {
            var o = "Hello World".ToOptional();

            var length = o.Match(s => s.Length, 0);
            Assert.That(length, Is.EqualTo(11));
        }

        [Test]
        public void MatchSomeThenWithNoneShouldReturnElseValue()
        {
            var o = Optional<string>.None;

            var length = o.Match(s => s.Length, 23);

            Assert.That(length, Is.EqualTo(23));
        }

        [Test]
        public void MatchSomeThenWithNoneAndUseDefaultShouldReturnElseValue()
        {
            var o = Optional<string>.None;

            var length = o.Match(s => s.Length);

            Assert.That(length, Is.EqualTo(0));
        }

        [Test]
        public void MatchWithNullFunctionExpectException()
        {
            var o = "Hello World".ToOptional();

            Assert.Throws<ArgumentNullException>(() => o.Match((Func<string, int>) null));
        }
    }
}