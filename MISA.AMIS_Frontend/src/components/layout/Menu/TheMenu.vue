<template>
  <div class="menu">
    <MenuItem
      @itemOnClick="itemOnClick"
      subClass="dashboard"
      menuItemText="Tổng quan"
      menuItemId="1"
      :currentId="currentId"
    />

    <MenuItem
      @itemOnClick="itemOnClick"
      subClass="cash"
      menuItemText="Tiền mặt"
      menuItemId="2"
      :currentId="currentId"
    />

    <MenuItem
      @itemOnClick="itemOnClick"
      subClass="bank"
      menuItemText="Tiền gửi"
      menuItemId="3"
      :currentId="currentId"
    />

    <MenuItem
      @itemOnClick="itemOnClick"
      subClass="buy"
      menuItemText="Mua hàng"
      menuItemId="4"
      :currentId="currentId"
    />

    <MenuItem
      @itemOnClick="itemOnClick"
      subClass="sale"
      menuItemText="Bán hàng"
      menuItemId="5"
      :currentId="currentId"
    />

    <MenuItem
      @itemOnClick="itemOnClick"
      subClass="invoice"
      menuItemText="Quản lí hóa đơn"
      menuItemId="6"
      :currentId="currentId"
    />

    <MenuItem
      @itemOnClick="itemOnClick"
      subClass="stock"
      menuItemText="Kho"
      menuItemId="7"
      :currentId="currentId"
    />

    <MenuItem
      @itemOnClick="itemOnClick"
      subClass="tools"
      menuItemText="Công cụ dụng cụ"
      menuItemId="8"
      :currentId="currentId"
    />

    <MenuItem
      @itemOnClick="itemOnClick"
      subClass="fixed-assets"
      menuItemText="Tài sản cố định"
      menuItemId="9"
      :currentId="currentId"
    />

    <MenuItem
      @itemOnClick="itemOnClick"
      subClass="tax"
      menuItemText="Thuế"
      menuItemId="10"
      :currentId="currentId"
    />

    <MenuItem
      @itemOnClick="itemOnClick"
      subClass="price"
      menuItemText="Giá thành"
      menuItemId="11"
      :currentId="currentId"
    />

    <MenuItem
      @itemOnClick="itemOnClick"
      subClass="general"
      menuItemText="Tổng hợp"
      menuItemId="12"
      :currentId="currentId"
    />

    <MenuItem
      @itemOnClick="itemOnClick"
      subClass="budget"
      menuItemText="Ngân sách"
      menuItemId="13"
      :currentId="currentId"
    />

    <MenuItem
      @itemOnClick="itemOnClick"
      subClass="report"
      menuItemText="Báo cáo"
      menuItemId="14"
      :currentId="currentId"
    />

    <MenuItem
      @itemOnClick="itemOnClick"
      subClass="finance"
      menuItemText="Phân tích tài chính"
      menuItemId="15"
      :currentId="currentId"
    />
  </div>
</template>

<script>
import MenuItem from "./TheMenuItem.vue";
export default {
  name: "Menu",

  components: {
    MenuItem,
  },

  props: {},

  data() {
    return {
      isSelected: false,
      currentId: "0",
    };
  },

  methods: {
    /**
     * Hàm bắt sự kiện bấm vào các mục trong menu
     * Created By: Ngọc 27/8/2021
     */
    itemOnClick(menuItemId) {
      this.currentId = menuItemId;
    },

    /**
     * Hàm bắt sự kiện bấm vào các mục khác thì bỏ border xanh
     * Created By: Ngọc 27/8/2021
     */
    // removeSelected(){
    //   this.isSelected = false;
    // }
  },
};
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
</style>
