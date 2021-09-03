<template>
  <div @click="myClick()">
    <Header />
    <Menu />
    <div class="content">
      <div class="content-header">
        <div class="title">Nhân viên</div>
        <Button
          @btn-click="btnAddOnClick"
          buttonText="Thêm mới nhân viên"
          id="btnAdd"
        />
      </div>
      <div class="filter">
        <div class="export" @click="ExportOnClick"></div>
        <div class="refresh" @click="RefreshOnClick"></div>
        <FieldInputIcon v-model="searchContent" />
      </div>
      <div id="gridEntity">
        <table>
          <thead>
            <tr>
              <th>
                <div
                  :class="['checkbox', { checked: checked }]"
                  @click="clickCheckboxTh"
                ></div>
              </th>
              <th><span>MÃ NHÂN VIÊN</span></th>
              <th><span>HỌ VÀ TÊN</span></th>
              <th><span>GIỚI TÍNH</span></th>
              <th><span>NGÀY SINH</span></th>
              <th><span>SỐ CMND</span></th>
              <th><span>CHỨC DANH</span></th>
              <th><span>TÊN ĐƠN VỊ</span></th>
              <th><span>SỐ TÀI KHOẢN</span></th>
              <th><span>TÊN NGÂN HÀNG</span></th>
              <th><span>CHI NHÁNH TK NGÂN HÀNG</span></th>
              <th><span>CHỨC NĂNG</span></th>
            </tr>
          </thead>
          <tbody>
            <tr
              v-for="(employee, index) in employees"
              :key="employee.EmployeeId"
              @click="trOnClick(index)"
              @dblclick="onDoubleClick(employee.EmployeeId)"
              :class="{ 'tr-select': isSelected[index] }"
            >
              <td>
                <CheckBox
                  @clickCheckboxTd="clickCheckboxTd"
                  :checked="isSelected[index]"
                />
              </td>
              <td>{{ employee.EmployeeCode }}</td>
              <td :title="employee.FullName">{{ employee.FullName }}</td>
              <td>{{ employee.GenderName }}</td>
              <td>{{ employee.DateOfBirth }}</td>
              <td>{{ employee.IdentityNumber }}</td>
              <td>{{ employee.PositionName }}</td>
              <td>{{ employee.DepartmentName }}</td>
              <td>{{ employee.BankAccount }}</td>
              <td :title="employee.BankName">{{ employee.BankName }}</td>
              <td>{{ employee.BankBranch }}</td>
              <td>
                <SelectFunction
                  :selectedId="employee.EmployeeId + ''"
                  @selectFunctionOnClick="selectFunctionOnClick"
                  @UpdateOnClick="UpdateOnClick(employee.EmployeeId)"
                />
              </td>
            </tr>
          </tbody>
        </table>
      </div>
      <PageNavigation
        :totalEntity="totalEntity"
        :totalPageNumber="totalPageNumber"
        :searchContent="searchContent"
        :entityPerPage="entityPerPage"
        @modifyNumber="modifyNumber"
        @updatePage="updatePage"
      />
    </div>

    <DropdownFunction
      :top="top"
      :count="count"
      @deleteOnClick="deleteOnClick"
      @duplicateOnClick="duplicateOnClick"
      @resetPos="resetPos"
    />

    <!-- 3 page làm mờ -->
    <!-- của popup -->
    <div :class="['pageCover z-index-1', { 'd-none': dialogHasDnone }]"></div>
    <!--  của warningpopup -->
    <div :class="['pageCover z-index-11', { 'd-none': warningHasDnone }]"></div>
    <!-- của loader -->
    <div :class="['pageCover z-index-1', { 'd-none': hideLoader }]"></div>

    <Loader :hideLoader="hideLoader" />

    <EmployeeDetail
      :dnone="dialogHasDnone"
      :employeeId="employeeId"
      :formMode="formMode"
      :response="response"
      @btnDialogCancelOnClick="btnDialogCancelOnClick"
      @closeFormDetail="closeFormDetail"
      @btnSaveOnClick="btnSaveOnClick"
      @btnOnlySaveOnClick="btnOnlySaveOnClick"
      @resetAfterSaveData="resetAfterSaveData"
      @callToastMessage="callToastMessage"
      @callAlertPopup="callAlertPopup"
    />

    <Popup
      :idPopup="idPopup"
      :dnone="warningHasDnone"
      :employeeId="employeeId"
      :warningText="warningText"
      :btnCancelText="btnCancelText"
      :btnConfirmText="btnConfirmText"
      :subClass="bonusClass"
      @btnCancelOnClick="btnCancelOnClick"
      @btnConfirmOnClick="btnConfirmOnClick"
      @btnNoOnClick="closeFormDetail"
    />

    <ToastMessage
      :subClass="subClass"
      :hideToastMessage="hideToastMessage"
      :toastMessageText="toastMessageText"
      :active="active"
      @closeToastMessage="closeToastMessage"
    />

    <AlertPopup
      :subClass="subClass"
      :hideAlertPopup="hideAlertPopup"
      :alertMessageText="alertMessageText"
      :active="active"
      @closeAlertPopup="closeAlertPopup"
    />
  </div>
</template>

<script>
import axios from "axios";
import CommonFn from "../../Common/Common.js";
import Constant from "../../Common/Constant.js";
import ResourceVN from "../../Common/ResourceVN.js";

import Header from "../../components/layout/TheHeader.vue";
import Button from "../../components/base/BaseButton.vue";
import Menu from "../../components/layout/Menu/TheMenu.vue";
import FieldInputIcon from "../../components/base/BaseFieldInputIcon.vue";
import DropdownFunction from "../../components/base/BaseDropDownFunction.vue";
import SelectFunction from "../../components/base/BaseSelectFunction.vue";
import CheckBox from "../../components/base/BaseCheckBox.vue";
import ToastMessage from "../../components/base/BaseToastMessage.vue";
import AlertPopup from "../../components/base/BaseAlertPopup.vue";
import PageNavigation from "../../components/base/BasePageNavigation.vue";
import Loader from "../../components/base/BaseLoader.vue";

import EmployeeDetail from "./EmployeeDetail.vue";
import Popup from "../../components/layout/ThePopup.vue";
export default {
  name: "EmployeeList",
  components: {
    Header,
    Menu,
    Button,
    DropdownFunction,
    SelectFunction,
    CheckBox,
    Loader,
    ToastMessage,
    AlertPopup,
    FieldInputIcon,
    PageNavigation,
    EmployeeDetail,
    Popup,
  },
  data() {
    return {
      //EmployeeList
      employees: [],
      //Đơn vị
      departments: [],
      //chức danh
      positions: [],
      //checkbox
      isSelected: [],
      //checkbox th
      checked: false,
      //ToastMessage
      hideToastMessage: true,
      toastMessageText: "",
      //AlertPopup
      hideAlertPopup: true,
      alertMessageText: "",
      //Loader
      hideLoader: true,

      //EmployeeDetail
      dialogHasDnone: true,
      employeeId: null,
      formMode: -1,
      subClass: "",

      //--------------- form cảnh báo/thông báo ------------------
      warningHasDnone: true,
      warningText: "",
      idPopup: "",
      btnCancelText: "",
      btnConfirmText: "",
      notifyMode: -1,
      response: "",
      bonusClass: "",
      mode: "",

      //------------------------ Menu -------------------------
      // Thay đổi menu
      isToggle: true,
      // Silde
      active: false,
      // Tô đậm danh mục nhân viên
      isFocusEmployee: true,

      // -------------------- Phân trang ----------------------
      // Tổng số nhân viên
      totalEntity: 0,
      // Tổng số trang
      totalPageNumber: 1,
      // Trang hiện tại
      currentPageNumber: 1,
      // Số bản ghi mỗi trang dự kiến
      entityPerPage: 10,

      // ------------ Tìm kiếm ------------
      // Nội dung tìm kiếm
      searchContent: "",
      // id chức danh
      positionId: "",
      //id Đơn vị
      departmentId: "",

      // -------------Chức năng-------------
      // vị trí,
      top: 0,
      selectedId: "",
      count: 0,
    };
  },

  methods: {
    /**
     * Hàm bắt sự kiện bấm trong trang
     * Created By: Ngọc 30/08/2021
     */
    myClick() {
      this.count++;
    },

    /**
     * Hàm bắt sự kiện bấm lựa chọn ở cột chức năng
     * Created By: Ngọc 30/08/2021
     */
    selectFunctionOnClick(top, selectedId) {
      this.top = top;
      this.employeeId = selectedId;
    },

    /**
     * Hàm reset vị trí của dropdown về 0
     * Created By: Ngọc 30/08/2021
     */
    resetPos(pos) {
      this.top = pos;
    },

    /**
     * Hàm xuất khẩu dữ liệu ra excel
     * Created By: Ngọc 02/09/2021
     */
    ExportOnClick() {
      let me = this;
      me.warningText = `${ResourceVN.CONFIRM_EXPORT}`;
      me.warningHasDnone = false;
      me.idPopup = "export-popup";
      me.btnCancelText = "Không";
      me.btnConfirmText = "Có";
    },

    /**
     * Hàm xuất khẩu dữ liệu ra excel
     * Created By: Ngọc 02/09/2021
     */
    ExportData() {
      axios
        .get(`${Constant.LocalUrl}Employees/export`, {
          responseType: "blob",
        })
        .then((res) => {
          const url = window.URL.createObjectURL(new Blob([res.data]));
          const a = document.createElement("a");
          a.href = url;
          const fileName = "file.xlsx";
          a.setAttribute("download", fileName);
          document.body.appendChild(a);
          a.click();
          a.remove();
        })
        .catch((err) => {
          console.log(err);
          this.callAlertPopup(`${ResourceVN.ERROR_MESSAGE_VN}`, "message-red");
        });
    },

    /**
     * Hàm Ẩn/hiện Loader
     * Created By: Ngọc 27/08/2021
     */
    RefreshOnClick() {
      this.hideLoader = false;
      setTimeout(() => {
        this.hideLoader = true;
      }, 1000);
      this.loadDataTable();
    },

    /**
     * Hàm mở popup
     * Created By: Ngọc 28/08/2021
     */
    btnAddOnClick() {
      let me = this;
      me.dialogHasDnone = false;
      this.formMode = 0;
    },

    /**
     * Hàm chọn 1 hàng
     * Created By: Ngọc 30/08/2021
     */
    trOnClick(index) {
      this.$set(this.isSelected, index, !this.isSelected[index]);
    },

    /**
     * Hàm mở form detail để sửa
     * Created By: Ngọc 30/08/2021
     */
    UpdateOnClick(employeeId) {
      this.dialogHasDnone = false;
      this.employeeId = employeeId;
      this.formMode = 1;
    },

    /**
     * Hàm bấm nút xóa
     * Created By: Ngọc 30/08/2021
     */
    deleteOnClick() {
      let me = this;

      let employeeCode = "";
      this.isSelected.forEach((selected, index) => {
        if (selected && me.employees[index].EmployeeId == me.employeeId) {
          employeeCode = me.employees[index].EmployeeCode;
        }
      });
      me.warningText = `${ResourceVN.CONFIRM_DELETE} <span><</span>${employeeCode}<span>></span> ${ResourceVN.CONFIRM}`;

      me.warningHasDnone = false;
      me.idPopup = "warning-popup";
      me.btnCancelText = "Không";
      me.btnConfirmText = "Có";
    },

    /**
     * Hàm bấm nút nhân bản
     * Created By: Ngọc 30/08/2021
     */
    duplicateOnClick() {
      let me = this;
      me.dialogHasDnone = false;
      me.formMode = 2;
    },

    /**
     * Hàm bấm nút xác nhận
     * Created By: Ngọc 30/08/2021
     * Modified By: Ngọc 02/09/2021
     */
    btnConfirmOnClick(idPopup) {
      let me = this;
      // Nếu form cảnh báo xóa được gọi
      me.warningHasDnone = true;
      if (idPopup == "warning-popup") {
        me.DeleteEmployee();
      } else if (idPopup == "export-popup") {
        me.ExportData();
      }
      //Form thông báo được gọi
      else {
        //Nút cất và thêm của from detail được bấm
        if (me.notifyMode == 0 || me.notifyMode == 2) {
          let responseTime = Number(new Date());
          me.response = `Lưu__${responseTime}`;
        }
        //Nút Hủy của from detail được bấm
        else if (me.notifyMode == 1) {
          let responseTime = Number(new Date());
          me.response = `Có__${responseTime}`;
        }
      }
      me.idPopup = "";
    },

    /**
     * Hàm xóa nhân viên
     * Created By: Ngọc 30/08/2021
     */
    DeleteEmployee() {
      let me = this;
      axios
        .delete(`${Constant.LocalUrl}Employees/${me.employeeId}`)
        .then(() => {
          me.callToastMessage(
            `${ResourceVN.DELETE_DATA_SUCCESS}`,
            "message-green"
          );
          // reset các hàng thành không được chọn
          me.isSelected = [];
          // Tải lại bảng
          me.RefreshOnClick();
        })
        .catch((err) => {
          console.log(err);
          me.callAlertPopup(`${ResourceVN.ERROR_MESSAGE_VN}`, "message-red");
        });
      me.checked = false;
    },

    /**
     * Hàm tăng/giảm số lượng thực thể theo trang
     * Created By: Ngọc 28/08/2021
     */
    modifyNumber(entityPerPage) {
      let me = this;
      me.entityPerPage = entityPerPage;
      me.RefreshOnClick();
    },

    /**
     * Hàm được gọi khi thay đổi page hoặc số lượng nhân viên/trang
     * Created By: Ngọc 28/08/2021
     */
    updatePage(currentPageNumber) {
      let me = this;
      me.currentPageNumber = currentPageNumber;
      me.RefreshOnClick();
    },

    /**
     * Hàm lấy dữ liệu cho table
     * Created By: Ngọc 28/08/2021
     */
    loadDataTable() {
      let me = this;
      me.employees = [];
      let url = `${Constant.LocalUrl}Employees/Paging?pageSize=${me.entityPerPage}&pageNumber=${me.currentPageNumber}`;

      if (me.searchContent != "") {
        url += `&searchContent=${me.searchContent}`;
      }

      axios
        .get(url)
        .then((res) => {
          if (res.status == 200) {
            me.employees = res.data.Entities;
            me.totalEntity = res.data.TotalRecord;
            me.totalPageNumber = res.data.TotalPageNumber;
            // format các employee
            me.format(me.employees);
            // reset các tr về không được chọn
            me.resetTr();
          } else if (res.status == 204) {
            me.totalEntity = 0;
            me.totalPageNumber = 1;
            me.currentPageNumber = 1;
          }
        })
        .catch((res) => {
          console.log(res);
          me.callAlertPopup(`${ResourceVN.ERROR_MESSAGE_VN}`, "message-red");
        });
    },

    /**
     * Hàm format sau khi lấy dữ liệu
     * Created By: Ngọc 28/08/2021
     */
    format(employees) {
      let me = this;
      employees.forEach(function (employee) {
        if (employee.DateOfBirth) {
          employee.DateOfBirth = CommonFn.formatDate(employee.DateOfBirth);
        }

        me.getDepartmentName(employee);

        employee.GenderName = CommonFn.getValueEnum(employee.Gender, "Gender");
      });
    },

    /**
     * Hàm render tên Đơn vị
     * Created By: Ngọc 28/08/2021
     */
    getDepartmentName(employee) {
      let me = this;
      me.departments.forEach(function (department) {
        if (employee.DepartmentId == department.DepartmentId) {
          employee.DepartmentName = department.DepartmentName;
        }
      });
    },

    /**
     * Hàm reset các tr về không được chọn
     * Created By: Ngọc 28/08/2021
     */
    resetTr() {
      let me = this;
      me.employees.forEach(() => {
        me.isSelected.push(false);
      });
    },

    /**
     * Hàm đóng popup
     * Created By: Ngọc 28/08/2021
     */
    btnCancelOnClick() {
      let me = this;
      me.warningHasDnone = true;
    },

    /**
     * Hàm bấm hủy ở form detail
     * Created By: Ngọc 28/08/2021
     */
    btnDialogCancelOnClick() {
      let me = this;
      me.warningHasDnone = false;
      me.idPopup = "confirm-popup";
      me.warningText = `${ResourceVN.CONFIRM_ADD_DATA_CHANGED}`;
      me.btnConfirmText = "Có";
      me.btnCancelText = "Hủy";
      me.notifyMode = 1;
    },

    /**
     * Hàm bấm hủy và không ở form detail
     * Created By: Ngọc 31/08/2021
     */
    closeFormDetail() {
      let me = this;
      me.warningHasDnone = true;
      me.dialogHasDnone = true;
      me.formMode = -1;
    },

    /**
     * Hàm thực hiện những reset sau khi lưu
     * Created By: Ngọc 28/08/2021
     * Modified By: Ngọc 30/08/2021
     */
    resetAfterSaveData() {
      let me = this;

      if (me.notifyMode == 2) {
        me.dialogHasDnone = false;
      } else {
        // Đống form thêm/sửa
        me.dialogHasDnone = true;
        // Reset formMode
        me.formMode = -1;
      }
      // Ẩn dropdown chức năng
      me.top = 0;
      // Đóng form thông báo
      me.warningHasDnone = true;
      // Reset mảng đánh dấu
      me.isSelected = [];

      // Tải lại dữ liệu cho table
      me.RefreshOnClick();
    },

    /**
     * Hàm bấm cất và thêm được EmployeeDetail gửi lên
     * Created By: Ngọc 28/08/2021
     */
    btnSaveOnClick(employeeCode) {
      let me = this;

      //Hiển thị popup thông báo lưu
      me.warningHasDnone = false;
      me.idPopup = "notify-popup";
      me.warningText = `${ResourceVN.CONFIRM_SAVE} <span><</span>${employeeCode}<span>></span> ${ResourceVN.CONFIRM}`;
      me.btnCancelText = "Hủy";
      me.btnConfirmText = "Lưu";
      me.notifyMode = 0;
    },

    /**
     * Hàm bấm cất được EmployeeDetail gửi lên
     * Created By: Ngọc 28/08/2021
     */
    btnOnlySaveOnClick(employeeCode) {
      let me = this;

      //Hiển thị popup thông báo lưu
      me.warningHasDnone = false;
      me.idPopup = "notify-popup";
      me.warningText = `${ResourceVN.CONFIRM_SAVE} <span><</span>${employeeCode}<span>></span> ${ResourceVN.CONFIRM}`;
      me.btnCancelText = "Hủy";
      me.btnConfirmText = "Lưu";
      me.notifyMode = 2;
    },

    /**
     * Hàm hiển thị toastmessage
     * Created By: Ngọc 29/08/2021
     */
    callToastMessage(warningText, subClass) {
      this.hideToastMessage = false;
      this.toastMessageText = warningText;
      this.subClass = subClass;
      this.active = true;
    },

    /**
     * Hàm hiển thị popup cảnh báo
     * Created By: Ngọc 29/08/2021
     */
    callAlertPopup(warningText, subClass) {
      this.hideAlertPopup = false;
      this.alertMessageText = warningText;
      this.subClass = subClass;
      this.active = true;
    },

    /**
     * Hàm đóng toastmessage
     * Created By: Ngọc 29/08/2021
     */
    closeToastMessage() {
      this.hideToastMessage = true;
      this.toastMessageText = "";
      this.subClass = "";
    },

    /**
     * Hàm đóng popup cảnh báo
     * Created By: Ngọc 29/08/2021
     */
    closeAlertPopup() {
      this.hideAlertPopup = true;
      this.alertMessageText = "";
      this.subClass = "";
    },

    /**
     * load dữ liệu Đơn vị
     * Created By: Ngọc 28/08/2021
     */
    getDepartment() {
      let me = this;
      axios
        .get(`${Constant.LocalUrl}Departments`)
        .then((res) => {
          me.departments = res.data;
        })
        .catch((res) => {
          console.log(res);
          me.callToastMessage(`${ResourceVN.ERROR_MESSAGE_VN}`, "message-red");
        });
    },

    /**
     * Hàm bấm checkbox ở td được BaseCheckBox gửi lên
     * Ngọc 1/08/2021
     */
    clickCheckboxTd(index) {
      let me = this;
      me.isSelected[index] = !me.isSelected[index];
      setTimeout(function () {
        me.checked = me.CheckAllCBTd();
      }, 10);
    },

    /**
     * Hàm kiểm tra tất cả checkbox của td có được chọn không
     * Ngọc 1/08/2021
     */
    CheckAllCBTd() {
      let me = true;
      for (var i = 0; i < this.isSelected.length; i++) {
        if (!this.isSelected[i]) {
          me = false;
          break;
        }
      }
      return me;
    },

    /**
     * Hàm bấm checkbox ở th được BaseCheckBox gửi lên
     * Ngọc 1/08/2021
     */
    clickCheckboxTh() {
      this.checked = !this.checked;
      if (this.checked) {
        this.isSelected.fill(true);
      } else {
        this.isSelected.fill(false);
      }
    },
  },

  created() {
    this.entityPerPage = 10;
    this.getDepartment();
  },

  watch: {},

  mounted() {},
};
</script>

<style></style>
