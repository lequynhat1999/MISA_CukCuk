<template>
  <div class="row header">
    <div class="text-left col-6">
      <div class="dropdown-title" id="dropdown-title-header">
        <Dropdown 
        :hiddenIcon = "hiddenIcon"
        :data="dataTitleHeader"
        :class="{'dropdown-title-header': isStyle}"
        :style="{'margin-left': '8px'}"
        />
      </div>
    </div>
    <div class="text-right col-6">
      <div class="text-right-row">
        <div class="icon-default"></div>
        <div class="text-name-user">Lê Quý Nhật</div>
        <div class="icon-option"></div>
      </div>
    </div>
  </div>
</template>

<script>
import Dropdown from "../../components/base/BaseDropdown.vue";
export default {
  name: "TheHeader",
  components: {
    Dropdown,
  },
  data() {
    return {
      // data cho dropdown titleHeader
      dataTitleHeader: [
        { Text: "Nhà hàng Biển Đông", Value: 0 },
        { Text: "Nhà hàng Biển Tây", Value: 1 },
        { Text: "Nhà hàng Biển Bắc", Value: 2 },
        { Text: "Nhà hàng Biển Nam", Value: 4 },
      ],
      // hidden css của dropdown titleHeader
      isStyle: true,
      // hidden icon delete của dropdown titleHeader
      hiddenIcon : true,
    };
  },
};
</script>

<style scoped>
@import "../../css/common/header.css";
</style>
    