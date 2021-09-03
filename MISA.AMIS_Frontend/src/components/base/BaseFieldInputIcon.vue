<template>
  <div class="d-flex p-relative">
    <input
      class="field-input-icon"
      type="text"
      placeholder="Tìm theo tên, mã nhân viên, số điện thoại"
      v-model="valueFinding"
      @input="startFinding"
    />
    <div class="glass"></div>
  </div>
</template>

<script>
import { debounce } from "debounce";

export default {
  name: "BaseFieldInputIcon",

  data() {
    return {
      valueFinding: this.searchContent,
    };
  },

  props: {
    searchContent: String,
  },

  methods: {
    /**
     * Hàm bắt sự kiện nhập vào ô tìm kiếm
     * Created By: Ngọc 28/08/2021
     */
    startFinding: debounce(function() {
      this.$emit("input", this.valueFinding);
    }, 500),
  },

  watch: {},
};
</script>

<style></style>
