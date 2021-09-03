export default class Resource {

    static Gender = {
        Female: "Nữ",
        Male: "Nam",
        Other: "Khác",
    }

    static ADD_DATA_SUCCESS = "Thêm dữ liệu thành công.";
    static EDIT_DATA_SUCCESS = "Sửa dữ liệu thành công.";
    static DUPLICATE_DATA_SUCCESS = "Nhân bản dữ liệu thành công.";
    static DELETE_DATA_SUCCESS = "Xóa dữ liệu thành công.";
    static EXPORT_DATA_SUCCESS = "Xuất bảng dữ liệu thành công.";

    static ERROR_MESSAGE_VN = "Có vấn đề xảy ra, Vui lòng liên hệ MISA.";

    static CANNOT_EMPTY = "Không được để trống các trường bắt buộc.";
    static ERROR_CODE = "Mã nhân viên sai định dạng.";
    static EMPLOYEE_CODE = "Mã nhân viên";
    static WRONG_DATE = "Ngày tháng không hợp lệ.";
    static WRONG_EMAIL_FORMAT = "Email không đúng định dạng.";


    static NOT_EMPTY = "không được để trống.";
    static WRONG_FORMAT = "sai định dạng.";
    static CONTAIN_ONLY_NUMBER = "chỉ chứa chữ số.";
    static CODE_EXIST = "đã tồn tại trong hệ thống, vui lòng kiểm tra lại.";
    static WRONG_DEPARTMENT = "không tồn tại";

    static CONFIRM_DELETE = "Bạn có thực sự muốn xóa Nhân viên";
    static CONFIRM_SAVE = "Bạn có thực sự muốn lưu Nhân viên";
    static CONFIRM_ADD_DATA_CHANGED = "Dữ liệu đã bị thay đổi. Bạn có muốn cất không?";
    static CONFIRM_EXPORT = "Bạn có thực sự muốn xuất khẩu nhân viên ra file Excel không?"
    static CONFIRM = "không?"
}