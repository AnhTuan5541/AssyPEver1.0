Các thư viện cần cài: FrameworkCore, FrameworkCore.SqlServer, Tools, JwtBearer, DirectoryServices(chỉ dùng để đăng nhập và lấy tt ng dùng dùng cho amkor)

Lệnh đồng bộ db tạo class trong project
Được lưu trong folder Data

Scaffold-DbContext "Data Source=10.201.21.84,50150;Initial Catalog=WaferMapViewer;Persist Security Info=True;User ID=cimitar2;Password=TFAtest1!2!;Trust Server Certificate=True" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Data -f

Xóa tất cả class trong folder Data, chỉ cần giữ file User, LogInfo, EsignContext

API Portal:
http://10.201.12.31:8007/api/common/
I. getUSP
    1. Method: GET 
    2. Parameter:
        2.1 catalogStr: Catalog Name, ex: WaferMapViewer
        2.2 uspName: User Store Procedure Name, ex: [dbo].[USP_ASSY_Call_SP]
        2.3 flagStr: Flag redirect to other Store Procedure, ex: LcrElement
        2.4 parameterStr: Parameter String seperate by ';', ex: getProcess 
II. updateUSP
    1. Method: POST
    2. Parameter: Same as GET method but use Body request JSON.
    ex:
    {
        "catalogStr":"PCS",
        "uspName": "[dbo].[USP_AutoSchedule_CallSP]"
        "flagStr":"testMethod",
        "parameterStr":"test parameter"
    }
