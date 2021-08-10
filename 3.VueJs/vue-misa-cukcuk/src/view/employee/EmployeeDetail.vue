<template>
  <div class="modal" :class="{ 'modal-hidden': isHidden }">
    <div class="modal-content">
      <span class="close" @click="btnCancelClick">&times;</span>
      <div class="header-info">
        <div class="text-title-header">THÔNG TIN NHÂN VIÊN</div>
      </div>
      <div class="content-info">
        <div class="content-info-box">
          <div class="row">
            <div class="col-3">
              <div class="img-default">
                <div class="img-employee"></div>
                <div class="text-img">
                  (Vui lòng chọn ảnh có định dạng .jpg, .jepg, .png, .gif.)
                </div>
              </div>
            </div>
            <div class="col-9">
              <div class="row">
                <div class="box-text-info">
                  <div class="title-info">A.THÔNG TIN CHUNG:</div>
                  <div class="underlined"></div>
                </div>
              </div>
              <div class="row">
                <div class="col-6">
                  <div class="row-wrapper">
                    <div class="title-input-info">
                      Mã nhân viên (<span style="color: red">*</span>)
                    </div>
                    <div class="box-input">
                      <!-- <ValidationProvider
                        rules="required"
                        name="Mã nhân viên"
                        v-slot="{ errors }"
                      > -->
                        <input
                          ref="txtEmployeeCode"
                          type="text"
                          class="input-info"
                          required
                          v-model="employee.EmployeeCode"
                        />
                        <!-- <span style="color: red">{{ errors[0] }}</span>
                      </ValidationProvider> -->
                    </div>
                  </div>
                </div>
                <div class="col-6">
                  <div class="row-wrapper">
                    <div class="title-input-info">
                      Họ và tên (<span style="color: red">*</span>)
                    </div>
                    <div class="box-input">
                      <ValidationProvider
                        rules="required"
                        name="Họ và tên"
                        v-slot="{ errors }"
                      >
                        <input
                          type="text"
                          class="input-info txtFullName"
                          required
                          v-model="employee.FullName"
                        />
                        <span style="color: red">{{ errors[0] }}</span>
                      </ValidationProvider>
                    </div>
                  </div>
                </div>
              </div>
              <div class="row">
                <div class="col-6">
                  <div class="box-wrapper">
                    <div class="title-input-info">Ngày sinh</div>
                    <div class="box-input">
                      <datepicker
                        class="input-info"
                        v-model="employee.DateOfBirth"
                        :format="'DD/MM/YYYY'"
                        :value-type="'YYYY-MM-DD'"
                      ></datepicker>
                    </div>
                  </div>
                </div>
                <div class="col-6">
                  <div class="box-wrapper">
                    <div class="title-input-info">Giới tính</div>
                    <div class="box-dropdown">
                      <Dropdown
                        @get="getValGender"
                        :value="employee.Gender"
                        :data="dataGender"
                        :style="{ 'margin-left': '0px' }"
                      />
                    </div>
                  </div>
                </div>
              </div>
              <div class="row">
                <div class="col-6">
                  <div class="box-wrapper">
                    <div class="title-input-info">
                      Số CMNTND/Căn cước (<span style="color: red">*</span>)
                    </div>
                    <div class="box-input">
                      <ValidationProvider
                        rules="required|numeric"
                        name="Số CMNTND/Căn cước"
                        v-slot="{ errors }"
                      >
                        <input
                          type="text"
                          class="input-info txtIdentityNumber"
                          required
                          v-model="employee.IdentityNumber"
                        />
                        <span style="color: red">{{ errors[0] }}</span>
                      </ValidationProvider>
                    </div>
                  </div>
                </div>
                <div class="col-6">
                  <div class="box-wrapper">
                    <div class="title-input-info">Ngày cấp</div>
                    <datepicker
                      class="input-info"
                      v-model="employee.IdentityDate"
                      :format="'DD/MM/YYYY'"
                      :value-type="'YYYY-MM-DD'"
                    ></datepicker>
                  </div>
                </div>
              </div>
              <div class="row">
                <div class="col-6">
                  <div class="box-wrapper">
                    <div class="title-input-info">Nơi cấp</div>
                    <div class="box-input">
                      <input
                        type="text"
                        class="input-info txtIdentityPlace"
                        v-model="employee.IdentityPlace"
                      />
                    </div>
                  </div>
                </div>
              </div>
              <div class="row">
                <div class="col-6">
                  <div class="box-wrapper">
                    <div class="title-input-info">
                      Email (<span style="color: red">*</span>)
                    </div>
                    <div class="box-input">
                      <ValidationProvider
                        rules="required|email"
                        name="Email"
                        v-slot="{ errors }"
                      >
                        <input
                          type="email"
                          class="input-info txtEmail"
                          placeholder="lqnhat@gmail.com"
                          required
                          v-model="employee.Email"
                        />
                        <span style="color: red">{{ errors[0] }}</span>
                      </ValidationProvider>
                    </div>
                  </div>
                </div>
                <div class="col-6">
                  <div class="box-wrapper">
                    <div class="title-input-info">
                      Số điện thoại (<span style="color: red">*</span>)
                    </div>
                    <div class="box-input">
                      <ValidationProvider
                        rules="required|digits:10"
                        name="Số điện thoại"
                        v-slot="{ errors }"
                      >
                        <input
                          type="text"
                          class="input-info txtPhoneNumber"
                          required
                          v-model="employee.PhoneNumber"
                        />
                        <span style="color: red">{{ errors[0] }}</span>
                      </ValidationProvider>
                    </div>
                  </div>
                </div>
              </div>
              <div class="row">
                <div class="box-text-info" style="margin-top: 16px">
                  <div class="title-info">B.THÔNG TIN CÔNG VIỆC:</div>
                  <div class="underlined"></div>
                </div>
              </div>
              <div class="row" style="margin-top: -6px">
                <div class="col-6">
                  <div class="box-wrapper">
                    <div class="title-input-info">Vị trí</div>
                    <div class="box-dropdown">
                      <Dropdown
                        @get="getValPosition"
                        :value="employee.PositionId"
                        :url="apiPosition"
                        :fields="fieldsPosition"
                        :data="dataPosition"
                        :name="namePosition"
                        :style="{ 'margin-left': '0px' }"
                      />
                    </div>
                  </div>
                </div>
                <div class="col-6">
                  <div class="box-wrapper">
                    <div class="title-input-info">Phòng ban</div>
                    <div class="box-dropdown">
                      <Dropdown
                        @get="getValDepartment"
                        :value="employee.DepartmentId"
                        :url="apiDepartment"
                        :fields="fieldsDepartment"
                        :name="nameDepartment"
                        :data="dataDepartment"
                        :style="{ 'margin-left': '0px' }"
                      />
                    </div>
                  </div>
                </div>
              </div>
              <div class="row">
                <div class="col-6">
                  <div class="box-wrapper">
                    <div class="title-input-info">Mã số thuế cá nhân</div>
                    <div class="box-input">
                      <input
                        type="text"
                        class="input-info txtPersonalTaxCode"
                        v-model="employee.PersonalTaxCode"
                      />
                    </div>
                  </div>
                </div>
                <div class="col-6">
                  <div class="box-wrapper">
                    <div class="title-input-info">Mức lương cơ bản</div>
                    <div class="box-input-salary">
                      <money
                        type="text"
                        class="input-info input-salary"
                        style="text-align: right"
                        value=""
                        v-model="employee.Salary"
                        v-bind="money"
                      ></money>
                      <div class="vnd">(VND)</div>
                    </div>
                  </div>
                </div>
              </div>
              <div class="row">
                <div class="col-6">
                  <div class="box-wrapper">
                    <div class="title-input-info">Ngày gia nhập công ty</div>
                    <datepicker
                      class="input-info"
                      v-model="employee.JoinDate"
                      :format="'DD/MM/YYYY'"
                      :value-type="'YYYY-MM-DD'"
                    ></datepicker>
                  </div>
                </div>
                <div class="col-6">
                  <div class="box-wrapper">
                    <div class="title-input-info">Tình trạng công việc</div>
                    <div class="box-dropdown">
                      <Dropdown
                        @get="getValWorkStatus"
                        :value="employee.WorkStatus"
                        :data="dataWorkStatus"
                        :style="{ 'margin-left': '0px' }"
                      />
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
      <div class="footer-info">
        <div class="box-footer">
          <div class="button-info-cancel">
            <button
              class="m-btn m-btn-defalut m-btn-cancel"
              @click="btnCancelClick"
            >
              <div class="box-button">
                <div class="text-cancel">Hủy</div>
              </div>
            </button>
          </div>
          <div class="button-info-save">
            <button
              class="m-btn m-btn-defalut m-btn-save"
              @click="saveBtnClick"
            >
              <div class="box-button">
                <div class="m-btn-icon btn-save">
                  <i class="fas fa-save"></i>
                </div>
                <div class="text-save">Lưu</div>
              </div>
            </button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import axios from "axios";
import moment from "moment";
import { Money } from "v-money";
import Dropdown from "../../components/base/BaseDropdown.vue";
import { extend } from "vee-validate";
import { email } from "vee-validate/dist/rules";
extend("minmax", {
  validate(value) {
    return {
      required: true,
      valid: ["", null, undefined].indexOf(value) === -1,
    };
  },
  computesRequired: true,
});
extend("email", email);

export default {
  name: "EmployeeDetail",
  components: {
    Dropdown,
    Money,
  },
  data() {
    return {
      money: {
        decimal: ",",
        thousands: ".",
        precision: 0,
        masked: false,
      },
      mode: 0,
      // mảng employee
      employee: {},
      // data cho dropdown department
      nameDepartment: "department",
      dataDepartment: [],
      fieldsDepartment: ["DepartmentId", "DepartmentName"],
      apiDepartment: "http://cukcuk.manhnv.net/api/Department",
      // data cho dropdown position
      namePosition: "position",
      dataPosition: [],
      fieldsPosition: ["PositionId", "PositionName"],
      apiPosition: "http://cukcuk.manhnv.net/v1/Positions",
      // data cho dropdown gender
      dataGender: [
        { Text: "Nam", Value: 1 },
        { Text: "Nữ", Value: 0 },
        { Text: "Không xác định", Value: 2 },
      ],
      nameGender: "gender",
      fieldsGender: "",
      apiGender: "",
      // data cho dropdown workStatus
      dataWorkStatus: [
        { Text: "Làm chính thức", Value: 0 },
        { Text: "Đang thực tập", Value: 1 },
        { Text: "Đã nghỉ việc", Value: 2 },
      ],
      nameWorkStatus: "workStatus",
      fieldsWorkStatus: "",
      apiWorkStatus: "",
      employeeId: "",
    };
  },
  props: {
    isHidden: {
      type: Boolean,
      default: true,
      required: true,
    },
  },
  methods: {
    show(mode, id) {
      this.mode = mode;
      this.employeeId = id;
      if (mode == 0) {
        this.employee = {};
        this.autoNewEmployeeCode();
        // clear error form
        this.$nextTick(() => {
          this.errors.clear();
        });
        debugger; // eslint-disable-line
      } else {
        debugger; // eslint-disable-line
        this.bindDataToForm();
      }
    },
    /**---------------------------------------------------
     * Hàm set value gender
     * CreateBy:LQNhat(2/8/2021)
     */
    getValGender(gender) {
      this.employee.Gender = gender;
    },
    /**---------------------------------------------------
     * Hàm set value position
     * CreateBy:LQNhat(2/8/2021)
     */
    getValPosition(position) {
      this.employee.PositionId = position;
    },
    /**---------------------------------------------------
     * Hàm set value department
     * CreateBy:LQNhat(2/8/2021)
     */
    getValDepartment(department) {
      this.employee.DepartmentId = department;
    },
    /**---------------------------------------------------
     * Hàm set value workStatus
     * CreateBy:LQNhat(2/8/2021)
     */
    getValWorkStatus(workStatus) {
      this.employee.WorkStatus = workStatus;
    },
    /**------------------------------------------
     * Đóng form chi tiết khi click btn trên form
     * CreateBy: LQNhat(30/07/2021)
     */
    btnCancelClick() {
      this.mode = 1;
      this.employee = {};
      this.$emit("cancelFormDetail", this.mode);
    },
    /**-----------------------------------------------------------------
     * Hàm sinh tự động mã nhân viên
     * CreateBy: LQNhat (4/8/2021)
     */
    autoNewEmployeeCode() {
      let self = this;
      axios
        .get(`http://cukcuk.manhnv.net/v1/Employees/NewEmployeeCode`)
        .then((res) => {
          let newEmployee = {};
          newEmployee.EmployeeCode = res.data;
          self.employee = newEmployee;
          self.$refs.txtEmployeeCode.focus();
          debugger; // eslint-disable-line
        })
        .catch((err) => {
          console.log(err);
        });
    },
    /**---------------------------------------------------
     * Hàm bắt sự kiện khi click vào nút lưu trong form
     * CreateBy: LQNhat(31/07/2021)
     */
    saveBtnClick() {
      if (this.mode == 0) {
        //add nv
        this.addEmployee();
        this.employee = {};
      } else {
        //edit nv
        this.editEmployee();
      }
    },
    /**---------------------------------------
     * Hàm thêm nhân viên từ form chi tiết
     * CreateBy: LQNhat(31/07/2021)
     */
    addEmployee() {
      var self = this;
      axios
        .post(`http://cukcuk.manhnv.net/v1/Employees`, self.employee)
        .then((res) => {
          self.$emit("loadData");
          self.$emit("closeForm");
          this.$toast.success("Thêm nhân viên thành công", {
            timeout: 2000,
          });
          self.employee = {};
          console.log(res);
        })
        .catch((res) => {
          console.log(res);
        });
    },
    /**---------------------------------------
     * Hàm sửa thông tin từ form chi tiết
     * CreateBy: LQNhat(31/07/2021)
     */
    editEmployee() {
      var self = this;
      axios
        .put(
          `http://cukcuk.manhnv.net/v1/Employees/${self.employeeId}`,
          self.employee
        )
        .then((res) => {
          self.$emit("loadData");
          self.$emit("closeForm");
          this.$toast.success("Sửa thông tin nhân viên thành công", {
            timeout: 2000,
          });
          console.log(res);
        })
        .catch((res) => {
          console.log(res);
        });
    },
    /**---------------------------------------------------------
     * Hàm bind dữ liệu từ table lên form thông tin chi tiết
     * CreateBy: LQNhat(31/07/2021)
     */
    bindDataToForm() {
      var self = this;
      // call api
      axios
        .get(`http://cukcuk.manhnv.net/v1/Employees/${self.employeeId}`)
        .then((res) => {
          self.employee = res.data;
          // format salary,date về đúng định dạng
          self.employee.DateOfBirth = self.formatDate(res.data.DateOfBirth);
          self.employee.JoinDate = self.formatDate(res.data.JoinDate);
          self.employee.IdentityDate = self.formatDate(res.data.IdentityDate);
          console.log(res.data);
          debugger; // eslint-disable-line
        })
        .catch((res) => {
          console.log(res);
        });
    },
    /**----------------------------------------------------------
     * Hàm format ngày tháng năm trên form chỉnh sửa
     * CreateBy:LQNhat(1/8/2021)
     */
    formatDate(date) {
      if (date) {
        return moment(String(date)).format("yyyy-MM-DD");
      }
    },
  },
};
</script>

<style>
@import "../../css/common/main.css";
</style>