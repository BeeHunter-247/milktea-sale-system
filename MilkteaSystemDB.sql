CREATE DATABASE MilkteaSaleDB;
GO

USE MilkteaSaleDB;
GO

-- Bảng Accounts (Quản lý tài khoản người dùng)
CREATE TABLE Accounts (
    AccountId INT PRIMARY KEY IDENTITY(1,1),
    AccountPassword NVARCHAR(80) NOT NULL,
    FullName NVARCHAR(80) NOT NULL,
    Email NVARCHAR(100) UNIQUE NOT NULL,
    AccountRole INT NOT NULL, -- 1: Admin, 2: Customer
    IsActive BIT DEFAULT 1,
    CreatedDate DATETIME DEFAULT GETDATE(),
    CONSTRAINT CHK_AccountRole CHECK (AccountRole IN (1, 2))
);
GO

-- Bảng Categories (Danh mục sản phẩm)
CREATE TABLE Categories (
    CategoryId INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    Description NVARCHAR(255),
    IsActive BIT DEFAULT 1
);
GO

-- Bảng Products (Sản phẩm chính như Milk Tea, Topping, Combo)
CREATE TABLE Products (
    ProductId INT PRIMARY KEY IDENTITY(1,1),
    CategoryId INT NOT NULL,
    [Name] NVARCHAR(100) NOT NULL,
    Price DECIMAL(18,2) NOT NULL,
    [Description] NVARCHAR(255),
    [Image] VARCHAR(MAX),
    ProductType INT NOT NULL DEFAULT 1, -- 1: Regular, 2: Extra, 3: Combo
    IsActive BIT DEFAULT 1,
    CONSTRAINT CHK_Price CHECK (Price >= 0),
    CONSTRAINT CHK_ProductType CHECK (ProductType IN (1, 2, 3)),
    FOREIGN KEY (CategoryId) REFERENCES Categories(CategoryId)
);
GO

-- Bảng ComboProducts (Liên kết giữa các sản phẩm trong combo)
CREATE TABLE ComboProducts (
    ComboProductId INT,       -- ProductId của sản phẩm là combo
    IncludedProductId INT,    -- ProductId của sản phẩm được bao gồm trong combo
    Quantity INT DEFAULT 1,
    CONSTRAINT CHK_Quantity CHECK (Quantity > 0),
    PRIMARY KEY (ComboProductId, IncludedProductId),
    FOREIGN KEY (ComboProductId) REFERENCES Products(ProductId),
    FOREIGN KEY (IncludedProductId) REFERENCES Products(ProductId)
);
GO

-- Bảng Orders (Đơn hàng, liên kết với Account)
CREATE TABLE Orders (
    OrderId INT PRIMARY KEY IDENTITY(1,1),
    AccountId INT NOT NULL, 
    CustomerName NVARCHAR(100) NOT NULL,
    OrderDate DATETIME DEFAULT GETDATE(),
    TotalAmount DECIMAL(18,2) NOT NULL,
    [Status] NVARCHAR(50) DEFAULT 'Pending', -- Pending, Processing, Completed, Cancelled
    CONSTRAINT CHK_TotalAmount CHECK (TotalAmount >= 0),
    CONSTRAINT CHK_Status CHECK ([Status] IN ('Pending', 'Processing', 'Completed', 'Cancelled')),
    FOREIGN KEY (AccountId) REFERENCES Accounts(AccountId)
);
GO

-- Bảng OrderDetails (Chi tiết đơn hàng)
CREATE TABLE OrderDetails (
    OrderDetailId INT PRIMARY KEY IDENTITY(1,1),
    OrderId INT NOT NULL,
    ProductId INT NOT NULL,
    Quantity INT NOT NULL,
    UnitPrice DECIMAL(18,2) NOT NULL,
    CONSTRAINT CHK_OrderDetail_Quantity CHECK (Quantity > 0),
    CONSTRAINT CHK_UnitPrice CHECK (UnitPrice >= 0),
    FOREIGN KEY (OrderId) REFERENCES Orders(OrderId),
    FOREIGN KEY (ProductId) REFERENCES Products(ProductId)
);
GO

-- Bảng Payments (Thanh toán)
CREATE TABLE Payments (
    PaymentId INT PRIMARY KEY IDENTITY(1,1),
    OrderId INT NOT NULL,
    PaymentDate DATETIME DEFAULT GETDATE(),
    Amount DECIMAL(18,2) NOT NULL,
    PaymentMethod NVARCHAR(50) NOT NULL, -- Cash, Credit Card, Mobile App
    [Status] NVARCHAR(50) DEFAULT 'Pending', -- Pending, Completed, Failed
    CONSTRAINT CHK_Payment_Amount CHECK (Amount >= 0),
    CONSTRAINT CHK_PaymentMethod CHECK (PaymentMethod IN ('Cash', 'Credit Card', 'Mobile App')),
    CONSTRAINT CHK_Payment_Status CHECK ([Status] IN ('Pending', 'Completed', 'Failed')),
    FOREIGN KEY (OrderId) REFERENCES Orders(OrderId)
);
GO

-- Thêm dữ liệu vào Accounts
INSERT INTO Accounts (AccountPassword, FullName, Email, AccountRole) VALUES
('@1', 'Administrator', 'admin@milktea.com', 1),
('@1', 'Nguyen Van A', 'customer1@gmail.com', 2),
('@1', 'Tran Thi B', 'customer2@gmail.com', 2);
GO

-- Thêm dữ liệu vào Categories
INSERT INTO Categories (Name, Description) VALUES
('Milk Tea', 'Các loại trà sữa'),
('Topping', 'Các loại topping bổ sung'),
('Combo', 'Các gói combo');
GO

-- Thêm dữ liệu vào Products (đã thay IsExtra, IsCombo bằng ProductType)
INSERT INTO Products (CategoryId, [Name], Price, [Description], [Image], ProductType) VALUES
(1, 'Milk Tea Trân Châu', 30000, 'Trà sữa truyền thống với trân châu dai', '', 1), -- Regular
(1, 'Milk Tea Matcha', 35000, 'Trà sữa vị matcha thơm ngon', '', 1),         -- Regular
(1, 'Milk Tea Strawberry', 32000, 'Trà sữa dâu ngọt ngào', '', 1),            -- Regular
(2, 'Trân Châu', 5000, 'Topping trân châu', '', 2),                           -- Extra
(2, 'Thạch Dừa', 6000, 'Topping thạch dừa', '', 2),                          -- Extra
(2, 'Pudding', 7000, 'Topping pudding', '', 2),                              -- Extra
(3, 'Combo Couple', 50000, '2 Milk Tea bất kỳ + 1 Topping', '', 3),          -- Combo
(3, 'Combo Family', 85000, '3 Milk Tea bất kỳ + 2 Topping', '', 3);          -- Combo
GO

-- Thêm dữ liệu vào ComboProducts
INSERT INTO ComboProducts (ComboProductId, IncludedProductId, Quantity) VALUES
(7, 1, 2), -- Combo Couple: 2 Milk Tea Trân Châu
(7, 4, 1), -- Combo Couple: 1 Trân Châu
(8, 2, 3), -- Combo Family: 3 Milk Tea Matcha
(8, 5, 2); -- Combo Family: 2 Thạch Dừa
GO

-- Thêm dữ liệu vào Orders
INSERT INTO Orders (AccountId, CustomerName, TotalAmount, Status) VALUES
(2, 'Nguyen Van A', 41000, 'Completed'),  -- Đơn hàng 1
(3, 'Tran Thi B', 50000, 'Pending'),      -- Đơn hàng 2
(2, 'Nguyen Van A', 85000, 'Processing'); -- Đơn hàng 3
GO

-- Thêm dữ liệu vào OrderDetails
INSERT INTO OrderDetails (OrderId, ProductId, Quantity, UnitPrice) VALUES
(1, 1, 1, 30000), -- Order 1: 1 Milk Tea Trân Châu
(1, 4, 1, 5000),  -- Order 1: 1 Trân Châu
(1, 5, 1, 6000),  -- Order 1: 1 Thạch Dừa
(2, 7, 1, 50000), -- Order 2: 1 Combo Couple
(3, 8, 1, 85000); -- Order 3: 1 Combo Family
GO

-- Thêm dữ liệu vào Payments
INSERT INTO Payments (OrderId, Amount, PaymentMethod, Status) VALUES
(1, 41000, 'Cash', 'Completed'),
(2, 50000, 'Credit Card', 'Pending'),
(3, 85000, 'Mobile App', 'Pending');
GO