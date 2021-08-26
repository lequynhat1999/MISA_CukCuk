<template>
  <div class="modal-delete" :class="{ 'modal-delete-hidden': isHiddenDelete }">
    <div class="modal-content-delete">
      <span class="close-delete" @click="cancelDelete">&times;</span>
      <div class="header-delete">
        <div class="text-header-delete">{{ this.titlePopupDelete }}</div>
      </div>
      <div class="content-delete">
        <div class="row">
          <div class="col-1">
            <div class="icon-warning">
              <span v-html="this.iconPopupDelete"></span>
            </div>
          </div>
          <div class="col-11">
            <div class="text-content-delete">
              {{ this.textPopupDelete }}
              <b v-for="employeeCode in arrEmployeeCode" :key="employeeCode">
                <span class="employeeCodeDelete">
                  {{ employeeCode + ", " }}
                </span>
              </b>
              không ?
            </div>
          </div>
        </div>
      </div>
      <div class="footer-delete">
        <div class="box-footer">
          <div class="button-delete-cancel">
            <button
              class="m-btn m-btn-defalut m-btn-cancel-delete"
              @click="cancelDelete"
            >
              <div class="box-button">
                <div class="text-cancel-delete">Không</div>
              </div>
            </button>
          </div>
          <div class="button-delete-confirm">
            <button
              class="m-btn m-btn-defalut m-btn-confirm"
              @click="confirmDelete"
            >
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
  props: {
    isHiddenDelete: {
      type: Boolean,
      default: true,
    },
    employee: {
      type: Object,
    },
    arrEmployeeCode: {
      type: Array,
    },
    titlePopupDelete: {
      type: String,
    },
    textPopupDelete: {
      type: String,
    },
    iconPopupDelete: {
      type: String,
    },
  },
  methods: {
    cancelDelete() {
      this.$emit("cancelDelete");
    },
    confirmDelete() {
      this.$emit("confirmDeleteEmployees");
    },
  },
};
</script>

<style scoped>
@import "../../css/common/main.css";
</style>