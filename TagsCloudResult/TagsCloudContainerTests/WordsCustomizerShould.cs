﻿using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;

namespace TagsCloudResult.TagsCloudContainerTests
{
    [TestFixture]
    internal class WordsCustomizerShould
    {
        private WordsCustomizer _wordsCustomizer;

        [SetUp]
        public void SetUp()
        {
               _wordsCustomizer = new WordsCustomizer();
        }

        [Test]
        public void IgnorePrepositions()
        {
            foreach (var word in WordsCustomizer.Prepositions())
                _wordsCustomizer.CustomizeWord(word).IsSuccess.Should().Be(false);
        }

        [Test]
        public void IgnoreWordsSpecifiedByFunction()
        {
            _wordsCustomizer.SetIgnoreFunc(w => w.StartsWith("a"));
            _wordsCustomizer.CustomizeWord("attribute").IsSuccess.Should().Be(false);
            _wordsCustomizer.CustomizeWord("bool").Value.Should().Be("bool");
        }

        [Test]
        public void SetWordToLowerCase()
        {
            _wordsCustomizer.CustomizeWord("Word").Value.Should().Be("word");
        }

        [Test]
        public void SetWordsToIgnoreShouldReplaceOldWordsToIgnore()
        {
            var newWordsToIgnore = new List<string> {"new", "words", "to", "ignore"};
            _wordsCustomizer.SetWordsToIgnore(newWordsToIgnore);

            _wordsCustomizer.GetWordsToIgnore().ShouldBeEquivalentTo(newWordsToIgnore);
        }

        [Test]
        public void AddWordsToIgnoreShouldAddNewWordsWithoutReplacingOldOnes()
        {
            var newWordsToIgnore = new List<string> { "new", "words", "to", "ignore" };
            _wordsCustomizer.AddWordsToIgnore(newWordsToIgnore);

            var expected = new List<string>();
            expected.AddRange(WordsCustomizer.Prepositions());
            expected.AddRange(newWordsToIgnore);

            _wordsCustomizer.GetWordsToIgnore().ShouldBeEquivalentTo(expected);
        }

        [Test]
        public void ThrowNullReferenceExceptionOnNull()
        {
            Action act = () => _wordsCustomizer.CustomizeWord(null);

            act.ShouldThrow<NullReferenceException>();
        }
    }
}
