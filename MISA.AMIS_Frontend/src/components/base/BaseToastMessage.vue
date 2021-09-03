<template>

    <div :class="['toast', subClass, { 'd-none': hideToastMessage }]">
        <div class="toast-message">
          <div class="toast-icon"></div>
          <div class="toast-text">{{ toastMessageText }}</div>
        </div>
    </div>
</template>

<script>

export default {
  name: "BaseToastMessage",
  components:{
  
  },

  props: {
    subClass: String,
    hideToastMessage: Boolean,
    toastMessageText: String,
  },

  methods: {
    /**
    * Hàm đóng toast-message
    * Created By: Ngọc 29/08/2021  
    */   
    closeToastMessage(){
      this.$emit('closeToastMessage');
    }
  },
  
  watch: {
    hideToastMessage: function () {
      setTimeout(() => {
        this.$emit("closeToastMessage");
      }, 2000);
    },
  },
};
</script>

<style></style>