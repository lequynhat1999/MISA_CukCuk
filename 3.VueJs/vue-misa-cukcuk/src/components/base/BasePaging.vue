<template>
  <div class="paging">
    <div class="text-paging">
      Hiển thị {{ this.start }}-{{ this.end}}/{{ this.amountPage }} nhân viên
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
export default {
  name: "BasePaging",
  data() {
    return {
      start: 0,
      end: 0,
      indexCurrent: 0,
    };
  },
  props: ["pageIndex", "pageSize","amountPage","numPages"],
   created() {
     this.pagingByNumPages();
  },
  watch:{
    numPages()
    {
      this.pagingByNumPages();
    }
  },
  methods: {
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
        this.end = this.amountPage;
      }
      this.$emit("paging",this.indexCurrent);
    },

    /**-------------------------------------------------------------------
     * Phân trang
     * CreateBy: LQNhat(16/08/2021)
     */
    pagingByNumPages()
    {
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
  },
};
</script>

<style scoped>
@import "../../css/common/paging.css";
</style>
