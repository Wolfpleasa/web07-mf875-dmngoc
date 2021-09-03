<template>
  <div
    ref="dropdown"
    :class="['dropdownFunction', { 'd-none': hideDropdownFunction }]"
  >
    <div class="fn-list" :active="active" :duration="1000">
      <div class="functionDelete" @click="deleteOnClick()">Xóa</div>
      <div class="functionDuplicate" @click="duplicateOnClick()">Nhân bản</div>
      <div class="functionDelete">Ngưng sử dụng</div>
    </div>
  </div>
</template>

<script>
export default {
  name: "BaseDropdownFunction",

  components: {},

  props: {
    selectedId: String,
    tabindex: String,
    top: Number,
    count: Number,
  },

  data() {
    return {
      //slidedown/up
      active: false,
      hideDropdownFunction: true,
      checkPos: 0,
    };
  },

  methods: {
    /**
     * Hàm bắt sự kiện xóa ở chức năng
     * Created By: Ngọc 30/08/2021
     */
    deleteOnClick() {
      this.$emit("deleteOnClick");
    },

    /**
     * Hàm bắt sự kiện nhân bản ở chức năng
     * Created By: Ngọc 30/08/2021
     */
    duplicateOnClick() {
      this.$emit("duplicateOnClick");
    },

    /**
     * Hàm reset vị trí của dropdown về 0
     * Created By: Ngọc 30/08/2021
     */
    resetPos(checkPos) {
      this.$emit("resetPos", checkPos);
    },
  },

  watch: {
    count: function() {
      let me = this;
      if (me.top == me.checkPos || me.top == 0) {
        me.checkPos = 0;
        this.hideDropdownFunction = true;
        me.resetPos(me.checkPos);
      } else {
        me.checkPos = me.top;
        this.hideDropdownFunction = false;
        this.$refs.dropdown.style.top = this.top + 25 + "px";
        if (this.top > document.body.clientHeight - 150) {
          this.$refs.dropdown.style.top = this.top - 109 + "px";
        }
      }
    },
  },
};
</script>

<style></style>
