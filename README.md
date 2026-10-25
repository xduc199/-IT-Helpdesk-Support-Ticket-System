[![Open in Visual Studio Code](https://classroom.github.com/assets/open-in-vscode-2e0aaae1b6195c2367325f4f02e2d04e9abb55f0b24a779b69b11b9e10269abc.svg)](https://classroom.github.com/online_ide?assignment_repo_id=23468004&assignment_repo_type=AssignmentRepo)
# ĐỒ ÁN MÔN HỌC (CAPSTONE PROJECT) - WORKSPACE

Chào mừng các bạn đến với không gian làm việc nhóm! Kho chứa (Repository) này sẽ là ngôi nhà chung của cả đội. Đồ án này áp dụng mô hình **Test-Driven Development (TDD)** và làm việc nhóm theo chuẩn Agile. Hãy đọc kỹ luật chơi dưới đây.

---

## 1. LUẬT CHƠI & NGUYÊN TẮC LÀM VIỆC NHÓM
Để bảo vệ mã nguồn và điểm số của cả nhóm, tuyệt đối tuân thủ:
1. **Không code thẳng lên nhánh `main`:** Mỗi tính năng mới phải được tạo trên một nhánh riêng (VD: `feature/cart`, `feature/payment`). Khi xong, tạo Pull Request (PR) để gộp code.
2. **Luôn Pull trước khi Push:** Để tránh xung đột (Merge Conflict), hãy tập thói quen kéo code mới nhất từ nhánh `main` về máy trước khi đẩy code của mình lên.
3. **Nghiêm cấm dùng lệnh `git push --force`:** Bất kỳ hành vi nào ghi đè lịch sử hệ thống làm mất code của thành viên khác sẽ bị **trừ 0 điểm** thái độ làm việc nhóm.
4. **Bảo vệ hệ thống chấm điểm:** Tuyệt đối không chỉnh sửa các file trong thư mục `.github/workflows/` và `tests/AutogradingTests/`.

---

## 2. CẤU TRÚC THƯ MỤC
Dự án được chia làm 3 phân khu rõ ràng:
* 📁 `docs/` : Nơi nộp tài liệu phân tích thiết kế, Database schema, Nhật ký họp nhóm và file thuyết minh đồ án tổng thể (Bản mêm để in nộp khi bảo vệ).
* 📁 `src/` : Nơi chứa toàn bộ mã nguồn API và Logic dự án.
* 📁 `tests/` :
  * 🔒 `AutogradingTests/` : Các bài test tiêu chuẩn do Giảng viên quản lý (Không chạm vào).
  * 🧑‍💻 `StudentTests/` : **Khu vực của sinh viên.** Nhóm phải tự viết các Unit Test/Integration Test cho nghiệp vụ của mình tại đây.

---

## 3. CHIẾN LƯỢC CHẤM ĐIỂM (GRADING TIER)
Điểm số của Đồ án được đánh giá qua 3 cột trụ trên GitHub Classroom:

### 🏆 Tiêu chí 1: Test Lõi Hệ Thống (Auto-graded)
Mỗi khi nhóm push code lên `main`, hệ thống sẽ tự động chạy các bài test của Giảng viên để kiểm tra:
* Dự án có Build thành công không?
* API có cấu hình Authorization chặn người dùng lạ không?
* Chức năng Đăng nhập / Đăng ký có đúng chuẩn không?

### 🧠 Tiêu chí 2: Test Nghiệp Vụ Sinh Viên Tự Viết (Student TDD)
* **Yêu cầu:** Mỗi nhóm bắt buộc phải viết ít nhất **05 kịch bản Test** cho các tính năng nghiệp vụ cốt lõi của đề tài (Ví dụ: Thêm giỏ hàng, Mượn trả sách...).
* Các file test này phải đặt trong thư mục `tests/StudentTests/`.
* Hệ thống sẽ tự động chạy thư mục này. Nếu code test của các bạn chạy thành công (Pass), nhóm sẽ nhận được điểm chuyên cần TDD.

### 👁️ Tiêu chí 3: Code Review (Thủ công bởi Giảng viên)
Đến ngày bảo vệ, Giảng viên sẽ kiểm tra:
1. Độ sạch của Code (Clean Code).
2. Tính hợp lý của các bài Test sinh viên đã viết (Chống hành vi viết test gian lận `Assert.True(true)`).
3. Lịch sử Commit (Đóng góp của từng thành viên).

---

## 4. HƯỚNG DẪN CHẠY DỰ ÁN TẠI LOCAL

**Bước 1: Clone dự án về máy**
```bash
git clone <đường-link-repo-của-nhóm>
cd <tên-thư-mục># Final-DotNet-Project
```
**Bước 2: Chạy thử toàn bộ Test**

Kiểm tra xem code hiện tại có vượt qua bài kiểm tra không:

```Bash
dotnet test tests/
```
**Bước 3: Chạy ứng dụng API**

```Bash
dotnet run --project src/API
```
Chúc cả đội phối hợp ăn ý và đạt điểm tối đa!


---
