using ESRI.ArcGIS.Geodatabase;
using EsriExtensionMethods;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace EsriExtensionMethods
{
    [TestClass]
    public class IRowExtensionMethodsTests
    {
        #region GetValue Tests

        [TestMethod]
        public void GetValue_ExistingFieldName_ReturnsValue()
        {
            // arrange
            var colName = TestMethods.RandomString(10);
            var colIndex = 0;
            var expectedValue = TestMethods.RandomString(5);

            var rowMock = CreateIRowMock(colName, colIndex);
            rowMock.Setup(r => r.get_Value(colIndex)).Returns(expectedValue);

            // act
            var result = rowMock.Object.GetValue(colName);

            // assert
            result.Should().Be(expectedValue);
        }

        [TestMethod]
        public void GetValue_DbNullValue_ReturnsNull()
        {
            // arrange
            var colName = TestMethods.RandomString(10);
            var colIndex = 0;

            var rowMock = CreateIRowMock(colName, colIndex);
            rowMock.Setup(r => r.get_Value(colIndex)).Returns(DBNull.Value);

            // act
            var result = rowMock.Object.GetValue(colName);

            // assert
            result.Should().BeNull();
        }

        [TestMethod]
        public void GetValue_OnNonExistingFieldName_ShouldThrow()
        {
            // arrange
            var nonExistingColumn = TestMethods.RandomString(10);
            var rowWithoutColumns = CreateIRowMock(nonExistingColumn, -1);

            // act
            Action act = () =>
                rowWithoutColumns.Object.GetValue(nonExistingColumn);

            // assert
            act.ShouldThrow<Exception>();
        }

        #endregion
        

        #region SetValue Tests

        [TestMethod]
        public void SetValue_ExistingFieldName_ShouldCallSetMethodOnRow()
        {
            // arrange
            var colName = TestMethods.RandomString(10);
            var colIndex = 0;
            var setValue = TestMethods.RandomString(5);

            var rowMock = CreateIRowMock(colName, colIndex);

            // act
            var result = rowMock.Object.SetValue(colName, setValue);

            // assert
            rowMock.Verify(r => r.set_Value(colIndex, setValue), Times.Once());
        }

        [TestMethod]
        public void SetValue_SetNullValue_ShouldCallSetMethodWithDBNull()
        {
            // arrange
            var colName = TestMethods.RandomString(10);
            var colIndex = 0;
            var setValue = TestMethods.RandomString(5);

            var rowMock = CreateIRowMock(colName, colIndex);

            // act
            var result = rowMock.Object.SetValue(colName, null);

            // assert
            rowMock.Verify(r => r.set_Value(colIndex, DBNull.Value), Times.Once());
        }

        [TestMethod]
        public void SetValue_OnNonExistingFieldName_ShouldThrow()
        {
            // arrange
            var nonExistingColumn = TestMethods.RandomString(10);
            var setValue = TestMethods.RandomString(5);

            var rowWithoutColumns = CreateIRowMock(nonExistingColumn, -1);

            // act
            Action act = () =>
                rowWithoutColumns.Object.SetValue(nonExistingColumn, setValue);

            // assert
            act.ShouldThrow<Exception>();
        }

        #endregion


        #region GetField Tests

        [TestMethod]
        public void GetField_OnNonExistingFieldName_ShouldThrow()
        {
            // arrange
            var nonExistingColumn = TestMethods.RandomString(10);
            var setValue = TestMethods.RandomString(5);

            var rowWithoutColumns = CreateIRowMock(nonExistingColumn, -1);

            // act
            Action act = () =>
                rowWithoutColumns.Object.GetField(nonExistingColumn);

            // assert
            act.ShouldThrow<Exception>();
        }

        [TestMethod]
        public void GetField_ExistingFieldName_ShouldReturnField()
        {
            // arrange
            var colName = TestMethods.RandomString(10);
            var colIndex = 0;
            var setValue = TestMethods.RandomString(5);
            var fieldMock = new Mock<IField>();

            var rowMock = CreateIRowMock(colName, colIndex, fieldMock.Object);

            // act
            var result = rowMock.Object.GetField(colName);

            // assert
            result.Should().Be(fieldMock.Object);
        }

        #endregion


        #region Private methods

        private Mock<IRow> CreateIRowMock(string fieldName, int fieldIndex, IField field = null)
        {
            var fieldsMock = new Mock<IFields>();
            fieldsMock.Setup(f => f.FindField(fieldName)).Returns(fieldIndex);
            var rowMock = new Mock<IRow>();
            rowMock.Setup(r => r.Fields).Returns(fieldsMock.Object);

            if (field != null)
                fieldsMock.Setup(f => f.get_Field(fieldIndex)).Returns(field);

            return rowMock;
        }

        #endregion

    }
}