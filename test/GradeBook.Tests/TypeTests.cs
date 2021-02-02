using System;
using Xunit;

namespace GradeBook.Tests
{

    public delegate string WriteLogDelegate(string logMessage);
    public class Typetests
    {
        [Fact]
        public void WriteLogDelegateCanPointToMethod()
        {
            WriteLogDelegate log;

            log = new WriteLogDelegate(ReturnMessage); // ei invoke, vaan et delegate osoittaa returnmessage funktioon
            // log = ReturnMessage ois shorthand.
            // log += ReturnMessage; voi invokata monia metodeja kun tuolleen lisää.

            var result = log("Hello!");
            Assert.Equal("Hello!", result);
        }

        string ReturnMessage(string message)
        {
            return message;
        }
        [Fact]
        public void StringsBehaveLikeValueTypes()
        {
            string name = "Andy";
            string upper = MakeUppercase(name);

            Assert.Equal("ANDY", upper);
        }

        private string MakeUppercase(string parameter)
        {
            return parameter.ToUpper();
        }

        [Fact]
        public void CSharpCanPassByRef()
        {
            var book1 = GetBook("Book 1");
            GetBookSetName(ref book1, "New Name");

            Assert.Equal("New Name", book1.Name);
        }

        private void GetBookSetName(ref InMemoryBook book, string name)
        {
            book = new InMemoryBook(name);
            book.Name = name;
        }
        [Fact]
        public void CSharpIsPassByValue()
        {
            var book1 = GetBook("Book 1");
            GetBookSetName(book1, "New Name");

            Assert.Equal("Book 1", book1.Name);
        }

        private void GetBookSetName(InMemoryBook book, string name)
        {
            book = new InMemoryBook(name);
            book.Name = name;
        }
        [Fact]
        public void CanSetNameFromReference()
        {
            var book1 = GetBook("Book 1");
            SetName(book1, "New Name");

            Assert.Equal("New Name", book1.Name);
        }

        private void SetName(InMemoryBook book, string name)
        {
            book.Name = name;
        }

        [Fact]
        public void GetBookReturnsDifferentObjects()
        {
            // arrange section
            var book1 = GetBook("Book 1");
            var book2 = GetBook("Book 2");

            Assert.Equal("Book 1", book1.Name);
            Assert.Equal("Book 2", book2.Name);
            Assert.NotSame(book1, book2);
        }
        [Fact]
        public void TwoVarsCanReferenceSameObject()
        {
            // arrange section
            var book1 = GetBook("Book 1");
            var book2 = book1;

            Assert.Same(book1, book2);
            Assert.True(Object.ReferenceEquals(book1,book2));
        }
        InMemoryBook GetBook(string name)
        {
            return new InMemoryBook(name);
        }
    }
}
