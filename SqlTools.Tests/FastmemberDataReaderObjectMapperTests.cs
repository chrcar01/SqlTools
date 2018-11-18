﻿using Moq;
using NUnit.Framework;
using SqlTools.Tests.Models;
using System;
using System.Data;

namespace SqlTools.Tests
{
    [TestFixture]
    public class FastmemberDataReaderObjectMapperTests
    {
        private class TestModel
        {
            public int? Age { get; set; }
            public int Id { get; } = 123;
            
        }

        private IDataReaderObjectMapper CreateMapper() => new FastmemberDataReaderObjectMapper();

        private Mock<IDataReader> CreateSuccessfulMockDataReader(int fieldCount)
        {
            var mockDataReader = new Mock<IDataReader>(MockBehavior.Strict);
            mockDataReader.Setup(x => x.IsClosed).Returns(false);
            mockDataReader.Setup(x => x.FieldCount).Returns(fieldCount);
            return mockDataReader;
        }

        [Test]
        public void MapReturnsNullIfNullDataReader() => Assert.That(CreateMapper().Map<State>(null), Is.Null);

        [Test]
        public void MapReturnsNullIfDataReaderClosed()
        {
            var mockDataReader = new Mock<IDataReader>(MockBehavior.Strict);
            mockDataReader.Setup(x => x.IsClosed).Returns(true);
            Assert.That(CreateMapper().Map<State>(mockDataReader.Object), Is.Null);
            mockDataReader.VerifyAll();
        }

        [Test]
        public void FieldMappedAccordingly()
        {
            var mockDataReader = CreateSuccessfulMockDataReader(1);
            mockDataReader.Setup(x => x.GetName(0)).Returns("Name");
            mockDataReader.Setup(x => x.IsDBNull(0)).Returns(false);
            mockDataReader.Setup(x => x.GetValue(0)).Returns("Colorado");

            var state = CreateMapper().Map<State>(mockDataReader.Object);
            Assert.That(state.Name, Is.EqualTo("Colorado"));
        }

        [Test]
        public void DoesNotGetTrippedUpOnDBNull()
        {
            var mockDataReader = CreateSuccessfulMockDataReader(1);
            mockDataReader.Setup(x => x.GetName(0)).Returns("Age");
            mockDataReader.Setup(x => x.IsDBNull(0)).Returns(true);

            var model = CreateMapper().Map<TestModel>(mockDataReader.Object);
            Assert.That(model.Age, Is.Null);
        }

        [Test]
        public void CanMapNullableValues()
        {
            var mockDataReader = CreateSuccessfulMockDataReader(1);
            mockDataReader.Setup(x => x.GetName(0)).Returns("Age");
            mockDataReader.Setup(x => x.GetValue(0)).Returns(48);
            mockDataReader.Setup(x => x.IsDBNull(0)).Returns(false);
            var model = CreateMapper().Map<TestModel>(mockDataReader.Object);
            Assert.That(model.Age, Is.EqualTo(48));
        }

        [Test]
        public void DoesNotGetTrippedUpOnZeroFields()
        {
            var mockDataReader = CreateSuccessfulMockDataReader(0);

            var model = CreateMapper().Map<TestModel>(mockDataReader.Object);
            Assert.That(model.Age, Is.Null);
        }

        [Test]
        public void ReadonlyPropertiesShouldBeSkipped()
        {
            var mockDataReader = CreateSuccessfulMockDataReader(1);
            mockDataReader.Setup(x => x.GetName(0)).Returns("Id"); // read-only on the model
            mockDataReader.Setup(x => x.IsDBNull(0)).Returns(false);
            mockDataReader.Setup(x => x.GetValue(0)).Returns(666);
            
            var model = CreateMapper().Map<TestModel>(mockDataReader.Object);
            Assert.That(model.Id, Is.EqualTo(123));
        }
    }
}
