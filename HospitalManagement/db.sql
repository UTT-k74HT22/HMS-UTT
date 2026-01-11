/* =========================================================
   HMS_DB - Inventory & Order Management (SQL Server)
   Idempotent script (safe to run multiple times)
   ========================================================= */
SET NOCOUNT ON;
GO

-- 1) Create DB if not exists
-- IF DB_ID(N'hms') IS NULL
--     BEGIN
--         EXEC(N'CREATE DATABASE hms;');
--     END
-- GO

USE hms;
GO

/* ===================== 1) ACCOUNTS ===================== */
IF OBJECT_ID(N'dbo.accounts', N'U') IS NULL
    BEGIN
        CREATE TABLE dbo.accounts
        (
            id            INT IDENTITY(1,1) PRIMARY KEY,
            username      VARCHAR(50)  NOT NULL,
            [password]    VARCHAR(255) NOT NULL,
            role          VARCHAR(20)  NOT NULL,
            is_active     BIT          NOT NULL CONSTRAINT df_accounts_is_active DEFAULT (1),
            last_login_at DATETIME2    NULL,
            created_at    DATETIME2    NOT NULL CONSTRAINT df_accounts_created_at DEFAULT (SYSDATETIME()),
            updated_at    DATETIME2    NOT NULL CONSTRAINT df_accounts_updated_at DEFAULT (SYSDATETIME()),
            CONSTRAINT uq_accounts_username UNIQUE (username),
            CONSTRAINT ck_accounts_role CHECK (role IN ('ADMIN','EMPLOYEE','CUSTOMER'))
        );
    END
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'idx_accounts_role' AND object_id = OBJECT_ID(N'dbo.accounts'))
CREATE INDEX idx_accounts_role ON dbo.accounts(role);
GO

CREATE OR ALTER TRIGGER dbo.trg_accounts_updated_at
    ON dbo.accounts
    AFTER UPDATE
    AS
BEGIN
    SET NOCOUNT ON;
    UPDATE a
    SET updated_at = SYSDATETIME()
    FROM dbo.accounts a
             INNER JOIN inserted i ON a.id = i.id;
END;
GO


/* ===================== 2) USER PROFILES ===================== */
IF OBJECT_ID(N'dbo.user_profiles', N'U') IS NULL
    BEGIN
        CREATE TABLE dbo.user_profiles
        (
            id         INT IDENTITY(1,1) PRIMARY KEY,
            account_id INT          NOT NULL,

            code       VARCHAR(50)  NOT NULL,
            full_name  VARCHAR(150) NOT NULL,
            phone      VARCHAR(20)  NULL,
            email      VARCHAR(100) NULL,
            address    VARCHAR(255) NULL,

            status     VARCHAR(20)  NOT NULL CONSTRAINT df_user_profiles_status DEFAULT ('ACTIVE'),
            created_at DATETIME2    NOT NULL CONSTRAINT df_user_profiles_created_at DEFAULT (SYSDATETIME()),
            updated_at DATETIME2    NOT NULL CONSTRAINT df_user_profiles_updated_at DEFAULT (SYSDATETIME()),

            CONSTRAINT uq_user_profiles_account UNIQUE (account_id),
            CONSTRAINT uq_user_profiles_code UNIQUE (code),
            CONSTRAINT uq_user_profiles_phone UNIQUE (phone),
            CONSTRAINT ck_user_profiles_status CHECK (status IN ('ACTIVE','INACTIVE','SUSPENDED'))
        );
    END
GO

-- FK profiles -> accounts (cascade)
IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'fk_profiles_account')
    BEGIN
        ALTER TABLE dbo.user_profiles
            ADD CONSTRAINT fk_profiles_account
                FOREIGN KEY (account_id) REFERENCES dbo.accounts(id)
                    ON DELETE CASCADE;
    END
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'idx_user_profiles_status' AND object_id = OBJECT_ID(N'dbo.user_profiles'))
CREATE INDEX idx_user_profiles_status ON dbo.user_profiles(status);
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'idx_user_profiles_account_id' AND object_id = OBJECT_ID(N'dbo.user_profiles'))
CREATE INDEX idx_user_profiles_account_id ON dbo.user_profiles(account_id);
GO

CREATE OR ALTER TRIGGER dbo.trg_user_profiles_updated_at
    ON dbo.user_profiles
    AFTER UPDATE
    AS
BEGIN
    SET NOCOUNT ON;
    UPDATE p
    SET updated_at = SYSDATETIME()
    FROM dbo.user_profiles p
             INNER JOIN inserted i ON p.id = i.id;
END;
GO


/* ===================== 3) EMPLOYEE PROFILES ===================== */
IF OBJECT_ID(N'dbo.employee_profiles', N'U') IS NULL
    BEGIN
        CREATE TABLE dbo.employee_profiles
        (
            id          INT IDENTITY(1,1) PRIMARY KEY,
            profile_id  INT            NOT NULL,

            position    VARCHAR(100)   NULL,
            department  VARCHAR(100)   NULL,
            hired_date  DATE           NULL,
            base_salary DECIMAL(18,2)  NOT NULL CONSTRAINT df_emp_base_salary DEFAULT (0),

            created_at  DATETIME2      NOT NULL CONSTRAINT df_emp_created_at DEFAULT (SYSDATETIME()),
            updated_at  DATETIME2      NOT NULL CONSTRAINT df_emp_updated_at DEFAULT (SYSDATETIME()),

            CONSTRAINT uq_employee_profiles_profile UNIQUE (profile_id)
        );
    END
GO

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'fk_employee_profile')
    BEGIN
        ALTER TABLE dbo.employee_profiles
            ADD CONSTRAINT fk_employee_profile
                FOREIGN KEY (profile_id) REFERENCES dbo.user_profiles(id)
                    ON DELETE CASCADE;
    END
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'idx_emp_department' AND object_id = OBJECT_ID(N'dbo.employee_profiles'))
CREATE INDEX idx_emp_department ON dbo.employee_profiles(department);
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'idx_emp_position' AND object_id = OBJECT_ID(N'dbo.employee_profiles'))
CREATE INDEX idx_emp_position ON dbo.employee_profiles(position);
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'idx_emp_profile_id' AND object_id = OBJECT_ID(N'dbo.employee_profiles'))
CREATE INDEX idx_emp_profile_id ON dbo.employee_profiles(profile_id);
GO

CREATE OR ALTER TRIGGER dbo.trg_employee_profiles_updated_at
    ON dbo.employee_profiles
    AFTER UPDATE
    AS
BEGIN
    SET NOCOUNT ON;
    UPDATE e
    SET updated_at = SYSDATETIME()
    FROM dbo.employee_profiles e
             INNER JOIN inserted i ON e.id = i.id;
END;
GO


/* ===================== 4) CUSTOMER PROFILES ===================== */
IF OBJECT_ID(N'dbo.customer_profiles', N'U') IS NULL
    BEGIN
        CREATE TABLE dbo.customer_profiles
        (
            id            INT IDENTITY(1,1) PRIMARY KEY,
            profile_id    INT         NOT NULL,

            customer_type VARCHAR(20) NOT NULL,
            tax_code      VARCHAR(50) NULL,

            created_at    DATETIME2   NOT NULL CONSTRAINT df_cus_created_at DEFAULT (SYSDATETIME()),
            updated_at    DATETIME2   NOT NULL CONSTRAINT df_cus_updated_at DEFAULT (SYSDATETIME()),

            CONSTRAINT uq_customer_profiles_profile UNIQUE (profile_id),
            CONSTRAINT ck_customer_type CHECK (customer_type IN ('RETAIL','WHOLESALE'))
        );
    END
GO

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'fk_customer_profile')
    BEGIN
        ALTER TABLE dbo.customer_profiles
            ADD CONSTRAINT fk_customer_profile
                FOREIGN KEY (profile_id) REFERENCES dbo.user_profiles(id)
                    ON DELETE CASCADE;
    END
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'idx_customer_type' AND object_id = OBJECT_ID(N'dbo.customer_profiles'))
CREATE INDEX idx_customer_type ON dbo.customer_profiles(customer_type);
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'idx_customer_profile_id' AND object_id = OBJECT_ID(N'dbo.customer_profiles'))
CREATE INDEX idx_customer_profile_id ON dbo.customer_profiles(profile_id);
GO

CREATE OR ALTER TRIGGER dbo.trg_customer_profiles_updated_at
    ON dbo.customer_profiles
    AFTER UPDATE
    AS
BEGIN
    SET NOCOUNT ON;
    UPDATE c
    SET updated_at = SYSDATETIME()
    FROM dbo.customer_profiles c
             INNER JOIN inserted i ON c.id = i.id;
END;
GO


/* ===================== 5) CATEGORIES ===================== */
IF OBJECT_ID(N'dbo.categories', N'U') IS NULL
    BEGIN
        CREATE TABLE dbo.categories
        (
            id            INT IDENTITY(1,1) PRIMARY KEY,
            code          VARCHAR(50)   NOT NULL,
            name          VARCHAR(100)  NOT NULL,
            description   NVARCHAR(MAX) NULL,
            parent_id     INT           NULL,
            is_active     BIT           NOT NULL CONSTRAINT df_categories_is_active DEFAULT (1),
            display_order INT           NOT NULL CONSTRAINT df_categories_display_order DEFAULT (0),
            created_at    DATETIME2     NOT NULL CONSTRAINT df_categories_created_at DEFAULT (SYSDATETIME()),
            updated_at    DATETIME2     NOT NULL CONSTRAINT df_categories_updated_at DEFAULT (SYSDATETIME()),
            CONSTRAINT uq_categories_code UNIQUE (code)
        );
    END
GO

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'fk_categories_parent')
    BEGIN
        ALTER TABLE dbo.categories
            ADD CONSTRAINT fk_categories_parent
                FOREIGN KEY (parent_id) REFERENCES dbo.categories(id)
                    ON DELETE NO ACTION;
    END
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'idx_categories_parent_id' AND object_id = OBJECT_ID(N'dbo.categories'))
CREATE INDEX idx_categories_parent_id ON dbo.categories(parent_id);
GO

CREATE OR ALTER TRIGGER dbo.trg_categories_updated_at
    ON dbo.categories
    AFTER UPDATE
    AS
BEGIN
    SET NOCOUNT ON;
    UPDATE c
    SET updated_at = SYSDATETIME()
    FROM dbo.categories c
             INNER JOIN inserted i ON c.id = i.id;
END;
GO

CREATE OR ALTER TRIGGER dbo.trg_categories_delete_setnull
    ON dbo.categories
    INSTEAD OF DELETE
    AS
BEGIN
    SET NOCOUNT ON;

    UPDATE c
    SET parent_id = NULL
    FROM dbo.categories c
             INNER JOIN deleted d ON c.parent_id = d.id;

    DELETE c
    FROM dbo.categories c
             INNER JOIN deleted d ON c.id = d.id;
END;
GO


/* ===================== 6) MANUFACTURERS ===================== */
IF OBJECT_ID(N'dbo.manufacturers', N'U') IS NULL
    BEGIN
        CREATE TABLE dbo.manufacturers
        (
            id             INT IDENTITY(1,1) PRIMARY KEY,
            code           VARCHAR(50)  NOT NULL,
            name           VARCHAR(150) NOT NULL,
            country        VARCHAR(100) NULL,
            address        VARCHAR(255) NULL,
            phone          VARCHAR(20)  NULL,
            email          VARCHAR(100) NULL,
            contact_person VARCHAR(100) NULL,
            created_at     DATETIME2    NOT NULL CONSTRAINT df_manu_created_at DEFAULT (SYSDATETIME()),
            updated_at     DATETIME2    NOT NULL CONSTRAINT df_manu_updated_at DEFAULT (SYSDATETIME()),
            CONSTRAINT uq_manufacturers_code UNIQUE(code)
        );
    END
GO

CREATE OR ALTER TRIGGER dbo.trg_manufacturers_updated_at
    ON dbo.manufacturers
    AFTER UPDATE
    AS
BEGIN
    SET NOCOUNT ON;
    UPDATE m
    SET updated_at = SYSDATETIME()
    FROM dbo.manufacturers m
             INNER JOIN inserted i ON m.id = i.id;
END;
GO


/* ===================== 7) PRODUCTS ===================== */
IF OBJECT_ID(N'dbo.products', N'U') IS NULL
    BEGIN
        CREATE TABLE dbo.products
        (
            id                    INT IDENTITY(1,1) PRIMARY KEY,
            category_id           INT            NOT NULL,
            manufacturer_id       INT            NULL,
            code                  VARCHAR(50)    NOT NULL,
            barcode               VARCHAR(100)   NULL,
            name                  VARCHAR(150)   NOT NULL,
            dosage_form           VARCHAR(100)   NULL,
            unit                  VARCHAR(50)    NULL,
            description           NVARCHAR(MAX)  NULL,
            image_url             VARCHAR(255)   NULL,
            standard_price        DECIMAL(18,2)  NOT NULL CONSTRAINT df_products_standard_price DEFAULT (0),
            requires_prescription BIT            NOT NULL CONSTRAINT df_products_requires_rx DEFAULT (0),
            status                VARCHAR(20)    NOT NULL CONSTRAINT df_products_status DEFAULT ('ACTIVE'),
            created_at            DATETIME2      NOT NULL CONSTRAINT df_products_created_at DEFAULT (SYSDATETIME()),
            updated_at            DATETIME2      NOT NULL CONSTRAINT df_products_updated_at DEFAULT (SYSDATETIME()),
            CONSTRAINT uq_products_code UNIQUE(code),
            CONSTRAINT uq_products_barcode UNIQUE(barcode),
            CONSTRAINT ck_products_status CHECK (status IN ('ACTIVE','INACTIVE','DISCONTINUED'))
        );
    END
GO

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'fk_products_category')
    BEGIN
        ALTER TABLE dbo.products
            ADD CONSTRAINT fk_products_category
                FOREIGN KEY (category_id) REFERENCES dbo.categories(id);
    END
GO

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'fk_products_manufacturer')
    BEGIN
        ALTER TABLE dbo.products
            ADD CONSTRAINT fk_products_manufacturer
                FOREIGN KEY (manufacturer_id) REFERENCES dbo.manufacturers(id)
                    ON DELETE SET NULL;
    END
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'idx_products_category_id' AND object_id = OBJECT_ID(N'dbo.products'))
CREATE INDEX idx_products_category_id ON dbo.products(category_id);
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'idx_products_manufacturer_id' AND object_id = OBJECT_ID(N'dbo.products'))
CREATE INDEX idx_products_manufacturer_id ON dbo.products(manufacturer_id);
GO

CREATE OR ALTER TRIGGER dbo.trg_products_updated_at
    ON dbo.products
    AFTER UPDATE
    AS
BEGIN
    SET NOCOUNT ON;
    UPDATE p
    SET updated_at = SYSDATETIME()
    FROM dbo.products p
             INNER JOIN inserted i ON p.id = i.id;
END;
GO


/* ===================== 8) WAREHOUSES ===================== */
IF OBJECT_ID(N'dbo.warehouses', N'U') IS NULL
    BEGIN
        CREATE TABLE dbo.warehouses
        (
            id           INT IDENTITY(1,1) PRIMARY KEY,
            code         VARCHAR(50)  NOT NULL,
            name         VARCHAR(150) NOT NULL,
            address      VARCHAR(255) NULL,
            phone        VARCHAR(20)  NULL,
            manager_name VARCHAR(100) NULL,
            is_active    BIT          NOT NULL CONSTRAINT df_warehouses_is_active DEFAULT (1),
            created_at   DATETIME2    NOT NULL CONSTRAINT df_warehouses_created_at DEFAULT (SYSDATETIME()),
            updated_at   DATETIME2    NOT NULL CONSTRAINT df_warehouses_updated_at DEFAULT (SYSDATETIME()),
            CONSTRAINT uq_warehouses_code UNIQUE(code)
        );
    END
GO

CREATE OR ALTER TRIGGER dbo.trg_warehouses_updated_at
    ON dbo.warehouses
    AFTER UPDATE
    AS
BEGIN
    SET NOCOUNT ON;
    UPDATE w
    SET updated_at = SYSDATETIME()
    FROM dbo.warehouses w
             INNER JOIN inserted i ON w.id = i.id;
END;
GO


/* ===================== 9) BATCHES ===================== */
IF OBJECT_ID(N'dbo.batches', N'U') IS NULL
    BEGIN
        CREATE TABLE dbo.batches
        (
            id               INT IDENTITY(1,1) PRIMARY KEY,
            product_id       INT            NOT NULL,
            batch_code       VARCHAR(100)   NOT NULL,
            import_price     DECIMAL(18,2)  NOT NULL CONSTRAINT df_batches_import_price DEFAULT (0),
            manufacture_date DATE           NULL,
            expiry_date      DATE           NULL,
            supplier_name    VARCHAR(150)   NULL,
            status           VARCHAR(20)    NOT NULL CONSTRAINT df_batches_status DEFAULT ('ACTIVE'),
            created_at       DATETIME2      NOT NULL CONSTRAINT df_batches_created_at DEFAULT (SYSDATETIME()),
            updated_at       DATETIME2      NOT NULL CONSTRAINT df_batches_updated_at DEFAULT (SYSDATETIME()),
            CONSTRAINT uq_batches_batch_code UNIQUE(batch_code),
            CONSTRAINT ck_batches_status CHECK (status IN ('ACTIVE','EXPIRED','BLOCKED','DEPLETED'))
        );
    END
GO

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'fk_batches_product')
    BEGIN
        ALTER TABLE dbo.batches
            ADD CONSTRAINT fk_batches_product
                FOREIGN KEY (product_id) REFERENCES dbo.products(id);
    END
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'idx_batches_product_id' AND object_id = OBJECT_ID(N'dbo.batches'))
CREATE INDEX idx_batches_product_id ON dbo.batches(product_id);
GO

CREATE OR ALTER TRIGGER dbo.trg_batches_updated_at
    ON dbo.batches
    AFTER UPDATE
    AS
BEGIN
    SET NOCOUNT ON;
    UPDATE b
    SET updated_at = SYSDATETIME()
    FROM dbo.batches b
             INNER JOIN inserted i ON b.id = i.id;
END;
GO


/* ===================== 10) INVENTORY ITEMS ===================== */
IF OBJECT_ID(N'dbo.inventory_items', N'U') IS NULL
    BEGIN
        CREATE TABLE dbo.inventory_items
        (
            id                INT IDENTITY(1,1) PRIMARY KEY,
            product_id        INT       NOT NULL,
            batch_id          INT       NULL,
            warehouse_id      INT       NOT NULL,
            quantity_on_hand  INT       NOT NULL CONSTRAINT df_inv_qoh DEFAULT (0),
            quantity_reserved INT       NOT NULL CONSTRAINT df_inv_qr DEFAULT (0),
            min_threshold     INT       NOT NULL CONSTRAINT df_inv_min DEFAULT (0),
            max_threshold     INT       NOT NULL CONSTRAINT df_inv_max DEFAULT (0),
            last_stock_check  DATE      NULL,
            created_at        DATETIME2 NOT NULL CONSTRAINT df_inv_created_at DEFAULT (SYSDATETIME()),
            updated_at        DATETIME2 NOT NULL CONSTRAINT df_inv_updated_at DEFAULT (SYSDATETIME()),
            CONSTRAINT uq_inventory UNIQUE (product_id, batch_id, warehouse_id)
        );
    END
GO

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'fk_inventory_product')
    BEGIN
        ALTER TABLE dbo.inventory_items
            ADD CONSTRAINT fk_inventory_product
                FOREIGN KEY (product_id) REFERENCES dbo.products(id);
    END
GO

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'fk_inventory_batch')
    BEGIN
        ALTER TABLE dbo.inventory_items
            ADD CONSTRAINT fk_inventory_batch
                FOREIGN KEY (batch_id) REFERENCES dbo.batches(id)
                    ON DELETE SET NULL;
    END
GO

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'fk_inventory_warehouse')
    BEGIN
        ALTER TABLE dbo.inventory_items
            ADD CONSTRAINT fk_inventory_warehouse
                FOREIGN KEY (warehouse_id) REFERENCES dbo.warehouses(id);
    END
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'idx_inventory_product_id' AND object_id = OBJECT_ID(N'dbo.inventory_items'))
CREATE INDEX idx_inventory_product_id ON dbo.inventory_items(product_id);
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'idx_inventory_batch_id' AND object_id = OBJECT_ID(N'dbo.inventory_items'))
CREATE INDEX idx_inventory_batch_id ON dbo.inventory_items(batch_id);
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'idx_inventory_warehouse_id' AND object_id = OBJECT_ID(N'dbo.inventory_items'))
CREATE INDEX idx_inventory_warehouse_id ON dbo.inventory_items(warehouse_id);
GO

CREATE OR ALTER TRIGGER dbo.trg_inventory_items_updated_at
    ON dbo.inventory_items
    AFTER UPDATE
    AS
BEGIN
    SET NOCOUNT ON;
    UPDATE x
    SET updated_at = SYSDATETIME()
    FROM dbo.inventory_items x
             INNER JOIN inserted i ON x.id = i.id;
END;
GO


/* ===================== 11) STOCK MOVEMENTS ===================== */
IF OBJECT_ID(N'dbo.stock_movements', N'U') IS NULL
    BEGIN
        CREATE TABLE dbo.stock_movements
        (
            id                   INT IDENTITY(1,1) PRIMARY KEY,
            movement_type        VARCHAR(20)  NOT NULL,
            product_id           INT          NOT NULL,
            batch_id             INT          NULL,
            warehouse_id         INT          NOT NULL,
            quantity             INT          NOT NULL,
            quantity_before      INT          NULL,
            quantity_after       INT          NULL,
            movement_date        DATETIME2    NOT NULL CONSTRAINT df_sm_movement_date DEFAULT (SYSDATETIME()),
            reference_type       VARCHAR(50)  NULL,
            reference_id         INT          NULL,
            performed_by_user_id INT          NULL,
            note                 VARCHAR(255) NULL,
            created_at           DATETIME2    NOT NULL CONSTRAINT df_sm_created_at DEFAULT (SYSDATETIME()),
            CONSTRAINT ck_sm_movement_type CHECK (movement_type IN ('IMPORT','EXPORT','ADJUST','TRANSFER'))
        );
    END
GO

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'fk_sm_product')
    BEGIN
        ALTER TABLE dbo.stock_movements
            ADD CONSTRAINT fk_sm_product
                FOREIGN KEY (product_id) REFERENCES dbo.products(id);
    END
GO

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'fk_sm_batch')
    BEGIN
        ALTER TABLE dbo.stock_movements
            ADD CONSTRAINT fk_sm_batch
                FOREIGN KEY (batch_id) REFERENCES dbo.batches(id)
                    ON DELETE SET NULL;
    END
GO

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'fk_sm_warehouse')
    BEGIN
        ALTER TABLE dbo.stock_movements
            ADD CONSTRAINT fk_sm_warehouse
                FOREIGN KEY (warehouse_id) REFERENCES dbo.warehouses(id);
    END
GO

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'fk_sm_user')
    BEGIN
        ALTER TABLE dbo.stock_movements
            ADD CONSTRAINT fk_sm_user
                FOREIGN KEY (performed_by_user_id) REFERENCES dbo.user_profiles(id)
                    ON DELETE SET NULL;
    END
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'idx_sm_product_id' AND object_id = OBJECT_ID(N'dbo.stock_movements'))
CREATE INDEX idx_sm_product_id ON dbo.stock_movements(product_id);
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'idx_sm_batch_id' AND object_id = OBJECT_ID(N'dbo.stock_movements'))
CREATE INDEX idx_sm_batch_id ON dbo.stock_movements(batch_id);
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'idx_sm_warehouse_id' AND object_id = OBJECT_ID(N'dbo.stock_movements'))
CREATE INDEX idx_sm_warehouse_id ON dbo.stock_movements(warehouse_id);
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'idx_sm_user_id' AND object_id = OBJECT_ID(N'dbo.stock_movements'))
CREATE INDEX idx_sm_user_id ON dbo.stock_movements(performed_by_user_id);
GO


/* ===================== 12) ORDERS ===================== */
IF OBJECT_ID(N'dbo.orders', N'U') IS NULL
    BEGIN
        CREATE TABLE dbo.orders
        (
            id                 INT IDENTITY(1,1) PRIMARY KEY,
            customer_id        INT            NOT NULL,
            order_number       VARCHAR(50)    NOT NULL,
            order_date         DATETIME2      NOT NULL CONSTRAINT df_orders_order_date DEFAULT (SYSDATETIME()),
            status             VARCHAR(20)    NOT NULL CONSTRAINT df_orders_status DEFAULT ('NEW'),
            subtotal           DECIMAL(18,2)  NOT NULL CONSTRAINT df_orders_subtotal DEFAULT (0),
            discount           DECIMAL(18,2)  NOT NULL CONSTRAINT df_orders_discount DEFAULT (0),
            tax                DECIMAL(18,2)  NOT NULL CONSTRAINT df_orders_tax DEFAULT (0),
            total_amount       DECIMAL(18,2)  NOT NULL CONSTRAINT df_orders_total DEFAULT (0),
            shipping_address   VARCHAR(255)   NULL,
            created_by_user_id INT            NULL,
            note               VARCHAR(255)   NULL,
            created_at         DATETIME2      NOT NULL CONSTRAINT df_orders_created_at DEFAULT (SYSDATETIME()),
            updated_at         DATETIME2      NOT NULL CONSTRAINT df_orders_updated_at DEFAULT (SYSDATETIME()),
            CONSTRAINT uq_orders_order_number UNIQUE(order_number),
            CONSTRAINT ck_orders_status CHECK (status IN ('NEW','CONFIRMED','PROCESSING','SHIPPED','COMPLETED','CANCELED'))
        );
    END
GO

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'fk_orders_customer')
    BEGIN
        ALTER TABLE dbo.orders
            ADD CONSTRAINT fk_orders_customer
                FOREIGN KEY (customer_id) REFERENCES dbo.user_profiles(id);
    END
GO

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'fk_orders_creator')
    BEGIN
        ALTER TABLE dbo.orders
            ADD CONSTRAINT fk_orders_creator
                FOREIGN KEY (created_by_user_id) REFERENCES dbo.user_profiles(id)
                    ON DELETE SET NULL;
    END
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'idx_orders_customer_id' AND object_id = OBJECT_ID(N'dbo.orders'))
CREATE INDEX idx_orders_customer_id ON dbo.orders(customer_id);
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'idx_orders_creator_id' AND object_id = OBJECT_ID(N'dbo.orders'))
CREATE INDEX idx_orders_creator_id ON dbo.orders(created_by_user_id);
GO

CREATE OR ALTER TRIGGER dbo.trg_orders_updated_at
    ON dbo.orders
    AFTER UPDATE
    AS
BEGIN
    SET NOCOUNT ON;
    UPDATE o
    SET updated_at = SYSDATETIME()
    FROM dbo.orders o
             INNER JOIN inserted i ON o.id = i.id;
END;
GO


/* ===================== 13) ORDER ITEMS ===================== */
IF OBJECT_ID(N'dbo.order_items', N'U') IS NULL
    BEGIN
        CREATE TABLE dbo.order_items
        (
            id         INT IDENTITY(1,1) PRIMARY KEY,
            order_id   INT            NOT NULL,
            product_id INT            NOT NULL,
            batch_id   INT            NULL,
            quantity   INT            NOT NULL,
            unit_price DECIMAL(18,2)  NOT NULL,
            discount   DECIMAL(18,2)  NOT NULL CONSTRAINT df_oi_discount DEFAULT (0),
            line_total DECIMAL(18,2)  NOT NULL
        );
    END
GO

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'fk_oi_order')
    BEGIN
        ALTER TABLE dbo.order_items
            ADD CONSTRAINT fk_oi_order
                FOREIGN KEY (order_id) REFERENCES dbo.orders(id)
                    ON DELETE CASCADE;
    END
GO

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'fk_oi_product')
    BEGIN
        ALTER TABLE dbo.order_items
            ADD CONSTRAINT fk_oi_product
                FOREIGN KEY (product_id) REFERENCES dbo.products(id);
    END
GO

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'fk_oi_batch')
    BEGIN
        ALTER TABLE dbo.order_items
            ADD CONSTRAINT fk_oi_batch
                FOREIGN KEY (batch_id) REFERENCES dbo.batches(id)
                    ON DELETE SET NULL;
    END
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'idx_oi_order_id' AND object_id = OBJECT_ID(N'dbo.order_items'))
CREATE INDEX idx_oi_order_id ON dbo.order_items(order_id);
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'idx_oi_product_id' AND object_id = OBJECT_ID(N'dbo.order_items'))
CREATE INDEX idx_oi_product_id ON dbo.order_items(product_id);
GO
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'idx_oi_batch_id' AND object_id = OBJECT_ID(N'dbo.order_items'))
CREATE INDEX idx_oi_batch_id ON dbo.order_items(batch_id);
GO


/* ===================== 14) INVOICES ===================== */
IF OBJECT_ID(N'dbo.invoices', N'U') IS NULL
    BEGIN
        CREATE TABLE dbo.invoices
        (
            id             INT IDENTITY(1,1) PRIMARY KEY,
            order_id       INT            NOT NULL,
            invoice_number VARCHAR(50)    NOT NULL,
            issue_date     DATETIME2      NOT NULL CONSTRAINT df_invoices_issue_date DEFAULT (SYSDATETIME()),
            due_date       DATETIME2      NULL,
            total_amount   DECIMAL(18,2)  NOT NULL,
            paid_amount    DECIMAL(18,2)  NOT NULL CONSTRAINT df_invoices_paid DEFAULT (0),
            status         VARCHAR(20)    NOT NULL CONSTRAINT df_invoices_status DEFAULT ('NEW'),
            created_at     DATETIME2      NOT NULL CONSTRAINT df_invoices_created_at DEFAULT (SYSDATETIME()),
            updated_at     DATETIME2      NOT NULL CONSTRAINT df_invoices_updated_at DEFAULT (SYSDATETIME()),
            CONSTRAINT uq_invoices_invoice_number UNIQUE(invoice_number),
            CONSTRAINT ck_invoices_status CHECK (status IN ('NEW','PAID','PARTIAL','CANCELED'))
        );
    END
GO

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'fk_invoice_order')
    BEGIN
        ALTER TABLE dbo.invoices
            ADD CONSTRAINT fk_invoice_order
                FOREIGN KEY (order_id) REFERENCES dbo.orders(id);
    END
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'idx_invoices_order_id' AND object_id = OBJECT_ID(N'dbo.invoices'))
CREATE INDEX idx_invoices_order_id ON dbo.invoices(order_id);
GO

CREATE OR ALTER TRIGGER dbo.trg_invoices_updated_at
    ON dbo.invoices
    AFTER UPDATE
    AS
BEGIN
    SET NOCOUNT ON;
    UPDATE v
    SET updated_at = SYSDATETIME()
    FROM dbo.invoices v
             INNER JOIN inserted i ON v.id = i.id;
END;
GO


/* ===================== 15) PAYMENTS ===================== */
IF OBJECT_ID(N'dbo.payments', N'U') IS NULL
    BEGIN
        CREATE TABLE dbo.payments
        (
            id             INT IDENTITY(1,1) PRIMARY KEY,
            invoice_id     INT            NOT NULL,
            payment_number VARCHAR(50)    NOT NULL,
            payment_date   DATETIME2      NOT NULL CONSTRAINT df_payments_payment_date DEFAULT (SYSDATETIME()),
            amount         DECIMAL(18,2)  NOT NULL,
            method         VARCHAR(50)    NOT NULL,
            status         VARCHAR(20)    NOT NULL CONSTRAINT df_payments_status DEFAULT ('SUCCESS'),
            created_at     DATETIME2      NOT NULL CONSTRAINT df_payments_created_at DEFAULT (SYSDATETIME()),
            CONSTRAINT uq_payments_payment_number UNIQUE(payment_number),
            CONSTRAINT ck_payments_status CHECK (status IN ('SUCCESS','FAILED','PENDING','CANCELED'))
        );
    END
GO

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'fk_payment_invoice')
    BEGIN
        ALTER TABLE dbo.payments
            ADD CONSTRAINT fk_payment_invoice
                FOREIGN KEY (invoice_id) REFERENCES dbo.invoices(id);
    END
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = N'idx_payments_invoice_id' AND object_id = OBJECT_ID(N'dbo.payments'))
CREATE INDEX idx_payments_invoice_id ON dbo.payments(invoice_id);
GO


INSERT INTO dbo.accounts (username, [password], role, is_active, created_at, updated_at)
VALUES ('admin', '123456789', 'ADMIN', 1, SYSDATETIME(), SYSDATETIME());
/*chạy lại để ko lỗi hin thị */
ALTER TABLE dbo.categories
ALTER COLUMN name NVARCHAR(100) NOT NULL;

ALTER TABLE dbo.manufacturers
ALTER COLUMN name NVARCHAR(150) NOT NULL;

ALTER TABLE dbo.manufacturers
ALTER COLUMN country NVARCHAR(100);

ALTER TABLE dbo.manufacturers
ALTER COLUMN address NVARCHAR(255);

ALTER TABLE dbo.manufacturers
ALTER COLUMN contact_person NVARCHAR(100);

ALTER TABLE dbo.products
ALTER COLUMN name NVARCHAR(150) NOT NULL;

ALTER TABLE dbo.products
ALTER COLUMN dosage_form NVARCHAR(100);

ALTER TABLE dbo.products
ALTER COLUMN unit NVARCHAR(50);

ALTER TABLE dbo.warehouses
ALTER COLUMN name NVARCHAR(255);

ALTER TABLE dbo.warehouses
ALTER COLUMN address NVARCHAR(255);
ALTER TABLE dbo.warehouses
ALTER COLUMN manager_name NVARCHAR(255);

ALTER TABLE dbo.batches
ALTER COLUMN supplier_name NVARCHAR(255);

      
ALTER TABLE dbo.order_items
    ADD warehouse_id INT NULL,
    note VARCHAR(255) NULL;
