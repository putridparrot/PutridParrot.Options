using System;
using System.IO;
using NUnit.Framework;
using PutridParrot.Options;

namespace Tests.PutridParrot.Options
{
    [TestFixture]
    public class OptionTests
    {
        public class Person
        {
        }

        [Test]
        public void NullValueBecomesNone()
        {
            Person p = null;
            var optional = p.ToOption();

            Assert.IsInstanceOf<None<Person>>(optional);
        }

        [Test]
        public void NonNullValueBecomesSome()
        {
            var p = new Person();
            var optional = p.ToOption();

            Assert.IsInstanceOf<Some<Person>>(optional);
        }

        [Test]
        public void GetValueFromNoneExpectException()
        {
            var none = Option<Person>.None;
            Assert.Throws<InvalidOperationException>(() =>
            {
                var p = none.Value;
            });
        }

        [Test]
        public void SomeInPatternMatchSwitch()
        {
            var option = new Person().ToOption();
            switch (option)
            {
                case Some<Person> some:
                    break;
                case None<Person> none:
                    Assert.Fail("Should be Some<>");
                    break;
            }
        }

        [Test]
        public void NoneInPatternMatchSwitch()
        {
            Person p = null;
            var option = p.ToOption();
            switch (option)
            {
                case Some<Person> some:
                    Assert.Fail("Should be None<>");
                    break;
                case None<Person> none:
                    break;
            }
        }

        [Test]
        public void FSharpStylePatternMatchSample()
        {
            /*
                let option = Option<string>.None
                let l = match option with
                | Some s -> s.Length
                | None -> 0
            */

            var option = Option<string>.None;
            var l = -1;
            switch (option)
            {
                case Some<string> some:
                    l = some.Value.Length;
                    break;
                case None<string> none:
                    l = 0;
                    break;
            }

            Assert.AreEqual(0, l);
        }

        [Test]
        public void IsNoneWhenSomeExpectFalse()
        {
            var some = "Hello".ToOption();
            Assert.IsFalse(some.IsNone());
        }

        [Test]
        public void IsNoneWhenNoneExpectTrue()
        {
            var none = Option<string>.None;
            Assert.IsTrue(none.IsNone());
        }

        [Test]
        public void IsSomeWhenSomeExpectTrue()
        {
            var some = "Hello".ToOption();
            Assert.IsTrue(some.IsSome());
        }

        [Test]
        public void IsSomeWhenNoneExpectFalse()
        {
            var none = Option<string>.None;
            Assert.IsFalse(none.IsSome());
        }

        [Test]
        public void IfWithNullPredicate()
        {
            var option = "Hello World".ToOption();

            Assert.Throws<ArgumentNullException>(() => option.If((Predicate<string>)null));
        }

        //

        [Test]
        public void IfWithSomeShouldReturnItself()
        {
            var result = "Some Value".ToOption();

            Assert.That(result.If(s => s == "Some Value"), Is.EqualTo(result));
        }

        [Test]
        public void IfWithDifferenceShouldReturnNone()
        {
            var result = "Some Value".ToOption();

            Assert.That(result.If(s => s == "Not Some Value"), Is.EqualTo(Option<string>.None));
        }

        [Test]
        public void MapFromOptionWithSomeToNewOptionWithNewSome()
        {
            var option = "Some Value".ToOption();

            var result = option.Map(s => new MemoryStream().ToOption());

            Assert.That(result, Is.Not.EqualTo(Option<Stream>.None));
        }

        [Test]
        public void MapWithOptionNoneShouldReturnNone()
        {
            var option = Option<string>.None;

            var result = option.Map(s => new MemoryStream().ToOption());

            Assert.That(result, Is.EqualTo(Option<MemoryStream>.None));
        }

        [Test]
        public void MapWithMapperReturningNullExpectNullReferenceException()
        {
            var option = "Hello World".ToOption();

            Assert.Throws<NullReferenceException>(() => option.Map(s => (Option<string>)null));
        }


        [Test]
        public void MapWhereMapperFuncReturnsNullExpectOptionNone()
        {
            var option = Option<string>.None;

            var result = option.Map(s => (Stream)null);

            Assert.That(result, Is.EqualTo(Option<Stream>.None));
        }

        [Test]
        public void MapOverloadWhereMapperFuncReturnsNullExpectOptionNone()
        {
            var option = Option<string>.None;

            var result = option.Map(s => (Option<Stream>)null);

            Assert.That(result, Is.EqualTo(Option<Stream>.None));
        }

        [Test]
        public void MapFromOptionWithSomeButNullMapperExpectException()
        {
            var option = "Some Value".ToOption();

            Assert.Throws<ArgumentNullException>(() => option.Map((Func<string, Stream>)null));
        }

        [Test]
        public void MapOverloadFromOptionalWithSomeButNullMapperExpectException()
        {
            var option = "Some Value".ToOption();

            Assert.Throws<ArgumentNullException>(() => option.Map((Func<string, Option<Stream>>)null));
        }

        [Test]
        public void OrWithOptionSomeExpectSomeValue()
        {
            var option = "Some Value".ToOption();

            var result = option.Or("Not Some Value");

            Assert.That(result, Is.EqualTo("Some Value"));
        }

        [Test]
        public void OrWithOptionNoneExpectOrValue()
        {
            var option = Option<string>.None;

            var result = option.Or("Not Some Value");

            Assert.That(result, Is.EqualTo("Not Some Value"));
        }

        [Test]
        public void OrWithOptionSomeExpectOptionSomeValue()
        {
            var option = "Some Value".ToOption();

            var result = option.Or(() => "Not Some Value");

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
        public void OrWithExceptionAndOptionSomeExpectOptionalValue()
        {
            var option = "Some Value".ToOption();

            var result = option.OrException(() => new ArgumentException());

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
            var option = Option<string>.None;

            Assert.Throws<ArgumentNullException>(() => option.OrException((Func<Exception>)null));
        }

        [Test]
        public void EqualsWithItselfExpectTrue()
        {
            var a = "Some Value".ToOption();

            Assert.That(a.Equals(a), Is.True);
        }

        [Test]
        public void EqualsWithTwoOptionsWithSameValuesExpectTrue()
        {
            var a = "Some Value".ToOption();
            var b = "Some Value".ToOption();

            Assert.That(a.Equals(b), Is.True);
        }

        [Test]
        public void EqualsWithTwoOptionsWithDifferentValuesExpectFalse()
        {
            var a = "Some Value".ToOption();
            var b = "Not Some Value".ToOption();

            Assert.That(a.Equals(b), Is.False);
        }

        [Test]
        public void OperatorEqualsWithLeftNullAndValueExpectFalse()
        {
            var a = (Option<string>)null;
            var b = "Not Some Value".ToOption();

            Assert.That(a == b, Is.False);
        }

        [Test]
        public void OperatorEqualsWithRightNullAndValueExpectFalse()
        {
            var a = "Some Value".ToOption();
            var b = (Option<string>)null;

            Assert.That(a == b, Is.False);
        }

        [Test]
        public void OperatorEqualsWithDifferentValuesExpectFalse()
        {
            var a = "Some Value".ToOption();
            var b = "Not Some Value".ToOption();

            Assert.That(a == b, Is.False);
        }

        [Test]
        public void OperatorNotEqualsWithDifferentValuesExpectTrue()
        {
            var a = "Some Value".ToOption();
            var b = "Not Some Value".ToOption();

            Assert.That(a != b, Is.True);
        }

        [Test]
        public void DoWithNullActionExpectArgumentNullException()
        {
            var option = "Some Value".ToOption();

            Assert.Throws<ArgumentNullException>(() => option.Do(null));
        }

        [Test]
        public void DoWithOptionSomeExpectActionInvoked()
        {
            var option = "Some Value".ToOption();

            var hasValue = false;
            option.Do(s => hasValue = true);

            Assert.That(hasValue, Is.True);
        }

        [Test]
        public void DoWithOptionNoneExpectNoActionInvoked()
        {
            var option = Option<string>.None;

            var hasValue = false;
            option.Do(s => hasValue = true);

            Assert.That(hasValue, Is.False);
        }

        [Test]
        public void ToOptionWithNonNullExpectNewOptionWithValue()
        {
            var option = "Hello World".ToOption();

            Assert.That(option.Value, Is.EqualTo("Hello World"));
        }

        [Test]
        public void ToOptionWithNullValueExpectOptionNone()
        {
            var option = ((string)null).ToOption();

            Assert.That(option, Is.EqualTo(Option<string>.None));
        }

        [Test]
        public void CastFromBaseToSuperExpectOptionSome()
        {
            var option = ((Stream)new MemoryStream()).ToOption();
            var cast = option.Cast<Stream, MemoryStream>();

            Assert.That(cast, Is.Not.EqualTo(Option<Stream>.None));
        }

        [Test]
        public void CastFromInvalidTypeExpectInvalidCastException()
        {
            var option = ((Stream)new MemoryStream()).ToOption();
            Assert.Throws<InvalidCastException>(() => option.Cast<Stream, FileStream>());
        }

        [Test]
        public void SafeCastFromInvalidTypeExpectOptionalNone()
        {
            var option = ((Stream)new MemoryStream()).ToOption();
            var cast = option.SafeCast<Stream, FileStream>();

            Assert.That(cast, Is.EqualTo(Option<FileStream>.None));
        }

        [Test]
        public void ToStringWithOptionSomeExpectOptionAndValue()
        {
            var option = "Hello World".ToOption();

            Assert.That(option.ToString(), Is.EqualTo("Option[Hello World]"));
        }

        [Test]
        public void ToStringWithOptionNoneExpectOptionNone()
        {
            var option = Option<string>.None;

            Assert.That(option.ToString(), Is.EqualTo("Option.None"));
        }

        [Test]
        public void IfTrueExpectOptionSome()
        {
            var option = "Hello World".ToOption();

            Assert.That(option.If(true), Is.EqualTo(option));
        }

        [Test]
        public void IfFalseExpectOptionNone()
        {
            var option = "Hello World".ToOption();

            Assert.That(option.If(false), Is.EqualTo(Option<string>.None));
        }

        [Test]
        public void GetValueOfDefaultIsOptionIsSomeThenExpectTheOptionValue()
        {
            var option = "Hello World".ToOption();

            Assert.That(option.GetValueOfDefault(), Is.EqualTo("Hello World"));
        }

        [Test]
        public void GetValueOfDefaultIsOptionIsNoneWithNoDefaultThenExpectDefaulOfT()
        {
            var option = ((string)null).ToOption();

            Assert.That(option.GetValueOfDefault(), Is.EqualTo(default(string)));
        }

        [Test]
        public void GetValueOfDefaultIsOptionIsNoneWithDefaultThenExpectDefaulOfT()
        {
            var option = ((string)null).ToOption();

            Assert.That(option.GetValueOfDefault("Hello World"), Is.EqualTo("Hello World"));
        }

        [Test]
        public void MatchSomeThenWithSomeValueShouldReturnValueResult()
        {
            var o = "Hello World".ToOption();

            var length = o.Match(s => s.Length, 0);
            Assert.That(length, Is.EqualTo(11));
        }

        [Test]
        public void MatchSomeThenWithNoneShouldReturnElseValue()
        {
            var o = Option<string>.None;

            var length = o.Match(s => s.Length, 23);

            Assert.That(length, Is.EqualTo(23));
        }

        [Test]
        public void MatchSomeThenWithNoneAndUseDefaultShouldReturnElseValue()
        {
            var o = Option<string>.None;

            var length = o.Match(s => s.Length);

            Assert.That(length, Is.EqualTo(0));
        }

        [Test]
        public void MatchWithNullFunctionExpectException()
        {
            var o = "Hello World".ToOption();

            Assert.Throws<ArgumentNullException>(() => o.Match((Func<string, int>)null));
        }
    }
}
