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
    CONSTRAINT CHK_AccountRole CHECK (AccountRole IN (1, 2))
);
GO

-- Bảng Categories (Danh mục sản phẩm)
CREATE TABLE Categories (
    CategoryId INT PRIMARY KEY IDENTITY(1,1),
    [Name] NVARCHAR(100) NOT NULL,
    [Description] NVARCHAR(255),
    IsActive BIT DEFAULT 1
);
GO

-- Bảng Products (Sản phẩm chính như Milk Tea, Topping, Combo)
CREATE TABLE Products (
    ProductId INT PRIMARY KEY IDENTITY(1,1),
    CategoryId INT NOT NULL,
    [Name] NVARCHAR(100) NOT NULL,
    Price DECIMAL(10,3) NOT NULL,
    [Description] NVARCHAR(255),
    [Image] VARCHAR(MAX),
    ProductType INT NOT NULL DEFAULT 1, -- 1: Regular, 2: Extra
    IsActive BIT DEFAULT 1,
    CONSTRAINT CHK_Price CHECK (Price >= 0),
    CONSTRAINT CHK_ProductType CHECK (ProductType IN (1, 2)),
    FOREIGN KEY (CategoryId) REFERENCES Categories(CategoryId)
);
GO

-- Bảng ComboProducts (Liên kết giữa các sản phẩm trong combo)
CREATE TABLE Combo (
    ComboId INT PRIMARY KEY IDENTITY(1, 1),       -- ProductId của sản phẩm là combo
	ComboName NVARCHAR(100),
	ComboPrice DECIMAL(10,3),
	[Description] NVARCHAR(MAX),
	[Image] NVARCHAR(MAX),
    ProductId1 INT, 
	ProductId2 INT,
	ProductId3 INT,
	IsActive BIT DEFAULT 1,
    FOREIGN KEY (ProductId1) REFERENCES Products(ProductId),
	FOREIGN KEY (ProductId2) REFERENCES Products(ProductId),
	FOREIGN KEY (ProductId3) REFERENCES Products(ProductId),
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
    ProductId INT NULL,
	ComboId INT NULL,
    Quantity INT NOT NULL,
    UnitPrice DECIMAL(18,2) NOT NULL,
    CONSTRAINT CHK_OrderDetail_Quantity CHECK (Quantity > 0),
    CONSTRAINT CHK_UnitPrice CHECK (UnitPrice >= 0),
    FOREIGN KEY (OrderId) REFERENCES Orders(OrderId),
    FOREIGN KEY (ProductId) REFERENCES Products(ProductId),
    FOREIGN KEY (ComboId) REFERENCES Combo(ComboId)
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
INSERT INTO Categories ([Name], [Description]) VALUES
('Milk Tea', N'Các loại trà sữa'),
('Topping', N'Các loại topping bổ sung'),
('Combo', N'Các gói combo');
GO

-- Thêm dữ liệu vào Products (đã thay IsExtra, IsCombo bằng ProductType)
INSERT INTO Products (CategoryId, [Name], Price, [Description], [Image], ProductType) VALUES
(1, N'Milk Tea Trân Châu', 30, N'Trà sữa truyền thống với trân châu dai', 'https://gongcha.com.vn/wp-content/uploads/2018/02/Tr%C3%A0-s%E1%BB%AFa-tr%C3%A0-%C4%91en-3.png', 1), -- Regular
(1, N'Milk Tea Matcha', 35, N'Trà sữa vị matcha thơm ngon', 'https://gongcha.com.vn/wp-content/uploads/2018/08/Strawberry-Oreo-Smoothie.png', 1),         -- Regular
(1, N'Milk Tea Strawberry', 32, N'Trà sữa dâu ngọt ngào', 'https://gongcha.com.vn/wp-content/uploads/2021/12/Yakult-Dao-Da-Xay.png', 1),            -- Regular
(2, N'Trân Châu', 5, N'Topping trân châu', 'https://gongcha.com.vn/wp-content/uploads/2018/03/Tr%C3%A2n-Ch%C3%A2u-%C4%90en.png', 2),                           -- Extra
(2, N'Thạch Dừa', 6, N'Topping thạch dừa', 'https://gongcha.com.vn/wp-content/uploads/2018/03/Kem-S%E1%BB%AFa.png', 2),                          -- Extra
(2, N'Pudding', 7, N'Topping pudding', 'https://gongcha.com.vn/wp-content/uploads/2018/03/%E5%B8%83%E4%B8%81-pudding.png', 2);                            -- Extra
GO

INSERT INTO Combo(ComboName, ComboPrice, [Description], [Image], ProductId1, ProductId2, ProductId3, IsActive) VALUES
(N'Combo Family', 75, N'Một sự kết hợp hoàn hảo dành cho cả gia đình! Combo Family mang đến hương vị thơm ngon từ trà sữa truyền thống, hòa quyện cùng lớp kem sánh mịn và topping hấp dẫn. Một lựa chọn tuyệt vời để cả nhà cùng nhau thưởng thức và tận hưởng khoảnh khắc ngọt ngào!', 'https://gongcha.com.vn/wp-content/uploads/2018/02/Tr%C3%A0-s%E1%BB%AFa-Chocolate-2.png', 1, 2, 3, 1),
(N'Combo Summer', 105, N'Mùa hè chưa bao giờ sảng khoái đến thế! Combo Summer mang đến hương vị trà matcha đá xay thanh mát, kết hợp với lớp kem mịn màng và vị ngọt dịu của sữa. Hoàn hảo để giải nhiệt và tiếp thêm năng lượng cho ngày dài!', 'https://gongcha.com.vn/wp-content/uploads/2018/02/Matcha-%C4%91%C3%A1-xay-2.png', 4, 5, 6, 1);

-- Thêm dữ liệu vào Orders
INSERT INTO Orders (AccountId, CustomerName, TotalAmount, Status) VALUES
(2, N'Nguyen Van A', 41, 'Completed'),  -- Đơn hàng 1
(3, N'Tran Thi B', 55, 'Pending'),      -- Đơn hàng 2
(2, N'Nguyen Van A', 85, 'Processing'); -- Đơn hàng 3
GO

-- Thêm dữ liệu vào OrderDetails
INSERT INTO OrderDetails (OrderId, ProductId, ComboId, Quantity, UnitPrice) VALUES
(1, 1, null, 1, 30), -- Order 1: 1 Milk Tea Trân Châu
(1, 4, null, 1, 5),  -- Order 1: 1 Trân Châu
(1, 5, null, 1, 6);  -- Order 1: 1 Thạch Dừa
GO

-- Thêm dữ liệu vào Payments
INSERT INTO Payments (OrderId, Amount, PaymentMethod, Status) VALUES
(1, 41000, 'Cash', 'Completed'),
(2, 50000, 'Credit Card', 'Pending'),
(3, 85000, 'Mobile App', 'Pending');
GO