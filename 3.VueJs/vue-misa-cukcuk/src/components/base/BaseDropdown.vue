<template>
  <div class="dropdown col-3 dropdown-filter" @click="toggleDropdown">
    <div class="dropdown-text">{{ selectedText }}</div>
    <div
      class="dropdown-icon-delete"
      :class="{ 'icon-delete-hidden': hiddenIcon }"
    >
      <i class="fas fa-times"></i>
    </div>
    <div class="dropdown-icon" :class="{rotate : isRotate}">
      <i class="fas fa-chevron-down"></i>
    </div>
    <div
      class="dropdown-data"
      :class="{ 'dropdown-hidden': isClose }"
      @click="getCurrentItem"
    >
      <div
        class="dropdown-item"
        v-for="(item, index) in items"
        :key="item.Value"
        @click="selectItem(index)"
        :value="item.Value"
        :class="{ 'dropdown-selected': activeItem(index) }"
      >
        <div
          class="dropdown-item-icon"
          :class="{ 'icon-dropdown-selected': activeItem(index) }"
        >
          <i class="fa fa-check"></i>
        </div>
        <div class="dropdown-item-text">
          {{ item.Text }}
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import axios from "axios";
export default {
  name: "BaseDropdown",
  props: ["url", "fields", "data", "hiddenIcon", "value"],
  async created() {
    await this.getItem();
    this.selectItem(0);
  },
  data() {
    return {
      isClose: true,
      selectedText: "",
      selectedValue: "",
      items: [],
      currentIndex: 0,
      iconSelected: `<i class="fa fa-check"></i>`,
      isRotate: false,
    };
  },

  watch: {
    async value() {
      // wait get full options
      this.items = [];
      await this.getItem();
      let i = this.getIndexByValue();
      if (i != -1) this.selectItem(i);
      else this.selectItem(0);
    },
  },

  methods: {
    /**--------------------------------------------------------------------
     * Hàm bắt sự kiện click gửi value sang cho bên employeeDetail
     * CreateBy: LQNhat(2/8/2021)
     */
    getCurrentItem() {
      this.$emit("get", this.selectedValue);
    },

    /**----------------------------------------------
     * Hàm xử lý việc toggle dropdown
     * CreateBy: LQNhat(1/8/2021)
     */
    toggleDropdown() {
      this.isClose = !this.isClose;
      this.isRotate = !this.isRotate;
    },

    /**---------------------------------------------------
     * Hàm call API để lấy dữ liệu
     * CreateBy: LQNhat(1/8/2021)
     */
    async getItem() {
      try {
        // push dữ liệu trong data vào trong items
        this.data.forEach((element) => {
          this.items.push(element);
        });
        // check url có null
        if (!this.checkUrl(this.url)) {
          let response = await axios.get(this.url);
          response.data.forEach((element) => {
            this.items.push({
              Text: element[this.fields[1]],
              Value: element[this.fields[0]],
            });
          });
        }
      } catch (error) {
        console.log(error);
      }
    },

    /**--------------------------------------------
     * Hàm select item
     * CreateBy: LQNhat(1/8/2021)
     */
    selectItem(index) {
      this.selectedText = this.items[index].Text;
      this.selectedValue = this.items[index].Value;
      this.currentIndex = index;
    },

    /**-----------------------------------------------------
     * Hàm check index của item
     * CreatBy:LQNhat(1/8/2021)
     */
    activeItem(index) {
      return this.currentIndex == index ? true : false;
    },

    /**------------------------------------------------------------------
     * Hàm check url truyền vào
     * CreateBy: LQNhat(1/8/2021)
     */
    checkUrl(url) {
      return typeof url === undefined || url === null || url === ""
        ? true
        : false;
    },

    /**--------------------------------------------------------------
     * Hàm trả về index của item khi so sánh value
     * CreateBy: LQNhat(2/8/2021)
     */
    getIndexByValue() {
      return this.items.findIndex((element, index) => {
        if (element.Value === this.value) {
          return index;
        }
      });
    },

    /**----------------------------------------------------------------------
     * Hàm check event khi click ra bên ngoài dropdown
     * CreateBy: LQNhat(1/8/2021)
     */
    close(e) {
      if (!this.$el.contains(e.target)) {
        this.isClose = true;
        this.isRotate = false;
      }
    },

    /**------------------------------------------------------
     * Hàm bắt sự kiện nút reload
     * khi reload sẽ đưa text dropdown về mặc định
     * CreateBy: LQNhat(14/08/2021)
     */
    setTextDefault()
    {
      this.selectedText = this.items[0].Text;
      this.selectedValue = this.items[0].Value;
      this.currentIndex = 0;
    }
  },

  mounted() {
    document.addEventListener("click", this.close);
  },
  // beforeDestroy() {
  //   document.removeEventListener("click", this.close);
  // },
};
</script>

<style scoped>
@import "../../css/common/main.css";
</style>