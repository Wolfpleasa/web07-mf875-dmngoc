<template>
  <div
    @click="itemOnClick"
    :class="['menu-item', { 'selected': menuItemId == currentId }]"
  >
    <div :class="['menu-item-icon', subClass]"></div>
    <div class="menu-item-text">{{ menuItemText }}</div>
  </div>
</template>

<script>
export default {
  name: "BaseMenuItem",

  props: {
    menuItemText: String,
    subClass: String,
    menuItemId: String,
    currentId: String,
  },

  data() {
    return {};
  },

  methods: {
    /**
     * Hàm bắt sự kiện bấm vào các mục trong menu
     * Created By: Ngọc 27/8/2021
     */
    itemOnClick() {
      this.$emit("itemOnClick", this.menuItemId);
    },

    /**
     * Hàm bắt sự kiện bấm vào các mục khác thì bỏ border xanh
     * Created By: Ngọc 27/8/2021
     */
    // removeSelected() {
    //   this.isSelected = false;
    // },
  },
};
</script>

<style></style>
