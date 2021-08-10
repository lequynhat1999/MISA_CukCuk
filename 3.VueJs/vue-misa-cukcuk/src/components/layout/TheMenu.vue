<template>
    <div class="navbar">
      <div class="logo-site">
        <div class="logo-box">
          <div class="toggle-icon"></div>
          <div class="amis-logo"></div>
        </div>
      </div>
      <div class="navbar-content">
        <div class="nav-item">
          <div class="nav-item-icon nav-item-dashboard"></div>
          <div class="nav-item-text">Tổng quan</div>
        </div>
        <div class="nav-item">
          <div class="nav-item-icon nav-item-report"></div>
          <div class="nav-item-text">Báo cáo</div>
        </div>
        <div class="nav-item">
          <div class="nav-item-icon nav-item-dashboard"></div>
          <div class="nav-item-text">Mua hàng</div>
        </div>
        <div class="nav-item">
          <div class="nav-item-icon nav-item-employee"></div>
          <div class="nav-item-text">Danh mục khách hàng</div>
        </div>
        <div class="nav-item menu-active">
          <div class="nav-item-icon nav-item-employee"></div>
          <div class="nav-item-text">Danh mục nhân viên</div>
        </div>
        <div class="nav-item">
          <div class="nav-item-icon nav-item-setting"></div>
          <div class="nav-item-text">Thiết kế hệ thống</div>
        </div>
      </div>
    </div>
</template>

<script>
export default {
    name: "TheMenu",
}
</script>

<style  scoped>
@import '../../css/common/navbar.css'
</style>