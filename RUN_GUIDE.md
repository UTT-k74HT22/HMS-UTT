# Hướng dẫn chạy ứng dụng

## 1. Chuẩn bị Database

```sql
-- Chạy file test_accounts.sql để tạo test accounts
source test_accounts.sql;
```

## 2. Test Accounts

### Admin (Full quyền)
- Username: `admin`
- Password: `admin123`
- Xem được: Tất cả menu

### Employee
- Username: `employee1`
- Password: `emp123`
- Xem được: Dashboard, Danh mục, Kho, Bán hàng, Khách hàng, Thống kê

### Customer
- Username: `customer1`
- Password: `cust123`
- Xem được: Sản phẩm, Đơn hàng, Hóa đơn, Thanh toán

## 3. Chạy ứng dụng

```bash
dotnet run
```

## 4. Flow

1. **Login Form** → Nhập username/password
2. **MainFrame** → Tự động mở với sidebar theo role
3. **Navigation** → Click menu để chuyển màn
4. **Logout** → Click "Đăng xuất" ở sidebar hoặc nút Profile

## 5. Các màn đã implement

- ✅ Dashboard
- ✅ Quản lý tài khoản (Account Management)
- ✅ Quản lý nhân viên (Employee Management)
- 🚧 Các màn khác đang phát triển...

## 6. Tính năng

- ✅ Login/Logout
- ✅ Role-based menu (Admin/Employee/Customer)
- ✅ Dynamic navigation
- ✅ Header với user info
- ✅ Sidebar với active state
- ✅ Footer
- ✅ Base UI Framework
- ✅ CRUD operations ready (Account, Employee)

## 7. Troubleshooting

### Lỗi kết nối database
→ Check appsettings.json, đảm bảo ConnectionString đúng

### Lỗi login
→ Chạy test_accounts.sql để tạo test accounts

### Lỗi compile
→ Restore packages: `dotnet restore`
