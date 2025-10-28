# Student Management System - WPF Application

## 📋 Tổng quan
Ứng dụng quản lý học sinh với 3 vai trò: **Admin**, **Teacher**, **Student**

## 🏗️ Kiến trúc
- **StudentManagementBusinessObject**: Entities (Task, ClassTask, StudentTask, Student, Class, Subject...)
- **OnlineEduTaskDAO**: Data Access Objects (Singleton pattern)
- **OnlineEduTaskRepository**: Repository pattern với interfaces
- **OnlineEduTaskServices**: Business logic layer
- **OnlineEduTaskWPF**: WPF UI với LoginWindow & MainWindow

## 🔐 Tài khoản mặc định

| Username | Password | Role | Mô tả |
|----------|----------|------|-------|
| admin | 123456 | Admin | Quản trị viên hệ thống |
| teacher1 | 123456 | Teacher | Giáo viên |
| student1 | 123456 | Student | Học sinh |

## 🚀 Chạy ứng dụng

### Bước 1: Build project
```powershell
cd "d:\FPT\FA25\prn212\code project\StudentManagementSystem\OnlineEduTaskWPF"
dotnet build
```

### Bước 2: Chạy ứng dụng
```powershell
dotnet run
```

### Bước 3: Đăng nhập
- Mở ứng dụng
- Nhập username và password từ bảng trên
- Click "Đăng nhập"

## 📊 Chức năng theo vai trò

### 👨‍💼 Admin
- **Dashboard**: Xem thống kê tổng quan (số học sinh, giáo viên, lớp học)
- **Quản lý học sinh**: Xem danh sách, thêm/sửa/xóa học sinh
- **Quản lý giáo viên**: Xem danh sách, thêm/sửa/xóa giáo viên
- **Quản lý lớp học**: Xem danh sách, thêm/sửa/xóa lớp học
- **Quản lý môn học**: Xem danh sách, thêm/sửa/xóa môn học

### 👨‍🏫 Teacher
- **Lớp học của tôi**: Xem danh sách lớp đang giảng dạy
- **Môn học giảng dạy**: Xem danh sách môn học
- **Tạo bài tập mới**: Tạo Task template, assign cho lớp (ClassTask), tự động tạo StudentTask
- **Kiểm tra nộp bài**: Xem submissions, chấm điểm, feedback

### 👨‍🎓 Student
- **Môn học của tôi**: Xem danh sách môn đang học
- **Bài tập của tôi**: Xem tasks, nộp bài, xem điểm & feedback
  - Filter: All, Pending, Submitted, Graded

## 🗄️ Database Schema (redesigned)

### Task (Template)
- TaskId, Title, Description
- CreatedByTeacherId, CreatedDate

### ClassTask (Assignment to Class)
- ClassTaskId, TaskId, ClassId, SubjectId
- DueDate, MaxScore

### StudentTask (Individual Submission)
- StudentTaskId, ClassTaskId, StudentId
- IsSubmitted, SubmissionContent, SubmissionDate
- Score, Feedback, GradedDate

## 🛠️ Tech Stack
- **.NET 8.0**: Framework
- **WPF**: UI (net8.0-windows)
- **Entity Framework Core 9.0.10**: ORM
- **SQL Server**: Database

## 📁 Cấu trúc Pages

### Admin Pages
- AdminDashboardPage.xaml
- ManageStudentsPage.xaml
- ManageTeachersPage.xaml
- ManageClassesPage.xaml
- ManageSubjectsPage.xaml

### Teacher Pages
- TeacherClassesPage.xaml
- TeacherSubjectsPage.xaml
- CreateTaskPage.xaml
- CheckSubmissionsPage.xaml

### Student Pages
- StudentSubjectsPage.xaml
- StudentTasksPage.xaml

## 🎨 UI Features
- Modern blue theme
- Role-based sidebar menu
- Frame navigation
- Card-style layouts
- Responsive DataGrids

## ⚡ Dữ liệu mẫu
App tự động seed data khi khởi động lần đầu:
- 1 Admin account
- 1 Teacher account
- 1 Student account
- 2 Classes (SE1801, SE1802)
- 2 Students
- 3 Subjects (PRN212, PRJ301, SWP391)
- ClassSubject assignments

## 🔄 Flow đăng nhập
1. LoginWindow.xaml hiển thị
2. UserAccountService xác thực
3. Nếu thành công → MainWindow(user)
4. MainWindow load menu theo role
5. Navigate vào Page mặc định theo role

## 🧪 Đã test
✅ Build successful
✅ All Pages compile
✅ LoginWindow → MainWindow navigation
✅ Database seed on startup
✅ Role-based menu system

## 📌 Next Steps (TODO)
- [ ] Implement full CRUD operations in Pages
- [ ] Add CreateTask functionality for Teacher
- [ ] Implement Submit/Grade workflow
- [ ] Add data validation
- [ ] Implement search & filter
- [ ] Add pagination for large datasets
- [ ] Unit tests
