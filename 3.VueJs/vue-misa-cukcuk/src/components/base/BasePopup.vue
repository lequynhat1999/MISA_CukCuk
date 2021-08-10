<template>
  <div class="modal-delete" :class="{'modal-delete-hidden': isHiddenDelete }">
    <div class="modal-content-delete">
      <span class="close-delete">&times;</span>
      <div class="header-delete">
        <div class="text-header-delete">Xóa bản ghi <span>{{employee.FullName}}</span> </div>
      </div>
      <div class="content-delete">
        <div class="row">
          <div class="col-1">
            <div class="icon-warning">
              <i class="fas fa-exclamation-triangle"></i>
            </div>
          </div>
          <div class="col-11">
            <div class="text-content-delete">
              Bạn có chắc chắn muốn xóa nhân viên
              <b><span class="employeeCodeDelete">{{employee.EmployeeCode}}</span></b> không ?
            </div>
          </div>
        </div>
      </div>
      <div class="footer-delete">
        <div class="box-footer">
          <div class="button-delete-cancel">
            <button class="m-btn m-btn-defalut m-btn-cancel-delete" @click="cancelDelete">
              <div class="box-button">
                <div class="text-cancel-delete">Không</div>
              </div>
            </button>
          </div>
          <div class="button-delete-confirm">
            <button class="m-btn m-btn-defalut m-btn-confirm" @click="confirmDelete">
              <div class="box-button">
                <div class="text-confirm">Có</div>
              </div>
            </button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
    name: "BasePopup",
    props:{
        isHiddenDelete:{
            type:Boolean,
            default: true,
        },
        employee:{
          type: Object
        }
    },
    methods: {
      cancelDelete()
      {
        this.$emit("cancelDelete");
      },
      confirmDelete()
      {
        this.$emit("confirmDelete");
      }

    },

};
</script>

<style scoped>
@import '../../css/common/main.css';
</style>