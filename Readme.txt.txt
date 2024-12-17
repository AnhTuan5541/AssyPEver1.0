Lệnh đồng bộ db tạo class trong project
Được lưu trong folder Data=

Scaffold-DbContext "Data Source=10.201.21.84,50150;Initial Catalog=eSIGN;Persist Security Info=True;User ID=cimitar2;Password=TFAtest1!2!;Trust Server Certificate=True" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Data -f

Xóa tất cả class trong folder Data, chỉ cần giữ file User, LogInfo, EsignContext