﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using TagsCloudResult.CircularCloudLayouter;

namespace TagsCloudResult.TagsCloudContainerTests
{
    [TestFixture]
    internal class WordLayouterShould
    {
        private WordLayouter _wordLayouter;

        [Test]
        public void ConstructorShouldNotFallWhenEmptyWordList()
        {
            Action act = () => GetCustomizedLayouter(x => new Size(), new List<string>());

            act.ShouldNotThrow();
        }

        [Test]
        public void SizeIncreasesAccordingToWordsNumber()
        {
            _wordLayouter = GetCustomizedLayouter(
                w => new Size(w.Count * 20 * w.Value.Length, w.Count * 20),
                new List<string> {"thelongword", "thelongword"});

            var itemsList = _wordLayouter.GetItemsToDraws().ToList();
            itemsList[0].Width.Should().Be(440);
            itemsList[0].Height.Should().Be(40);
        }

        [Test]
        public void ComplexTest()
        {
            _wordLayouter = GetCustomizedLayouter(
                w => new Size(w.Count * 20 * w.Value.Length, w.Count * 20),
                new List<string> {"one", "two", "three", "three"});

            var expected = new List<ItemToDraw<Word>>
            {
                new ItemToDraw<Word>(new Word("three", 2), 60, 0, 200, 40),
                new ItemToDraw<Word>(new Word("one", 1), 60, 40, 60, 20),
                new ItemToDraw<Word>(new Word("two", 1), 0, 0, 60, 20)
            };

            _wordLayouter.GetItemsToDraws().ToList().ShouldBeEquivalentTo(expected);
        }

        private static WordLayouter GetCustomizedLayouter(Func<Word, Size> getWordSize,
            IEnumerable<string> wordsToHandle, double directionValue = 1, Point center = new Point())
        {
            return new WordLayouter(
                new WordStorage(new WordsCustomizer(), wordsToHandle),
                getWordSize,
                new CircularCloudLayout(new RectangleStorage(center, new Direction(directionValue))));
        }
    }
}