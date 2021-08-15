<template>
  <div
    class="table table-employee"
    style="overflow-y: scroll; height: calc(100vh - 214px)"
  >
    <table border="0" cellspacing="0" width="100%">
      <thead>
        <tr>
          <th
            v-for="(item, index) in headers"
            :key="index"
            :style="{ 'text-align': item.align }"
          >
            {{ item.text }}
            <span v-html="iconSort"></span>
          </th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="employee in employees" :key="employee.EmployeeId">
          <td>{{ employee.EmployeeCode }}</td>
          <td :title="employee.FullName">
            {{ employee.FullName }}
          </td>
          <td>{{ formatGender(employee.Gender) }}</td>
          <td style="text-align: center">
            {{ formatDate(employee.DateOfBirth) }}
          </td>
          <td>{{ employee.PhoneNumber }}</td>
          <td :title="employee.Email">{{ employee.Email }}</td>
          <td>
            {{ employee.PositionName }}
          </td>
          <td>
            {{ employee.DepartmentName }}
          </td>
          <td style="text-align: right">
            {{ formatPrice(employee.Salary) }}
          </td>
          <td>{{ formatWorkStatus(employee.WorkStatus) }}</td>
          <td>
            <div class="option-icon">
              <i
                class="fas fa-edit icon-edit"
                @click="rowTableClick(employee.EmployeeId)"
                title="Sửa"
              ></i>
              <i
                class="fas fa-trash-alt"
                @click.prevent="rightClickRow(employee)"
                title="Xóa"
              ></i>
            </div>
          </td>
        </tr>
        <tr v-if="employees.length == 0 ? true : false">
          <td colspan="12" style="text-align: center">Không có dữ liệu để hiển thị</td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script>
import moment from "moment";
export default {
  name: "BaseTable",
  data() {
    return {
      iconSort: `<i class="fas fa-sort icon-table"></i>`,
      employeeId: "",
      test: "red",
    };
  },
  props: {
    headers: {
      type: Array,
    },
    employees: {
    },
    isHidden: {
      type: Boolean,
      default: true,
    },
    mode: {
      type: Number,
      default: 0,
    },
  },
  methods: {
    /**------------------------------------------------
     * Hàm bắt sự kiện click vào từng dòng trong table
     * CreateBy: LQNhat(30/07/2021)
     */
    rowTableClick(employeeId) {
      this.employeeId = employeeId;
      this.$emit("rowClick", employeeId);
    },

    /**-------------------------------------------------------
     * Hàm bắt sự kiện chuột phải vào từng dòng trong table
     * CreateBy:LQNhat(1/8/2021)
     */
    rightClickRow(employee) {
      this.$emit("deleteRow", employee);
    },

    /**----------------------------------------------------------
     * Hàm format ngày tháng năm trên table
     * CreateBy:LQNhat(1/8/2021)
     */
    formatDate(value) {
      if (value) {
        return moment(String(value)).format("DD/MM/YYYY");
      }
    },

    /**
     * Hàm format lương trên table
     * CreateBy:LQNhat(1/8/2021)
     */
    formatPrice(value) {
      let val = (value / 1).toFixed(0).replace(".", ",");
      return val.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ".");
    },
    /**
     * Hàm format về text của giới tính
     * CreateBy:LQNhat(14/8/2021)
     */
    formatGender(value) {
      switch (value) {
        case 0:
          return "Nữ";
        case 1:
          return "Nam";
        case 2:
          return "Không xác định";
      }
    },
    /**
     * Hàm format về text của trạng thái công việc
     * CreateBy: LQNhat(14/08/2021)
     */
    formatWorkStatus(value) {
      switch (value) {
        case 0:
          return "Làm chính thức";
        case 1:
          return "Đang thực tập";
        case 2:
          return "Đang học việc";
        case 3:
          return "Đang nghỉ phép";
        case 4:
          return "Đã nghỉ việc";
      }
    },
  },
};
</script>

<style scoped>
@import "../../css/common/table.css";
.option-icon {
  text-align: center;
  font-size: 16px;
}

.icon-edit {
  padding-right: 8px;
}
</style>