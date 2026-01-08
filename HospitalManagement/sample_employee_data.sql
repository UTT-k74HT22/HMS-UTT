-- Sample Employee Data for Testing
-- Run this script after running db.sql

USE hms;
GO

-- Insert Employee Accounts
INSERT INTO dbo.accounts (username, [password], role, is_active, created_at, updated_at)
VALUES 
    ('emp001', '123456', 'EMPLOYEE', 1, SYSDATETIME(), SYSDATETIME()),
    ('emp002', '123456', 'EMPLOYEE', 1, SYSDATETIME(), SYSDATETIME()),
    ('emp003', '123456', 'EMPLOYEE', 1, SYSDATETIME(), SYSDATETIME()),
    ('emp004', '123456', 'EMPLOYEE', 1, SYSDATETIME(), SYSDATETIME()),
    ('emp005', '123456', 'EMPLOYEE', 1, SYSDATETIME(), SYSDATETIME());
GO

-- Insert User Profiles for Employees
INSERT INTO dbo.user_profiles (account_id, code, full_name, phone, email, address, status, created_at, updated_at)
VALUES 
    ((SELECT id FROM dbo.accounts WHERE username = 'emp001'), 'NV001', N'Nguyễn Văn An', '0912345001', 'an.nguyen@hospital.com', N'Hà Nội', 'ACTIVE', SYSDATETIME(), SYSDATETIME()),
    ((SELECT id FROM dbo.accounts WHERE username = 'emp002'), 'NV002', N'Trần Thị Bình', '0912345002', 'binh.tran@hospital.com', N'Hà Nội', 'ACTIVE', SYSDATETIME(), SYSDATETIME()),
    ((SELECT id FROM dbo.accounts WHERE username = 'emp003'), 'NV003', N'Lê Văn Cường', '0912345003', 'cuong.le@hospital.com', N'Hồ Chí Minh', 'ACTIVE', SYSDATETIME(), SYSDATETIME()),
    ((SELECT id FROM dbo.accounts WHERE username = 'emp004'), 'NV004', N'Phạm Thị Dung', '0912345004', 'dung.pham@hospital.com', N'Đà Nẵng', 'ACTIVE', SYSDATETIME(), SYSDATETIME()),
    ((SELECT id FROM dbo.accounts WHERE username = 'emp005'), 'NV005', N'Hoàng Văn Em', '0912345005', 'em.hoang@hospital.com', N'Hải Phòng', 'ACTIVE', SYSDATETIME(), SYSDATETIME());
GO

-- Insert Employee Profiles
INSERT INTO dbo.employee_profiles (profile_id, position, department, hired_date, base_salary, created_at, updated_at)
VALUES 
    ((SELECT id FROM dbo.user_profiles WHERE code = 'NV001'), N'Bác sĩ', N'Khoa Nội', '2023-01-15', 15000000, SYSDATETIME(), SYSDATETIME()),
    ((SELECT id FROM dbo.user_profiles WHERE code = 'NV002'), N'Y tá', N'Khoa Nhi', '2023-03-20', 8000000, SYSDATETIME(), SYSDATETIME()),
    ((SELECT id FROM dbo.user_profiles WHERE code = 'NV003'), N'Dược sĩ', N'Nhà thuốc', '2022-11-10', 12000000, SYSDATETIME(), SYSDATETIME()),
    ((SELECT id FROM dbo.user_profiles WHERE code = 'NV004'), N'Kế toán', N'Phòng Tài chính', '2023-05-01', 10000000, SYSDATETIME(), SYSDATETIME()),
    ((SELECT id FROM dbo.user_profiles WHERE code = 'NV005'), N'Quản lý', N'Phòng Hành chính', '2022-08-15', 18000000, SYSDATETIME(), SYSDATETIME());
GO

PRINT 'Sample employee data inserted successfully!';

