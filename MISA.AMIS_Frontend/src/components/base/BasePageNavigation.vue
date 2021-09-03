<template>
  <div class="page-navigator">
    <!-- <div class="ml-10" id="div1-paging"></div> -->
    <div class="ml-10">
      Tổng số: <b>{{ totalEntity }}</b> bản ghi
    </div>
    <div class="d-flex">
      <div class="modifyNumber">
        <Dropdown
          tabindex="0"
          defaultName="10 bản ghi trên 1 trang"
          itemId="Paging"
          itemName="PagingName"
          @chooseDropdownItem="chooseDropdownItem"
        />
      </div>
      <div class="paging">
        <div class="btn prev-page" @click="pageNumberOnClick('prev-page')">
          Trước
        </div>

        <div
          v-for="value,index in rangeWithDots"
          :key="index"
          :class="[
            'btn page-number',
            { 'page-selected': value == currentPageNumber },
          ]"
          @click="pageNumberOnClick(value)"
        >
          {{ value }}
        </div>

        <div class="btn next-page" @click="pageNumberOnClick('next-page')">
          Sau
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import Dropdown from "./BaseDropdown.vue";
export default {
  name: "BasePageNavigation",
  components: {
    Dropdown,
  },

  props: {
    totalEntity: Number,
    totalPageNumber: Number,
    searchContent: String,
  },

  data() {
    return {
      // Số trang hiện lên giao diện
      range: [],
      rangeWithDots: [],
      // Trang hiện tại
      currentPageNumber: 1,

      
    };
  },

  methods: {

    /**
     * Hàm chọn số lượng bản ghi trên 1 trang
     * Created By: Ngọc 28/08/2021
     */
    chooseDropdownItem(itemName) {
      let entityPerPage = Number(itemName.split(" ")[0]);
      this.$emit("modifyNumber", entityPerPage);
    },

    /**
     * Hàm dùng để chuyển trang
     * Created By: Ngọc 28/08/2021
     * Modified By: Ngọc 01/09/2021
     */
    pageNumberOnClick(btnPage) {
      let me = this;
      switch (btnPage) {
        case "prev-page":
          if (me.currentPageNumber > 1) me.currentPageNumber -= 1;
          break;
        case "next-page":
          if (me.currentPageNumber < me.totalPageNumber)
            me.currentPageNumber += 1;
          break;
        default:
          if(btnPage != "...")
            me.currentPageNumber = btnPage;
          break;
      }
      me.updatePage();
    },

    /**
     * Hàm cập nhật trang đang được xem
     * Created By: Ngọc 28/08/2021
     * Modified By: Ngọc 01/09/2021
     */
    updateCenterNumber(currentPageNumber, totalPageNumber) {
      let me = this;
      me.currentPageNumber = Math.min(me.currentPageNumber, me.totalPageNumber);
      var current = currentPageNumber,
        last = totalPageNumber,
        delta = 1,
        lowerLimit = current - delta,
        upperLimit = current + delta,
        l;

      for (let i = 1; i <= last; i++) {
        if (i == 1 || i == last || (i >= lowerLimit && i <= upperLimit)) {
          me.range.push(i);
        }
      }

      if(last == 4){
        me.rangeWithDots.splice(0, 0, 1, 2, 3, 4);
        return;
      }

      if (current == 1 && last>=5) {
        me.rangeWithDots.splice(0, 0, 1, 2, 3, "...", last);
        return;
      }

      if (current == last && last>=5) {
        me.rangeWithDots.splice(0, 0, 1, "...", last-2, last-1, last);
        return;
      }

      for (let i of me.range) {
        if (l) {
          if (i - l !== 1) {
            me.rangeWithDots.push("...");
          }
        }

        me.rangeWithDots.push(i);
        l = i;
      }
    },

    /**
     * Hàm gọi lên hàm update ở cha
     * Created By: Ngọc 28/08/2021
     * Modified By: Ngọc 01/09/2021
     */
    updatePage() {
      let me = this;
      me.$emit("updatePage", me.currentPageNumber);
      me.range = [];
      me.rangeWithDots = [];
      me.updateCenterNumber(me.currentPageNumber, me.totalPageNumber);
    },
  },

  created() {
    this.updatePage();
  },

  watch: {
    searchContent: function () {
      this.updatePage();
    },

    totalPageNumber: function () {
      this.updatePage();
    },
  },
};
</script>

<style></style>
