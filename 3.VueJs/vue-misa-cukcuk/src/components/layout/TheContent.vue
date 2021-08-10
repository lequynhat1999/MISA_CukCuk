<template>
    <div class="content">
      <EmployeeList/>
    </div>
</template>

<script>
import EmployeeList from '../../view/employee/EmployeeList.vue';
export default {
    name: 'TheContent',
    components: {
      EmployeeList
    }

}
</script>

<style scoped>
@import '../../css/common/content.css';
@import '../../css/page/employee.css'
</style>