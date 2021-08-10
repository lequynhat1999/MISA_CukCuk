<template>
  <div class="paging">
    <div class="text-paging">Hiển thị 1-10/1000 nhân viên</div>
    <div class="pagination">
      <div class="back-to-start"></div>
      <div class="pre-page"></div>
      <div class="number-page">
        <div class="circle-number paging-active"><div class="page-number">1</div></div>
        <div class="circle-number"><div class="page-number">2</div></div>
        <div class="circle-number"><div class="page-number">3</div></div>
      </div>
      <div class="next-page"></div>
      <div class="go-to-end"></div>
    </div>
    <div class="totalPage">
      <span class="amountPage">{{this.amountPage}}</span> nhân viên trên 1 trang
    </div>
  </div>
</template>

<script>
import axios from "axios";
export default {
    name: "BasePaging",
    data() {
      return {
        amountPage: 0,
      }
    },
    created() {
      this.loadData();
    },
    methods: {
      loadData() {
      axios
        .get("http://cukcuk.manhnv.net/v1/Employees")
        .then((res) => {
          this.amountPage = res.data.length;
        })
        .catch((res) => {
          console.log(res);
        });
    },
    },
};
</script>

<style scoped>
@import '../../css/common/paging.css';
</style>
