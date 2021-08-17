<template>
  <div class="paging">
    <div class="text-paging">
      Hiển thị {{ this.start }}-{{ this.end}}/{{ this.amount }} nhân viên
    </div>
    <div class="pagination">
      <div class="back-to-start" @click="clickNumberPage(1)"></div>
      <div
        class="pre-page"
        @click="clickNumberPage(indexCurrent - 1 < 1 ? 1 : indexCurrent - 1)"
      ></div>
      <div class="number-page">
        <a
          class="circle-number"
          v-for="index in numPages"
          :key="index"
          :class="index == indexCurrent ? 'paging-active' : ''"
          @click="clickNumberPage(index)"
        >
          <div class="page-number">{{index}}</div>
        </a>
      </div>
      <div class="next-page" @click="clickNumberPage(indexCurrent + 1 > numPages ? numPages : indexCurrent + 1)"></div>
      <div class="go-to-end" @click="clickNumberPage(numPages)"></div>
    </div>
    <div class="totalPage">
      <span class="amountPage">{{ this.pageSize }}</span> nhân viên trên 1 trang
    </div>
  </div>
</template>

<script>
import axios from "axios";
export default {
  name: "BasePaging",
  data() {
    return {
      start: 0,
      end: 0,
      numPages: 0,
      indexCurrent: 0,
      amount: 0,
    };
  },
  props: ["pageIndex", "pageSize"],
  async created() {
    await this.loadData();
     this.paging();
  },
  methods: {
    /**---------------------------------------------------
     * Lấy toàn bộ danh sách
     * CreateBy: LQNhat(16/08/2021)
     */
    async loadData() {
      var self = this;
      // binding data
      await axios
        .get("https://localhost:44338/api/v1/employees")
        .then((res) => {
          self.amount = res.data.length;
        })
        .catch((res) => {
          console.log(res);
        });
    },

    /**----------------------------------------------------------
     * Chuyển trang
     * CreateBy: LQNhat(16/08/2021)
     */
    clickNumberPage(index)
    {
      this.indexCurrent = index;
      if(this.indexCurrent < this.numPages)
      {
        this.start = (this.indexCurrent -1)*this.pageSize +1;
        this.end = this.start + this.pageSize -1;
      }
      else
      {
        this.start = (this.indexCurrent -1)*this.pageSize +1;
        this.end = this.amount;
      }
      this.$emit("paging",this.indexCurrent);
    },

    /**-------------------------------------------------------------------
     * Phân trang
     * CreateBy: LQNhat(16/08/2021)
     */
    paging()
    {
      if(this.amount % this.pageSize == 0)
      {
        this.numPages = this.amount / this.pageSize;
      }
      else
      {
        this.numPages = Math.floor(this.amount / this.pageSize) + 1;
      }
      this.indexCurrent = this.pageIndex;
      this.start = this.indexCurrent;
      this.end = this.pageSize;
    },

    /**------------------------------------------------
     * Khi click nút reload thì reset lại paging
     * CreateBy: LQNhat(16/08/2021)
     */
    resetPaging()
    {
      this.indexCurrent = 1;
      this.start = this.indexCurrent;
      this.end = this.pageSize;
    },

    /**----------------------------------------------------------
     * Hiển thị nhiều trang
     * CreateBy: LQNhat(16/08/2021)
     */
    // displayPaging()
    // {
    //   var total = this.numPages;
    //   var current = this.indexCurrent;
    //   if(current <= 3)
    //   {
    //     return [1,2,3];
    //   }
    //   else if(current > 3 && current < total - 1)
    //   {

    //   }
    // }
  },
};
</script>

<style scoped>
@import "../../css/common/paging.css";
</style>
