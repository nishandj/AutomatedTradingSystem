// See https://aka.ms/new-console-template for more information

new InternalTransfer().CallOrders();

public class InternalTransfer
{
    public void CallOrders()
    {
        List<Order> orders = new List<Order>();
        orders.Add(new Order() { Id = 1, Quantity = 10, Price = 100 });
        orders.Add(new Order() { Id = 2, Quantity = -3, Price = 100 });
        orders.Add(new Order() { Id = 3, Quantity = -5, Price = 100 });
        orders.Add(new Order() { Id = 4, Quantity = 5, Price = 100 });
        orders.Add(new Order() { Id = 5, Quantity = -2, Price = 100 });
        //1/2/3/5
        var result = Collect(orders);
        result.Sort();
        Console.WriteLine("//1/2/3/5");
        foreach (var i in result)
        {
            Console.WriteLine(i);
        }

        orders = new List<Order>();
        orders.Add(new Order() { Id = 1, Quantity = 10, Price = 100 });
        orders.Add(new Order() { Id = 2, Quantity = -5, Price = 100 });
        //[]
        result = Collect(orders);
        result.Sort();
        Console.WriteLine("//[]");
        foreach (var i in result)
        {
            Console.WriteLine(i);
        }

        orders = new List<Order>();
        orders.Add(new Order() { Id = 1, Quantity = 10, Price = 100 });
        orders.Add(new Order() { Id = 2, Quantity = -5, Price = 100 });
        orders.Add(new Order() { Id = 3, Quantity = -5, Price = 100 });
        orders.Add(new Order() { Id = 4, Quantity = 5, Price = 100 });
        orders.Add(new Order() { Id = 5, Quantity = -5, Price = 100 });
        orders.Add(new Order() { Id = 6, Quantity = 2, Price = 100 });
        //1,2,3,4,5
        result = Collect(orders);
        result.Sort();
        Console.WriteLine("//1,2,3,4,5");
        foreach (var i in result)
        {
            Console.WriteLine(i);
        }

        orders = new List<Order>();
        orders.Add(new Order() { Id = 1, Quantity = 10, Price = 100 });
        orders.Add(new Order() { Id = 2, Quantity = -10, Price = 200 });
        //[]
        result = Collect(orders);
        result.Sort();
        Console.WriteLine("//[]");
        foreach (var i in result)
        {
            Console.WriteLine(i);
        }

        orders = new List<Order>();
        orders.Add(new Order() { Id = 1, Quantity = 3, Price = 100 });
        orders.Add(new Order() { Id = 2, Quantity = 2, Price = 100 });
        orders.Add(new Order() { Id = 3, Quantity = 3, Price = 100 });
        orders.Add(new Order() { Id = 4, Quantity = 1, Price = 100 });
        orders.Add(new Order() { Id = 5, Quantity = 5, Price = 100 });
        //2
        result = new MinOverCancellation().Collect(orders, 2);
        result.Sort();
        Console.WriteLine("//2");
        foreach (var i in result)
        {
            Console.WriteLine(i);
        }

        orders = new List<Order>();
        orders.Add(new Order() { Id = 1, Quantity = 3, Price = 100 });
        orders.Add(new Order() { Id = 2, Quantity = 2, Price = 100 });
        orders.Add(new Order() { Id = 3, Quantity = 3, Price = 100 });
        orders.Add(new Order() { Id = 4, Quantity = 1, Price = 100 });
        orders.Add(new Order() { Id = 5, Quantity = 5, Price = 100 });
        //2/5
        result = new MinOverCancellation().Collect(orders, 7);
        result.Sort();
        Console.WriteLine("//2/5");
        foreach (var i in result)
        {
            Console.WriteLine(i);
        }

        orders = new List<Order>();
        orders.Add(new Order() { Id = 1, Quantity = 5, Price = 100 });
        orders.Add(new Order() { Id = 2, Quantity = 2, Price = 100 });
        orders.Add(new Order() { Id = 3, Quantity = 7, Price = 100 });
        orders.Add(new Order() { Id = 4, Quantity = 2, Price = 100 });
        //2/4
        result = new MinOverCancellation().Collect(orders, 3);
        result.Sort();
        Console.WriteLine("//2/4");
        foreach (var i in result)
        {
            Console.WriteLine(i);
        }

        orders = new List<Order>();
        orders.Add(new Order() { Id = 1, Quantity = 5, Price = 100 });
        orders.Add(new Order() { Id = 2, Quantity = 5, Price = 100 });
        orders.Add(new Order() { Id = 3, Quantity = 5, Price = 100 });
        orders.Add(new Order() { Id = 4, Quantity = 6, Price = 100 });
        orders.Add(new Order() { Id = 5, Quantity = 15, Price = 100 });
        orders.Add(new Order() { Id = 6, Quantity = 55, Price = 100 });
        //5
        result = new MinOverCancellation().Collect(orders, 12);
        result.Sort();
        Console.WriteLine("//5");
        foreach (var i in result)
        {
            Console.WriteLine(i);
        }

        Console.ReadLine();
    }
    public List<int> Collect(List<Order> orders)
    {
        List<int> result = new List<int>();
        if (orders != null)
        {
            var distinctPriceList = orders.Select(x => x.Price).Distinct().ToList();
            foreach (var price in distinctPriceList)
            {
                List<Order> sellingFillList = orders.Where(x => x.Quantity < 0 && x.Price == price).ToList();
                List<Order> buyingFillList = orders.Where(x => x.Quantity > 0 && x.Price == price).ToList();

                if (sellingFillList.Count == 0 || buyingFillList.Count == 0)
                    continue;

                var sumOfSellingFillQuantity = sellingFillList.Sum(x => x.Quantity);
                var sumOfBuyingFillQuantity = buyingFillList.Sum(x => x.Quantity);

                sumOfSellingFillQuantity = sumOfSellingFillQuantity * -1;

                List<Order> minimumOrderList, maximumOrderList;
                if (sumOfSellingFillQuantity > sumOfBuyingFillQuantity)
                {
                    minimumOrderList = buyingFillList;
                    maximumOrderList = sellingFillList;
                }
                else
                {
                    minimumOrderList = sellingFillList;
                    maximumOrderList = buyingFillList;
                }

                decimal sum = 0;
                decimal limit = minimumOrderList.Sum(x => x.Quantity);
                if (limit < 0)
                    limit = limit * -1;

                var balanceOrderList = maximumOrderList.TakeWhile(x => { var temp = sum; sum += x.Quantity; return temp < limit; }).ToList();
                var sumOfBalanceOrderList = balanceOrderList.Sum(x => x.Quantity);
                if (sumOfBalanceOrderList < 0)
                    sumOfBalanceOrderList = sumOfBalanceOrderList * -1;
                var substractPriceTotal = sumOfBalanceOrderList - limit;
                if (balanceOrderList.Count > 0 && substractPriceTotal == 0)
                {
                    minimumOrderList.AddRange(balanceOrderList);
                    result = minimumOrderList.Select(x => x.Id).ToList();
                }
            }
        }

        result.Sort();
        return result;
    }
}

public class MinOverCancellation
{
    public List<int> Collect(List<Order> orders, int cancelTarget)
    {
        List<int> result = new List<int>();

        var directMatchOrders = orders.Where(x => x.Quantity == cancelTarget).FirstOrDefault();
        if (directMatchOrders != null)
            return new List<int>() { directMatchOrders.Id };

        int tempCompare = 0;
        List<Order> ordersMinOverCancel = new List<Order>();
        CalculateDifference(orders, cancelTarget, ordersMinOverCancel, tempCompare);
        while (ordersMinOverCancel.Count == 0)
        {
            tempCompare++;
            CalculateDifference(orders, cancelTarget, ordersMinOverCancel, tempCompare);
            if (tempCompare > orders.Count)
                break;
        }
        if (ordersMinOverCancel.Count == 0)
            ordersMinOverCancel.Add(orders.Aggregate((x, y) => Math.Abs(x.Quantity - cancelTarget) < Math.Abs(y.Quantity - cancelTarget) ? x : y));

        result = ordersMinOverCancel.Select(x => x.Id).ToList();
        result.Sort();

        return result;
    }

    private static int CalculateDifference(List<Order> orders, int cancelTarget, List<Order> ordersMinOverCancel, int tempCompare)
    {
        int temp = int.MaxValue;
        for (int i = 0; i < orders.Count - 1; i++)
        {
            for (int j = i + 1; j < orders.Count; j++)
            {
                if (Math.Abs(orders[i].Quantity + orders[j].Quantity - cancelTarget) < temp)
                {
                    temp = Math.Abs(orders[i].Quantity + orders[j].Quantity - cancelTarget);
                    if (temp == tempCompare && (orders[i].Quantity + orders[j].Quantity) >= cancelTarget)
                    {
                        ordersMinOverCancel.Add(orders[i]);
                        ordersMinOverCancel.Add(orders[j]);
                    }
                }
            }
        }
        return temp;
    }
}

public class Order
{
    public int Id;
    public int Quantity;
    public decimal Price;
}
