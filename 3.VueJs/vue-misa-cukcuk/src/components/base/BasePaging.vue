<template>
  <div class="paging">
    <div class="text-paging">
      Hiển thị {{ this.start }}-{{ this.end }}/{{ this.amountPage }} nhân viên
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
          v-for="index in textPages"
          :key="index.Value"
          :class="index.Value == indexCurrent ? 'paging-active' : ''"
          @click="clickNumberPage(index.Value)"
        >
          <div class="page-number">{{ index.Text }}</div>
        </a>
      </div>
      <div
        class="next-page"
        @click="
          clickNumberPage(
            indexCurrent + 1 > numPages ? numPages : indexCurrent + 1
          )
        "
      ></div>
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
      displayPages: 5,
      totalPages: [],
      textPages: [],
    };
  },
  // numPages: tổng số trang, amountPage: tổng số bản ghi
  props: ["pageIndex", "pageSize", "amountPage", "numPages"],
  // created() {
  //   this.pagingByNumPages();
  // },
  watch:{
    amountPage()
    {
      this.pagingByNumPages();
    }
  },
  // mounted() {
  //   this.pagingByNumPages();
  // },

  methods: {
    /**----------------------------------------------------------
     * Chuyển trang
     * CreateBy: LQNhat(16/08/2021)
     */
    async clickNumberPage(index) {
      this.indexCurrent = index;
      if (this.indexCurrent < this.numPages) {
        this.start = (this.indexCurrent - 1) * this.pageSize + 1;
        this.end = this.start + this.pageSize - 1;
      } else {
        this.start = (this.indexCurrent - 1) * this.pageSize + 1;
        this.end = this.amountPage;
      }
      await this.$emit("paging", this.indexCurrent);
      this.pagingByNumPages();
    },

    /**-------------------------------------------------------------------
     * Phân trang
     * CreateBy: LQNhat(16/08/2021)
     */
    pagingByNumPages() {
      this.textPages = [];
      this.totalPages = [];
      
      // push text và value vào mảng totalPages
      for (var i = 1; i <= this.numPages; i++) {
        this.totalPages.push({ Text: i + "", Value: i });
      }

      this.indexCurrent = this.pageIndex;

      //check index của page hiện tại
      if (this.indexCurrent <= Math.round(this.displayPages / 2)) {
        this.textPages = this.totalPages.slice(
          0,
          this.numPages < this.displayPages ? this.numPages : this.displayPages
        );
      } else if (
        this.indexCurrent > Math.round(this.displayPages / 2) &&
        this.indexCurrent <= this.numPages - Math.floor(this.displayPages / 2)
      ) {
        this.textPages = this.totalPages.slice(
          this.indexCurrent - 3,
          this.indexCurrent + 2
        );
      }
      else
      {
        this.textPages = this.totalPages.slice(this.numPages < this.displayPages ? 0 : this.numPages - this.displayPages,this.numPages);
      }

      // tính start và end
      this.start = (this.indexCurrent - 1) * this.pageSize + 1;
      this.end =
        this.indexCurrent < this.numPages
          ? this.start + this.pageSize - 1
          : this.amountPage;

      debugger; // eslint-disable-line
    },

    /**------------------------------------------------
     * Khi click nút reload thì reset lại paging
     * CreateBy: LQNhat(16/08/2021)
     */
    resetPaging() {
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
