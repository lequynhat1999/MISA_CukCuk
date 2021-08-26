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
      <div class="delete-employees">
        <button
          class="m-btn m-btn-defalut m-btn-delete"
          @click="btnDeleteEmployee"
          :disabled="isDisabled"
          :class="{ 'btn-disaabled': isDisabled }"
        >
          <div class="m-btn-icon icon-delete">
            <i class="far fa-trash-alt"></i>
          </div>
          <div class="text-add">Xóa nhân viên</div>
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
            v-model="keysearch"
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
          ref="textDropdownPostion"
          @get="getValDepartment"
          :url="apiDepartment"
          :fields="fieldsDepartment"
          :data="dataDepartment"
        />
        <Dropdown
          ref="textDropdownDepartment"
          @get="getValPosition"
          :url="apiPosition"
          :fields="fieldsPosition"
          :data="dataPosition"
        />
      </div>
      <div class="filter-right col-1">
        <div class="filter-right-btn">
          <button
            class="m-second-button m-btn-refresh"
            @click="reloadTableAndFilter"
          ></button>
        </div>
      </div>
    </div>

    <Table
      :headers="headers"
      :employees="employees"
      :isHidden="isHidden"
      :mode="modeFormDetail"
      ref="resetInputChecked"
      @rowClick="rowClick"
      @changeDisabled="changeDisabled"
    ></Table>

    <Paging
      :pageIndex="pageIndex"
      :pageSize="pageSize"
      :amountPage="amountPage"
      :numPages="numPages"
      ref="resetPaging"
      @paging="paging"
    >
    </Paging>

    <EmployeeDetailDialog
      :isHidden="isHidden"
      :mode="modeFormDetail"
      ref="modeForm"
      @cancelFormDetail="cancelFormDetail"
      @btnAddClick="btnAddClick"
      @reloadTableAndFilter="reloadTableAndFilter"
      @closeForm="closeForm"
    />

    <Popup
      :isHiddenDelete="isHiddenDelete"
      :employee="employee"
      :arrEmployeeCode="arrEmployeeCode"
      :titlePopupDelete="titlePopupDelete"
      :textPopupDelete="textPopupDelete"
      :iconPopupDelete="iconPopupDelete"
      @cancelDelete="cancelDelete"
      @confirmDeleteEmployees="confirmDeleteEmployees"
    />
    <Loading :isLoading="isLoading" />
  </div>
</template>

<script>
let headers = [
  { text: "", align: "center" },
  { text: "Mã nhân viên", align: "left" },
  { text: "Họ và tên", align: "left" },
  { text: "Giới tính", align: "left" },
  { text: "Ngày sinh", align: "center" },
  { text: "Số điện thoại", align: "left" },
  { text: "Email", align: "left" },
  { text: "Vị trí", align: "left" },
  { text: "Phòng ban", align: "left" },
  { text: "Mức lương cơ bản", align: "right" },
  { text: "Tình trạng công việc", align: "left" },
  // { text: "Thao tác", align: "left" },
];
import axios from "axios";
import EmployeeDetailDialog from "../employee/EmployeeDetail.vue";
import Table from "../../components/base/BaseTable.vue";
import Paging from "../../components/base/BasePaging.vue";
import Title from "../../components/base/BaseTitle.vue";
import Popup from "../../components/base/BasePopup.vue";
import Dropdown from "../../components/base/BaseDropdown.vue";
import Loading from "../../components/base/BaseLoading.vue";
export default {
  name: "EmployeeList",
  components: {
    EmployeeDetailDialog,
    Table,
    Paging,
    Title,
    Popup,
    Dropdown,
    Loading,
  },
  data() {
    return {
      // số trang
      numPages: 0,
      // số bản ghi trong 1 trang
      amountPage: 0,
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
      apiDepartment: "https://localhost:44338/api/v1/departments",
      // props cho dropdown position
      dataPosition: [{ Text: "Tất cả vị trí", Value: "" }],
      fieldsPosition: ["PositionId", "PositionName"],
      apiPosition: "https://localhost:44338/api/v1/positions",
      // data cho việc filter
      keysearch: "",
      departmentId: "",
      positionId: "",
      pageIndex: 1,
      pageSize: 5,
      // arr Id for delete
      arrEmployeeId: [],
      arrEmployeeCode: [],
      // disable btn delete
      isDisabled: true,
      // prop cho popup
      titlePopupDelete: "",
      textPopupDelete: "",
      iconPopupDelete: "",
      // loading
      isLoading: false,
      countChecked: 0,
    };
  },
  created() {
    // lấy ra toàn bộ data
    this.getEmployeesByFilter(
      this.pageIndex,
      this.pageSize,
      this.positionId,
      this.departmentId,
      this.keysearch
    );
  },
  methods: {
    /*-----------------------------------------------------------------
     *Lấy ra danh sách nhân viên theo các tiêu chí và phân trang
     *CreateBy: LQNhat(14/08/2021)
     */
     getEmployeesByFilter() {
      var self = this;
      self.isLoading = true;
       axios
        .get(
          `https://localhost:44338/api/v1/employees/filter?pageIndex=${this.pageIndex}
        &pageSize=${this.pageSize}&positionId=${this.positionId}&departmentId=${this.departmentId}&keysearch=${this.keysearch}`
        )
        .then((res) => {
          self.employees = res.data.Data;
          self.amountPage = res.data.TotalRecord;
          self.numPages = res.data.TotalPage;
          self.employees.forEach((item) => {
            item.Checked = false;
          });
          self.isLoading = false;
          // debugger; // eslint-disable-line
        })
        .catch((error) => {
          console.log(error);
        });
    },

    /**----------------------------------------------------------
     * Phân trang
     * CreateBy: LQNhat(16/08/2021)
     */
    paging(indexPage) {
      this.pageIndex = indexPage;
      this.getEmployeesByFilter(
        this.pageIndex,
        this.pageSize,
        this.positionId,
        this.departmentId,
        this.keysearch
      );
      // debugger; // eslint-disable-line
    },

    /**---------------------------------------------------
     * Hàm set value position để filter
     * CreateBy:LQNhat(2/8/2021)
     */
    getValPosition(position) {
      this.positionId = position;
      this.$refs.resetPaging.resetPaging();
      this.pageIndex = 1;
      this.getEmployeesByFilter(
        this.pageIndex,
        this.pageSize,
        this.positionId,
        this.departmentId,
        this.keysearch
      );
    },

    /**---------------------------------------------------
     * Hàm set value department để filter
     * CreateBy:LQNhat(2/8/2021)
     */
    getValDepartment(department) {
      this.departmentId = department;
      this.$refs.resetPaging.resetPaging();
      this.pageIndex = 1;
      this.getEmployeesByFilter(
        this.pageIndex,
        this.pageSize,
        this.positionId,
        this.departmentId,
        this.keysearch
      );
    },

    /**----------------------------------------------------
     * Hàm bắt sự kiện click nút xóa trên input search
     * CreateBy: LQNhat(30/07/2021)
     */
    clearValueInput() {
      this.keysearch = "";
      this.isClose = true;
      // load lại table
      this.reloadTableAndFilter();
    },
    /**-----------------------------------------------------------
     * Hàm bắt sự kiện hiển thị nút xóa khi nhập vào input search
     * khi có text thì search theo text
     * CreateBy: LQNhat(30/07/2021)
     */
    openIconDelete() {
      if (this.keysearch == "") {
        this.isClose = true;
        this.reloadTableAndFilter();
      } else {
        this.isClose = false;
        this.$refs.resetPaging.resetPaging();
        this.pageIndex = 1;
        this.getEmployeesByFilter(
          this.pageIndex,
          this.pageSize,
          this.positionId,
          this.departmentId,
          this.keysearch
        );
      }
    },

    /**-------------------------------------------------------
     * Hàm đóng form chi tiết khi click vào btn trên form
     * CreateBy: LQNhat(6/8/2021)
     */
    cancelFormDetail(mode) {
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
      this.modeFormDetail = 1;
      this.$refs.modeForm.show(this.modeFormDetail, employeeId);
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
      this.clearArr();
    },

    /**-----------------------------------------------
     * Bắt sự kiện nút xóa nhiều nhân viên
     * CreateBy:LQNHAT(22/08/2021)
     */
    btnDeleteEmployee() {
      this.employees.forEach((employee) => {
        if (employee.Checked == true) {
          // push employeeId checked vào arrEmployeeId,
          this.arrEmployeeId.push(employee.EmployeeId);
          // push employeeCode checked vào arrEmployeeCode
          this.arrEmployeeCode.push(employee.EmployeeCode);
        } else {
          // nếu trùng thì xóa đi
          this.arrEmployeeId = this.arrEmployeeId.filter(
            (item) => item != employee.EmployeeId
          );
          this.arrEmployeeCode = this.arrEmployeeCode.filter(
            (item) => item != employee.EmployeeCode
          );
        }
      });
      this.isHiddenDelete = !this.isHiddenDelete;
      this.titlePopupDelete = "Xóa nhân viên";
      this.textPopupDelete = "Bạn có chắc chắn muốn xóa nhân viên";
      this.iconPopupDelete = `<i class="fas fa-exclamation-triangle"></i>`;
      debugger; // eslint-disable-line
    },

    /**------------------------------------------------------------------------
     * Bắt sự kiện click xác nhận xóa nhiều nhân viên
     * CreateBy: LQNHAT(23/08/2021)
     */
    confirmDeleteEmployees() {
      var self = this;
      axios
        .delete(
          `https://localhost:44338/api/v1/employees?entitesId=${this.arrEmployeeId}`
        )
        .then((res) => {
          console.log(res);
          self.reloadTableAndFilter();
          self.$toast.success("Xóa nhân viên thành công", {
            timeout: 2000,
          });
          self.isHiddenDelete = true;
          self.clearArr();
          self.isDisabled = true;
        })
        .catch((res) => {
          console.log(res);
        });
    },

    /**----------------------------------------------------
     * Sau khi thao tác với popup thì clear arr đi
     * CreateBy:LQNHAT(23/08/2021)
     */
    clearArr() {
      this.employees.forEach((employee) => {
        // nếu trùng thì xóa đi
        this.arrEmployeeId = this.arrEmployeeId.filter(
          (item) => item != employee.EmployeeId
        );
        this.arrEmployeeCode = this.arrEmployeeCode.filter(
          (item) => item != employee.EmployeeCode
        );
      });
    },

    /**--------------------------------------------------------------
     * Bắt sự kiện click vào ô input thì push id checked vào mảng
     * CreateBy:LQNHAT(23/08/2021)
     */
    changeDisabled() {
      this.isDisabled = !this.isDisabled;
      this.employees.forEach((employee) => {
        if (employee.Checked == false) {
          return 1;
        }
      });

    },

    /**
     * C
     */
    exportEmployee() {
      var self = this;
      axios
        .get(`https://localhost:44338/api/v1/employees/export`)
        .then((res) => {
          console.log(res);
          self.reloadTableAndFilter();
          self.$toast.success("Xuất dữ liệu thành công", {
            timeout: 2000,
          });
          self.isHiddenDelete = true;
        })
        .catch((res) => {
          console.log(res);
        });
    },

    /**---------------------------------------------------------------
     * Hàm reload table và các control filter
     * CreateBy: LQNhat(14/08/2021)
     */
    reloadTableAndFilter() {
      var self = this;
      self.$refs.resetPaging.resetPaging();
      self.pageIndex = 1;
      self.isLoading = true;
      axios
        .get(
          `https://localhost:44338/api/v1/employees/filter?pageIndex=${this.pageIndex}
            &pageSize=${this.pageSize}
          `
        )
        .then((res) => {
          self.employees = res.data.Data;
          self.amountPage = res.data.TotalRecord;
          self.numPages = res.data.TotalPage;
          self.$refs.textDropdownPostion.setTextDefault();
          self.$refs.textDropdownDepartment.setTextDefault();
          self.$refs.resetPaging.pagingByNumPages();
          self.keysearch = "";
          self.isClose = true;
          self.departmentId = "";
          self.positionId = "";
          self.isLoading = false;
        })
        .catch((res) => {
          console.log(res);
        });
    },
  },
};
</script>

<style>
</style>