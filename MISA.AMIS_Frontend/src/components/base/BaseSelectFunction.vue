<template>
  <div class="p-relative d-flex al-center">
    <div class="functionUpdate" @click="UpdateOnClick()">Sửa</div>
    <div
      :class="['selectFunction', { selected: !dnone }]"
      v-on-clickaway="closeDropDown"
      @click="selectFunctionOnClick()"
      ref="myDiv"
    >
      <div :class="['arrow', rotate ? 'rot-180' : 'rot-0']"></div>
    </div>
  </div>
</template>

<script>
import { mixin as clickaway } from "vue-clickaway";

export default {
  mixins: [clickaway],
  name: "BaseSelectFunction",

  components: {},

  props: {
    selectedId: String,
  },

  data() {
    return {
      // quay mũi tên
      rotate: false,
      // ẩn/hiện các item
      dnone: true,
      // vị trí
      top: 0,
    };
  },

  methods: {
    /**
     * Sự kiện đóng dropdown(sử dụng vue-clickaway)
     * Created By:Ngọc 28/08/2021
     */
    closeDropDown() {
      let me = this;
      me.rotate = false;
      me.dnone = true;
    },

    /**
     * Sự kiện bấm để hiện/ ẩn dropdown
     * Created By:Ngọc 28/08/2021
     */
    selectFunctionOnClick() {
      let me = this;
      me.rotate = !me.rotate;
      me.dnone = !me.dnone;
      me.active = !me.active;
      me.top = me.$refs.myDiv.getBoundingClientRect().top;
      if (me.dnone == true) {
        me.top = 0;
      }
      me.$emit("selectFunctionOnClick", me.top, me.selectedId);
    },

    /**
     * Sự kiện bấm sửa ở chức năng
     * Created By:Ngọc 28/08/2021
     */
    UpdateOnClick() {
      this.$emit("UpdateOnClick");
    },
  },
};
</script>

<style></style>
