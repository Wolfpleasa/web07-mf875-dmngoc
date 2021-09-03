<template>
  <div
    :id="idPopup"
    :class="[' z-index-12', { 'd-none': dnone }]"
  >
    <div class="middle">
      <div class="mode"></div>
      <div class="text" v-html="warningText"></div>
    </div>
    <div class="bottom">
      <div>
        <Button
          tabindex="0"
          @btn-click="btnCancelOnClick"
          :buttonText="btnCancelText"
          id="btnCancel"
          subClass="cancel"
        />
      </div>

      <div class="save-area">
        <Button
          tabindex="0"
          @btn-click="btnConfirmOnClick"
          :buttonText="btnConfirmText"
          id="btnSave"
          subClass="save d-flex"
        />

        <div v-if="idPopup == 'confirm-popup'">
          <Button
            tabindex="0"
            @btn-click="btnNoOnClick"
            buttonText="Không"
            id="btn"
            subClass="cancel"
          />
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import Button from "../base/BaseButton.vue";

export default {
  name: "WarningPopup",
  components: {
    Button,
  },

  props: {
    dnone: {
      type: Boolean,
      default: true,
      require: true,
    },
    warningText: String,
    idPopup: String,
    mode: String,
    btnConfirmText: String,
    btnCancelText: String,
    subClass: String,
  },

  data() {
    return {
      employee: {},
      reFocus: false,
    };
  },
  methods: {
    /**
     * Hàm đóng popup
     * Created By: Ngọc 28/08/2021
     */
    btnCancelOnClick() {
      let me = this;
      me.$emit("btnCancelOnClick");
    },

    /**
     * Hàm bấm nút xác nhận
     * Created By: Ngọc 28/08/2021
     */
    btnConfirmOnClick() {
      this.$emit("btnConfirmOnClick", this.idPopup);
    },
    
    /**
     * Hàm bấm nút không
     * Created By: Ngọc 28/08/2021
     */
    btnNoOnClick(){
      this.$emit("btnNoOnClick");
    }
  },

  watch: {
    dnone: function () {
      this.reFocus = !this.reFocus;
    },
  },
};
</script>

<style scoped></style>
