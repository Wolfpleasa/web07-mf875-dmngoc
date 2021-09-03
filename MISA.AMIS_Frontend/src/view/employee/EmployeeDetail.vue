<template>
  <div
    id="formDetail"
    :employeeId="employeeId"
    :formMode="formMode"
    :class="['z-index-2', { 'd-none': dnone }]"
  >
    <div class="head">
      <div class="head-title">
        <div class="head-text">Thông tin nhân viên</div>
        <div class="d-flex">
          <CheckBox />
          <div class="mr-10">Là khách hàng</div>
        </div>
        <div class="d-flex">
          <CheckBox />
          <div>Là nhà cung cấp</div>
        </div>
      </div>
      <div class="formDetail-close">
        <div class="head-help mr-6"></div>
        <div class="head-close" @click="btnDialogCancelOnClick"></div>
      </div>
    </div>
    <div class="main">
      <div class="d-flex w-full">
        <div class="w-50 pd-r-24">
          <div class="row">
            <div class="code">
              <Input
                labelText="Mã"
                tabindex="0"
                type="text"
                FieldName="EmployeeCode"
                obligate="true"
                autoFocus="true"
                ref="txtRequiredEmployeeCode"
                placeholder="NV-1234"
                :reFocus="reFocus"
                v-model="employee.EmployeeCode"
                :initValue="employee.EmployeeCode"
              />
            </div>
            <div class="name">
              <Input
                labelText="Tên"
                tabindex="0"
                type="text"
                FieldName="FullName"
                ref="txtRequiredFullName"
                obligate="true"
                v-model="employee.FullName"
                :initValue="employee.FullName"
              />
            </div>
          </div>
          <div class="row">
            <div class="department w-full">
              <ComboBox
                labelText="Đơn vị"
                tabindex="0"
                defaultName=""
                obligate="true"
                Url="Departments"
                ref="ComboBoxDepartment"
                :selectedId="employee.DepartmentId + ''"
                itemId="DepartmentId"
                itemName="DepartmentName"
                @chooseComboBoxItem="chooseComboBoxItem"
              />
            </div>
          </div>
          <div class="row">
            <div class="positionName w-full">
              <Input
                labelText="Chức danh"
                tabindex="0"
                type="text"
                FieldName="PositionName"
                ref="txtPositionName"
                v-model="employee.PositionName"
                :initValue="employee.PositionName"
              />
            </div>
          </div>
        </div>
        <div class="w-50">
          <div class="row">
            <div class="birth">
              <!-- <Input
                labelText="Ngày sinh"
                tabindex="0"
                type="date"
                FieldName="DateOfBirth"
                ref="txtDateOfBirth"
                checkDate="true"
                v-model="employee.DateOfBirth"
                :initValue="employee.DateOfBirth"
              /> -->
              <label for="">Ngày sinh</label>
              <DatePicker
                labelText="Ngày sinh"
                v-model="employee.DateOfBirth"
                :format="'DD/MM/YYYY'"
                :value-type="'YYYY-MM-DD'"
                placeholder="DD/MM/YYYY"
                :disabled-date="(date) => date >= new Date()"
                style="width: 100%; outline-color: #2ca01c"
              >
                <Input
                  tabindex="0"
                  type="date"
                  FieldName="DateOfBirth"
                  ref="txtDateOfBirth"
                  value=""
                  checkDate="true"
                  v-model="employee.DateOfBirth"
                  :initValue="employee.DateOfBirth"
                />
              </DatePicker>
            </div>

            <div class="gender">
              <Radio
                labelText="Giới tính"
                tabindex="0"
                FieldName="Gender"
                itemId="Gender"
                :dnone="dnone"
                :selectedId="employee.Gender + ''"
                itemName="GenderName"
                ref="RadioGender"
                @chooseRadioItem="chooseRadioItem"
              />
            </div>
          </div>
          <div class="row">
            <div class="identityNumber">
              <Input
                labelText="Số CMND"
                tabindex="0"
                type="text"
                placeholder="9-12 chữ số"
                onlyHasNumber="true"
                FieldName="IdentityNumber"
                ref="txtIdentityNumber"
                v-model="employee.IdentityNumber"
                :initValue="employee.IdentityNumber"
              />
            </div>
            <div class="identityDate">
              <!-- <Input
                labelText="Ngày cấp"
                type="date"
                tabindex="0"
                FieldName="IdentityDate"
                checkDate="true"
                ref="txtIdentityDate"
                v-model="employee.IdentityDate"
                :initValue="employee.IdentityDate"
              /> -->
              <label for="">Ngày cấp</label>
              <DatePicker
                v-model="employee.IdentityDate"
                :format="'DD/MM/YYYY'"
                :value-type="'YYYY-MM-DD'"
                placeholder="DD/MM/YYYY"
                :disabled-date="(date) => date >= new Date()"
                style="width: 100%; outline-color: #2ca01c"
              >
                <Input
                  ref="txtIdentityDate"
                  type="date"
                  value=""
                  tabindex="0"
                  FieldName="IdentityDate"
                  v-model="employee.IdentityDate"
                  :initValue="employee.IdentityDate"
                  checkDate="true"
                />
              </DatePicker>
            </div>
          </div>
          <div class="row">
            <div class="identityPlace w-full">
              <Input
                labelText="Nơi cấp"
                tabindex="0"
                type="text"
                FieldName="IdentityPlace"
                ref="txtIdentityPlace"
                obligate="false"
                v-model="employee.IdentityPlace"
                :initValue="employee.IdentityPlace"
              />
            </div>
          </div>
        </div>
      </div>
      <div class="row mt-16">
        <div class="address w-full">
          <Input
            labelText="Địa chỉ"
            tabindex="0"
            type="text"
            FieldName="Address"
            ref="txtAddress"
            obligate="false"
            v-model="employee.Address"
            :initValue="employee.Address"
          />
        </div>
      </div>
      <div class="row w-75">
        <div class="phoneNumber">
          <Input
            labelText="ĐT di động"
            tabindex="0"
            type="text"
            FieldName="PhoneNumber"
            ref="txtPhoneNumber"
            v-model="employee.PhoneNumber"
            :initValue="employee.PhoneNumber"
          />
        </div>
        <div class="phoneNumber">
          <Input
            labelText="ĐT cố định"
            tabindex="0"
            type="text"
            FieldName="permanentPhoneNumber"
            ref="txtPermanentPhoneNumber"
            v-model="employee.PermanentPhoneNumber"
            :initValue="employee.PermanentPhoneNumber"
          />
        </div>
        <div class="email">
          <Input
            labelText="Email"
            tabindex="0"
            type="text"
            placeholder="youremail@exam.com"
            FieldName="Email"
            ref="txtEmail"
            v-model="employee.Email"
            :initValue="employee.Email"
          />
        </div>
      </div>
      <div class="row w-75">
        <div class="bankAccount">
          <Input
            labelText="Tài khoản ngân hàng"
            tabindex="0"
            type="text"
            FieldName="BankAccount"
            ref="txtBankAccount"
            v-model="employee.BankAccount"
            :initValue="employee.BankAccount"
          />
        </div>
        <div class="bankName">
          <Input
            labelText="Tên ngân hàng"
            tabindex="0"
            type="text"
            FieldName="BankName"
            ref="txtBankName"
            v-model="employee.BankName"
            :initValue="employee.BankName"
          />
        </div>
        <div class="bankBranch">
          <Input
            labelText="Chi nhánh"
            tabindex="0"
            type="text"
            FieldName="BankBranch"
            ref="txtBankBranch"
            v-model="employee.BankBranch"
            :initValue="employee.BankBranch"
          />
        </div>
      </div>
    </div>
    <div class="foot">
      <div>
        <Button
          tabindex="0"
          @btn-click="btnDialogCancelOnClick"
          buttonText="Hủy"
          id="btnCancel"
          subClass="cancel"
        />
      </div>

      <div class="save-area">
        <Button
          tabindex="0"
          @btn-click="btnOnlySaveOnClick"
          buttonText="Cất và Thêm"
          id="btnSave"
          subClass="save d-flex"
        />

        <Button
          tabindex="0"
          @btn-click="btnSaveOnClick"
          buttonText="Cất"
          id="btn"
          subClass="cancel"
        />
      </div>
    </div>
  </div>
</template>

<script>
import DatePicker from "vue2-datepicker";
import "vue2-datepicker/index.css";
import axios from "axios";
import CommonFn from "../../Common/Common.js";
import Constant from "../../Common/Constant.js";
import ResourceVN from "../../Common/ResourceVN.js";

import Button from "../../components/base/BaseButton.vue";
import Input from "../../components/base/BaseInput.vue";
import CheckBox from "../../components/base/BaseCheckBox.vue";
import Radio from "../../components/base/BaseRadio.vue";
import ComboBox from "../../components/base/BaseComboBox1.vue";

export default {
  name: "EmployeeDetail",
  components: {
    Button,
    Input,
    CheckBox,
    Radio,
    ComboBox,
    DatePicker,
  },

  props: {
    dnone: {
      type: Boolean,
      default: true,
      require: true,
    },
    employeeId: {
      type: String,
      default: "",
      require: true,
    },
    formMode: Number,
    response: String,
  },

  data() {
    return {
      employee: {},
      origin: {},
      selectedId: "",
      initValue: "",
      reFocus: false,
    };
  },

  watch: {
    formMode: function () {
      let me = this;
      if (this.formMode == 0) {
        me.resetFormDetail();
      } else if (this.formMode == 1) {
        me.setEditForm();
      } else if (this.formMode == 2) {
        me.setDuplicateForm();
      }
    },

    dnone: function () {
      this.reFocus = !this.reFocus;
    },

    response: async function () {
      let me = this;
      let responseMess = me.response.split("__")[0];
      if (responseMess == "Lưu") {
        //Thêm nhân viên mới
        if (me.formMode == 0) {
          me.addEmployee();
        }
        // sửa nhân viên
        else if (me.formMode == 1) {
          me.editEmployee();
        } else if (me.formMode == 2) {
          me.duplicateEmployee();
        }
      }
      if (responseMess == "Có") {
        let isValid = await me.validateForm();
        if (isValid) {
          //Thêm nhân viên mới
          if (me.formMode == 0) {
            me.addEmployee();
          }
          // sửa nhân viên
          else if (me.formMode == 1) {
            me.editEmployee();
          } else if (me.formMode == 2) {
            me.duplicateEmployee();
          }
        }
      }
    },
  },

  methods: {
    /**
     * Hàm thêm nhân viên mới
     * Created By: Ngọc 28/08/2021
     */
    addEmployee() {
      let me = this;
      axios
        .post(`${Constant.LocalUrl}Employees/`, me.employee)
        .then(() => {
          me.$emit(
            "callToastMessage",
            `${ResourceVN.ADD_DATA_SUCCESS}`,
            "message-green"
          );
          me.$emit("resetAfterSaveData");
          me.resetFormDetail();
        })
        .catch((error) => {
          console.log(error);
          me.$emit(
            "callAlertPopup",
            `${error.response.data.userMsg}`,
            "message-red"
          );
        });
    },

    /**
     * Hàm thêm nhân viên mới
     * Created By: Ngọc 28/08/2021
     */
    editEmployee() {
      let me = this;
      axios
        .put(`${Constant.LocalUrl}Employees/${me.employeeId}`, me.employee)
        .then(() => {
          me.$emit(
            "callToastMessage",
            `${ResourceVN.EDIT_DATA_SUCCESS}`,
            "message-green"
          );
          me.$emit("resetAfterSaveData");
        })
        .catch((error) => {
          console.log(error);
          me.$emit(
            "callAlertPopup",
            `${error.response.data.userMsg}`,
            "message-red"
          );
        });
    },

    /**
     * Hàm thêm nhân viên nhân bản
     * Created By: Ngọc 28/08/2021
     */
    duplicateEmployee() {
      let me = this;

      axios
        .post(`${Constant.LocalUrl}Employees/`, me.employee)
        .then(() => {
          me.$emit(
            "callToastMessage",
            `${ResourceVN.DUPLICATE_DATA_SUCCESS}`,
            "message-green"
          );
          me.$emit("resetAfterSaveData");
        })
        .catch((error) => {
          console.log(error);
          me.$emit(
            "callAlertPopup",
            `${error.response.data.userMsg}`,
            "message-red"
          );
        });
    },

    /**
     * Hàm bind dữ liệu vào form khi sửa
     * Created By: Ngọc 28/08/2021
     */
    setEditForm() {
      let me = this;
      me.removeError();
      axios
        .get(`${Constant.LocalUrl}Employees/${me.employeeId}`)
        .then((res) => {
          me.employee = res.data;
          me.employee.DateOfBirth = CommonFn.convertDate(res.data.DateOfBirth);
          me.employee.IdentityDate = CommonFn.convertDate(
            res.data.IdentityDate
          );
          Object.assign(me.origin, me.employee);
        })
        .catch((err) => {
          console.log(err);
          me.$emit(
            "callAlertPopup",
            `${ResourceVN.ERROR_MESSAGE_VN}`,
            "message-red"
          );
        });
    },

    /**
     * Hàm bind dữ liệu vào form khi nhân bản
     * Created By: Ngọc 28/08/2021
     */
    setDuplicateForm() {
      let me = this;
      me.removeError();
      axios
        .get(`${Constant.LocalUrl}Employees/${me.employeeId}`)
        .then(async (res) => {
          let newCode = await me.getNewCode();
          me.employee = res.data;
          me.employee.DateOfBirth = CommonFn.formatDate(res.data.DateOfBirth);
          me.employee.IdentityDate = CommonFn.convertDate(
            res.data.IdentityDate
          );
          me.employee.EmployeeCode = newCode;
          Object.assign(me.origin, me.employee);
        })
        .catch((err) => {
          console.log(err);
          me.$emit(
            "callAlertPopup",
            `${ResourceVN.ERROR_MESSAGE_VN}`,
            "message-red"
          );
        });
    },

    /**
     * Hàm reset form detail
     * Created By: Ngọc 28/08/2021
     */
    resetFormDetail() {
      let me = this;
      // bỏ các tooltip cảnh báo nếu đang lỗi
      me.removeError();
      (async () => {
        // cho object nhân viên về rỗng
        let newEntity = {};
        newEntity.DepartmentId = "";
        newEntity.Gender = 1;
        // gọi hàm lấy mã nhân viên mới
        newEntity.EmployeeCode = await me.getNewCode();
        me.employee = newEntity;
        me.origin = {};
        Object.assign(me.origin, me.employee);
      })();
    },

    /**
     * Hàm loại bỏ các tooltip cảnh báo
     * Created By: Ngọc 28/08/2021
     */
    removeError() {
      let me = this;
      for (let [key] of Object.entries(me.$refs)) {
        if (key.includes("txt") || key.includes("ComboBox"))
          me.$refs[key].removeError();
      }
    },

    /**
     * Hàm đóng formDetail
     * Created By: Ngọc 28/08/2021
     */
    btnDialogCancelOnClick() {
      let me = this;
      me.isOriginalData()
        ? me.$emit("closeFormDetail")
        : me.$emit("btnDialogCancelOnClick");
    },

    /**
     * Hàm so sánh có sự thay đổi dữ liệu trong form không
     * Created By: Ngọc 31/08/2021
     */
    isOriginalData() {
      let me = this;
      for (let key in me.employee) {
        if (me.origin[key] != me.employee[key]) return false;
      }
      return true;
    },

    /**
     * Sự kiện bấm nút cất và thêm
     * Created By: Ngọc 28/08/2021
     * Modified By: Ngọc 02/09/2021
     */
    async btnSaveOnClick() {
      let me = this;
      let isValid = await me.validateForm();

      if (isValid) {
        me.$emit("btnSaveOnClick", me.employee.EmployeeCode);
      }
    },

    /**
     * Sự kiện bấm nút cất
     * Created By: Ngọc 28/08/2021
     * Modified By: Ngọc 02/09/2021
     */
    async btnOnlySaveOnClick() {
      let me = this;
      let isValid = await me.validateForm();
      if (isValid) {
        me.$emit("btnOnlySaveOnClick", me.employee.EmployeeCode);
      }
    },

    /**
     * Hàm lấy mã nhân viên mới
     * Created By: Ngọc 30/08/2021
     */
    getNewCode() {
      return new Promise((resolve) => {
        axios
          .get(`${Constant.LocalUrl}Employees/NewEmployeeCode`)
          .then((res) => {
            resolve(res.data);
          })
          .catch((err) => {
            console.log(err);
            this.$emit(
              "callAlertPopup",
              `${ResourceVN.ERROR_MESSAGE_VN}`,
              "message-red"
            );
          });
      });
    },

    //--------------------------------------------VALIDATE--------------------------------------------------//
    /**
     * Hàm kiểm tra dữ liệu
     * Created By: Ngọc 31/08/2021
     * Modified By: Ngọc 02/09/2021
     */
    async validateForm() {
      let me = this,
        isValid = true;

      if (isValid) {
        isValid = me.isRequired();
      }

      if (isValid) {
        isValid = me.validateComboBox();
      }

      if (isValid) {
        isValid = me.validateCode();
      }

      if (isValid) {
        isValid = me.validateFieldNumber();
      }

      if (isValid) {
        isValid = me.validateFieldDate();
      }

      if (isValid) {
        isValid = me.validateEmail();
      }

      if (isValid) {
        await me
          .checkCodeExist(me.formMode)
          .then((data) => {
            isValid = data;
          })
          .catch((err) => {
            isValid = err.fail;
          });
      }

      return isValid;
    },

    /**
     * Hàm kiểm tra bắt buộc nhập các dữ liệu bắt buộc
     * Created By: Ngọc 31/08/2021
     */
    isRequired() {
      let me = this;
      let isValid = true;

      for (let [key, value] of Object.entries(me.$refs)) {
        if (key.includes("Required")) {
          let valRes = me.$refs[key].isRequired();

          if (valRes == false) {
            me.$emit(
              "callAlertPopup",
              `${value.label} ${ResourceVN.NOT_EMPTY}`,
              "message-red"
            );
            isValid = false;
          }
        }
      }

      return isValid;
    },

    /**
     * Hàm kiểm tra mã nhân viên có đúng định dạng không
     * Created By: Ngọc 31/08/2021
     */
    validateCode() {
      let me = this;
      let isValid = true;

      for (let [key, value] of Object.entries(me.$refs)) {
        if (key.includes("Code")) {
          let valRes = me.$refs[key].validateCode();

          if (valRes == false) {
            me.$emit("callAlertPopup", `${value.toolTipText}`, "message-red");
            isValid = false;
          }
        }
      }

      return isValid;
    },

    /**
     * Hàm kiểm tra mã nhập vào có bị trùng không
     * Created By: Ngọc 31/08/2021
     * Modified By: Ngọc 02/09/2021
     */
    checkCodeExist(formMode) {
      return new Promise((resolve, reject) => {
        let me = this;

        for (let [key, value] of Object.entries(me.$refs)) {
          if (key.includes("Code")) {
            let url = `${Constant.LocalUrl}Employees/Paging?pageSize=5&pageNumber=1&searchContent=${value.inputValue}`;
            axios
              .get(url)
              .then((res) => {
                console.log(res.data);
                if (res.status == 200 && (formMode == 0 || formMode == 2)) {
                  me.$emit(
                    "callAlertPopup",
                    `${ResourceVN.EMPLOYEE_CODE} <${value.inputValue}> ${ResourceVN.CODE_EXIST}`,
                    "message-yellow"
                  );
                  reject({ fail: false }, 500);
                } else if (
                  res.status == 200 &&
                  formMode == 1 &&
                  me.employeeId != res.data.Entities[0].EmployeeId
                ) {
                  me.$emit(
                    "callAlertPopup",
                    `${ResourceVN.EMPLOYEE_CODE} <${value.inputValue}> ${ResourceVN.CODE_EXIST}`,
                    "message-yellow"
                  );
                  reject({ fail: false }, 500);
                } else {
                  resolve(true);
                }
              })
              .catch((err) => {
                console.log(err);
                // me.$emit(
                //   "callAlertPopup",
                //   `${ResourceVN.ERROR_MESSAGE_VN}`,
                //   "message-red"
                // );
              });
          }
        }
      });
    },

    /**
     * Hàm kiểm tra các ComboBox đã được chọn chưa
     * Created By: Ngọc 31/08/2021
     */
    validateComboBox() {
      let me = this;
      let isValid = true;

      for (let [key, value] of Object.entries(me.$refs)) {
        if (key.includes("ComboBox")) {
          let valRes = me.$refs[key].validateComboBox();

          if (valRes == false) {
            me.$emit("callAlertPopup", `${value.toolTipText}`, "message-red");
            isValid = false;
          }
        }
      }

      return isValid;
    },

    /**
     * Hàm kiểm tra chỉ chứa chữ số
     * Created By: Ngọc 31/08/2021
     */
    validateFieldNumber() {
      let me = this;
      let isValid = true;

      for (let [key, value] of Object.entries(me.$refs)) {
        if (key.includes("Number")) {
          let valRes = me.$refs[key].validateFieldNumber();

          if (valRes == false) {
            me.$emit(
              "callAlertPopup",
              `${value.label} ${ResourceVN.CONTAIN_ONLY_NUMBER}`,
              "message-red"
            );
            isValid = false;
          }
        }
      }

      return isValid;
    },

    /**
     * Hàm kiểm tra xem ngày tháng hợp lệ không
     * Created By: Ngọc 31/08/2021
     */
    validateFieldDate() {
      let me = this,
        isValid = true;

      for (let [key] of Object.entries(me.$refs)) {
        if (key.includes("Date")) {
          let valRes = me.$refs[key].validateFieldDate();

          if (valRes == false) {
            me.$emit(
              "callAlertPopup",
              `${ResourceVN.WRONG_DATE}`,
              "message-red"
            );
            isValid = false;
          }
        }
      }

      return isValid;
    },

    /**
     * Hàm kiểm tra Email
     * Created By: Ngọc 31/08/2021
     */
    validateEmail() {
      let me = this,
        isValid = true,
        valRes = me.$refs.txtEmail.validateEmail();

      if (valRes == false) {
        me.$emit(
          "callAlertPopup",
          `${ResourceVN.WRONG_EMAIL_FORMAT}`,
          "message-red"
        );
        isValid = false;
      }

      return isValid;
    },

    /**
     * Hàm lấy thông tin của đơn vị được chọn
     * Created By: Ngọc 31/08/2021
     */
    chooseComboBoxItem(itemId, itemValue) {
      this.employee[itemId] = itemValue;
    },

    /**
     * Hàm lấy thông tin của giới tính được chọn
     * Created By: Ngọc 31/08/2021
     */
    chooseRadioItem(itemId, itemValue) {
      this.employee[itemId] = itemValue;
      console.log(itemId, itemValue);
    },
  },
};
</script>

<style scoped>
</style>