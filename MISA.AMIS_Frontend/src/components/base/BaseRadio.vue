<template>
  <div>
    <div v-if="labelText.length > 0">
      <label for="">{{ labelText }} </label><br />
    </div>

    <div class="d-flex" >
      <div
        v-for="(item, index) in items"
        :key="item[itemId]"
        :tabindex="tabindex"
        :Value="item[itemId]"
        v-on:keyup.space="chooseRadioItemByKey($event,index)"
        @click="chooseRadioItem(item[itemId], itemId, index)"
        :class="['radio-item', index == currentIndex ? 'selected' : '']"
      >
        <div class="radio"></div>
        <div class="text">{{ item[itemName] }}</div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: "BaseInput",
  props: {
    labelText: String,
    tabindex: String,
    itemId: String,
    itemName: String,
    FieldName: String,
    dnone: Boolean,
    selectedId: String,
  },
  data() {
    return {
      items: [],
      currentIndex: 0,
    };
  },

  methods: {
    /**
     * Sự kiện chọn 1 lựa chọn bằng phím
     * Created By: Ngọc 31/08/2021
     */
    chooseRadioItemByKey(event,index) {
      if (event.code == "Space") {
        event.preventDefault();
        let item = this.items[index],
          itemValue = item[this.itemId];
        this.chooseRadioItem(itemValue, this.itemId, index);
      }
    },

    /**
     * Hàm bắt sự kiện chọn giới tính
     * Created By: Ngọc 28/08/2021
     */
    chooseRadioItem(itemValue, itemId, index) {
      let me = this;
      me.currentIndex = index;
      me.$emit("chooseRadioItem", itemId, itemValue);
    },

    /**
     * Xem hàm sửa có gọi không để chọn giới tính tương ứng
     * Created By: Ngọc 29/08/2021
     */
    setValueRadio() {
      let me = this;
      // nếu selectedId(Gender) mà tồn tại(tức là được Employeelist truyền vào => form sửa dùng)
      if ((me.selectedId + "").length > 0) {
        // duyệt từng items(gender)
        me.items.forEach(function (item, index) {
          // nếu selectedId(Gender) từ cha truyền vào mà trùng với itemId(Gender) trong list
          if (me.selectedId == item[me.itemId]) {
            // gán currentIndex = index (để hàng tương ứng được tích)
            me.currentIndex = index;
          }
        });
      } else {
        // form thêm dùng
        // Tự chọn giới tính nam
        me.currentIndex = 0;
      }
    },
  },

  created() {
    let me = this;
    switch (me.itemName) {
      case "GenderName":
        this.items = [
          {
            Gender: 1,
            GenderName: "Nam",
          },
          {
            Gender: 0,
            GenderName: "Nữ",
          },
          {
            Gender: 2,
            GenderName: "Khác",
          },
        ];
        break;

      default:
        break;
    }
  },

  watch: {
    selectedId: function () {
      this.setValueRadio();
    },
  },
};
</script>

<style scoped>
</style>
