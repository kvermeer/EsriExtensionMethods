using ESRI.ArcGIS.Geodatabase;
using EsriExtensionMethods;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace GeluidRegister.Tests.EsriExtensionMethods.Staging
{
    [TestClass]
    public class IRowExtensionMethodsTests
    {
        [TestMethod]
        public void GetValue_ExistingFieldName_ReturnsValue()
        {
            // arrange
            var nonExistingColumn = GetAColumnName();
            var colIndex = 0;
            var expectedValue = GetAValue();

            var fieldsMock = new Mock<IFields>();
            fieldsMock.Setup(f => f.FindField(It.IsAny<string>())).Returns(colIndex);

            var rowMock = new Mock<IRow>();
            rowMock.Setup(r => r.Fields).Returns(fieldsMock.Object);
            rowMock.Setup(r => r.get_Value(colIndex)).Returns(expectedValue);

            // act
            var result = rowMock.Object.GetValue(nonExistingColumn);

            // assert
            result.Should().Be(expectedValue);
        }

        [TestMethod]
        public void GetValue_DbNullValue_ReturnsNull()
        {
            // arrange
            var nonExistingColumn = GetAColumnName();
            var colIndex = 0;

            var fieldsMock = new Mock<IFields>();
            fieldsMock.Setup(f => f.FindField(It.IsAny<string>())).Returns(colIndex);

            var rowMock = new Mock<IRow>();
            rowMock.Setup(r => r.Fields).Returns(fieldsMock.Object);
            rowMock.Setup(r => r.get_Value(colIndex)).Returns(DBNull.Value);

            // act
            var result = rowMock.Object.GetValue(nonExistingColumn);

            // assert
            result.Should().BeNull();
        }

        [TestMethod]
        public void GetValue_OnNonExistingFieldName_ShouldThrow()
        {
            // arrange
            var nonExistingColumn = GetAColumnName();

            var fieldsMock = new Mock<IFields>();
            fieldsMock.Setup(f => f.FindField(It.IsAny<string>())).Returns(-1);

            var rowWithoutColumns = new Mock<IRow>();
            rowWithoutColumns.Setup(r => r.Fields).Returns(fieldsMock.Object);

            // act
            Action act = () =>
                rowWithoutColumns.Object.GetValue(nonExistingColumn);

            // assert
            act.ShouldThrow<Exception>();
        }

        private static string GetAColumnName()
        {
            return "A_FIELD";
        }

        private static object GetAValue()
        {
            return "A_VALUE";
        }
    }
}