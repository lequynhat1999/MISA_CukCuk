<template>
  <div class="navbar" :class="{'small-menu' : isHidden}">
    <div class="logo-site">
      <div class="logo-box">
        <div class="toggle-icon" @click="toggleMenu"></div>
        <div class="amis-logo" :class="{'hidden-menu': isHidden}"></div>
      </div>
    </div>
    <div class="navbar-content">
      <div class="nav-item">
        <div class="nav-item-icon nav-item-dashboard"></div>
        <div class="nav-item-text" :class="{'hidden-menu': isHidden}">Tổng quan</div>
      </div>
      <div class="nav-item">
        <div class="nav-item-icon nav-item-report"></div>
        <div class="nav-item-text" :class="{'hidden-menu': isHidden}">Báo cáo</div>
      </div>
      <div class="nav-item">
        <div class="nav-item-icon nav-item-dashboard"></div>
        <div class="nav-item-text" :class="{'hidden-menu': isHidden}">Mua hàng</div>
      </div>
      <router-link to="/customer">
        <div class="nav-item" :class="{'menu-active': isActive}" @click="activeMenu">
          <div class="nav-item-icon nav-item-employee"></div>
          <div class="nav-item-text" :class="{'hidden-menu': isHidden}">Danh mục khách hàng</div>
        </div>
      </router-link>
      <router-link to="/employee">
        <div class="nav-item" :class="{'menu-active': !isActive}" @click="activeMenu">
          <div class="nav-item-icon nav-item-employee"></div>
          <div class="nav-item-text" :class="{'hidden-menu': isHidden}">Danh mục nhân viên</div>
        </div>
      </router-link>
      <div class="nav-item">
        <div class="nav-item-icon nav-item-setting"></div>
        <div class="nav-item-text" :class="{'hidden-menu': isHidden}">Thiết kế hệ thống</div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  name: "TheMenu",
  data() {
    return {
      isActive: false,
      isHidden: false,
    }
  },
  methods: {
    activeMenu()
    {
      this.isActive = !this.isActive;
    },
    toggleMenu()
    {
      this.isHidden = ! this.isHidden;
    }
  },
}

</script>

<style  scoped>
@import "../../css/common/navbar.css";
.hidden-menu{
  display: none;
}
.small-menu{
  width: 50px;
}
</style>