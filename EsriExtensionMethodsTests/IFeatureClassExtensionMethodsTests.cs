using ESRI.ArcGIS.Geodatabase;
using EsriExtensionMethods;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;

namespace EsriExtensionMethodsTests
{
    /// <summary>
    /// Summary description for IFeatureClassExtensionMethodsTests
    /// </summary>
    [TestClass]
    public class IFeatureClassExtensionMethodsTests
    {
        private const bool Recycling = true;
        private static readonly IFeature NullFeature = null;

        [TestMethod]
        public void SearchWrapper_WithEmptyFeatureSetAndNullQuery_ShouldReturnEmptyList()
        {
            // arrange
            var emptyFC = Mock.Of<IFeatureClass>(fc =>
                fc.Search(null, Recycling) == Mock.Of<IFeatureCursor>(cursor =>
                    cursor.NextFeature() == NullFeature));

            // act
            var emptyFeatureSet = emptyFC.SearchWrapper(null, Recycling);

            // assert
            emptyFeatureSet.Should().BeEmpty();
        }

        [TestMethod]
        public void SearchWrapper_WithEmptyFeatureSetAndWithGivenArgs_ShouldReturnEmptyList()
        {
            // arrange
            var aQuery = Mock.Of<IQueryFilter>();
            var emptyFC = Mock.Of<IFeatureClass>(fc =>
                fc.Search(aQuery, Recycling) == Mock.Of<IFeatureCursor>(cursor =>
                    cursor.NextFeature() == NullFeature));

            // act
            var emptyRowSet = emptyFC.SearchWrapper(aQuery, Recycling);

            // assert
            emptyRowSet.Should().BeEmpty();
        }

        [TestMethod]
        public void SearchWrapper_WithGivenArgs_ShouldPassThemToSearch()
        {
            // arrange
            var aQuery = Mock.Of<IQueryFilter>();

            var emptyFC = new Mock<IFeatureClass>();
            emptyFC.Setup(fc => fc.Search(aQuery, Recycling)).Returns(
                Mock.Of<IFeatureCursor>(cursor => cursor.NextFeature() == NullFeature));

            // act
            Func<IEnumerable<IFeature>> act = () =>
                emptyFC.Object.SearchWrapper(aQuery, Recycling);

            // assert
            act.Enumerating().ShouldNotThrow<NullReferenceException>();
            emptyFC.Verify(fc => fc.Search(aQuery, Recycling), Times.Once());
          }

        // test: SearchWrapper should yield all elements that the cursor has to give
        [TestMethod]
        public void SearchWrapper_OnFeatureClassWithTwoFeatures_ShouldReturnThemAll()
        {
            // arrange
            var i = 0;
            var cursorMock = new Mock<IFeatureCursor>();
            cursorMock.Setup(cursor => cursor.NextFeature())
                .Returns(() => i++ < 2 ? Mock.Of<IFeature>() : null);
            var emptyTable = Mock.Of<IFeatureClass>(fc =>
                fc.Search(null, Recycling) == cursorMock.Object);

            // act
            var twoRows = emptyTable.SearchWrapper(null, Recycling);

            // assert
            twoRows.Should().HaveCount(2);
        }
    }

}
