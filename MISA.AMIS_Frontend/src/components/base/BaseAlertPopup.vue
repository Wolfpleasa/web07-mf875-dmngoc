<template>

    <div :class="['alert', subClass, { 'd-none': hideAlertPopup }]">
        <div class="alert-message">
          <div class="alert-icon"></div>
          <div class="alert-text">{{ alertMessageText }}</div>
        </div>
        <div class="alert-close" >
          <Button
          tabindex="0"
          @btn-click="closeAlertPopup"
          buttonText="Đóng"
        />
        </div>
    </div>
</template>

<script>
import Button from "../../components/base/BaseButton.vue";

export default {
  name: "BaseAlertPopup",
  components:{
    Button,
  },

  props: {
    subClass: String,
    hideAlertPopup: Boolean,
    alertMessageText: String,
  },

  methods: {
    /**
    * Hàm đóng alert-message
    * Created By: Ngọc 29/08/2021  
    */   
    closeAlertPopup(){
      this.$emit('closeAlertPopup');
    }
  },
  
  watch: {
 
  },
};
</script>

<style></style>