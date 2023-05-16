namespace AutomatedTradingSystem.UnitTest
{
    public class Tests
    {
        public InternalTransfer _InternalTransfer { get; set; }
        public MinOverCancellation _MinOverCancellation { get; set; }
        [SetUp]
        public void Setup()
        {
            _InternalTransfer = new InternalTransfer();
            _MinOverCancellation = new MinOverCancellation();
        }

        [Test]
        public void InternalTransfer_EqualTest1()
        {
            //Assign
            List<Order> orders = new List<Order>();
            orders.Add(new Order() { Id = 1, Quantity = 10, Price = 100 });
            orders.Add(new Order() { Id = 2, Quantity = -3, Price = 100 });
            orders.Add(new Order() { Id = 3, Quantity = -5, Price = 100 });
            orders.Add(new Order() { Id = 4, Quantity = 5, Price = 100 });
            orders.Add(new Order() { Id = 5, Quantity = -2, Price = 100 });
            List<int> expected = new List<int>(new int[] { 1, 2, 3, 5 });//1/2/3/5
            //Act
            List<int> actual = _InternalTransfer.Collect(orders);
            //Assert
            Assert.That(expected, Is.EqualTo(actual));
        }

        [Test]
        public void InternalTransfer_EqualTest2()
        {
            //Assign
            List<Order> orders = new List<Order>();
            orders.Add(new Order() { Id = 1, Quantity = 10, Price = 100 });
            orders.Add(new Order() { Id = 2, Quantity = -5, Price = 100 });
            List<int> expected = new List<int>(new int[] { });//[]
            //Act
            List<int> actual = _InternalTransfer.Collect(orders);
            //Assert
            Assert.That(expected, Is.EqualTo(actual));
        }

        [Test]
        public void InternalTransfer_EqualTest3()
        {
            //Assign
            List<Order> orders = new List<Order>();
            orders.Add(new Order() { Id = 1, Quantity = 10, Price = 100 });
            orders.Add(new Order() { Id = 2, Quantity = -5, Price = 100 });
            orders.Add(new Order() { Id = 3, Quantity = -5, Price = 100 });
            orders.Add(new Order() { Id = 4, Quantity = 5, Price = 100 });
            orders.Add(new Order() { Id = 5, Quantity = -5, Price = 100 });
            orders.Add(new Order() { Id = 6, Quantity = 2, Price = 100 });
            List<int> expected = new List<int>(new int[] { 1, 2, 3, 4, 5 });//1/2/3/4/5
            //Act
            List<int> actual = _InternalTransfer.Collect(orders);
            //Assert
            Assert.That(expected, Is.EqualTo(actual));
        }

        [Test]
        public void InternalTransfer_EqualTest4()
        {
            //Assign
            List<Order> orders = new List<Order>();
            orders.Add(new Order() { Id = 1, Quantity = 10, Price = 100 });
            orders.Add(new Order() { Id = 2, Quantity = -10, Price = 200 });
            List<int> expected = new List<int>(new int[] { });//[]
            //Act
            List<int> actual = _InternalTransfer.Collect(orders);
            //Assert
            Assert.That(expected, Is.EqualTo(actual));
        }

        [Test]
        public void MinOverCancellation_EqualTest1()
        {
            //Assign
            List<Order> orders = new List<Order>();
            orders.Add(new Order() { Id = 1, Quantity = 3, Price = 100 });
            orders.Add(new Order() { Id = 2, Quantity = 2, Price = 100 });
            orders.Add(new Order() { Id = 3, Quantity = 3, Price = 100 });
            orders.Add(new Order() { Id = 4, Quantity = 1, Price = 100 });
            orders.Add(new Order() { Id = 5, Quantity = 5, Price = 100 });

            List<int> expected = new List<int>(new int[] { 2 });//[]
            //Act
            List<int> actual = _MinOverCancellation.Collect(orders, 2);
            //Assert
            Assert.That(expected, Is.EqualTo(actual));
        }

        [Test]
        public void MinOverCancellation_EqualTest2()
        {
            //Assign
            List<Order> orders = new List<Order>();
            orders.Add(new Order() { Id = 1, Quantity = 3, Price = 100 });
            orders.Add(new Order() { Id = 2, Quantity = 2, Price = 100 });
            orders.Add(new Order() { Id = 3, Quantity = 3, Price = 100 });
            orders.Add(new Order() { Id = 4, Quantity = 1, Price = 100 });
            orders.Add(new Order() { Id = 5, Quantity = 5, Price = 100 });

            List<int> expected = new List<int>(new int[] { 2, 5 });//[2/5]
            //Act
            List<int> actual = _MinOverCancellation.Collect(orders, 7);
            //Assert
            Assert.That(expected, Is.EqualTo(actual));
        }

        [Test]
        public void MinOverCancellation_EqualTest3()
        {
            //Assign
            List<Order> orders = new List<Order>();
            orders.Add(new Order() { Id = 1, Quantity = 5, Price = 100 });
            orders.Add(new Order() { Id = 2, Quantity = 2, Price = 100 });
            orders.Add(new Order() { Id = 3, Quantity = 7, Price = 100 });
            orders.Add(new Order() { Id = 4, Quantity = 2, Price = 100 });

            List<int> expected = new List<int>(new int[] { 2, 4 });//[2/4]
            //Act
            List<int> actual = _MinOverCancellation.Collect(orders, 3);
            //Assert
            Assert.That(expected, Is.EqualTo(actual));
        }

        [Test]
        public void MinOverCancellation_EqualTest4()
        {
            //Assign
            List<Order> orders = new List<Order>();
            orders.Add(new Order() { Id = 1, Quantity = 5, Price = 100 });
            orders.Add(new Order() { Id = 2, Quantity = 5, Price = 100 });
            orders.Add(new Order() { Id = 3, Quantity = 5, Price = 100 });
            orders.Add(new Order() { Id = 4, Quantity = 6, Price = 100 });
            orders.Add(new Order() { Id = 5, Quantity = 15, Price = 100 });
            orders.Add(new Order() { Id = 6, Quantity = 55, Price = 100 });

            List<int> expected = new List<int>(new int[] { 5 });//[2/4]
            //Act
            List<int> actual = _MinOverCancellation.Collect(orders, 12);
            //Assert
            Assert.That(expected, Is.EqualTo(actual));
        }
    }
}