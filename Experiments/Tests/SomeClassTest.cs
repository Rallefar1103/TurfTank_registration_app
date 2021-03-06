using Microsoft.VisualStudio.TestPlatform.TestHost;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Project; 
using System;

namespace Unittests
{
    [TestFixture]
    public class SomeClassTest
    {
        private SomeClass FakeSomeClass(decimal salary)
        {
            return new SomeClass(salary);
        }

        [TestCase(4, 5, "Testing for small number")]
        [TestCase(-1, 0, "Testing for negative number")]
        [TestCase(123, 124, "Testing for large number")]
        public void AddOne_SingleNumber_ReturnsNumberIncrementedByOne(int Arranged, int Expected, string TestName)
        {
            int ArrangedValue = Arranged;
            int INITIAL_SALARY = 0; 
            SomeClass StubObject = FakeSomeClass(INITIAL_SALARY);
            int ExpectedValue = Expected;

            int ActualValue = StubObject.AddOne(ArrangedValue);

            Assert.AreEqual(
                ExpectedValue, ActualValue,
                TestName
            );
        }
        [TestCase(4, 4, "Testing for small number")]
        [TestCase(0, 0, "Testing for zero")]
        public void IncreaseSalary_SingleNumber_IncreaseInternalSalaryAttribute(int Arranged, int Expected, string TestName)
        {
            int ArrangedValue = Arranged;
            int INITIAL_SALARY = 0;
            SomeClass MockObject = FakeSomeClass(INITIAL_SALARY);
            int ExpectedValue = Expected;

            MockObject.IncreaseSalary(ArrangedValue);

            Assert.AreEqual(
                ExpectedValue, MockObject.Salary, 
                TestName 
            );
        }
        [TestCase(1, "Salary must not increase by one", "Testing for errormessage in SomeClass.IncreaseSalary when the parameter is 1")]
        public void IncreaseSalary_IntOne_ShouldThrowException(int Arranged, string Expected, string TestName)
        {
            int ArrangedValue = Arranged;
            int INITIAL_SALARY = 0;
            SomeClass MockObject = FakeSomeClass(INITIAL_SALARY);
            string ExpectedString = Expected;

            CustomException exception = Assert.Throws<Project.CustomException>(() => MockObject.IncreaseSalary(ArrangedValue));
            Assert.AreEqual(exception.Message, ExpectedString, TestName);
        }
    }
}