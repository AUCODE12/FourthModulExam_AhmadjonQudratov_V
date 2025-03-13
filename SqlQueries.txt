CREATE TABLE Customers (
    CustomerID INT PRIMARY KEY IDENTITY,
    Name NVARCHAR(100),
    Email NVARCHAR(100)
);


CREATE TABLE Orders (
    OrderID INT PRIMARY KEY IDENTITY,
    CustomerID INT FOREIGN KEY REFERENCES Customers(CustomerID),
    OrderDate DATETIME DEFAULT GETDATE()
);

CREATE TABLE Products (
    ProductID INT PRIMARY KEY IDENTITY,
    Name NVARCHAR(100),
    Price DECIMAL(10,2)
);
CREATE TABLE OrderItems (
    OrderItemID INT PRIMARY KEY IDENTITY,
    OrderID INT FOREIGN KEY REFERENCES Orders(OrderID),
    ProductID INT FOREIGN KEY REFERENCES Products(ProductID),
    Quantity INT,
    Price DECIMAL(10,2)
);


INSERT INTO Customers (Name, Email) VALUES
('John Doe', 'john.doe@email.com'),
('Alice Smith', 'alice.smith@email.com'),
('Bob Johnson', 'bob.johnson@email.com'),
('Emma Brown', 'emma.brown@email.com'),
('James Wilson', 'james.wilson@email.com'),
('Olivia Martinez', 'olivia.martinez@email.com'),
('Liam Garcia', 'liam.garcia@email.com'),
('Sophia Lee', 'sophia.lee@email.com'),
('Mason Hall', 'mason.hall@email.com'),
('Isabella Allen', 'isabella.allen@email.com'),
('Ethan Young', 'ethan.young@email.com'),
('Ava King', 'ava.king@email.com'),
('Michael Wright', 'michael.wright@email.com'),
('Charlotte Scott', 'charlotte.scott@email.com'),
('Daniel Green', 'daniel.green@email.com'),
('Harper Adams', 'harper.adams@email.com'),
('Benjamin Baker', 'benjamin.baker@email.com'),
('Amelia Gonzalez', 'amelia.gonzalez@email.com'),
('Lucas Nelson', 'lucas.nelson@email.com'),
('Mia Carter', 'mia.carter@email.com');


INSERT INTO Products (Name, Price) VALUES
('Laptop', 1200.00),
('Smartphone', 800.00),
('Headphones', 150.00),
('Smartwatch', 250.00),
('Keyboard', 50.00),
('Mouse', 30.00),
('Monitor', 300.00),
('External HDD', 100.00),
('Gaming Chair', 400.00),
('Desk Lamp', 40.00),
('USB Flash Drive', 20.00),
('Wireless Router', 120.00),
('Graphics Card', 500.00),
('Power Bank', 70.00),
('Webcam', 60.00),
('Microphone', 110.00),
('VR Headset', 700.00),
('Portable Speaker', 90.00),
('Coffee Maker', 130.00),
('Fitness Tracker', 200.00);


INSERT INTO Orders (CustomerID, OrderDate) VALUES
(1, '2024-01-15'),
(2, '2024-01-20'),
(3, '2024-02-05'),
(4, '2024-02-10'),
(5, '2024-02-15'),
(6, '2024-02-20'),
(7, '2024-03-05'),
(8, '2024-03-10'),
(9, '2024-03-15'),
(10, '2024-03-20'),
(11, '2024-04-05'),
(12, '2024-04-10'),
(13, '2024-04-15'),
(14, '2024-04-20'),
(15, '2024-05-05'),
(16, '2024-05-10'),
(17, '2024-05-15'),
(18, '2024-05-20'),
(19, '2024-06-05'),
(20, '2024-06-10');


INSERT INTO OrderItems (OrderID, ProductID, Quantity, Price) VALUES
(1, 1, 1, 1200.00), 
(1, 2, 2, 800.00),
(2, 3, 3, 150.00),
(2, 4, 1, 250.00),
(3, 5, 2, 50.00),
(3, 6, 1, 30.00),
(4, 7, 1, 300.00),
(4, 8, 2, 100.00),
(5, 9, 1, 400.00),
(5, 10, 3, 40.00),
(6, 11, 4, 20.00),
(6, 12, 1, 120.00),
(7, 13, 1, 500.00),
(7, 14, 2, 70.00),
(8, 15, 1, 60.00),
(8, 16, 1, 110.00),
(9, 17, 1, 700.00),
(9, 18, 2, 90.00),
(10, 19, 1, 130.00),
(10, 20, 3, 200.00);

-- 1 
select c.CustomerID, c.Name, count(o.OrderID) as TotalOrdered
from Customers c
left join Orders o on c.CustomerID = o.CustomerID
group by c.CustomerID, c.Name;

--2
select c.CustomerID, c.Name, sum(oi.Quantity * oi.Price) AS TotalSpent
from Customers c
inner join Orders o ON c.CustomerID = o.CustomerID
inner join OrderItems oi ON o.OrderID = oi.OrderID
group by c.CustomerID, c.Name
having sum(oi.Quantity * oi.Price) > 500;

--3
SELECT AVG(TotalQuantity) AS AvgProductsPerOrder
FROM (
    SELECT o.OrderID, SUM(oi.Quantity) AS TotalQuantity
    FROM Orders o
    JOIN OrderItems oi ON o.OrderID = oi.OrderID
    GROUP BY o.OrderID
) AS OrderQuantities;

--buni o'zim qilolmadi tepadagi kod gptdan manikida avg bilan sumni bittada ishlatib bo'lmas ekan
select o.OrderID, avg(sum(oi.Quantity)) as ortachaMahsulotSoni
from OrderItems oi
inner join Orders o
on oi.OrderID = o.OrderID
group by o.OrderID
order by ortachaMahsulotSoni 


--4
select o.OrderID, sum(oi.Quantity * oi.Price) as OrderTotal
from Orders o
inner join OrderItems oi on o.OrderID = oi.OrderID
group by o.OrderID
having sum(oi.Quantity * oi.Price) > 1000;

--5
select top 1 * from Orders
where CustomerID = (select CustomerID from Customers where name = 'Alice Smith')
order by OrderDate desc;