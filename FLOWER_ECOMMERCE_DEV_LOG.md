# 🌸 FLOWER E-COMMERCE - DEVELOPMENT LOG
**Date**: January 31, 2026  
**Tech Stack**: .NET 9 Web API, SQL Server (EF Core), Vite + React/Vue  
**Architecture**: Clean Architecture / CQRS (MediatR)  
**Security**: Argon2, JWT with Refresh Tokens, Rate Limiting  
**Status**: ✅ Domain Layer Completed

---

## 📋 PROJECT OVERVIEW

### Tech Stack
- **Backend**: .NET 9 Web API
- **Database**: SQL Server with EF Core
- **Frontend**: Vite + React/Vue
- **Architecture**: Clean Architecture + CQRS (MediatR)
- **Security**: 
  - Argon2 Password Hashing
  - JWT with Refresh Tokens
  - Rate Limiting
- **Standards**: SOLID, Clean Code, DRY

### Project Goals
✅ Scalable  
✅ Production-ready  
✅ Secure  
✅ Maintainable  

---

## 🗂️ PROJECT STRUCTURE

```
FlowerShop.Domain/
├── Entities/
│   ├── User.cs
│   ├── RefreshToken.cs
│   ├── Category.cs
│   ├── Product.cs
│   ├── ProductCategory.cs
│   ├── ProductImage.cs
│   ├── ProductVariant.cs
│   ├── Cart.cs
│   ├── CartItem.cs
│   ├── Order.cs
│   ├── OrderItem.cs
│   ├── ShippingAddress.cs
│   ├── ProductReview.cs
│   ├── Coupon.cs
│   ├── CouponUsage.cs
│   └── Wishlist.cs
├── Enums/
│   ├── UserRole.cs
│   ├── OrderStatus.cs
│   ├── PaymentStatus.cs
│   ├── PaymentMethod.cs
│   └── DiscountType.cs
├── Common/
│   ├── BaseEntity.cs
│   ├── BaseAuditableEntity.cs
│   ├── ISoftDelete.cs
│   └── IConcurrencyControl.cs
└── ValueObjects/
    └── Money.cs

FlowerShop.Infrastructure/
├── Data/
│   ├── ApplicationDbContext.cs (TODO)
│   ├── Configurations/ (TODO)
│   └── Migrations/ (TODO)

FlowerShop.Application/ (TODO)
FlowerShop.API/ (TODO)
FlowerShop.Web/ (TODO)
```

---

## 📊 DATABASE SCHEMA DESIGN

### **Core Tables**

#### **Users** - Authentication & User Management
```sql
Users
├── Id (GUID, PK)
├── Email (nvarchar(255), UNIQUE, NOT NULL)
├── PasswordHash (nvarchar(500), NOT NULL) -- Argon2
├── FullName (nvarchar(200), NOT NULL)
├── PhoneNumber (nvarchar(20), NULL)
├── Role (nvarchar(50), NOT NULL) -- Admin, Customer
├── IsEmailVerified (bit, DEFAULT 0)
├── EmailVerificationToken (nvarchar(500), NULL)
├── IsActive (bit, DEFAULT 1)
├── CreatedAt (datetime2, NOT NULL)
├── UpdatedAt (datetime2, NULL)
├── LastLoginAt (datetime2, NULL)
├── IsDeleted (bit, DEFAULT 0)
├── DeletedAt (datetime2, NULL)
└── DeletedBy (GUID, NULL)
```

#### **RefreshTokens** - JWT Security
```sql
RefreshTokens
├── Id (GUID, PK)
├── UserId (GUID, FK -> Users.Id)
├── Token (nvarchar(500), NOT NULL)
├── ExpiresAt (datetime2, NOT NULL)
├── CreatedAt (datetime2, NOT NULL)
├── RevokedAt (datetime2, NULL)
├── IsRevoked (bit, DEFAULT 0)
├── IsDeleted (bit, DEFAULT 0)
├── DeletedAt (datetime2, NULL)
└── DeletedBy (GUID, NULL)
```

### **Product Catalog**

#### **Categories** - Hierarchical Product Categories
```sql
Categories
├── Id (GUID, PK)
├── Name (nvarchar(100), NOT NULL)
├── Slug (nvarchar(100), UNIQUE, NOT NULL)
├── Description (nvarchar(500), NULL)
├── ImageUrl (nvarchar(500), NULL)
├── ParentCategoryId (GUID, FK -> Categories.Id, NULL)
├── DisplayOrder (int, DEFAULT 0)
├── IsActive (bit, DEFAULT 1)
├── CreatedAt (datetime2, NOT NULL)
├── UpdatedAt (datetime2, NULL)
├── IsDeleted (bit, DEFAULT 0)
├── DeletedAt (datetime2, NULL)
└── DeletedBy (GUID, NULL)
```

#### **Products** - Main Product Entity
```sql
Products
├── Id (GUID, PK)
├── Name (nvarchar(200), NOT NULL)
├── Slug (nvarchar(200), UNIQUE, NOT NULL)
├── Description (nvarchar(MAX), NULL)
├── ShortDescription (nvarchar(500), NULL)
├── SKU (nvarchar(50), UNIQUE, NOT NULL)
├── Price (decimal(18,2), NOT NULL)
├── CompareAtPrice (decimal(18,2), NULL)
├── CostPerItem (decimal(18,2), NULL)
├── StockQuantity (int, NOT NULL, DEFAULT 0)
├── LowStockThreshold (int, DEFAULT 5)
├── Weight (decimal(10,2), NULL)
├── IsActive (bit, DEFAULT 1)
├── IsFeatured (bit, DEFAULT 0)
├── ViewCount (int, DEFAULT 0)
├── SoldCount (int, DEFAULT 0)
├── RowVersion (rowversion, NOT NULL) -- Concurrency Control
├── CreatedAt (datetime2, NOT NULL)
├── UpdatedAt (datetime2, NULL)
├── IsDeleted (bit, DEFAULT 0)
├── DeletedAt (datetime2, NULL)
└── DeletedBy (GUID, NULL)
```

#### **ProductCategories** - Many-to-Many Join Table
```sql
ProductCategories
├── ProductId (GUID, FK -> Products.Id, PK)
└── CategoryId (GUID, FK -> Categories.Id, PK)
```

#### **ProductImages** - Product Image Gallery
```sql
ProductImages
├── Id (GUID, PK)
├── ProductId (GUID, FK -> Products.Id, NOT NULL)
├── ImageUrl (nvarchar(500), NOT NULL)
├── AltText (nvarchar(200), NULL)
├── DisplayOrder (int, DEFAULT 0)
├── IsThumbnail (bit, DEFAULT 0)
├── CreatedAt (datetime2, NOT NULL)
├── UpdatedAt (datetime2, NULL)
├── IsDeleted (bit, DEFAULT 0)
├── DeletedAt (datetime2, NULL)
└── DeletedBy (GUID, NULL)
```

#### **ProductVariants** - Product Size/Type Variations
```sql
ProductVariants
├── Id (GUID, PK)
├── ProductId (GUID, FK -> Products.Id, NOT NULL)
├── Name (nvarchar(100), NOT NULL)
├── SKU (nvarchar(50), UNIQUE, NOT NULL)
├── Price (decimal(18,2), NOT NULL)
├── StockQuantity (int, NOT NULL)
├── IsActive (bit, DEFAULT 1)
├── RowVersion (rowversion, NOT NULL) -- Concurrency Control
├── CreatedAt (datetime2, NOT NULL)
├── UpdatedAt (datetime2, NULL)
├── IsDeleted (bit, DEFAULT 0)
├── DeletedAt (datetime2, NULL)
└── DeletedBy (GUID, NULL)
```

### **Shopping & Orders**

#### **Carts** - Shopping Cart
```sql
Carts
├── Id (GUID, PK)
├── UserId (GUID, FK -> Users.Id, NULL)
├── SessionId (nvarchar(200), NULL)
├── CreatedAt (datetime2, NOT NULL)
├── UpdatedAt (datetime2, NULL)
├── IsDeleted (bit, DEFAULT 0)
├── DeletedAt (datetime2, NULL)
└── DeletedBy (GUID, NULL)
```

#### **CartItems** - Cart Line Items
```sql
CartItems
├── Id (GUID, PK)
├── CartId (GUID, FK -> Carts.Id, NOT NULL)
├── ProductId (GUID, FK -> Products.Id, NOT NULL)
├── ProductVariantId (GUID, FK -> ProductVariants.Id, NULL)
├── Quantity (int, NOT NULL)
├── UnitPrice (decimal(18,2), NOT NULL)
├── CreatedAt (datetime2, NOT NULL)
├── UpdatedAt (datetime2, NULL)
├── IsDeleted (bit, DEFAULT 0)
├── DeletedAt (datetime2, NULL)
└── DeletedBy (GUID, NULL)
```

#### **Orders** - Customer Orders
```sql
Orders
├── Id (GUID, PK)
├── OrderNumber (nvarchar(50), UNIQUE, NOT NULL)
├── UserId (GUID, FK -> Users.Id, NOT NULL)
├── Status (nvarchar(50), NOT NULL)
├── PaymentStatus (nvarchar(50), NOT NULL)
├── PaymentMethod (nvarchar(50), NOT NULL)
├── SubTotal (decimal(18,2), NOT NULL)
├── ShippingFee (decimal(18,2), NOT NULL)
├── DiscountAmount (decimal(18,2), DEFAULT 0)
├── TotalAmount (decimal(18,2), NOT NULL)
├── Note (nvarchar(500), NULL)
├── CreatedAt (datetime2, NOT NULL)
├── UpdatedAt (datetime2, NULL)
├── CompletedAt (datetime2, NULL)
├── IsDeleted (bit, DEFAULT 0)
├── DeletedAt (datetime2, NULL)
└── DeletedBy (GUID, NULL)
```

**Formula**: $T_{order} = \sum_{i=1}^{n} (Q_i \times P_i) + F_{ship} - D_{coupon}$

Where:
- $Q_i$: Quantity of item $i$
- $P_i$: Unit price of item $i$ at purchase time
- $F_{ship}$: Shipping fee
- $D_{coupon}$: Coupon discount amount

#### **OrderItems** - Order Line Items
```sql
OrderItems
├── Id (GUID, PK)
├── OrderId (GUID, FK -> Orders.Id, NOT NULL)
├── ProductId (GUID, FK -> Products.Id, NOT NULL)
├── ProductName (nvarchar(200), NOT NULL)
├── ProductVariantId (GUID, FK -> ProductVariants.Id, NULL)
├── Quantity (int, NOT NULL)
├── UnitPrice (decimal(18,2), NOT NULL)
├── SubTotal (decimal(18,2), NOT NULL)
├── CreatedAt (datetime2, NOT NULL)
├── UpdatedAt (datetime2, NULL)
├── IsDeleted (bit, DEFAULT 0)
├── DeletedAt (datetime2, NULL)
└── DeletedBy (GUID, NULL)
```

#### **ShippingAddresses** - Delivery Information
```sql
ShippingAddresses
├── Id (GUID, PK)
├── OrderId (GUID, FK -> Orders.Id, UNIQUE, NOT NULL)
├── RecipientName (nvarchar(200), NOT NULL)
├── PhoneNumber (nvarchar(20), NOT NULL)
├── AddressLine1 (nvarchar(300), NOT NULL)
├── AddressLine2 (nvarchar(300), NULL)
├── City (nvarchar(100), NOT NULL)
├── District (nvarchar(100), NULL)
├── Ward (nvarchar(100), NULL)
├── PostalCode (nvarchar(20), NULL)
├── CreatedAt (datetime2, NOT NULL)
├── UpdatedAt (datetime2, NULL)
├── IsDeleted (bit, DEFAULT 0)
├── DeletedAt (datetime2, NULL)
└── DeletedBy (GUID, NULL)
```

### **Reviews & Ratings**

#### **ProductReviews** - Customer Product Reviews
```sql
ProductReviews
├── Id (GUID, PK)
├── ProductId (GUID, FK -> Products.Id, NOT NULL)
├── UserId (GUID, FK -> Users.Id, NOT NULL)
├── OrderId (GUID, FK -> Orders.Id, NULL)
├── Rating (int, NOT NULL) -- 1-5
├── Title (nvarchar(200), NULL)
├── Comment (nvarchar(1000), NULL)
├── IsVerifiedPurchase (bit, DEFAULT 0)
├── IsApproved (bit, DEFAULT 0)
├── CreatedAt (datetime2, NOT NULL)
├── UpdatedAt (datetime2, NULL)
├── IsDeleted (bit, DEFAULT 0)
├── DeletedAt (datetime2, NULL)
└── DeletedBy (GUID, NULL)
```

### **Discounts & Promotions**

#### **Coupons** - Discount Codes
```sql
Coupons
├── Id (GUID, PK)
├── Code (nvarchar(50), UNIQUE, NOT NULL)
├── Description (nvarchar(300), NULL)
├── DiscountType (nvarchar(20), NOT NULL)
├── DiscountValue (decimal(18,2), NOT NULL)
├── MinimumOrderAmount (decimal(18,2), NULL)
├── MaxDiscountAmount (decimal(18,2), NULL)
├── UsageLimit (int, NULL)
├── UsedCount (int, DEFAULT 0)
├── StartDate (datetime2, NOT NULL)
├── EndDate (datetime2, NOT NULL)
├── IsActive (bit, DEFAULT 1)
├── CreatedAt (datetime2, NOT NULL)
├── UpdatedAt (datetime2, NULL)
├── IsDeleted (bit, DEFAULT 0)
├── DeletedAt (datetime2, NULL)
└── DeletedBy (GUID, NULL)
```

#### **CouponUsages** - Coupon Usage Tracking
```sql
CouponUsages
├── Id (GUID, PK)
├── CouponId (GUID, FK -> Coupons.Id, NOT NULL)
├── UserId (GUID, FK -> Users.Id, NOT NULL)
├── OrderId (GUID, FK -> Orders.Id, NOT NULL)
├── UsedAt (datetime2, NOT NULL)
├── CreatedAt (datetime2, NOT NULL)
├── UpdatedAt (datetime2, NULL)
├── IsDeleted (bit, DEFAULT 0)
├── DeletedAt (datetime2, NULL)
└── DeletedBy (GUID, NULL)
```

### **Wishlist**

#### **Wishlists** - User Favorite Products
```sql
Wishlists
├── Id (GUID, PK)
├── UserId (GUID, FK -> Users.Id, NOT NULL)
├── ProductId (GUID, FK -> Products.Id, NOT NULL)
├── CreatedAt (datetime2, NOT NULL)
├── UpdatedAt (datetime2, NULL)
├── IsDeleted (bit, DEFAULT 0)
├── DeletedAt (datetime2, NULL)
└── DeletedBy (GUID, NULL)

UNIQUE INDEX on (UserId, ProductId)
```

---

## 🏗️ DOMAIN LAYER ARCHITECTURE

### **Production-Ready Features**

#### **A. Soft Delete Mechanism**
**Purpose**: Preserve data integrity for reporting and auditing

**Interface**: `ISoftDelete`
```csharp
public interface ISoftDelete
{
    bool IsDeleted { get; set; }
    DateTime? DeletedAt { get; set; }
    Guid? DeletedBy { get; set; }
}
```

**Implementation**: All entities inherit from `BaseEntity` which implements `ISoftDelete`

**Benefits**:
- ✅ Never lose historical data
- ✅ Support for data recovery
- ✅ Audit trail preservation
- ✅ Reporting accuracy

---

#### **B. Optimistic Concurrency Control**
**Purpose**: Prevent race conditions in inventory management

**Interface**: `IConcurrencyControl`
```csharp
public interface IConcurrencyControl
{
    byte[] RowVersion { get; set; }
}
```

**Applied to**:
- ✅ `Product` entity
- ✅ `ProductVariant` entity

**Scenario**: Two customers buying the last item simultaneously
- Without concurrency control: Negative stock (❌)
- With concurrency control: Second customer gets error, first succeeds (✅)

**EF Core Configuration**:
```csharp
entity.Property(e => e.RowVersion)
    .IsRowVersion()
    .IsConcurrencyToken();
```

---

#### **C. Domain Logic - Financial Calculations**

**Order Total Calculation**:
$$T_{order} = \sum_{i=1}^{n} (Q_i \times P_i) + F_{ship} - D_{coupon}$$

**Key Domain Methods**:

**Order.cs**:
- `CalculateTotals()` - Calculate order totals
- `ApplyDiscount(decimal)` - Apply coupon discount
- `UpdateStatus(OrderStatus)` - Update order status with validation
- `MarkAsPaid()` - Mark payment as completed
- `Cancel()` - Cancel order

**OrderItem.cs**:
- `CalculateSubTotal()` - Calculate item subtotal: $SubTotal = Q_i \times P_i$
- `UpdateQuantity(int)` - Update quantity and recalculate

**Product.cs**:
- `IsInStock(int)` - Check stock availability
- `IsLowStock()` - Check if stock is low
- `ReduceStock(int)` - Reduce stock with validation
- `RestoreStock(int)` - Restore stock (e.g., order cancellation)

**ProductVariant.cs**:
- `IsInStock(int)` - Check variant stock
- `ReduceStock(int)` - Reduce variant stock
- `RestoreStock(int)` - Restore variant stock

**CartItem.cs**:
- `GetTotal()` - Calculate cart item total
- `UpdateQuantity(int)` - Update quantity
- `IncreaseQuantity(int)` - Increase quantity
- `DecreaseQuantity(int)` - Decrease quantity

**Coupon.cs**:
- `CalculateDiscount(decimal)` - Calculate discount for order
- `IsValid()` - Check if coupon is valid
- `IncrementUsage()` - Increment usage count
- `DecrementUsage()` - Decrement usage count

---

#### **D. Value Objects**

**Money.cs** - Immutable monetary value representation
```csharp
public record Money
{
    public decimal Amount { get; init; }
    public string Currency { get; init; } = "VND";
    
    // Operators: +, -, *, >, <, >=, <=
}
```

**Benefits**:
- ✅ Type safety for monetary values
- ✅ Prevent negative amounts
- ✅ Currency consistency
- ✅ Automatic rounding to 2 decimals

---

## 📐 DESIGN PATTERNS & PRINCIPLES

### **SOLID Principles**

**S - Single Responsibility**
- Each entity has a single, well-defined purpose
- Domain logic encapsulated within relevant entities

**O - Open/Closed**
- Entities open for extension via inheritance
- Closed for modification via interfaces

**L - Liskov Substitution**
- `BaseEntity` can be substituted with any derived entity
- Interface segregation (`ISoftDelete`, `IConcurrencyControl`)

**I - Interface Segregation**
- Small, focused interfaces
- Entities implement only needed interfaces

**D - Dependency Inversion**
- Depend on abstractions (`ISoftDelete`, `IConcurrencyControl`)
- Not on concrete implementations

---

### **Domain-Driven Design (DDD)**

**Rich Domain Model**:
- ❌ Anemic models (just getters/setters)
- ✅ Rich models with behavior and business rules

**Entities**:
- Have identity (GUID)
- Can change over time
- Equality based on ID

**Value Objects**:
- No identity
- Immutable
- Equality based on value (`Money`)

**Aggregates**:
- `Order` aggregate root
  - Contains `OrderItems`, `ShippingAddress`
- `Cart` aggregate root
  - Contains `CartItems`
- `Product` aggregate root
  - Contains `ProductImages`, `ProductVariants`

**Domain Events** (TODO):
- `OrderCreatedEvent`
- `OrderCancelledEvent`
- `ProductStockChangedEvent`
- `CouponAppliedEvent`

---

## 🔒 SECURITY FEATURES

### **Password Security**
- ✅ Argon2 hashing algorithm
- ✅ Never store plain text passwords
- ✅ Salt automatically handled by Argon2

### **JWT Authentication**
- ✅ Access tokens (short-lived)
- ✅ Refresh tokens (long-lived, stored in DB)
- ✅ Token revocation support
- ✅ Secure token storage

### **Data Protection**
- ✅ Soft delete prevents data loss
- ✅ Audit trail (CreatedAt, UpdatedAt, DeletedAt)
- ✅ User tracking (CreatedBy, UpdatedBy, DeletedBy)

### **Rate Limiting** (TODO)
- Prevent brute force attacks
- API throttling
- DDoS protection

---

## 📊 PERFORMANCE OPTIMIZATIONS

### **Database Indexes** (TODO in Fluent API)

```sql
-- Search & SEO
CREATE INDEX IX_Products_Slug ON Products(Slug);
CREATE INDEX IX_Categories_Slug ON Categories(Slug);

-- Product Filtering
CREATE INDEX IX_Products_IsActive_IsFeatured ON Products(IsActive, IsFeatured);
CREATE INDEX IX_Products_IsActive_StockQuantity ON Products(IsActive, StockQuantity);

-- Order Queries
CREATE INDEX IX_Orders_UserId_Status ON Orders(UserId, Status);
CREATE INDEX IX_Orders_OrderNumber ON Orders(OrderNumber);
CREATE INDEX IX_Orders_CreatedAt ON Orders(CreatedAt DESC);

-- Security
CREATE INDEX IX_RefreshTokens_Token ON RefreshTokens(Token);
CREATE INDEX IX_RefreshTokens_UserId_IsRevoked ON RefreshTokens(UserId, IsRevoked);
CREATE INDEX IX_Users_Email ON Users(Email);

-- Soft Delete Filtering
CREATE INDEX IX_Products_IsDeleted ON Products(IsDeleted);
CREATE INDEX IX_Orders_IsDeleted ON Orders(IsDeleted);
CREATE INDEX IX_Users_IsDeleted ON Users(IsDeleted);
```

### **Query Optimization Strategies**

**Pagination**:
- Always use `.Skip()` and `.Take()`
- Include total count for UI

**Lazy Loading**:
- Disable by default
- Use explicit loading or eager loading

**Projections**:
- Use DTOs for API responses
- Select only needed fields

**Caching** (TODO):
- Product catalog
- Categories
- User sessions

---

## 🎯 BUSINESS RULES

### **Product Management**
1. ✅ SKU must be unique across products and variants
2. ✅ Stock cannot go negative (enforced by domain logic)
3. ✅ Low stock threshold alerts
4. ✅ Price must be greater than 0
5. ✅ Soft delete preserves product history

### **Order Processing**
1. ✅ Order number auto-generated (format: ORD-YYYYMMDD-NNNN)
2. ✅ Order total calculated using formula
3. ✅ Cannot cancel delivered orders
4. ✅ Cannot modify cancelled orders
5. ✅ Stock reduced on order creation
6. ✅ Stock restored on order cancellation
7. ✅ Payment status transitions validated

### **Coupon System**
1. ✅ Coupon must be active and within date range
2. ✅ Usage limit enforced
3. ✅ Minimum order amount required
4. ✅ Max discount cap applied
5. ✅ Discount cannot exceed order subtotal

### **Inventory Management**
1. ✅ Optimistic concurrency control prevents overselling
2. ✅ Stock quantity updated atomically
3. ✅ Low stock notifications
4. ✅ Variant-level stock tracking

### **User Management**
1. ✅ Email must be unique
2. ✅ Email verification required
3. ✅ Soft delete for user accounts
4. ✅ Role-based access control

---

## ✅ COMPLETED TASKS

- [x] Database schema design
- [x] Domain entities implementation
- [x] Enums for type safety
- [x] Base entity with soft delete
- [x] Concurrency control interface
- [x] Rich domain model with business logic
- [x] Value objects (Money)
- [x] Financial calculation methods
- [x] Stock management methods
- [x] Order status validation
- [x] Coupon validation logic

---

## 📝 TODO - NEXT STEPS

### **Infrastructure Layer**
- [ ] Create `ApplicationDbContext`
- [ ] Fluent API configurations for all entities
- [ ] Configure indexes
- [ ] Configure relationships (one-to-many, many-to-many)
- [ ] Configure soft delete query filter
- [ ] Configure concurrency tokens
- [ ] Seed data for categories, roles
- [ ] Create initial migration

### **Application Layer**
- [ ] CQRS structure with MediatR
- [ ] DTOs (Data Transfer Objects)
- [ ] AutoMapper profiles
- [ ] Validation with FluentValidation
- [ ] Command handlers
- [ ] Query handlers
- [ ] Domain event handlers

### **API Layer**
- [ ] Controllers
- [ ] JWT authentication setup
- [ ] Argon2 password hashing service
- [ ] Authorization policies
- [ ] Rate limiting middleware
- [ ] Exception handling middleware
- [ ] API versioning
- [ ] Swagger/OpenAPI documentation
- [ ] CORS configuration

### **Frontend**
- [ ] Setup Vite + React/Vue
- [ ] State management (Redux/Pinia)
- [ ] API client service
- [ ] Authentication flow
- [ ] Product catalog UI
- [ ] Shopping cart
- [ ] Checkout process
- [ ] Order management
- [ ] Admin panel

### **Testing**
- [ ] Unit tests for domain logic
- [ ] Integration tests for repositories
- [ ] API integration tests
- [ ] End-to-end tests

### **DevOps**
- [ ] Docker containerization
- [ ] CI/CD pipeline
- [ ] Environment configurations
- [ ] Logging (Serilog)
- [ ] Monitoring
- [ ] Database backup strategy

---

## 🔧 CONFIGURATION NOTES

### **appsettings.json** (TODO)
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=FlowerShopDb;Trusted_Connection=True;TrustServerCertificate=True"
  },
  "JwtSettings": {
    "Secret": "YOUR_SECRET_KEY_HERE",
    "Issuer": "FlowerShopAPI",
    "Audience": "FlowerShopClient",
    "AccessTokenExpirationMinutes": 15,
    "RefreshTokenExpirationDays": 7
  },
  "Argon2Settings": {
    "TimeCost": 3,
    "MemoryCost": 65536,
    "Parallelism": 4
  },
  "RateLimiting": {
    "PermitLimit": 100,
    "Window": 60
  }
}
```

### **NuGet Packages Required**
```xml
<!-- Domain Layer -->
<!-- No external dependencies -->

<!-- Infrastructure Layer -->
<PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="9.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="9.0.0" />

<!-- Application Layer -->
<PackageReference Include="MediatR" Version="12.2.0" />
<PackageReference Include="AutoMapper" Version="12.0.1" />
<PackageReference Include="FluentValidation" Version="11.9.0" />

<!-- API Layer -->
<PackageReference Include="Microsoft.AspNetCore.Authentication.JwtBearer" Version="9.0.0" />
<PackageReference Include="Konscious.Security.Cryptography.Argon2" Version="1.3.0" />
<PackageReference Include="Swashbuckle.AspNetCore" Version="6.5.0" />
<PackageReference Include="Serilog.AspNetCore" Version="8.0.0" />
<PackageReference Include="AspNetCoreRateLimit" Version="5.0.0" />
```

---

## 📚 REFERENCES

### **Domain-Driven Design**
- Eric Evans - Domain-Driven Design: Tackling Complexity in the Heart of Software
- Vaughn Vernon - Implementing Domain-Driven Design

### **Clean Architecture**
- Robert C. Martin - Clean Architecture
- Jason Taylor - Clean Architecture Template

### **Security**
- OWASP Top 10
- JWT Best Practices
- Argon2 Password Hashing

### **E-commerce Best Practices**
- Shopify Development Patterns
- Magento Architecture
- WooCommerce Standards

---

## 💡 KEY INSIGHTS

### **Why Soft Delete?**
In e-commerce, hard deleting records can:
- ❌ Break order history
- ❌ Lose financial audit trails
- ❌ Remove customer purchase records
- ❌ Disrupt analytics and reporting

Soft delete ensures:
- ✅ Data integrity
- ✅ Regulatory compliance (GDPR allows deletion but requires audit trail)
- ✅ Analytics accuracy
- ✅ Recovery capability

### **Why Concurrency Control?**
Race conditions in e-commerce lead to:
- ❌ Overselling (negative inventory)
- ❌ Double-booking
- ❌ Lost updates
- ❌ Customer dissatisfaction

Optimistic concurrency ensures:
- ✅ Data consistency
- ✅ Accurate inventory
- ✅ Better user experience
- ✅ Trust in the system

### **Why Domain Logic in Entities?**
Anemic domain models (just data, no behavior) lead to:
- ❌ Business logic scattered in services
- ❌ Duplicate validation
- ❌ Hard to test
- ❌ Hard to maintain

Rich domain models provide:
- ✅ Single source of truth for business rules
- ✅ Self-documenting code
- ✅ Easier testing
- ✅ Better encapsulation

---

## 🎓 LEARNING RESOURCES

### **Clean Architecture & DDD**
- [Microsoft Clean Architecture](https://github.com/jasontaylordev/CleanArchitecture)
- [Domain-Driven Design Reference](https://www.domainlanguage.com/ddd/reference/)

### **EF Core**
- [EF Core Documentation](https://docs.microsoft.com/en-us/ef/core/)
- [EF Core Best Practices](https://docs.microsoft.com/en-us/ef/core/performance/)

### **Security**
- [JWT.io](https://jwt.io/)
- [OWASP API Security](https://owasp.org/www-project-api-security/)

---

## 📞 CONTACT & NOTES

**Developer Notes**:
- Always run migrations in a transaction
- Test concurrency scenarios before deployment
- Monitor query performance regularly
- Keep domain logic in domain layer
- Never expose entities directly via API (use DTOs)

**Next Session Goals**:
1. Complete `ApplicationDbContext`
2. Write Fluent API configurations
3. Create first migration
4. Test database creation

---

**Last Updated**: January 31, 2026  
**Status**: Domain Layer Complete ✅  
**Next**: Infrastructure Layer - DbContext & Configurations

---

## 🔖 QUICK REFERENCE

### **Entity Relationships Cheat Sheet**

```
User (1) ─────── (∞) RefreshToken
User (1) ─────── (∞) Order
User (1) ─────── (∞) ProductReview
User (1) ─────── (∞) Wishlist
User (1) ─────── (1) Cart

Product (∞) ─────── (∞) Category (via ProductCategory)
Product (1) ─────── (∞) ProductImage
Product (1) ─────── (∞) ProductVariant
Product (1) ─────── (∞) ProductReview

Order (1) ─────── (∞) OrderItem
Order (1) ─────── (1) ShippingAddress
Order (∞) ─────── (∞) Coupon (via CouponUsage)

Cart (1) ─────── (∞) CartItem
CartItem (∞) ─────── (1) Product
CartItem (∞) ─────── (1) ProductVariant (optional)

Coupon (1) ─────── (∞) CouponUsage
```

### **Status Enums**

**OrderStatus**:
1. Pending → 2. Processing → 3. Shipped → 4. Delivered
                           ↘ 5. Cancelled

**PaymentStatus**:
1. Pending → 2. Paid
          ↘ 3. Failed
          ↘ 4. Refunded

### **Formula Quick Reference**

**Order Total**:
```
T_order = Σ(Qi × Pi) + F_ship - D_coupon
```

**Order Item Subtotal**:
```
SubTotal_i = Qi × Pi
```

**Coupon Discount (Percentage)**:
```
D_coupon = min(SubTotal × (DiscountValue / 100), MaxDiscountAmount)
```

**Coupon Discount (Fixed)**:
```
D_coupon = min(DiscountValue, SubTotal)
```

---

**END OF DEVELOPMENT LOG**
