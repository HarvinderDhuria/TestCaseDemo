using System;
using System.Collections.Generic;
using Moq;
using Xunit;

namespace TestCaseDemo
{
    public class NameExtractionTest
    {
        private Mock<INameProvider> _provider;
        private readonly NameExtractor _nameExtractor;
        public NameExtractionTest()
        {
            _provider = new Mock<INameProvider>();
            _nameExtractor = new NameExtractor(_provider.Object);
        }

        [Fact]
        public void SingleWordShouldBeTheLastName()
        {
            _nameExtractor.NameProvider.Name = "Cruise";
            var response = _nameExtractor.NameProvider.Extract();
            Assert.True(response.LastName == "Cruise");
        }

        [Fact]
        public void ShouldSetSecondWordAsLastNameIfFirstWordIsTitle()
        {
            _nameExtractor.NameProvider.Name = "Mr Anderson";
            var response = _nameExtractor.NameProvider.Extract();
            Assert.True(response.LastName == "Anderson");
        }

        [Fact]
        public void ShouldSetFirstNameIfThereAreTwoOrMoreWords_AndFirstWordIsNotTitle()
        {
            _nameExtractor.NameProvider.Name = "Peter Parker";
            var response = _nameExtractor.NameProvider.Extract();
            Assert.True(response.FirstName == "Peter");
        }

        [Fact]
        public void ShouldSetLastNameIfThereAreTwoOrMoreWords_AndFirstWordIsNotTitle()
        {
            _nameExtractor.NameProvider.Name = "Peter Parker";
            var response = _nameExtractor.NameProvider.Extract();
            Assert.True(response.LastName == "Parker");
        }

        [Fact]
        public void TitleShouldBeTheFirstWordInName()
        {
            _nameExtractor.NameProvider.Name = "Dr John Watson";
            var response = _nameExtractor.NameProvider.Extract();
            Assert.True(response.Title == "Dr");
        }

        [Fact]
        public void TitleShouldNotAppearInName_AfterFirstPosition()
        {
            _nameExtractor.NameProvider.Name = "John Dr Watson";
            var ex = Assert.Throws<Exception>(() => _nameExtractor.NameProvider.Extract());
            Assert.Equal(ex.Message,"Title should not appear in the middle of the name");
        }

        [Fact]
        public void ShouldThrowsException_IfThereAreMoreThanThreeWords_InName()
        {
            _nameExtractor.NameProvider.Name = "Name is Bond James Bond";
            var ex = Assert.Throws<Exception>(() => _nameExtractor.NameProvider.Extract());
            Assert.Equal(ex.Message, "Name should not have more than three words in it");
        }
  
    }

    public class NameExtractionResult
    {
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class NameExtractor
    {
        public INameProvider NameProvider { get; set; }
        public NameExtractor(INameProvider provider)
        {
            NameProvider = provider;
        }
    }

    public interface INameProvider
    {
        string Name { get; set; }

        NameExtractionResult Extract();
    }
}
