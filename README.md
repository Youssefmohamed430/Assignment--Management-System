# Assignment Management System 🎓

The **Assignment Management System** is a web-based backend project that allows efficient management of courses, users (students & instructors), assignments, submissions, notifications, and grading.

## 🚀 Features
- **User Management**
  - Handle students and instructors.
- **Course Management**
  - Create, Read, Update, and Delete courses.
- **Assignment Management**
  - Create, Update, Delete, and Retrieve assignments.
  - Retrieve all assignments for a course.
  - Get a single assignment by Id.
- **Course Enrollment**
  - Students can enroll in courses.
- **Notifications**
  - Students are notified when a new assignment is added to their enrolled courses.
- **Assignment Submissions**
  - Students can submit their assignments with files/URLs.
- **Grading**
  - Instructors can update grades for students.
  - Instructors can add assignments to their own courses.

## 🛠️ Technical Stack
- **Backend Framework**: ASP.NET Core (.NET 7/8)  
- **Language**: C#  
- **Database**: SQL Server with Entity Framework Core  
- **Authentication & Authorization**: JWT (JSON Web Token) with role-based access control  
- **API Design**: RESTful APIs with clean route design and well-structured URLs  
- **ORM & Querying**: Entity Framework Core with LINQ  
- **Design Patterns**: Factory Pattern for modular and extensible design  
- **Programming Paradigm**: Object-Oriented Programming (OOP)  
- **Code Quality**: Clean Code principles for maintainability and readability  
- **Tools**: Postman for API documentation & testing  
- **Notification Service**: Custom notification logic (server-side triggered)  

## 📂 Project Structure
- `Controllers/` → API endpoints  
- `Services/` → Business logic  
- `DataLayer/` → Database models & DTOs  
- `Helpers/` → Utility functions & authentication  

---

## ⚡ Getting Started
1. Clone the repository.  
2. Update `appsettings.json` with your SQL Server connection string.  
3. Run database migrations using Entity Framework.  
4. Launch the API with `dotnet run`.  
5. Import the Postman Collection to test the APIs.  

---

## 📌 Notes
- Students receive real-time notifications when new assignments are added.  
- System enforces roles:  
  - **Student** → Enroll in courses, submit assignments.  
  - **Instructor** → Manage courses, add assignments, update grades.  
