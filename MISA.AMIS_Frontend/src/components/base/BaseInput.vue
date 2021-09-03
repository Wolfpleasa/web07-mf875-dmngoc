<template>
  <div class="p-relative">
    <div v-if="labelText.length > 0">
      <label for=""
        >{{ labelText }}
        <span class="cl-red" v-if="obligate == 'true'">*</span></label
      ><br />
    </div>
    <ToolTip :hideToolTip="hideToolTip" :toolTipText="toolTipText" />
    <input
      :tabindex="tabindex"
      :type="type"
      :placeholder="placeholder"
      :FieldName="FieldName"
      :class="['textbox-default', subClass, { notValid: hasNotValid }]"
      ref="inputREF"
      :obligate="obligate"
      :onlyHasNumber="onlyHasNumber"
      :checkDate="checkDate"
      v-model="inputValue"
      @input="onInput($event.target.value)"
      @blur="onBlur($event.target.value)"
    />
  </div>
</template>

<script>
import { mixin as clickaway } from "vue-clickaway";
import CommonFn from "../../Common/Common.js";
import ResourceVN from "../../Common/ResourceVN.js";

import ToolTip from "./BaseToolTip.vue";

export default {
  mixins: [clickaway],
  name: "BaseInput",
  components: {
    ToolTip,
  },

  props: {
    labelText: String,
    tabindex: String,
    type: String,
    placeholder: String,
    FieldName: String,
    initValue: String,
    subClass: String,
    // xác định tự động cho con trỏ chuột vào ô mã nhân viên
    autoFocus: String,
    reFocus: Boolean,
    // Xác định các trường chỉ chứa số
    onlyHasNumber: String,
    // xác định các trường bắt buộc
    obligate: String,
    // Xác định các trường ngày tháng
    checkDate: String,
  },
  data() {
    return {
      // Giá trị/Nội dung hiện tại
      inputValue: "",
      // Ẩn/hiện viền đỏ
      hasNotValid: false,
      // Ẩn/hiện tooltip
      hideToolTip: true,
      // Nội dung tooltip
      toolTipText: "",
      //label
      label: this.labelText,
    };
  },

  methods: {
    /**
    * Hàm focus vào ô Mã
    * Created By: Ngọc 02/09/2021
    */
    focusOnCode(){
      if (this.autoFocus == "true") {
        this.$refs.inputREF.focus();
      }
    },

    /**
     * Hàm kiểm tra dữ liệu input
     * Created By: Ngọc 31/08/2021
     */
    onInput(inputValue) {
      let me = this;
      //emit thắng vào v-model của cha
      me.$emit("input", inputValue);

      // Nếu các ô bắt buộc nhập đã có dữ liệu thì bỏ viền đỏ
      if (inputValue != "" && me.obligate == "true") {
        me.hasNotValid = false;
        me.hideToolTip = true;
      }

      // Nếu ô ngày tháng đã đúng thì bỏ viền đỏ
      if (
        inputValue != "" &&
        CommonFn.isDateFormat(inputValue) &&
        me.checkDate == "true"
      ) {
        me.hasNotValid = false;
        me.hideToolTip = true;
      }

      // Nếu các ô chỉ chứa chữ số đúng định dạng rồi thì bỏ viền đỏ
      if (
        inputValue != "" &&
        CommonFn.isNumber(inputValue) &&
        me.onlyHasNumber == "true"
      ) {
        me.hasNotValid = false;
        me.hideToolTip = true;
      }

      //Nếu Email nhập đúng định dạng thì bỏ viền đỏ
      if (me.FieldName == "Email" && inputValue != "") {
        var at = inputValue.indexOf("@");
        var dot = inputValue.lastIndexOf(".");
        var space = inputValue.indexOf(" ");
        if (
          at != -1 && //có ký tự @
          at != 0 && //ký tự @ không nằm ở vị trí đầu
          dot != -1 && //có ký tự .
          dot > at + 1 &&
          dot < inputValue.length - 1 && //phải có ký tự nằm giữa @ và . cuối cùng
          space == -1 //không có khoẳng trắng
        ) {
          me.hasNotValid = false;
          me.hideToolTip = true;
        }
      }
    },

    /**
     * Hàm kiểm tra khi vừa nhập liệu xong
     * Created By: Ngọc 31/08/2021
     */
    onBlur(inputValue) {
      let me = this;
      // Nếu các ô bắt buộc không có dữ liệu thì hiện viền đỏ và tooltip
      if (inputValue == "" && me.obligate == "true") {
        me.hasNotValid = true;
        me.hideToolTip = false;
        me.toolTipText = `${me.labelText} ${ResourceVN.NOT_EMPTY}`;
      }

      // Nếu các mã nhân viên sai thì hiện viền đỏ và tooltip
      if (
        me.FieldName == "EmployeeCode" &&
        !CommonFn.isCodeFormat(inputValue)
      ) {
        me.hasNotValid = true;
        me.hideToolTip = false;
        me.toolTipText = `${me.labelText} ${ResourceVN.WRONG_FORMAT}`;
      }

      // Nếu ô ngày tháng không đúng thì hiện viền đỏ và tooltip
      if (
        inputValue != "" &&
        !CommonFn.isDateFormat(inputValue) &&
        me.checkDate == "true"
      ) {
        me.hasNotValid = true;
        me.hideToolTip = false;
        me.toolTipText = `${ResourceVN.WRONG_DATE}`;
      }

      // Nếu các ô chỉ chứa chữ số không đúng định dạng rồi thì hiện viền đỏ
      if (
        inputValue != "" &&
        !CommonFn.isNumber(inputValue) &&
        me.onlyHasNumber == "true"
      ) {
        me.hasNotValid = true;
        me.hideToolTip = false;
        me.toolTipText = `${me.labelText} ${ResourceVN.CONTAIN_ONLY_NUMBER}`;
      }

      //Nếu Email nhập không đúng định dạng thì hiện viền đỏ
      if (me.FieldName == "Email" && inputValue != "") {
        var at = inputValue.indexOf("@");
        var dot = inputValue.lastIndexOf(".");
        var space = inputValue.indexOf(" ");
        if (
          at == -1 || //không có ký tự @
          at == 0 || //ký tự @ nằm ở vị trí đầu
          dot == -1 || //không có ký tự .
          (dot == at + 1 && dot < inputValue.length - 1) || // @ và . cuối cùng cạnh nhau
          space != -1 // có khoẳng trắng
        ) {
          me.hasNotValid = true;
          me.hideToolTip = false;
          me.toolTipText = `${ResourceVN.WRONG_EMAIL_FORMAT}`;
        }
      }
    },

    /**
     * Hàm kiểm tra bắt buộc nhập các dữ liệu bắt buộc
     * Created By: Ngọc 31/08/2021
     */
    isRequired() {
      let me = this,
        val = me.$refs.inputREF.value;

      if (me.obligate == "true" && val == "") {
        // Thêm border đỏ
        me.hasNotValid = true;
    
        me.toolTipText = `${me.labelText} ${ResourceVN.NOT_EMPTY}`;
        return false;
      }
      //bỏ boder đỏ
      me.hasNotValid = false;

      return true;
    },

    /**
     * Hàm kiểm tra mã nhân viên có đúng định dạng không
     * Created By: Ngọc 31/08/2021
     */
    validateCode() {
      let me = this,
        val = me.$refs.inputREF.value;

      if (me.FieldName == "EmployeeCode" && !CommonFn.isCodeFormat(val)) {
        // Thêm border đỏ
        me.hasNotValid = true;
        me.toolTipText = `${me.labelText} ${ResourceVN.WRONG_FORMAT}`;
        return false;
      }
      //bỏ boder đỏ
      me.hasNotValid = false;
      me.toolTipText = ""
      return true;
    },

    /**
     * Hàm kiểm tra các trường chỉ chứa chữ số
     * Created By: Ngọc 31/08/2021
     */
    validateFieldNumber() {
      let me = this,
        number = me.$refs.inputREF.value;
      if (!number) return true;
      let val = CommonFn.isNumber(number);

      if (!val && me.onlyHasNumber == "true") {
        //thêm border đỏ
        me.hasNotValid = true;
        // hiện tooltip
        me.hideToolTip = false;
        me.toolTipText = `${me.labelText} ${ResourceVN.CONTAIN_ONLY_NUMBER}`;
        return false;
      }
      //bỏ boder đỏ
      me.hasNotValid = false;
      // Ẩn tooltip
      me.hideToolTip = true;

      return true;
    },

    /**
     * Hàm kiểm tra định dạng ngày thánng
     * Created By: Ngọc 31/08/2021
     */
    validateFieldDate() {
      let me = this,
        date = me.$refs.inputREF.value;
      if (!date) return true;

      if (!CommonFn.isDateFormat(date) &&  me.checkDate == "true") {
        //thêm border đỏ
        me.hasNotValid = true;   
        me.toolTipText = `${ResourceVN.WRONG_DATE}`;
        return false;
      }
      //bỏ boder đỏ
      me.hasNotValid = false;   
      return true;
    },

    /**
     * Hàm kiểm tra Email
     * Created By: Ngọc 31/08/2021
     */
    validateEmail() {
      let me = this;

      if (me.FieldName == "Email") {
        let value = this.$refs.inputREF.value;
        if (!value) return true;
        var at = value.indexOf("@");
        var dot = value.lastIndexOf(".");
        var space = value.indexOf(" ");
        if (
          at != -1 && //có ký tự @
          at != 0 && //ký tự @ không nằm ở vị trí đầu
          dot != -1 && //có ký tự .
          dot > at + 1 &&
          dot < value.length - 1 && //phải có ký tự nằm giữa @ và . cuối cùng
          space == -1 //không có khoẳng trắng
        ) {
          me.hasNotValid = false;
  
        } else {
          me.hasNotValid = true;
  
          me.toolTipText = `${ResourceVN.WRONG_EMAIL_FORMAT}`;
          return false;
        }
      }
      return true;
    },

    /**
     * Hàm loại bỏ lỗi và tooltip cảnh báo khi mở lại form
     * Created By: Ngọc 31/08/2021
     */
    removeError() {
      let me = this;
      // Bỏ boder đỏ
      me.hasNotValid = false;
      // Ẩn tooltip
      me.hideToolTip = true;
    },
  },
  watch: {
    initValue: function () {
      this.inputValue = this.initValue;
    },

    reFocus: function () {
      if (this.autoFocus) {
        this.$refs.inputREF.focus();
      }
    },
  },
};
</script>

<style scoped>
</style>
