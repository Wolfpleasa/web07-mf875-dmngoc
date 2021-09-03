
<template>
  <div :class="['p-relative', className]" v-on-clickaway="closeComboBox">
    <div :class="{ 'd-none': !labelText }">
      <label for=""
        >{{ labelText }}
        <span class="cl-red" v-if="obligate == 'true'">*</span></label
      ><br />
    </div>
    <ToolTip :hideToolTip="hideToolTip" :toolTipText="toolTipText" />

    <div
      :class="['combo-box-select mt-4', { notValid: hasNotValid }]"
      v-on:keydown="keydownOnSelect($event)"
      v-on:keyup.enter="chooseComboBoxItemByKey()"
      ref="myComboBox"
    >
      <input
        :tabindex="tabindex"
        class="inp"
        v-model="currentName"
        ref="myInput"
        @input="onInput()"
        @blur="onBlur()"
        @focus="onFocus()"
      />
      <div class="select-arrow" @click="selectOnClick">
        <div :class="['arrow', rotate ? 'rot-180' : 'rot-0']"></div>
      </div>
    </div>

    <div :class="['comboBox', { 'd-none': dnone }]">
      <slide-up-down :active="active" :duration="1000">
        <div
          v-for="(item, index) in items"
          :key="item[itemId]"
          :class="[
            'comboBox-item',
            currentId == item[itemId] ? 'bg-select' : '',
            index == currentIndex ? 'bg-hover' : '',
          ]"
          @click="chooseComboBoxItem(item[itemId], item[itemName], index)"
        >
          {{ item[itemName] }}
        </div>
      </slide-up-down>
    </div>
  </div>
</template>

<script>
import axios from "axios";
import { mixin as clickaway } from "vue-clickaway";

import ToolTip from "./BaseToolTip.vue";
import Constant from "../../Common/Constant";
import ResourceVN from "../../Common/ResourceVN";
export default {
  mixins: [clickaway],
  name: "BaseComboBox",

  components: {
    ToolTip,
  },

  props: {
    className: String,
    Url: String,
    itemId: String,
    itemName: String,
    selectedId: String,
    tabindex: String,
    labelText: String,
    obligate: String,
  },

  data() {
    return {
      label: this.labelText,
      //các department,position
      items: [],
      // quay mũi tên
      rotate: false,
      // ẩn/hiện các item
      dnone: true,
      // tên hiển thị
      currentName: this.defaultName,
      currentIndex: -1,
      currentId: "",
      //slidedown/up
      active: false,
      // border đỏ
      hasNotValid: false,
      //hiện/ẩn tooltip
      hideToolTip: true,
      //nội dung tooltip
      toolTipText: "",
    };
  },
  methods: {
    /**
     * Hàm bấm tab để tự hoàn thiện
     * Created By: Ngọc 01/09/2021
     */
    filter() {
      let me = this;
      me.items.forEach(function (item, index) {
        if (
          item[me.itemName]
            .toLowerCase()
            .includes(me.currentName.toLowerCase()) &&
          me.currentName != ""
        ) {
          me.currentId = item[me.itemId];
          me.currentIndex = index;
          me.currentName = item[me.itemName];
        }
      });
      me.$emit("chooseComboBoxItem", me.itemId, me.currentId);
    },

    /**
     * Nếu comboBox chưa có giá trị thì hiện thông báo
     * Created By: Ngọc 31/08/2021
     * Modified By: Ngọc 03/09/2021
     */
    onBlur() {
      let me = this;
      me.filter();
      if (me.currentName == "") {
        // hiện viền đỏ
        me.hasNotValid = true;
        // hiện tooltip
        me.hideToolTip = false;
        me.toolTipText = `${me.labelText} ${ResourceVN.NOT_EMPTY}`;
      } else if (me.currentId == "" && me.currentName != "") {
        // hiện viền đỏ
        me.hasNotValid = true;
        // hiện tooltip
        me.hideToolTip = false;
        me.toolTipText = `${me.labelText} ${ResourceVN.WRONG_DEPARTMENT}`;
      } else {
        me.hasNotValid = false;
        // ẩn tooltip
        me.hideToolTip = true;
        me.$refs.myComboBox.style.borderColor = "#babec5";
      }
    },

    /**
     * Hàm kiểm tra comboBox đã có giá trị chưa
     * Created By: Ngọc 31/08/2021
     */
    onInput() {
      if (this.currentName == "") {
        this.currentId = "";
      }
    },

    /**
     * Hàm focus vào ô nhập
     * Created By: Ngọc 31/08/2021
     */
    onFocus() {
      let me = this;
      me.hasNotValid = false;
      me.hideToolTip = true;
      this.rotate = true;
      this.dnone = false;
      this.active = true;
      me.$refs.myComboBox.style.borderColor = "#2CA01C";
    },

    /**
     * Hàm ngăn chặn sự kiện mặc định của nút tab, dùng để thực hiện được nút enter
     * Created By: Ngọc 31/08/2021
     */
    keydownOnSelect(event) {
      if (event.code == "Enter") {
        event.preventDefault();
      }
      if (event.code == "Tab") {
        this.rotate = false;
        this.dnone = true;
        this.active = false;
      }
      if (event.code == "ArrowDown") {
        event.preventDefault();
        this.currentIndex = this.currentIndex < 0 ? -1 : this.currentIndex;
        this.currentIndex = (this.currentIndex + 1) % this.items.length;
      } else if (event.code == "ArrowUp") {
        event.preventDefault();
        this.currentIndex = this.currentIndex < 0 ? 0 : this.currentIndex;
        this.currentIndex =
          this.currentIndex == 0
            ? this.items.length - 1
            : this.currentIndex - 1;
      }
    },

    /**
     * Sự kiện chọn 1 lựa chọn bằng phím
     * Created By: Ngọc 31/08/2021
     */
    chooseComboBoxItemByKey() {
      let item = this.items[this.currentIndex],
        itemName = item[this.itemName],
        itemValue = item[this.itemId];
      this.chooseComboBoxItem(itemValue, itemName, this.currentIndex);
    },

    /**
     * Sự kiện chọn 1 lựa chọn
     * Created By: Ngọc 31/08/2021
     * Modified By: Ngọc 01/09/2021
     */
    chooseComboBoxItem(itemValue, itemName, index) {
      let me = this;
      me.hasNotValid = false;
      me.hideToolTip = true;
      me.$refs.myInput.focus();
      me.currentIndex = index;
      me.currentId = itemValue;
      me.selectOnClick();
      me.currentName = itemName;
      me.$emit("chooseComboBoxItem", this.itemId, itemValue);
    },

    /**
     * Sự kiện bấm để hiện/ ẩn ComboBox
     * Created By: Ngọc 29/08/2021
     */
    selectOnClick() {
      this.rotate = !this.rotate;
      this.dnone = !this.dnone;
      this.active = !this.active;
    },

    /**
     * Sự kiện đóng ComboBox(sử dụng vue-clickaway)
     * Created By: Ngọc 29/08/2021
     */
    closeComboBox() {
      this.rotate = false;
      this.dnone = true;
      this.active = false;
    },

    /**
     * Xem hàm sửa có gọi không để lưu giá trị vào ComboBox
     * Created By: Ngọc 29/08/2021
     * Modified By: Ngọc 01/09/2021
     */
    setValueComboBox() {
      let me = this;
      // nếu selectedId(DepartmentId) mà tồn tại(tức là được Employeelist truyền vào => form sửa dùng)
      if ((me.selectedId + "").length > 0) {
        // duyệt từng items(department)
        me.items.forEach(function (item, index) {
          // nếu selectedId(DepartmentId) từ cha truyền vào mà trùng với itemId(DepartmentId) trong list
          if (me.selectedId == item[me.itemId]) {
            // gán currentId = selectedId (để hàng tương ứng được tích)
            me.currentId = me.selectedId;
            me.currentIndex = index;
            // hiện thị tên của itemName(DepartmentName) của hàng được chọn
            me.currentName = item[me.itemName];
          }
        });
      } else {
        // form thêm dùng
        // không hàng nào được tích
        me.currentId = "";
        me.currentIndex = -1;
        // không hiển thị tên nào cả
        me.currentName = "";
      }
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

    /**
     * Hàm kiểm tra xem ComboBox có giá trị hay không
     * Created By: Ngọc 29/08/2021
     * Modified By: Ngọc 03/09/2021
     */
    validateComboBox() {
      let me = this;     
      if (me.currentName == "") {
        // hiện viền đỏ
        me.hasNotValid = true;     
        me.toolTipText = `${me.labelText} ${ResourceVN.NOT_EMPTY}`;
        return false;
      } else if (me.currentId == "" && me.currentName != "") {
        // hiện viền đỏ
        me.hasNotValid = true;
        me.toolTipText = `${me.labelText} ${ResourceVN.WRONG_DEPARTMENT}`;
        return false;
      } else {
        // bỏ viền đỏ
        me.hasNotValid = false;
        me.toolTipText = "";
        return true;
      }
    },
  },

  created() {
    let me = this;
    switch (me.itemName) {
      case "PagingName":
        break;

      default:
        axios
          .get(`${Constant.LocalUrl}${me.Url}`)
          .then((res) => {
            me.items = res.data;
          })
          .catch((res) => {
            console.log(res);
          });
        break;
    }

    me.currentName = me.defaultName;
  },

  watch: {
    selectedId: function () {
      this.setValueComboBox();
    },
  },
};
</script>

<style></style>

            