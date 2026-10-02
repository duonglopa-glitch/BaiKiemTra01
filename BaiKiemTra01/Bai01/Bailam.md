# Câu 1: Value Types vs Reference Types (Stack vs Heap)
##	Value Types (struct, int, double, bool, enum...):
-	Vùng nhớ: Lưu trực tiếp giá trị dữ liệu thực tế trên Stack (trừ trường hợp nó là một biến thành phần bên trong một Reference Type thì sẽ nằm cùng đối tượng đó trên Heap).
-	Cơ chế: Khi gán hoặc truyền tham số, C# sẽ sao chép toàn bộ giá trị sang vùng nhớ mới. Biến mới và biến cũ độc lập hoàn toàn.

##	Reference Types (class, string, object, interface, delegate...):
-	Vùng nhớ: Dữ liệu thực tế được lưu trên Heap. Biến khai báo trên Stack chỉ chứa địa chỉ tham chiếu (con trỏ) chỉ đến vùng nhớ đó trên Heap.
-	Cơ chế: Khi gán hoặc truyền tham số, C# chỉ sao chép địa chỉ tham chiếu. Cả hai biến sẽ cùng trỏ đến một vùng dữ liệu duy nhất trên Heap; thay đổi qua biến này sẽ ảnh hưởng đến biến kia.
# Câu 2: Init-only Properties (init) vs set và thực tế sử dụng
##	Khác biệt:
-	set: Cho phép thay đổi giá trị của thuộc tính bất kỳ lúc nào trong vòng đời của đối tượng.
-	init: Chỉ cho phép gán giá trị một lần duy nhất khi khởi tạo đối tượng (thông qua Constructor hoặc Object Initializer new Person { Name = "A" }). Sau khi khởi tạo xong, thuộc tính trở thành chỉ đọc (Read-only) và không thể sửa đổi.

##	Trường hợp sử dụng thực tế:
-	Khai báo các đối tượng dữ liệu bất biến (Immutable Objects) như DTO (Data Transfer Object), Request/Response Models, hoặc các đối tượng cấu hình (Configuration Settings). Việc này giúp ngăn chặn vô tình sửa đổi dữ liệu sai vị trí trong suốt quá trình xử lý, giúp code an toàn hơn trong môi trường đa luồng (thread-safe).
# Câu 3: Phương thức virtual (lớp cha) vs override (lớp con)
-	virtual (ở lớp cha): Tuyên bố rằng phương thức này cho phép các lớp con định nghĩa lại. Lớp cha cung cấp sẵn một phần cài đặt mặc định, nhưng không bắt buộc lớp con phải ghi đè.

-	override (ở lớp con): Xác nhận lớp con chính thức ghi đè và thay thế logic của phương thức virtual từ lớp cha.

-	Tác dụng với Đa hình: Khi gọi phương thức thông qua một tham chiếu kiểu lớp cha nhưng trỏ đến đối tượng lớp con, chương trình sẽ kiểm tra bảng ảo (vtable) tại thời điểm chạy (Runtime) và ưu tiên thi hành phương thức có từ khóa override ở lớp con thay vì phương thức virtual ở lớp cha.
# Câu 4: Tại sao thành phần static không thể truy xuất qua thể hiện (Instance)?
###	Bản chất lưu trữ: Thành phần static thuộc về cấp độ Lớp (Class), được cấp phát bộ nhớ duy nhất một lần khi Type Loader nạp Class vào bộ nhớ. Nó tồn tại độc lập và chia sẻ chung cho toàn bộ chương trình, không thuộc sở hữu của bất kỳ đối tượng (Instance) nào tạo ra bởi toán tử new.

##	Lý do thiết kế:
-	Tránh nhầm lẫn ngữ nghĩa: Nếu cho phép gọi instance.StaticMember, lập trình viên dễ hiểu nhầm đây là dữ liệu riêng của Instance đó, trong khi thực tế nó là dữ liệu dùng chung.
-	Rõ ràng và tối ưu: Ép buộc gọi qua tên lớp (ClassName.StaticMember) giúp code thể hiện rõ ý đồ thiết kế, giúp Trình biên dịch (Compiler) liên kết trực tiếp địa chỉ vùng nhớ static mà không cần kiểm tra đối tượng có bị null hay không tại thời điểm chạy.

