<template>
  <div class="box-content">
    <div class="content-title">
      <Title :title="title" />
      <div class="add-employee">
        <button class="m-btn m-btn-defalut" id="btn-add" @click="btnAddClick">
          <div class="m-btn-icon icon-add"></div>
          <div class="text-add">Thêm nhân viên</div>
        </button>
      </div>
    </div>
    <div class="filter-bar row">
      <div class="filter-left col-11">
        <div class="box-search col-3" style="width: 290px">
          <input
            type="text"
            class="icon-search input-search"
            style="width: 250px"
            placeholder="Tìm kiếm theo Mã, Tên hoặc Số điện thoại"
            v-model="dataInput"
            @input="openIconDelete"
          />
          <div
            class="icon-input-delete"
            :class="{ 'icon-delete-close': isClose }"
          >
            <i class="fas fa-times" @click="clearValueInput"></i>
          </div>
        </div>
        <Dropdown
          :url="apiDepartment"
          :fields="fieldsDepartment"
          :data="dataDepartment"
        />
        <Dropdown
          :url="apiPosition"
          :fields="fieldsPosition"
          :data="dataPosition"
        />
      </div>
      <div class="filter-right col-1">
        <div class="filter-right-btn">
          <button
            class="m-second-button m-btn-refresh"
            @click="loadData"
          ></button>
        </div>
      </div>
    </div>

    <Table
      :headers="headers"
      :employees="employees"
      :isHidden="isHidden"
      :mode="modeFormDetail"
      @rowClick="rowClick"
      @deleteRow="deleteRow"
    ></Table>

    <Paging></Paging>

    <EmployeeDetailDialog
      :isHidden="isHidden" 
      :mode="modeFormDetail"
      ref="modeForm"
      @cancelFormDetail="cancelFormDetail"
      @btnAddClick="btnAddClick"
      @loadData="loadData"
      @closeForm="closeForm"
    />

    <Popup
      :isHiddenDelete="isHiddenDelete"
      :employee="employee"
      @cancelDelete="cancelDelete"
      @confirmDelete="confirmDelete"
    />
  </div>
</template>

<script>
let headers = [
  { text: "Mã nhân viên", attrName: "EmployeeCode", align: "left"},
  { text: "Họ và tên", attrName: "FullName", align: "left"},
  { text: "Giới tính", attrName: "GenderName", align: "left" },
  { text: "Ngày sinh", attrName: "DateOfBirth", align: "center" },
  { text: "Số điện thoại", attrName: "PhoneNumber", align: "left"},
  { text: "Email", attrName: "Email", align: "left"},
  { text: "Vị trí", attrName: "PositionName" , align: "left"},
  { text: "Phòng ban", attrName: "DepartmentName" , align: "left"},
  { text: "Mức lương cơ bản", attrName: "Salary", align: "right" },
  { text: "Tình trạng công việc", attrName: "WorkStatus" , align: "left"},
  { text: "Thao tác" ,align: "left"},
];
import axios from "axios";
import EmployeeDetailDialog from "../employee/EmployeeDetail.vue";
import Table from "../../components/base/BaseTable.vue";
import Paging from "../../components/base/BasePaging.vue";
import Title from "../../components/base/BaseTitle.vue";
import Popup from "../../components/base/BasePopup.vue";
import Dropdown from "../../components/base/BaseDropdown.vue";
export default {
  name: "EmployeeList",
  components: {
    EmployeeDetailDialog,
    Table,
    Paging,
    Title,
    Popup,
    Dropdown,
  },
  data() {
    return {
      // dữ liệu người dùng nhập vào trên thanh input search
      dataInput: "",
      // header cho table
      headers: headers,
      // 1 object nhân viên
      employee: {},
      // mảng các object nhân viên
      employees: [],
      // id nhân viên
      employeeId: "",
      // hidden form chi tiet
      isHidden: true,
      // hidden icon delete trong input search
      isClose: true,
      // mode de check add hay edit
      modeFormDetail: 0,
      // title
      title: "Danh sách nhân viên",
      // hidden form popup
      isHiddenDelete: true,
      // props cho dropdown department
      dataDepartment: [{ Text: "Tất cả phòng ban", Value: "" }],
      fieldsDepartment: ["DepartmentId", "DepartmentName"],
      apiDepartment: "http://cukcuk.manhnv.net/api/Department",
      // props cho dropdown position
      dataPosition: [{ Text: "Tất cả vị trí", Value: "" }],
      fieldsPosition: ["PositionId", "PositionName"],
      apiPosition: "http://cukcuk.manhnv.net/v1/Positions",
    };
  },
  created() {
    //load dữ liệu lên table
    this.loadData();
  },
  methods: {
    /**----------------------------------------------------
     * Hàm bắt sự kiện click nút xóa trên input search
     * CreateBy: LQNhat(30/07/2021)
     */
    clearValueInput() {
      this.dataInput = "";
      this.isClose = true;
    },
    /**-----------------------------------------------------------
     * Hàm bắt sự kiện hiển thị nút xóa khi nhập vào input search
     * CreateBy: LQNhat(30/07/2021)
     */
    openIconDelete() {
      if (this.dataInput == "") {
        this.isClose = true;
      } else {
        this.isClose = false;
      }
    },
    /**-------------------------------------------------------
     * Hàm đóng form chi tiết khi click vào btn trên form
     * CreateBy: LQNhat(6/8/2021)
     */
    cancelFormDetail(mode){
      this.isHidden = !this.isHidden;
      this.modeFormDetail = mode;
    },
    /**----------------------------------------------------
     * Hiển thị form chi tiết khi click btn Thêm nhân viên
     * Đóng form chi tiết khi click nút close trên form
     * CreateBy: LQNhat(30/07/2021)
     */
    btnAddClick() {
      this.isHidden = !this.isHidden;
      this.modeFormDetail = 0;
      this.$refs.modeForm.show(this.modeFormDetail);
    },
    /**------------------------------------------------
     * Hàm bắt sự kiện click vào icon sửa trong dòng
     * CreateBy: LQNhat(30/07/2021)
     */
    rowClick(employeeId) {
      this.isHidden = !this.isHidden;
      // this.employeeId = employeeId;
      this.modeFormDetail = 1;
      this.$refs.modeForm.show(this.modeFormDetail, employeeId);
    },
    /**----------------------------------------------------
     * Hàm bắt sự kiện click vào icon xóa trong dòng
     * CreateBy:LQNhat(31/07/2021)
     */
    deleteRow(employee) {
      this.isHiddenDelete = !this.isHiddenDelete;
      this.employee = employee;
      this.employeeId = employee.EmployeeId;
    },
    /**-----------------------------------
     * Hàm binding data lên table
     * CreateBy: LQNhat(31/07/2021)
     */
    loadData() {
      var self = this;
      // binding data
      axios
        .get("http://cukcuk.manhnv.net/v1/Employees")
        .then((res) => {
          self.employees = res.data;
        })
        .catch((res) => {
          console.log(res);
        });
    },
    /**----------------------------------
     * Hàm đóng form thông tin chi tiết
     * CreatBy: LQNhat(31/07/2021)
     */
    closeForm() {
      this.isHidden = true;
    },
    /**-----------------------------------------------------------
     * Hàm bắt sự kiện đóng form popup khi click vào nút cancel
     * CreatBy: LQNhat(31/07/2021)
     */
    cancelDelete() {
      this.isHiddenDelete = !this.isHiddenDelete;
    },
    /**----------------------------------------------------------
     * Hàm băt sự kiện xóa bản ghi khi click vào nút confirm
     * CreatBy: LQNhat(31/07/2021)
     */
    confirmDelete() {
      var self = this;
      axios
        .delete(`http://cukcuk.manhnv.net/v1/Employees/${this.employeeId}`)
        .then((res) => {
          console.log(res);
          self.isHiddenDelete = !self.isHiddenDelete;
          self.loadData();
          self.$toast.success("Xóa nhân viên thành công", {
            timeout: 2000,
          });
        });
    },
  },
};
</script>

<style>
</style>