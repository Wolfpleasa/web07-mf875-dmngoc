<template>
   <div :class="['gears' , {'d-none' : hideLoader}]">
        <img src="../../assets/icon/biggear.png" alt="gear" class="big">

        <img src="../../assets/icon/smallgear.png" alt="gear" class="small">
    </div>
</template>

<script>
export default {
  name: "BaseLoader",

  props: {
    hideLoader: Boolean
  },

  methods: {
 
  },

  watch: {
   
  },
};
</script>

<style scoped>

</style>
