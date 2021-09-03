<template>
  <div :class="['p-relative', className]">

    <div
      :class="['dropdown', { 'd-none': dnone }]"
      :itemId="itemId"
      :itemName="itemName"
    >
      <slide-up-down :active="active" :duration="1000">
        <div
          v-for="(item, index) in items"
          :key="item[itemId]"
          :Value="item[itemId]"
          :class="['dropdown-item', index == currentIndex ? 'bg-select' : '']"
          @click="chooseDropdownItem(item[itemName], index)"
        >
          {{ item[itemName] }}
        </div>
      </slide-up-down>
    </div>

    <div
      class="select"
      :tabindex="tabindex"
      v-on:keyup.enter="SelectOnClick"
      v-on:keydown="keydownOnSelect($event)"
      v-on:keyup.space="chooseDropdownItemByKey($event)"
      v-on-clickaway="closeDropDown"
    >
      <div class="inp" @click="SelectOnClick">{{ currentName }}</div>
      <div class="select-arrow" @click="SelectOnClick">
        <div
          :class="['arrow', rotate ? 'rot-180' : 'rot-0']"
        ></div>
      </div>
    </div>
  </div>
</template>

<script>
import { mixin as clickaway } from "vue-clickaway";

export default {
  mixins: [clickaway],
  name: "BaseDropdown",

  props: {
    defaultName: String,
    className: String,
    Url: String,
    itemId: String,
    itemName: String,
    tabindex: String,
  },

  data() {
    return {
      //các department,position
      items: [],
      // quay mũi tên
      rotate: false,
      // ẩn/hiện các item
      dnone: true,
      // tên hiển thị
      currentName: this.defaultName,
      currentIndex: -1,
      //slidedown/up
      active: false,
    };
  },
  methods: {
    /**
     * Hàm ngăn chặn sự kiện mặc định của nút tab, dùng thì mới thực hiện được nút enter
     * Created By:Ngọc 31/08/2021
     */
    keydownOnSelect(event) {
      if (event.code == "Enter") {
        event.preventDefault();
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
     * Created By:Ngọc 31/08/2021
     */
    chooseDropdownItemByKey(event) {
      if (event.code == "Space") {
        event.preventDefault();
        let item = this.items[this.currentIndex];
        let itemName = item[this.itemName];
        this.chooseDropdownItem(itemName, this.currentIndex);
      }
    },

    /**
     * Sự kiện chọn 1 lựa chọn
     * Created By:Ngọc 28/08/2021
     */
    chooseDropdownItem(itemName, index) {
      this.currentIndex = index;
      this.HideX = false;
      this.SelectOnClick();
      this.currentName = itemName;
      this.$emit("chooseDropdownItem", itemName);
    },

    /**
     * Sự kiện bấm để hiện/ ẩn dropdown
     * Created By:Ngọc 28/08/2021
     */
    SelectOnClick() {
      this.rotate = !this.rotate;
      this.dnone = !this.dnone;
      this.active = !this.active;
    },

    /**
     * Sự kiện đóng dropdown(sử dụng vue-clickaway)
     * Created By:Ngọc 28/08/2021
     */
    closeDropDown() {
      this.rotate = false;
      this.dnone = true;
      this.active = false;
    },
  },

  created() {
    let me = this;
    switch (me.itemName) {
      case "PagingName":
        this.items = [
          {
            Paging: 0,
            PagingName: "10 bản ghi trên 1 trang",
          },
          {
            Paging: 1,
            PagingName: "20 bản ghi trên 1 trang",
          },
          {
            Paging: 2,
            PagingName: "50 bản ghi trên 1 trang",
          },
          {
            Paging: 3,
            PagingName: "75 bản ghi trên 1 trang",
          },
          {
            Paging: 4,
            PagingName: "100 bản ghi trên 1 trang",
          },
        ];
        me.currentIndex = 0;
        break;

      default:
        break;
    }

    me.currentName = me.defaultName;
  },

  watch: {

  },
};
</script>

<style></style>
